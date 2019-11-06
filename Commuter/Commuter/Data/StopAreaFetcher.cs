﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Commuter;

namespace Commuter.Data
{
    public class StopAreaFetcher
    {
        private readonly OpenApiClient client;

        public StopAreaFetcher(OpenApiClient openApiClient)
        {
            client = openApiClient;
        }

        public async Task<IEnumerable<StopArea>> GetNearestStopAreasAsync(double longitude, double latitude, int radius)
        {
            var stopAreas = await client.GetNearestStopAreasAsync(longitude, latitude, radius);
            return stopAreas.Select(x => new StopArea
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