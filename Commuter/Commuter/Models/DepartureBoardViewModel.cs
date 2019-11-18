using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Services;

using Microsoft.Extensions.Logging;

namespace Commuter.Models
{
    public class DepartureBoardViewModel : ObservableCollection<StopAreaViewModel>, IDepartureBoard
    {
        private readonly ILogger<DepartureBoardViewModel> logger;
        private readonly IThreadDispatcher threadDispatcher;
        private readonly IStopAreaViewModelFactory stopAreaVmFactory;
        private readonly IStopPointViewModelFactory stopPointVmFactory;
        private readonly IDepartureViewModelFactory departureVmFactory;
        private readonly IDeviationViewModelFactory deviationVmFactory;
        private const int DepartureDisplayLimit = 4;

        public DepartureBoardViewModel(ILogger<DepartureBoardViewModel> logger,
            IThreadDispatcher threadDispatcher,
            IStopAreaViewModelFactory stopAreaVmFactory,
            IStopPointViewModelFactory stopPointVmFactory,
            IDepartureViewModelFactory departureVmFactory,
            IDeviationViewModelFactory deviationVmFactory)
        {
            this.logger = logger;
            this.threadDispatcher = threadDispatcher;
            this.stopAreaVmFactory = stopAreaVmFactory;
            this.stopPointVmFactory = stopPointVmFactory;
            this.departureVmFactory = departureVmFactory;
            this.deviationVmFactory = deviationVmFactory;
        }

        public async Task UpdateAsync(IEnumerable<IStopArea> data, CancellationToken cancellationToken = default)
        {
            try
            {
                logger.LogDebug("Loading Departure Board");

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await threadDispatcher.InvokeOnMainThreadAsync(async () =>
                {
                    await UpdateStopAreas(data).ConfigureAwait(true);
                });

                logger.LogDebug("StopAreas updated");

                foreach (var stopArea in this)
                {
                    await threadDispatcher.InvokeOnMainThreadAsync(async () =>
                    {
                        logger.LogDebug($"Fetched StopPoints and Departures for StopArea {stopArea.Name}");

                        var sa = data.First(sa => sa.StopAreaId == stopArea.StopAreaId);

                        await UpdateStopPoints(stopArea, sa.StopPoints).ConfigureAwait(true);

                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }

                        SortStopPointsByName(stopArea);

                        logger.LogDebug($"Updated StopPoints for StopArea {stopArea.Name}");
                    });
                }
            }
            catch (Exception)
            {
                // Leave it here for debugging
                throw;
            }
        }

        private void SortStopPointsByName(StopAreaViewModel stopArea)
        {
            stopArea.SortBy(x => x.Name);

            logger.LogDebug($"¨Sorted StopPoints");
        }

        public async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            await threadDispatcher.InvokeOnMainThreadAsync(() =>
            {
                Clear();
            });
            logger.LogDebug($"Cleared Departure Board");
        }

        private async Task UpdateStopPoints(StopAreaViewModel stopArea, IEnumerable<Data.StopPoint> fetchedStopPoints)
        {
            try
            {
                foreach (var fetchedStopPoint in fetchedStopPoints)
                {
                    var stopPoint = stopArea.FirstOrDefault(x => x.Name == fetchedStopPoint.Name);
                    if (stopPoint == null)
                    {
                        stopPoint = await stopPointVmFactory.CreateViewModelAsync(fetchedStopPoint).ConfigureAwait(true);

                        await UpdateDepartures(stopArea, fetchedStopPoint, stopPoint).ConfigureAwait(true);

                        stopArea.Add(stopPoint);

                        logger.LogDebug($"Added StopPoint {stopPoint.Name} to StopArea {stopArea.Name}");
                    }
                    else
                    {
                        CleanUpDepartures(stopArea, stopPoint);

                        await UpdateDepartures(stopArea, fetchedStopPoint, stopPoint).ConfigureAwait(true);
                    }

                    SortDeparturesByDepartureTime(stopArea, stopPoint);
                }
            }
            catch (Exception exc)
            {
                // Leave it here for debugging
                Console.WriteLine(exc);
                throw;
            }
        }

        private void SortDeparturesByDepartureTime(StopAreaViewModel stopArea, StopPointViewModel stopPoint)
        {
            if (!stopPoint.OrderBy(x => x.Time).SequenceEqual(stopPoint))
            {
                stopPoint.SortBy(x => x.Time);

                logger.LogDebug($"Sorted Departures at StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
            }
        }

        private async Task UpdateDepartures(StopAreaViewModel stopArea, Data.StopPoint fetchedStopPoint, StopPointViewModel stopPoint)
        {
            foreach (var fetchedDeparture in fetchedStopPoint.Departures.Take(GetDepartureCacheLimit()))
            {
                var departure = stopPoint.FirstOrDefault(x => x.RunNo == fetchedDeparture.RunNo);
                if (departure == null)
                {
                    // INFO: Make sure we don't add back a deleted Departure.

                    if (IsOverdue(fetchedDeparture.DepartureTime))
                    {
                        continue;
                    }

                    departure = await departureVmFactory.CreateViewModelAsync(fetchedDeparture).ConfigureAwait(true);

                    await UpdateDeviations(fetchedDeparture, departure).ConfigureAwait(true);

                    stopPoint.Add(departure);

                    logger.LogDebug($"Added Departure {departure.Name} {departure.Towards} with {departure.RunNo} to StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
                }
                else
                {
                    await departureVmFactory.UpdateViewModelAsync(fetchedDeparture, departure).ConfigureAwait(true);

                    await UpdateDeviations(fetchedDeparture, departure).ConfigureAwait(true);

                    logger.LogDebug($"Updated Departure {departure.Name} {departure.Towards} with {departure.RunNo} to StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");

                }
            }
        }

        private async Task UpdateDeviations(Data.Departure fetchedDeparture, DepartureViewModel departure)
        {
            foreach (var deviation in departure.Deviations.ToArray())
            {
                if (!fetchedDeparture.Deviations.Any(d => d.Header == deviation.Header && d.Urgency == deviation.Urgency && d.Importance == deviation.Importance && d.Influence == deviation.Influence))
                {
                    departure.Deviations.Remove(deviation);
                    logger.LogDebug($"Removed Deviation from Departure {departure.Name} {departure.Name} {departure.Towards} with {departure.RunNo}");
                }
            }

            if (fetchedDeparture.Deviations == null)
            {
                return;
            }

            if (fetchedDeparture != null)
            {
                foreach (var fetchedDeviation in fetchedDeparture?.Deviations)
                {
                    var deviationViewModel = departure.Deviations
                        .FirstOrDefault(d =>
                            d.Header == fetchedDeviation.Header &&
                            d.Urgency == fetchedDeviation.Urgency &&
                            d.Importance == fetchedDeviation.Importance &&
                            d.Influence == fetchedDeviation.Influence);

                    if (deviationViewModel == null)
                    {
                        deviationViewModel = await deviationVmFactory.CreateViewModelAsync(fetchedDeviation).ConfigureAwait(true);
                        departure.Deviations.Add(deviationViewModel);
                    }
                    else
                    {
                        await deviationVmFactory.UpdateViewModelAsync(fetchedDeviation, deviationViewModel).ConfigureAwait(true);
                    }
                }
            }
        }

        private static int GetDepartureCacheLimit()
        {
            return DepartureDisplayLimit + (int)Math.Ceiling(DepartureDisplayLimit * 0.5);
        }

        private void CleanUpDepartures(StopAreaViewModel stopArea, StopPointViewModel stopPoint)
        {
            foreach (var departure in stopPoint.Where(d => IsOverdue(d.Time)).ToArray())
            {
                stopPoint.Remove(departure);
                logger.LogDebug($"Removed Departure {departure.Name} {departure.Towards} with {departure.RunNo} from StopPoint {stopPoint.Name} in StopArea {stopArea.Name}");
            }
        }

        private static bool IsOverdue(DateTime time)
        {
            var now = DateTime.Now.Truncate(TimeSpan.FromSeconds(1));
            return now > time.AddMinutes(1);
        }

        private async Task UpdateStopAreas(IEnumerable<IStopArea> fetchedStopAreas)
        {
            // INFO: Delete StopAreas that have not been recently fetched
            CleanUpStopAreas(fetchedStopAreas);

            foreach (var fetchedStopArea in fetchedStopAreas)
            {
                var stopArea = this.FirstOrDefault(x => x.StopAreaId == fetchedStopArea.StopAreaId);
                if (stopArea == null)
                {
                    stopArea = await stopAreaVmFactory.CreateViewModelAsync(fetchedStopArea).ConfigureAwait(true);

                    Add(stopArea);
                    logger.LogDebug($"Added StopArea {stopArea.Name}");
                }
                else
                {
                    // INFO: The distance can change out in the field
                    stopArea.Distance = fetchedStopArea.Distance;

                    await stopAreaVmFactory.UpdateViewModelAsync(fetchedStopArea, stopArea).ConfigureAwait(true);

                    logger.LogDebug($"Updated StopArea {stopArea.Name}");
                }
            }

            SortStopAreasByDistance();
        }

        private void SortStopAreasByDistance()
        {
            if (!this.OrderBy(x => x.Distance).SequenceEqual(this))
            {
                this.SortBy(x => x.Distance);

                logger.LogDebug($"¨Sorted StopAreas");
            }
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
