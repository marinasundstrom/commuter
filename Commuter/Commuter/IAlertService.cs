using System.Threading.Tasks;

namespace Commuter
{
    public interface IAlertService
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task DisplayAlert(string title, string message, string accept, string cancel);
    }
}