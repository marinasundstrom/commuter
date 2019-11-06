using System.Threading.Tasks;

namespace Commuter
{
    public static class Alert
    {
        public static Task Display(string title, string message, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public static Task DisplayAlert(string title, string message, string accept, string cancel)
        {
            return App.Current.MainPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}
