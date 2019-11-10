using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Commuter.Services;

using Microsoft.Extensions.Logging;

using Xamarin.Essentials;

namespace Commuter.Data
{
    public class DataFetcher : IDataFetcher
    {
        private readonly IStopAreaFetcher stopAreaFetcher;
        private readonly IDepartureFetcher departureFetcher;
        private readonly IGeoLocationService geoLocationService;
        private readonly ILogger<DataFetcher> logger;

        public DataFetcher(
            IStopAreaFetcher stopAreaFetcher,
            IDepartureFetcher departureFetcher,
            IGeoLocationService geoLocationService,
            ILogger<DataFetcher> logger)
        {
            this.stopAreaFetcher = stopAreaFetcher;
            this.departureFetcher = departureFetcher;
            this.geoLocationService = geoLocationService;
            this.logger = logger;
        }

        public async IAsyncEnumerable<IStopArea> FetchData([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var fetchedStopAreas = await GetStopAreasAsync(cancellationToken);

            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            logger.LogDebug("Fetched StopAreas");

            await foreach (var fsa in FetchStopAreaStopPoints(fetchedStopAreas))
            {
                yield return fsa;
            }
        }

        private async IAsyncEnumerable<IStopArea> FetchStopAreaStopPoints(IEnumerable<Data.StopArea> fetchedStopAreas, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var fsa in fetchedStopAreas)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return new StopAreaInternal(
                    fsa.StopAreaId,
                    fsa.Name!,
                    fsa.Distance,
                    fsa.X,
                    fsa.Y,
                    await departureFetcher.GetDeparturesByStopPointAsync(fsa.StopAreaId, GetDesiredDepartureTime(), cancellationToken: cancellationToken));
            }
        }

        private async Task<Location> GetCoordinates()
        {
            //if (Utils.IsRunningInSimulator || Debugger.IsAttached)
            //{
            //    //return await Task.FromResult(new Location(55.608975, 12.9985393)); // Malmö C
            //    //return await Task.FromResult(new Location(55.605618, 13.0206813)); // Värnhemstorget
            //    //return await Task.FromResult(new Location(55.480216, 13.499789)); // Skurup
            //}
            //else
            //{
            return await geoLocationService.GetLocationAsync();
            //}
        }

        private async Task<IEnumerable<Data.StopArea>> GetStopAreasAsync(CancellationToken cancellationToken = default)
        {
            var location = await GetCoordinates();
            var radius = 400;
            return await stopAreaFetcher.GetNearestStopAreasAsync(location.Latitude, location.Longitude, radius, cancellationToken);
        }

        private static DateTime GetDesiredDepartureTime()
        {
            return DateTime.Now.Truncate(TimeSpan.FromSeconds(1));
        }
    }
}
