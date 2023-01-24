using System.Windows.Controls;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public class ComboBoxDropDownOnKeyboardFocusBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            AddEvents();
        }

        private void AddEvents()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
            AssociatedObject.LostKeyboardFocus += AssociatedObject_LostKeyboardFocus;
        }

        private void RemoveEvents()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
            AssociatedObject.LostKeyboardFocus -= AssociatedObject_LostKeyboardFocus;
        }

        private void AssociatedObject_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            AssociatedObject.IsDropDownOpen = false;
        }

        private void AssociatedObject_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            AssociatedObject.IsDropDownOpen = true;
        }

        protected override void OnDetaching()
        {
            RemoveEvents();
        }
    }
}
