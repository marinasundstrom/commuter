using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commuter.Data
{
    internal class StopAreaFetcher : IStopAreaFetcher
    {
        private readonly OpenApiClient client;

        public StopAreaFetcher(OpenApiClient openApiClient)
        {
            client = openApiClient;
        }

        public async Task<IEnumerable<StopArea>> GetNearestStopAreasAsync(double longitude, double latitude, int radius, CancellationToken cancellationToken = default)
        {
            var stopAreas = await client.GetNearestStopAreasAsync(longitude, latitude, radius, cancellationToken);
            return stopAreas?.Select(x => new StopArea
            {
                StopAreaId = (int)x.Id,
                Name = x.Name,
                X = x.X,
                Y = x.Y,
                Distance = x.Distance
            }).ToArray();
        }
    }
}
