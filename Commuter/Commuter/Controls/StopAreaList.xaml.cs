
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Commuter.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StopAreaList : ListView
    {
        public StopAreaList()
        {
            InitializeComponent();
        }
    }
}
