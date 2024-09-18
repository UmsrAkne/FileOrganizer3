using System;
using System.Windows.Data;

namespace FileOrganizer3.Views.Converters
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && int.TryParse(value.ToString(), out var result))
            {
                return result;
            }

            return 5;  // 変換できない場合はデフォルト値を返す
        }
    }
}