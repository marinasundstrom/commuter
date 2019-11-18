using System.Linq;

using Commuter.Controls;

using Xamarin.Forms;

namespace Commuter
{
    public class AlternateColorDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? EvenTemplate { get; set; }
        public DataTemplate? UnevenTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (((ItemsControl)container).ItemsSource == null)
            {
                return EvenTemplate!;
            }

            // TODO: Maybe some more error handling here
            return (((ItemsControl)container).ItemsSource.OfType<object>().ToList().IndexOf(item) % 2 == 0 ? EvenTemplate : UnevenTemplate)!;
        }
    }
}
