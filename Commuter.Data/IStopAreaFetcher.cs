using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Commuter.Data
{
    public interface IStopAreaFetcher
    {
        Task<IEnumerable<StopArea>> GetNearestStopAreasAsync(double longitude, double latitude, int radius, CancellationToken cancellationToken = default);
    }
}