using System.ComponentModel;

using Commuter.Models;

using Xamarin.Forms;

namespace Commuter
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            await ((MainViewModel)BindingContext).Initialize();
        }

        protected override void OnDisappearing()
        {
            ((MainViewModel)BindingContext).Dispose();
        }
    }
}
