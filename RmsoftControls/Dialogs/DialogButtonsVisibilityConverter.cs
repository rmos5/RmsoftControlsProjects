using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RmsoftControls.Dialogs
{
    public class DialogButtonsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DialogButtons prm = (DialogButtons)parameter;

            DialogButtons val = (DialogButtons)value;

            return (val & prm) > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
