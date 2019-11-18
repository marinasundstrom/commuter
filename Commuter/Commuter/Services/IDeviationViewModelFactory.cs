using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{
    public interface IDeviationViewModelFactory
    {
        Task<DeviationViewModel> CreateViewModelAsync(Deviation deviation);
        Task UpdateViewModelAsync(Deviation deviation, DeviationViewModel deviationViewModel);
    }
}
