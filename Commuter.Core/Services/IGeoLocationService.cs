using System.Threading.Tasks;

namespace Commuter.Services
{
    public interface IGeoLocationService
    {
        Task<Location> GetLocationAsync();
    }
}
