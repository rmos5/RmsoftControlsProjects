using System;
using System.Globalization;
using System.Windows.Data;

namespace RmsoftControlsTestApp.Converters
{
    public class EmptyToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value as string;
            if (val == null || val.ToString().Length == 0)
                return null;

            
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value as string;
            if (val == null || val.ToString().Length == 0)
                return null;


            return value;
        }
    }
}
