using System;
using System.Globalization;

namespace Commuter.Converters
{
    internal class TimeConverter : ValueConverter<DateTime, string>
    {
        public override string Convert(DateTime time, Type targetType, CultureInfo culture)
        {
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
