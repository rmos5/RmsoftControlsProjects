using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public class SelectAllTextOnFocusBehavior : Behavior<TextBoxBase>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject == null)
                return;
            AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
        }

        private void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxBase obj = (TextBoxBase)sender;
            obj.SelectAll();
        }

        private void AssociatedObject_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBoxBase obj = (TextBoxBase)sender;
            obj.SelectAll();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
        }
    }
}
