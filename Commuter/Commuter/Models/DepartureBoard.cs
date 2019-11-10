using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Commuter.Data;

using Microsoft.Extensions.Logging;

using Xamarin.Forms;

namespace Commuter.Models
{
    public class DepartureBoardViewModel : ObservableCollection<StopArea>, IDepartureBoard
    {
        private readonly ILogger<DepartureBoardViewModel> logger;
        private const int DepartureDisplayLimit = 4;

        public DepartureBoardViewModel(ILogger<DepartureBoardViewModel> logger)
        {
            this.logger = logger;
        }

        public async Task UpdateAsync(IEnumerable<IStopArea> data)
        {
            try
            {
                logger.LogDebug("Loading Departure Board");

                await Device.InvokeOnMainThreadAsync(() =>
                {
                    UpdateStopAreas(data);

                    logger.LogDebug("StopAreas updated");

                    foreach (var stopArea in this)
                    {
                        logger.LogDebug($"Fetched StopPoints and Departures for StopArea {stopArea.Name}");

                        var sa = data.First(sa => sa.StopAreaId == stopArea.StopAreaId);

                        UpdateStopPoints(stopArea, sa.StopPoints);

                        SortStopPointsByName(stopArea);

                        logger.LogDebug($"Updated StopPoints for StopArea {stopArea.Name}");
                    }
                });
            }
            catch (Exception)
            {
                // Leave it here for debugging
                throw;
            }
        }

        private void SortStopPointsByName(StopArea stopArea)
        {
            stopArea.SortBy(x => x.Name);

            logger.LogDebug($"¨Sorted StopPoints");
        }

        public async Task ClearAsync()
        {
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
            foreach (var fetchedDeparture in fetchedStopPoint.Departures.Take(GetDepartureCacheLimit()))
            {
                var departure = stopPoint.FirstOrDefault(x => x.RunNo == fetchedDeparture.RunNo);
                if (departure == null)
                {
                    departure = new Departure
                    {
                        RunNo = fetchedDeparture.RunNo
                    };

                    SetDepartureProperties(fetchedDeparture, departure);

                    stopPoint.Add(departure);

                    logger.LogDebug($"Added Departure {departure.Name} {departure.Towards} with {departure.RunNo} to StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
                }
                else
                {
                    SetDepartureProperties(fetchedDeparture, departure);
                }

                logger.LogDebug($"Updated Departure {departure.Name} {departure.Towards} with {departure.RunNo} to StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
            }
        }

        private static void SetDepartureProperties(Data.Departure fetchedDeparture, Departure departure)
        {
            departure.LineType = fetchedDeparture.LineType;
            departure.Line = fetchedDeparture.Line;
            departure.Name = fetchedDeparture.Name;
            departure.Towards = fetchedDeparture.Towards;
            departure.Time = fetchedDeparture.DepartureTime;
        }

        private static int GetDepartureCacheLimit()
        {
            return DepartureDisplayLimit + (int)Math.Ceiling(DepartureDisplayLimit * 0.5);
        }

        private void CleanUpDepartures(StopArea stopArea, StopPoint stopPoint)
        {
            foreach (var departure in stopPoint.Where(x => DateTime.Now.Truncate(TimeSpan.FromSeconds(1)) > x.Time).ToArray())
            {
                logger.LogDebug($"Removed Departure {departure.Name} {departure.Towards} with {departure.RunNo} from StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
                stopPoint.Remove(departure);
            }
        }

        private void UpdateStopAreas(IEnumerable<IStopArea> fetchedStopAreas)
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

        private void CleanUpStopAreas(IEnumerable<IStopArea> fetchedStopAreas)
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
