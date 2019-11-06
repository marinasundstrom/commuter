using System.Threading;
using System.Threading.Tasks;

namespace Commuter.Models
{
    public interface IDepartureBoard
    {
        bool IsLoadingData { get; set; }

        Task UpdateAsync(CancellationToken cancellationToken = default);

        Task ClearAsync();
    }
}
