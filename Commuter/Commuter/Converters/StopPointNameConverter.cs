using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal class StopPointNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string name)
            {
                if (char.IsDigit(name[0]))
                {
                    return $"Track {name}";
                }

                return $"Position {name}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
