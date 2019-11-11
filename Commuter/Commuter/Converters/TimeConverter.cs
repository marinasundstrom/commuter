using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (DateTime)value;

            if (((int)(time - DateTime.Now).TotalMinutes) <= 0)
            {
                return "Now";
            }

            if ((time - DateTime.Now) > TimeSpan.FromMinutes(30))
            {
                return $"{time.ToString("HH:mm")}";
            }

            return $"{(time - DateTime.Now).Minutes} min";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
