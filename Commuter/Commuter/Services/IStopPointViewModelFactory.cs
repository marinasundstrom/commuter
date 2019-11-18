using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{
    public interface IStopPointViewModelFactory
    {
        Task<StopPointViewModel> CreateViewModelAsync(StopPoint stopPoint);
        Task UpdateViewModelAsync(StopPoint stopPoint, StopPointViewModel stopPointViewModel);
    }
}
