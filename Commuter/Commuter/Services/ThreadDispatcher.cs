using System;
using System.Threading.Tasks;

namespace Commuter.Services
{
    internal sealed class ThreadDispatcher : IThreadDispatcher
    {
        public Task InvokeOnMainThreadAsync(Func<Task> func)
        {
            return Xamarin.Forms.Device.InvokeOnMainThreadAsync(func);
        }

        public Task InvokeOnMainThreadAsync(Action action)
        {
            return Xamarin.Forms.Device.InvokeOnMainThreadAsync(action);
        }
    }
}
