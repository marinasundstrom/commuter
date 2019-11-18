using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{

    public sealed class DeviationViewModelFactory : IDeviationViewModelFactory
    {
        public async Task<DeviationViewModel> CreateViewModelAsync(Deviation deviation)
        {
            var viewModel = new DeviationViewModel();
            await UpdateViewModelAsync(deviation, viewModel);
            return viewModel;
        }

        public Task UpdateViewModelAsync(Deviation deviation, DeviationViewModel deviationViewModel)
        {
            deviationViewModel.Header = deviation.Header;
            deviationViewModel.ShortText = deviation.ShortText;
            deviationViewModel.Importance = deviation.Importance;
            deviationViewModel.Urgency = deviation.Urgency;
            deviationViewModel.Influence = deviation.Influence;

            return Task.CompletedTask;
        }
    }
}
