using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Xamarin.Forms;

namespace Commuter.Models
{
    public class MainViewModel : ObservableObject, IDisposable
    {
        private Command? refreshCommand;
        private readonly ILogger<MainViewModel> logger;
        private CancellationTokenSource? cancellationTokenSource;
        private Timer? timer = null;
        private bool isRefreshing;
        private DateTime lastFetch;

        public MainViewModel(IDepartureBoard departureBoardViewModel, ILogger<MainViewModel> logger)
        {
            DepartureBoard = departureBoardViewModel;
            this.logger = logger;
        }

        public IDepartureBoard DepartureBoard { get; }

        public bool IsRefreshing { get => isRefreshing; set => SetProperty(ref isRefreshing, value); }

        public async Task Initialize()
        {
            try
            {
                await DepartureBoard.UpdateAsync();

                lastFetch = DateTime.Now;

                if (timer == null)
                {
                    cancellationTokenSource = new CancellationTokenSource();
                    timer = new Timer(async data => await Cycle(cancellationTokenSource.Token), null, 10000, 10000);
                }
            }
            catch (Exception exception)
            {
                await HandleException(exception);
            }
        }

        private async Task Cycle(CancellationToken cancellationToken)
        {
            try
            {
                await DepartureBoard.UpdateAsync(cancellationToken);
                lastFetch = DateTime.Now;
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
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                }
                await DepartureBoard.ClearAsync();

                logger.LogInformation("Cleared timetable");

                await Task.Delay(50);

                await DepartureBoard.UpdateAsync();

                lastFetch = DateTime.Now;
            }
            catch (Exception exception)
            {
                await HandleException(exception);
            }

            IsRefreshing = false;
        }));

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
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
            if (timer != null)
            {
                timer.Dispose();
            }
            IsRefreshing = false;
        }
    }
}
