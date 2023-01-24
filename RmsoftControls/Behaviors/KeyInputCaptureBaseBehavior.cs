using RmsoftControls.InputCaptureControls;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RmsoftControls.Behaviors
{
    public abstract class KeyInputCaptureBaseBehavior : KeyInputDetectBaseBehavior
    {
        public KeyInputCaptureCollection InputCaptureItems
        {
            get { return (KeyInputCaptureCollection)GetValue(InputCaptureItemsProperty); }
            set { SetValue(InputCaptureItemsProperty, value); }
        }

        public static readonly DependencyProperty InputCaptureItemsProperty =
            DependencyProperty.Register("InputCaptureItems", typeof(KeyInputCaptureCollection), typeof(KeyInputCaptureBaseBehavior), new PropertyMetadata(null));

        protected override bool IsKeyInputDetected(KeyEventArgs e)
        {
            return base.IsKeyInputDetected(e)
                || InputCaptureItems?.Any(o => e.Key == o.StartKey && e.KeyboardDevice.Modifiers == o.StartKeyModifiers) == true;
        }
    }
}
