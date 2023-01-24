using System.Windows;

namespace RmsoftControls.InputCaptureControls
{
    public class KeyInputCaptureEventArgs : RoutedEventArgs
    {
        public KeyInputCaptureItem KeyInputCaptureItem { get; }

        public string CapturedText { get; }

        public KeyInputCaptureEventArgs(RoutedEvent routedEvent, KeyInputCaptureItem keyInputCaptureItem, string capturedText = "")
            : base(routedEvent)
        {
            KeyInputCaptureItem = keyInputCaptureItem;
            CapturedText = capturedText;
        }

        public override string ToString()
        {
            return $"{KeyInputCaptureItem};{CapturedText}";
        }
    }
}
