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
        public async Task Test1()
        {
            var departureBoardMock = new Mock<IDepartureBoard>();
            var dataFetcherMock = new Mock<DataFetcher>();
            var departureBoardPeriodicUpdaterMock = new Mock<DepartureBoardPeriodicUpdater>();
            var loggerMock = new Mock<ILogger<MainViewModel>>();

            var mainViewModel = new MainViewModel(
                departureBoardMock.Object,
                dataFetcherMock.Object,
                departureBoardPeriodicUpdaterMock.Object,
                loggerMock.Object);

            await mainViewModel.Initialize();

            await Task.Delay(5000);

            mainViewModel.Dispose();

            await Task.Delay(5000);

            await mainViewModel.Initialize();
        }
    }
}
