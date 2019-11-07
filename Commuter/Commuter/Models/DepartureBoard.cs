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
        private readonly DataFetcher dataFetcher;
        private readonly ILogger<DepartureBoardViewModel> logger;
        private readonly CancellationTokenSource? cancellationTokenSource;
        private readonly DateTime lastFetch;
        private bool isLoadingData;
        private const int DepartureDisplayLimit = 4;

        public DepartureBoardViewModel(DataFetcher dataFetcher, ILogger<DepartureBoardViewModel> logger)
        {
            this.dataFetcher = dataFetcher;
            this.logger = logger;
        }

        public bool IsLoadingData { get => isLoadingData; set => SetProperty(ref isLoadingData, value); }

        public async Task UpdateAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogDebug("Loading Departure Board");

                if (IsLoadingData)
                {
                    logger.LogDebug("Load is already in progress");
                    return;
                }

                IsLoadingData = true;

                var fetchedStopAreas = await dataFetcher.FetchData(cancellationToken);

                if (!fetchedStopAreas.Any())
                {
                    logger.LogDebug("No data found.");
                    IsLoadingData = false;
                    return;
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogDebug("Update was cancelled");
                    return;
                }

                await Device.InvokeOnMainThreadAsync(() =>
                {
                    UpdateStopAreas(fetchedStopAreas);

                    logger.LogDebug("StopAreas updated");

                    foreach (var stopArea in this)
                    {
                        logger.LogDebug($"Fetched StopPoints and Departures for StopArea {stopArea.Name}");

                        var sa = fetchedStopAreas.First(sa => sa.StopAreaId == stopArea.StopAreaId);

                        UpdateStopPoints(stopArea, sa.StopPoints);

                        logger.LogDebug($"Updated StopPoints for StopArea {stopArea.Name}");
                    }
                });
            }
            finally
            {
                IsLoadingData = false;
            }

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
            logger.LogDebug($"Cleared Departure Board");
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

                CleanUpDepartures(stopArea, stopPoint);

                UpdateDepartures(stopArea, fetchedStopPoint, stopPoint);

                SortDeparturesByDepartureTime(stopArea, stopPoint);
            }
        }

        private void SortDeparturesByDepartureTime(StopArea stopArea, StopPoint stopPoint)
        {
            stopPoint.SortBy(x => x.Time);

            logger.LogDebug($"Sorted Departures at StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
        }

        private void UpdateDepartures(StopArea stopArea, Data.StopPoint fetchedStopPoint, StopPoint stopPoint)
        {
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

                departure.LineType = fetchedDeparture.LineType;
                departure.Line = fetchedDeparture.Line;
                departure.Name = fetchedDeparture.Name;
                departure.Towards = fetchedDeparture.Towards;
                departure.Time = fetchedDeparture.DepartureTime;

                logger.LogDebug($"Updated Departure {departure.Name} {departure.Towards} with {departure.RunNo} to StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
            }
        }

        private void CleanUpDepartures(StopArea stopArea, StopPoint stopPoint)
        {
            foreach (var departure in stopPoint.Where(x => DateTime.Now.Truncate(TimeSpan.FromSeconds(1)) > x.Time).ToArray())
            {
                logger.LogDebug($"Removed Departure {departure.Name} {departure.Towards} with {departure.RunNo} from StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
                stopPoint.Remove(departure);
            }
        }

        private void UpdateStopAreas(IEnumerable<(int StopAreaId, string Name, int Distance, float X, float Y, IEnumerable<Data.StopPoint> StopPoints)> fetchedStopAreas)
        {
            // INFO: Delete StopAreas that have not been recently fetched
            CleanUpStopAreas(fetchedStopAreas);

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

                // INFO: The distance can change out in the field
                stopArea.Distance = fetchedStopArea.Distance;

                logger.LogDebug($"Updated StopArea {stopArea.Name}");
            }

            SortStopAreasByDistance();
        }

        private void SortStopAreasByDistance()
        {
            this.SortBy(x => x.Distance);

            logger.LogDebug($"¨Sorted StopAreas");
        }

        private void CleanUpStopAreas(IEnumerable<(int StopAreaId, string Name, int Distance, float X, float Y, IEnumerable<Data.StopPoint> StopPoints)> fetchedStopAreas)
        {
            foreach (var stopArea in this.ToArray())
            {
                if (!fetchedStopAreas.Any(d => d.StopAreaId == stopArea.StopAreaId))
                {
                    Remove(stopArea);
                    logger.LogDebug($"Removed StopArea {stopArea.Name}");
                }
            }
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
