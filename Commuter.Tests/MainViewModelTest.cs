using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;
using Commuter.Services;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Commuter.Tests
{
    public class MainViewModelTest
    {
        [Fact]
        public async Task IsInitializedCorrectly()
        {
            var threadDispatcherMock = new Mock<IThreadDispatcher>();
            threadDispatcherMock
                .Setup(x => x.InvokeOnMainThreadAsync(It.IsAny<Action>()))
                .Returns<Action>(arg => Task.Run(arg));

            threadDispatcherMock
              .Setup(x => x.InvokeOnMainThreadAsync(It.IsAny<Func<Task>>()))
              .Returns<Func<Task>>(arg => arg());

            var alertServiceMock = new Mock<IAlertService>();

            var departureBoardMock = new Mock<IDepartureBoard>();

            var dataFetcherMock = new Mock<IDataFetcher>();
            dataFetcherMock
                .Setup(x => x.FetchData(default))
                .Returns(EmptyAsyncEnumerable());

            var departureBoardPeriodicUpdaterMock = new Mock<IDepartureBoardPeriodicUpdater>();
            departureBoardPeriodicUpdaterMock
                .SetupGet(x => x.WhenUpdated)
                .Returns(new Subject<IEnumerable<IStopArea>>());

            var loggerMock = new Mock<ILogger<MainViewModel>>();

            var mainViewModel = new MainViewModel(
                threadDispatcherMock.Object,
                alertServiceMock.Object,
                departureBoardMock.Object,
                dataFetcherMock.Object,
                departureBoardPeriodicUpdaterMock.Object,
                loggerMock.Object);

            await mainViewModel.Initialize();

            dataFetcherMock.Verify(x => x.FetchData(default), Times.Exactly(1));
            departureBoardMock.Verify(x => x.ClearAsync(default), Times.Exactly(1));
            departureBoardMock.Verify(x => x.UpdateAsync(It.IsAny<IEnumerable<IStopArea>>(), default), Times.Exactly(1));

            departureBoardPeriodicUpdaterMock.Verify(x => x.Start(), Times.Exactly(1));

            mainViewModel.Dispose();

            departureBoardPeriodicUpdaterMock.Verify(x => x.Stop(), Times.Exactly(1));
        }

        private async IAsyncEnumerable<IStopArea> EmptyAsyncEnumerable()
        {
            await Task.Delay(50);

            yield break;
        }
    }
}
