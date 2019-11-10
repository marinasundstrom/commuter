using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commuter.Data;

namespace Commuter.Services
{
    public interface IDepartureBoardPeriodicUpdater
    {
        IObservable<IEnumerable<IStopArea>> WhenUpdated { get; }

        Task Cycle();
        void Dispose();
        void Start();
        void Stop();
    }
}