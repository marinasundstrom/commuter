using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{
    public sealed class StopPointViewModelFactory : IStopPointViewModelFactory
    {
        public async Task<StopPointViewModel> CreateViewModelAsync(StopPoint stopPoint)
        {
            var viewModel = new StopPointViewModel();
            await UpdateViewModelAsync(stopPoint, viewModel);
            return viewModel;
        }

        public Task UpdateViewModelAsync(StopPoint stopPoint, StopPointViewModel stopPointViewModel)
        {
            stopPointViewModel.Name = stopPoint.Name;

            return Task.CompletedTask;
        }
    }
}
