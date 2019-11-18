using System;
using System.Threading.Tasks;

namespace Commuter.Services
{
    public interface IThreadDispatcher
    {
        Task InvokeOnMainThreadAsync(Func<Task> func);

        Task InvokeOnMainThreadAsync(Action action);
    }
}
