using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal class DistanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var distance = (int)value;

            if (distance == 0)
            {
                return "Here";
            }

            return distance > 1 ? $"{distance} meters" : $"1 meter";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
