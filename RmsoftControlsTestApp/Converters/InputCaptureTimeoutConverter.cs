using System;
using System.Globalization;
using System.Windows.Data;

namespace RmsoftControlsTestApp.Converters
{
    public class InputCaptureTimeoutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? val = value as int?;
            if (val.HasValue)
                return val.Value;
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}
