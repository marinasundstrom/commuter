using System.Threading.Tasks;

using Xamarin.Essentials;

namespace Commuter.Services
{
    internal class GeoLocationService : IGeoLocationService
    {
        public Task<Location> GetLocationAsync()
        {
            return Geolocation.GetLocationAsync();
        }
    }
}
