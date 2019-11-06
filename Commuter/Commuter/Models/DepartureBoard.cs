using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Helpers;

using Microsoft.Extensions.Logging;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace Commuter.Models
{
    public class DepartureBoardViewModel : ObservableCollection<StopArea>, IDepartureBoard
    {
        private readonly StopAreaFetcher stopAreaFetcher;
        private readonly DepartureFetcher departureFetcher;
        private readonly ILogger<DepartureBoardViewModel> logger;
        private readonly CancellationTokenSource? cancellationTokenSource;
        private readonly DateTime lastFetch;
        private bool isLoadingData;
        private const int DepartureDisplayLimit = 4;

        public DepartureBoardViewModel(StopAreaFetcher stopAreaFetcher, DepartureFetcher departureFetcher, ILogger<DepartureBoardViewModel> logger)
        {
            this.stopAreaFetcher = stopAreaFetcher;
            this.departureFetcher = departureFetcher;
            this.logger = logger;
        }

        public bool IsLoadingData { get => isLoadingData; set => SetProperty(ref isLoadingData, value); }

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
        {
            logger.LogInformation("Loading Departure Board");

            if (IsLoadingData)
            {
                logger.LogInformation("Load is already in progress");
                return;
            }

            IsLoadingData = true;

            var fetchedStopAreas = await GetStopAreasAsync();

            if (!fetchedStopAreas.Any())
            {
                logger.LogInformation("No StopAreas found.");
                IsLoadingData = false;
                return;
            }

            logger.LogInformation("Fetched StopAreas");

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                UpdateStopAreas(fetchedStopAreas);

                logger.LogInformation("StopAreas updated");

                foreach (var stopArea in this)
                {
                    var fetchedStopPoints = await departureFetcher.GetDeparturesByStopPointAsync(stopArea.StopAreaId, GetDesiredDepartureTime());

                    logger.LogInformation($"Fetched StopPoints and Departures for StopArea {stopArea.Name}");

                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        UpdateStopPoints(stopArea, fetchedStopPoints);

                        logger.LogInformation($"Updated StopPoints for StopArea {stopArea.Name}");
                    });
                }
            });

            IsLoadingData = false;
        }

        public async Task ClearAsync()
        {
            while (IsLoadingData)
            {
                await Task.Delay(500);
            }
            await Device.InvokeOnMainThreadAsync(() =>
            {
                Clear();
            });
            logger.LogInformation($"Cleared Departure Board");
        }

        private static async Task<Location> GetCoordinates()
        {
            if (Utils.IsRunningInSimulator)
            {
                return await Task.FromResult(new Location(55.6906897, 13.1899686));
            }
            else
            {
                return await Xamarin.Essentials.Geolocation.GetLocationAsync();
            }
        }

        private async Task<IEnumerable<Data.StopArea>> GetStopAreasAsync()
        {
            var location = await GetCoordinates();
            var radius = 400;
            return await stopAreaFetcher.GetNearestStopAreasAsync(location.Latitude, location.Longitude, radius);
        }

        private void UpdateStopPoints(StopArea stopArea, IEnumerable<Data.StopPoint> fetchedStopPoints)
        {
            foreach (var fetchedStopPoint in fetchedStopPoints)
            {
                var stopPoint = stopArea.FirstOrDefault(x => x.Name == fetchedStopPoint.Name);
                if (stopPoint == null)
                {
                    stopPoint = new StopPoint()
                    {
                        Name = fetchedStopPoint.Name
                    };
                    stopArea.Add(stopPoint);
                    logger.LogDebug($"Added StopPoint {stopPoint.Name} to StopArea {stopArea.Name}");
                }

                // INFO: Clean up departed departures.

                foreach (var departure in stopPoint.Where(x => DateTime.Now.Truncate(TimeSpan.FromSeconds(1)) > x.Time).ToArray())
                {
                    logger.LogDebug($"Removed Departure {departure.Name} {departure.Towards} with {departure.RunNo} from StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
                    stopPoint.Remove(departure);
                }

                foreach (var fetchedDeparture in fetchedStopPoint.Departures.Take(DepartureDisplayLimit))
                {
                    var departure = stopPoint.FirstOrDefault(x => x.RunNo == fetchedDeparture.RunNo);
                    if (departure == null)
                    {
                        departure = new Departure
                        {
                            RunNo = fetchedDeparture.RunNo
                        };
                        stopPoint.Add(departure);
                        logger.LogDebug($"Added Departure {departure.Name} {departure.Towards} with {departure.RunNo} to StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        departure.LineType = fetchedDeparture.LineType;
                        departure.Line = fetchedDeparture.Line;
                        departure.Name = fetchedDeparture.Name;
                        departure.Towards = fetchedDeparture.Towards;
                        departure.Time = fetchedDeparture.DepartureTime;
                    });

                    logger.LogDebug($"Updated Departure {departure.Name} {departure.Towards} with {departure.RunNo} to StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
                }

                stopPoint.SortBy(x => x.Time);

                logger.LogDebug($"Sorted Departures at StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
            }
        }

        private static DateTime GetDesiredDepartureTime()
        {
            return DateTime.Now.Truncate(TimeSpan.FromSeconds(1));
        }

        private void UpdateStopAreas(IEnumerable<Data.StopArea> fetchedStopAreas)
        {

            // INFO: Delete StopAreas that have not been recently fetched
            foreach (var stopArea in this.ToArray())
            {
                if (!fetchedStopAreas.Any(d => d.StopAreaId == stopArea.StopAreaId))
                {
                    Remove(stopArea);
                    logger.LogDebug($"Removed StopArea {stopArea.Name}");
                }
            }

            foreach (var fetchedStopArea in fetchedStopAreas)
            {
                var stopArea = this.FirstOrDefault(x => x.StopAreaId == fetchedStopArea.StopAreaId);
                if (stopArea == null)
                {
                    stopArea = new StopArea()
                    {
                        StopAreaId = fetchedStopArea.StopAreaId,
                        Name = fetchedStopArea.Name,
                        X = fetchedStopArea.X,
                        Y = fetchedStopArea.Y,
                    };
                    Add(stopArea);
                    logger.LogDebug($"Added StopArea {stopArea.Name}");
                }

                // INFO: The distance can change in the field
                stopArea.Distance = fetchedStopArea.Distance;

                logger.LogDebug($"Updated StopArea {stopArea.Name}");
            }

            this.SortBy(x => x.Distance);

            logger.LogDebug($"¨Sorted StopAreas");
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (object.Equals(value, field))
            {
                return false;
            }

            field = value;

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
