using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Commuter.Data;
using Commuter.Models;
using Commuter.Services;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

namespace Commuter.Tests
{
    public class DepartureBoardViewModelTest
    {
        [Fact]
        public async Task PopulateFirstTime()
        {
            var loggerMock = new Mock<ILogger<DepartureBoardViewModel>>();

            var threadDispatcherMock = new Mock<IThreadDispatcher>();
            threadDispatcherMock
                .Setup(x => x.InvokeOnMainThreadAsync(It.IsAny<Action>()))
                .Returns<Action>(arg => Task.Run(arg));

            threadDispatcherMock
              .Setup(x => x.InvokeOnMainThreadAsync(It.IsAny<Func<Task>>()))
              .Returns<Func<Task>>(arg => arg());

            var stopAreaViewModelFactoryMock = new Mock<IStopAreaViewModelFactory>();
            stopAreaViewModelFactoryMock
                .Setup(x => x.CreateViewModelAsync(It.IsAny<IStopArea>()))
               .Returns<IStopArea>(x => Task.FromResult(new StopAreaViewModel() { StopAreaId = x.StopAreaId }));

            var stopPointViewModelFactoryMock = new Mock<IStopPointViewModelFactory>();
            stopPointViewModelFactoryMock
               .Setup(x => x.CreateViewModelAsync(It.IsAny<StopPoint>()))
               .Returns<StopPoint>(x => Task.FromResult(new StopPointViewModel() { Name = x.Name }));

            var departureViewModelFactoryMock = new Mock<IDepartureViewModelFactory>();
            departureViewModelFactoryMock
                .Setup(x => x.CreateViewModelAsync(It.IsAny<Departure>()))
                .Returns<Departure>(x => Task.FromResult(new DepartureViewModel() { RunNo = x.RunNo, Time = x.DepartureTime }));

            var deviationViewModelFactoryMock = new Mock<IDeviationViewModelFactory>();
            deviationViewModelFactoryMock
                .Setup(x => x.CreateViewModelAsync(It.IsAny<Deviation>()))
                .Returns<Deviation>(x => Task.FromResult(new DeviationViewModel() { ShortText = x.ShortText }));

            var viewModel = new DepartureBoardViewModel(
                loggerMock.Object,
                threadDispatcherMock.Object,
                stopAreaViewModelFactoryMock.Object,
                stopPointViewModelFactoryMock.Object,
                departureViewModelFactoryMock.Object,
                deviationViewModelFactoryMock.Object);

            var data = new IStopArea[]
            {
                new StopAreaInternal
                {
                    StopAreaId = 1,
                    StopPoints = new StopPoint[]
                    {
                        new StopPoint {
                            Name = "A",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 11,
                                    DepartureTime = DateTime.Now
                                },
                                new Departure {
                                    RunNo = 12,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 13,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 14,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                        new StopPoint {
                            Name = "B",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 21,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 22,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 23,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 24,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                    }
                },
                new StopAreaInternal
                {
                    StopAreaId = 2,
                    StopPoints = new StopPoint[]
                    {
                        new StopPoint {
                            Name = "A",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 31,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 32,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 33,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 34,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                        new StopPoint {
                            Name = "B",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 41,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 42,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 43,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 44,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                        new StopPoint {
                            Name = "Track 1",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 51,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 52,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 53,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 54,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                        new StopPoint {
                            Name = "Track 2",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 61,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 62,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 63,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 64,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                    }
                },
                new StopAreaInternal
                {
                    StopAreaId = 3,
                    StopPoints = new StopPoint[]
                    {
                        new StopPoint {
                            Name = "A",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 71,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 72,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 73,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 74,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                        new StopPoint {
                            Name = "B",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 81,
                                    DepartureTime = DateTime.Now
                                },
                                new Departure {
                                    RunNo = 82,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 83,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 84,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                }
                            }
                        },
                    }
                }
            };

            await viewModel.UpdateAsync(data);

            stopAreaViewModelFactoryMock
                .Verify(x => x.CreateViewModelAsync(It.IsAny<IStopArea>()), Times.Exactly(3));

            stopPointViewModelFactoryMock
                .Verify(x => x.CreateViewModelAsync(It.IsAny<StopPoint>()), Times.Exactly(8));

            departureViewModelFactoryMock
                .Verify(x => x.CreateViewModelAsync(It.IsAny<Departure>()), Times.Exactly(32));

            Assert.True(viewModel.Count == 3);

        }

        [Fact]
        public async Task UpdateViewModels()
        {
            var loggerMock = new Mock<ILogger<DepartureBoardViewModel>>();

            var threadDispatcherMock = new Mock<IThreadDispatcher>();
            threadDispatcherMock
                .Setup(x => x.InvokeOnMainThreadAsync(It.IsAny<Action>()))
                .Returns<Action>(arg => Task.Run(arg));

            threadDispatcherMock
              .Setup(x => x.InvokeOnMainThreadAsync(It.IsAny<Func<Task>>()))
              .Returns<Func<Task>>(arg => arg());

            var stopAreaViewModelFactoryMock = new Mock<IStopAreaViewModelFactory>();
            stopAreaViewModelFactoryMock
                .Setup(x => x.CreateViewModelAsync(It.IsAny<IStopArea>()))
               .Returns<IStopArea>(x => Task.FromResult(new StopAreaViewModel() { StopAreaId = x.StopAreaId }));

            var stopPointViewModelFactoryMock = new Mock<IStopPointViewModelFactory>();
            stopPointViewModelFactoryMock
               .Setup(x => x.CreateViewModelAsync(It.IsAny<StopPoint>()))
               .Returns<StopPoint>(x => Task.FromResult(new StopPointViewModel() { Name = x.Name }));

            var departureViewModelFactoryMock = new Mock<IDepartureViewModelFactory>();
            departureViewModelFactoryMock
                .Setup(x => x.CreateViewModelAsync(It.IsAny<Departure>()))
                .Returns<Departure>(x => Task.FromResult(new DepartureViewModel() { RunNo = x.RunNo, Time = x.DepartureTime }));

            var deviationViewModelFactoryMock = new Mock<IDeviationViewModelFactory>();
            deviationViewModelFactoryMock
                .Setup(x => x.CreateViewModelAsync(It.IsAny<Deviation>()))
                .Returns<Deviation>(x => Task.FromResult(new DeviationViewModel() { ShortText = x.ShortText }));

            var viewModel = new DepartureBoardViewModel(
                loggerMock.Object,
                threadDispatcherMock.Object,
                stopAreaViewModelFactoryMock.Object,
                stopPointViewModelFactoryMock.Object,
                departureViewModelFactoryMock.Object,
                deviationViewModelFactoryMock.Object);

            var data = new IStopArea[]
            {
                new StopAreaInternal
                {
                    StopAreaId = 1,
                    StopPoints = new StopPoint[]
                    {
                        new StopPoint {
                            Name = "A",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 11,
                                    DepartureTime = DateTime.Now
                                },
                                new Departure {
                                    RunNo = 12,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 13,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 14,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                        new StopPoint {
                            Name = "B",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 21,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 22,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 23,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 24,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                    }
                },
                new StopAreaInternal
                {
                    StopAreaId = 2,
                    StopPoints = new StopPoint[]
                    {
                        new StopPoint {
                            Name = "A",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 31,
                                    DepartureTime = DateTime.Now.AddMinutes(1)
                                },
                                new Departure {
                                    RunNo = 32,
                                    DepartureTime = DateTime.Now.AddMinutes(2)
                                },
                                new Departure {
                                    RunNo = 33,
                                    DepartureTime = DateTime.Now.AddMinutes(3)
                                },
                                new Departure {
                                    RunNo = 34,
                                    DepartureTime = DateTime.Now.AddMinutes(4)
                                }
                            }
                        },
                    }
                }
            };

            await viewModel.UpdateAsync(data);

            stopAreaViewModelFactoryMock
                .Verify(x => x.CreateViewModelAsync(It.IsAny<IStopArea>()), Times.Exactly(2));

            stopPointViewModelFactoryMock
                .Verify(x => x.CreateViewModelAsync(It.IsAny<StopPoint>()), Times.Exactly(3));

            departureViewModelFactoryMock
                .Verify(x => x.CreateViewModelAsync(It.IsAny<Departure>()), Times.Exactly(12));

            var data2 = new IStopArea[]
            {
                new StopAreaInternal
                {
                    StopAreaId = 2,
                    StopPoints = new StopPoint[]
                    {
                        new StopPoint {
                            Name = "A",
                            Departures = new List<Departure>
                            {
                                new Departure {
                                    RunNo = 31,
                                    DepartureTime = DateTime.Now.AddMinutes(10)
                                },
                                new Departure {
                                    RunNo = 32,
                                    DepartureTime = DateTime.Now.AddMinutes(20)
                                },
                                new Departure {
                                    RunNo = 33,
                                    DepartureTime = DateTime.Now.AddMinutes(30)
                                },
                                new Departure {
                                    RunNo = 34,
                                    DepartureTime = DateTime.Now.AddMinutes(40)
                                }
                            }
                        },
                    }
                },
            };

            await viewModel.UpdateAsync(data2);

            departureViewModelFactoryMock
                .Verify(x => x.UpdateViewModelAsync(It.IsAny<Departure>(), It.IsAny<DepartureViewModel>()), Times.Exactly(4));

            Assert.True(viewModel.Count == 1);
        }
    }
}
