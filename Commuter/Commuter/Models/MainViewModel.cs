﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
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
        private readonly IDataFetcher dataFetcher;
        private readonly IDepartureBoardPeriodicUpdater departureBoardPeriodicUpdater;
        private readonly ILogger<MainViewModel> logger;
        private bool isRefreshing;
        private DateTime lastFetch;
        private IDisposable? disposable;

        public MainViewModel(
            IDepartureBoard departureBoardViewModel,
            IDataFetcher dataFetcher,
            IDepartureBoardPeriodicUpdater departureBoardPeriodicUpdater,
            ILogger<MainViewModel> logger)
        {
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
            try
            {
                await DepartureBoard.UpdateAsync(data);

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

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Alert.Display("No connection", "Please try again later.", "OK");
            });
        }

        private async Task HandleException(Exception exception)
        {
            logger.LogError(exception, "Something went wrong");

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Alert.Display("Error", "Failed to update the departure board.", "OK");
            });
        }

        public void Dispose()
        {
            disposable?.Dispose();
            departureBoardPeriodicUpdater.Stop();
            IsRefreshing = false;
        }
    }
}
