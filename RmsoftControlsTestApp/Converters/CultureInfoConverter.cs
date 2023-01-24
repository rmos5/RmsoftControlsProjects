using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace RmsoftControlsTestApp.Converters
{
    public class CultureInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo val = value as CultureInfo ?? culture;

            return XmlLanguage.GetLanguage(val.TextInfo.CultureName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = value as string;
            if (string.IsNullOrWhiteSpace(val))
                val = culture.TextInfo.CultureName;

            return CultureInfo.GetCultureInfo(val);
        }
    }
}
