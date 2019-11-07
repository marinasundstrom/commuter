using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commuter.Helpers;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;

namespace Commuter.Data
{
    public class DataFetcher
    {
        private readonly StopAreaFetcher stopAreaFetcher;
        private readonly DepartureFetcher departureFetcher;
        private readonly ILogger<DataFetcher> logger;

        public DataFetcher(StopAreaFetcher stopAreaFetcher, DepartureFetcher departureFetcher, ILogger<DataFetcher> logger)
        {
            this.stopAreaFetcher = stopAreaFetcher;
            this.departureFetcher = departureFetcher;
            this.logger = logger;
        }

        public async Task<IEnumerable<(int StopAreaId, string? Name, int Distance, float X, float Y, IEnumerable<Data.StopPoint> StopPoints)>> FetchData(CancellationToken cancellationToken = default)
        {
            var fetchedStopAreas = await GetStopAreasAsync(cancellationToken);

            logger.LogDebug("Fetched StopAreas");

            return await FetchStopAreaStopPoints(fetchedStopAreas);
        }

        private async Task<IEnumerable<(int StopAreaId, string? Name, int Distance, float X, float Y, IEnumerable<Data.StopPoint> StopPoints)>> FetchStopAreaStopPoints(IEnumerable<Data.StopArea> fetchedStopAreas, CancellationToken cancellationToken = default)
        {
            var results = new List<(int StopAreaId, string? Name, int Distance, float X, float Y, IEnumerable<StopPoint> StopPoints)>();

            foreach(var fsa in fetchedStopAreas)
            {
                results.Add((fsa.StopAreaId, fsa.Name, Distance: (int)fsa.Distance, fsa.X, fsa.Y, StopPoints: await departureFetcher.GetDeparturesByStopPointAsync(fsa.StopAreaId, GetDesiredDepartureTime(), cancellationToken: cancellationToken)));
            }

            return results.AsEnumerable();
        }

        private static async Task<Location> GetCoordinates()
        {
            if (Utils.IsRunningInSimulator || Debugger.IsAttached)
            {
                //return await Task.FromResult(new Location(55.608975, 12.9985393)); // Malmö C
                return await Task.FromResult(new Location(55.605618, 13.0206813)); // Värnhemstorget
            }
            else
            {
                return await Xamarin.Essentials.Geolocation.GetLocationAsync();
            }
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
