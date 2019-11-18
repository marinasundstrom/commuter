using System;
using System.Globalization;

using Xamarin.Forms;

namespace Commuter.Converters
{
    internal abstract class ValueConverter<TValue, TResult> : IValueConverter
    {
        public abstract TResult Convert(TValue value, Type targetType, CultureInfo culture);

        public virtual TResult ConvertBack(TValue value, Type targetType, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TValue)value, targetType, culture)!;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TValue)value, targetType, culture)!;
        }
    }

    internal abstract class ValueConverter<TValue, TParameter, TResult> : IValueConverter
    {
        public abstract TResult Convert(TValue value, Type targetType, TParameter parameter, CultureInfo culture);

        public virtual TResult ConvertBack(TValue value, Type targetType, TParameter parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TValue)value, targetType, (TParameter)parameter, culture)!;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TValue)value, targetType, (TParameter)parameter, culture)!;
        }
    }
}
