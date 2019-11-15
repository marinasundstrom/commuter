using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Commuter.Data
{
    public interface IDepartureFetcher
    {
        Task<IEnumerable<StopPoint>> GetDeparturesByStopPointAsync(int stopArea, DateTime departureTime = default, CancellationToken cancellationToken = default);
    }
}