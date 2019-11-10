using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Commuter.Data;

using Microsoft.Extensions.Logging;

namespace Commuter.Services
{
    public class DepartureBoardPeriodicUpdater : IDisposable, IDepartureBoardPeriodicUpdater
    {
        private readonly IDataFetcher dataFetcher;
        private readonly ILogger<DepartureBoardPeriodicUpdater> logger;
        private Timer? timer;
        private readonly Subject<IEnumerable<IStopArea>> whenUpdatedSubject;

        public IObservable<IEnumerable<IStopArea>> WhenUpdated => whenUpdatedSubject;

        public DepartureBoardPeriodicUpdater(IDataFetcher dataFetcher, ILogger<DepartureBoardPeriodicUpdater> logger)
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
            var fetchedStopAreas = new List<IStopArea>();
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
