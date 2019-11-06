
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Xamarin.Forms;

namespace Commuter
{
    public partial class App : Application
    {
        public App(IHost host)
        {
            InitializeComponent();

            MainPage = host.Services.GetService<MainPage>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

