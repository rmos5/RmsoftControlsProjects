using System;
using System.Globalization;
using System.Windows.Data;

namespace RmsoftControls.Converters
{
    internal class FontSizeScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double? val = value as double?;
            double scale = 1.0;

            if (double.TryParse(parameter as string, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedScale))
                scale = parsedScale;
            
            if (val.HasValue)
            {
                val = scale * val.Value;
                val = Math.Floor(val.Value);
            }

            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
