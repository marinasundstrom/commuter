using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skanetrafiken.API.DepartureArrival;
using Skanetrafiken.API.NearestStopArea;

namespace Commuter
{
    public interface IOpenApiClient
    {
        Task<IEnumerable<GetDepartureArrivalResponseGetDepartureArrivalResultLine>> GetGetDepartureArrivalsAsync(int stopAreaId, DateTime departureDateTime, CancellationToken cancellationToken = default);
        Task<IEnumerable<GetNearestStopAreaResponseGetNearestStopAreaResultNearestStopArea>> GetNearestStopAreasAsync(double x, double y, int radius, CancellationToken cancellationToken = default);
    }
}