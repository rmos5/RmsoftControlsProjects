using System.Windows;
using System.Windows.Input;

namespace RmsoftControls.Behaviors
{
    public class KeyInputCaptureSetFocusedBehavior : KeyInputCaptureBaseBehavior
    {
        protected override void OnKeyInputDetected(UIElement sender, KeyEventArgs e)
        {
            e.KeyboardDevice.Focus(AssociatedObject);
        }
    }
}
