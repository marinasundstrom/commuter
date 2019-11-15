using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal class StrikeOldTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Models.Departure dep)
            {
                return dep.HasNewTime ? TextDecorations.Strikethrough : TextDecorations.None;
            }

            return TextDecorations.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
