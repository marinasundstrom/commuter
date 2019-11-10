using System.Threading.Tasks;

using Xamarin.Essentials;

namespace Commuter.Services
{
    public interface IGeoLocationService
    {
        Task<Location> GetLocationAsync();
    }
}
