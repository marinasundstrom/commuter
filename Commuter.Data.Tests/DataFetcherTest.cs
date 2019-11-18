using System;
using System.Threading.Tasks;

using Commuter.Services;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Commuter.Data.Tests
{
    public class DataFetcherTest
    {
        [Fact]
        public async Task Test1()
        {
            var stopPointFetcher = new Mock<IStopAreaFetcher>();
            stopPointFetcher.SetupSequence(o => o.GetNearestStopAreasAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), default))
                            .ReturnsAsync(new StopArea[] {
                                new StopArea
                                {
                                    StopAreaId = 1
                                },
                                new StopArea
                                {
                                     StopAreaId = 2
                                },
                                new StopArea
                                {
                                     StopAreaId = 3
                                }
                            }).ReturnsAsync(new StopPoint[] {
                                new StopPoint
                                {
                                    Name = "Track 1"
                                },
                                new StopPoint
                                {
                                  Name = "Track 2"
                                }
                            }).ReturnsAsync(new StopPoint[] {
                                new StopPoint
                                {
                                    Name = "A"
                                },
                                new StopPoint
                                {
                                  Name = "B"
                                },
                                new StopPoint
                                {
                                 Name = "C"
                                },
                                 new StopPoint
                                {
                                      Name = "D"
                                },
                                new StopPoint
                                {
                                     Name = "E"
                                }
                           });
            departureFetcher.SetupSequence(o => o.GetDeparturesByStopPointAsync(It.IsAny<int>(), It.IsAny<DateTime>(), default))
                           .ReturnsAsync(new StopPoint[] {
                                new StopPoint
                                {
                                    Name = "A"
                                },
                                new StopPoint
                                {
                                      Name = "B"
                                },
                                new StopPoint
                                {
                                     Name = "C"
                                }
                           });

            var geoLocationService = new Mock<IGeoLocationService>();
            var logger = new Mock<ILogger<DataFetcher>>();

            var dataFetcher = new DataFetcher(
                stopPointFetcher.Object,
                departureFetcher.Object,
                geoLocationService.Object,
                logger.Object);

            await foreach (var foo in dataFetcher.FetchData())
            {
                Console.WriteLine(foo);
            }
        }
    }
}
