using System.Threading.Tasks;

namespace Commuter
{
    public sealed class AlertService : IAlertService
    {
        public Task DisplayAlert(string title, string message, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task DisplayAlert(string title, string message, string accept, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
