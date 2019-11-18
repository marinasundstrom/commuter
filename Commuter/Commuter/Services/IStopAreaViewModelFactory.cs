using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{
    public interface IStopAreaViewModelFactory
    {
        Task<StopAreaViewModel> CreateViewModelAsync(IStopArea stopArea);
        Task UpdateViewModelAsync(IStopArea stopArea, StopAreaViewModel stopAreaViewModel);
    }
}
