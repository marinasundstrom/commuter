using System.Collections.Generic;
using System.Threading;

namespace Commuter.Data
{
    public interface IDataFetcher
    {
        IAsyncEnumerable<IStopArea> FetchData(CancellationToken cancellationToken = default);
    }
}
