using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal class FontColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Models.Departure dep)
            {
                switch (dep.LineType)
                {
                    case "Stadsbuss":
                    case "Pågatågen":
                    case "Öresundståg":
                        return "White";
                }
            }

            return "Black";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
