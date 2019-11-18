using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Services;

using Microsoft.Extensions.Logging;

using Xamarin.Forms;

namespace Commuter.Models
{
    public class MainViewModel : ObservableObject, IDisposable
    {
        private Command? refreshCommand;
        private readonly IThreadDispatcher threadDispatcher;
        private readonly IAlertService alertService;
        private readonly IDataFetcher dataFetcher;
        private readonly IDepartureBoardPeriodicUpdater departureBoardPeriodicUpdater;
        private readonly ILogger<MainViewModel> logger;
        private bool isRefreshing;
        private DateTime lastFetch;
        private IDisposable? disposable;
        private CancellationTokenSource? cts;

        public MainViewModel(
            IThreadDispatcher threadDispatcher,
            IAlertService alertService,
            IDepartureBoard departureBoardViewModel,
            IDataFetcher dataFetcher,
            IDepartureBoardPeriodicUpdater departureBoardPeriodicUpdater,
            ILogger<MainViewModel> logger)
        {
            this.threadDispatcher = threadDispatcher;
            this.alertService = alertService;
            DepartureBoard = departureBoardViewModel;
            this.dataFetcher = dataFetcher;
            this.departureBoardPeriodicUpdater = departureBoardPeriodicUpdater;
            this.logger = logger;
        }

        public IDepartureBoard DepartureBoard { get; }

        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

        public async Task Initialize()
        {
            try
            {
                await CleanAndRefresh();

                disposable = departureBoardPeriodicUpdater.WhenUpdated.Subscribe(Cycle);

                await Task.Delay(10000);

                departureBoardPeriodicUpdater.Start();
            }
            catch (HttpRequestException exception)
            {
                await HandleRequestException(exception);
            }
            catch (Exception exception)
            {
                await HandleException(exception);
            }
        }

        private async Task CleanAndRefresh()
        {
            try
            {
                cts?.Cancel();
                cts = null;

                var data = new List<IStopArea>();
                await foreach (var item in dataFetcher.FetchData())
                {
                    data.Add(item);
                }
                await DepartureBoard.ClearAsync();
                await DepartureBoard.UpdateAsync(data);
            }
            catch (Exception)
            {
                throw;
            }

            lastFetch = DateTime.Now;
        }

        private async void Cycle(IEnumerable<IStopArea> data)
        {
            cts?.Dispose();
            cts = new CancellationTokenSource();

            try
            {
                await DepartureBoard.UpdateAsync(data, cts.Token);

                lastFetch = DateTime.Now;
            }
            catch (HttpRequestException exception)
            {
                await HandleRequestException(exception);
            }
            catch (Exception exception)
            {
                await HandleException(exception);
            }
        }

        public Command RefreshCommand => refreshCommand ?? (refreshCommand = new Command(async () =>
        {
            IsRefreshing = true;

            try
            {
                await CleanAndRefresh();

                lastFetch = DateTime.Now;
            }
            catch (HttpRequestException exception)
            {
                await HandleRequestException(exception);
            }
            catch (Exception exception)
            {
                await HandleException(exception);
            }

            IsRefreshing = false;
        }));

        private async Task HandleRequestException(HttpRequestException exception)
        {
            logger.LogError(exception, "Something went wrong");

            await threadDispatcher.InvokeOnMainThreadAsync(async () =>
            {
                await alertService.DisplayAlert("No connection", "Please try again later.", "OK");
            });
        }

        private async Task HandleException(Exception exception)
        {
            logger.LogError(exception, "Something went wrong");

            await threadDispatcher.InvokeOnMainThreadAsync(async () =>
            {
                await alertService.DisplayAlert("Error", "Failed to update the departure board.", "OK");
            });
        }

        public void Dispose()
        {
            cts?.Dispose();
            cts = null;
            disposable?.Dispose();
            departureBoardPeriodicUpdater.Stop();
            IsRefreshing = false;
        }
    }
}
