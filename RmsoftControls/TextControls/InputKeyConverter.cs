using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace RmsoftControls.TextControls
{
    /// <summary>
    /// <see cref="Key"/> value converter.
    /// </summary>
    public class InputKeyConverter : IValueConverter
    {
        public readonly Key DefaultKey = Key.Return;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Key result;
            if (Enum.TryParse(value?.ToString(), out result))
                return result.ToString();

            return DefaultKey.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Key result;
            if (Enum.TryParse(value?.ToString(), out result))
                return result;

            return DefaultKey;
        }
    }
}
