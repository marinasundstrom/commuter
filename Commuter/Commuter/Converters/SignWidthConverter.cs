using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal class SignWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value.ToString();

            if (text.Length <= 3)
            {
                return 30;
            }
            if (text.Length == 4)
            {
                return 40;
            }
            else
            {
                return null!;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
