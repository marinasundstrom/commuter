using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Models.Departure dep)
            {
                switch (dep.LineType)
                {
                    case "Regionbuss":
                        return "#ed9b02";

                    case "Skåne Expressen":
                        return "#f5a101";

                    case "Stadsbuss":
                        return "#509d2f";

                    case "Pågatågen":
                        return "#615d9e";

                    case "Öresundståg":
                        return "#90887f";
                }
            }

            return "Pink";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
