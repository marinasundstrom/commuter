using System.Threading.Tasks;

using Xamarin.Essentials;

namespace Commuter.Services
{
    internal class GeoLocationService : IGeoLocationService
    {
        public async Task<Location> GetLocationAsync()
        {
            var location = await Geolocation.GetLocationAsync();
            return new Location(location.Latitude, location.Longitude, location.Altitude);
        }
    }
}
