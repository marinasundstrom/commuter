using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Services;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Commuter.Tests
{
    public class DataFetcherTest
    {
        [Fact]
        public async Task Test1()
        {
            var stopAreaFetherMock = new Mock<IStopAreaFetcher>();
            stopAreaFetherMock
                .Setup(x => x.GetNearestStopAreasAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), default))
                .ReturnsAsync(new[] {
                    new StopArea
                    {
                        StopAreaId = 1,
                         Name = "Centralen",
                          Distance = 234,
                           X = 4,
                           Y = 2
                    }
                    });

            var departureFetcherMock = new Mock<IDepartureFetcher>();
            departureFetcherMock.Setup(x => x.GetDeparturesByStopPointAsync(It.IsAny<int>(), DateTime.Now, default))
                .ReturnsAsync(new[] {
                    new StopPoint
                    {
                        Name = "B",
                        Departures = new List<Departure>
                        {
                            new Departure
                            {
                                RunNo = 345,
                                 Name = "42 Foobar",
                                  Towards = "Boo",
                                   DepartureTime = DateTime.Now,
                                    No = 42,
                                     LineType = "Bus"
                            }
                        }
                    }
                    });

            var geoLocationServiceMock = new Mock<IGeoLocationService>();
            geoLocationServiceMock.Setup(x => x.GetLocationAsync())
                .ReturnsAsync(new Location(343, 878));

            var loggerMock = new Mock<ILogger<DataFetcher>>();

            var dataFetcher = new DataFetcher(stopAreaFetherMock.Object, departureFetcherMock.Object, geoLocationServiceMock.Object, loggerMock.Object);
            await foreach (var data in dataFetcher.FetchData())
            {
                Console.WriteLine(data);
            }
        }
    }
}
