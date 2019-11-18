using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{
    public sealed class StopAreaViewModelFactory : IStopAreaViewModelFactory
    {
        public async Task<StopAreaViewModel> CreateViewModelAsync(IStopArea stopArea)
        {
            var viewModel = new StopAreaViewModel();
            await UpdateViewModelAsync(stopArea, viewModel);
            return viewModel;
        }

        public Task UpdateViewModelAsync(IStopArea stopArea, StopAreaViewModel stopAreaViewModel)
        {
            stopAreaViewModel.StopAreaId = stopArea.StopAreaId;
            stopAreaViewModel.Name = stopArea.Name;
            stopAreaViewModel.X = stopArea.X;
            stopAreaViewModel.Y = stopArea.Y;
            stopAreaViewModel.Distance = stopArea.Distance;

            return Task.CompletedTask;
        }
    }
}
