using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Commuter.Data;
using Microsoft.Extensions.Logging;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Commuter.Services
{
    public class DepartureBoardPeriodicUpdater : IDisposable
    {
        private readonly DataFetcher dataFetcher;
        private readonly ILogger<DepartureBoardPeriodicUpdater> logger;
        private Timer? timer;
        private Subject<IEnumerable<IStopArea>> whenUpdatedSubject;

        public IObservable<IEnumerable<IStopArea>> WhenUpdated => whenUpdatedSubject;

        public DepartureBoardPeriodicUpdater(DataFetcher dataFetcher, ILogger<DepartureBoardPeriodicUpdater> logger)
        {
            this.dataFetcher = dataFetcher;
            this.logger = logger;

            whenUpdatedSubject = new Subject<IEnumerable<IStopArea>>();
        }

        public void Start()
        {
            timer = new Timer(async data => await Cycle(), null, 0, 10000);
        }

        public void Stop()
        {
            timer?.Dispose();
        }

        public async Task Cycle()
        {
            List<IStopArea> fetchedStopAreas = new List<IStopArea>();
            await foreach (var item in dataFetcher.FetchData())
            {
                fetchedStopAreas.Add(item);
            }
            whenUpdatedSubject.OnNext(fetchedStopAreas);
        }

        public void Dispose()
        {
            whenUpdatedSubject.Dispose();
            Stop();
        }
    }
}
