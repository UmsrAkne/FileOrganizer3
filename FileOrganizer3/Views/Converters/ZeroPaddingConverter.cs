using System;
using System.Globalization;
using System.Windows.Data;

namespace FileOrganizer3.Views.Converters
{
    public class ZeroPaddingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return value;
            }

            return ((int)value).ToString((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && int.TryParse(str, out var result))
            {
                return result;
            }

            return value;
        }
    }
}