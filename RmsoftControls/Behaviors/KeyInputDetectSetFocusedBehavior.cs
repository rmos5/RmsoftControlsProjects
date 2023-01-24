using System.Windows;
using System.Windows.Input;

namespace RmsoftControls.Behaviors
{
    public class KeyInputDetectSetFocusedBehavior : KeyInputDetectBaseBehavior
    {
        protected override void OnKeyInputDetected(UIElement sender, KeyEventArgs e)
        {
            e.KeyboardDevice.Focus(AssociatedObject);
        }
    }
}
