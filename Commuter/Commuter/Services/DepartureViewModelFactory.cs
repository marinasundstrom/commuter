using System;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;

namespace Commuter.Services
{
    public sealed class DepartureViewModelFactory : IDepartureViewModelFactory
    {
        public async Task<DepartureViewModel> CreateViewModelAsync(Departure departure)
        {
            var viewModel = new DepartureViewModel();
            await UpdateViewModelAsync(departure, viewModel);
            return viewModel;
        }

        public Task UpdateViewModelAsync(Departure departure, DepartureViewModel departureViewModel)
        {
            departureViewModel.RunNo = departure.RunNo;
            departureViewModel.LineType = departure.LineType;
            departureViewModel.No = departure.No;
            departureViewModel.Name = departure.Name;
            departureViewModel.Towards = departure.Towards;
            departureViewModel.Time = departure.DepartureTime;

            UpdateTime(departure, departureViewModel);

            return Task.CompletedTask;
        }

        private static void UpdateTime(Departure departure, DepartureViewModel departureViewModel)
        {
            if (departure.DepartureTimeDeviation != null)
            {
                var newTime = departure.DepartureTime.AddMinutes(departure.DepartureTimeDeviation ?? 0);

                if (newTime.Truncate(TimeSpan.FromMinutes(1)) > departure.DepartureTime.Truncate(TimeSpan.FromMinutes(1)))
                {
                    departureViewModel.NewTime = newTime;
                }
                else
                {
                    departureViewModel.NewTime = null;
                }
            }
            else
            {
                departureViewModel.NewTime = null;
            }
        }
    }
}
