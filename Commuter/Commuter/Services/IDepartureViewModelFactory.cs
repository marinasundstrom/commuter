using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{
    public interface IDepartureViewModelFactory
    {
        Task<DepartureViewModel> CreateViewModelAsync(Departure departure);
        Task UpdateViewModelAsync(Departure departure, DepartureViewModel departureViewModel);
    }
}
