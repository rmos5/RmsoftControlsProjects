using System.Windows;

namespace RmsoftControls.InputCaptureControls
{
    public static partial class KeyInputCapture
    {
        public static bool GetIsCaptureEventScope(FrameworkElement obj)
        {
            return (bool)obj.GetValue(IsCaptureEventScopeProperty);
        }

        public static void SetIsCaptureEventScope(FrameworkElement obj, bool value)
        {
            obj.SetValue(IsCaptureEventScopeProperty, value);
        }

        public static readonly DependencyProperty IsCaptureEventScopeProperty =
            DependencyProperty.RegisterAttached("IsCaptureEventScope", typeof(bool), typeof(KeyInputCapture), new PropertyMetadata(false));

        public static bool GetIsInputCaptureStarted(FrameworkElement obj)
        {
            return (bool)obj.GetValue(IsInputCaptureStartedProperty);
        }

        public static readonly DependencyProperty IsInputCaptureStartedProperty =
            DependencyProperty.RegisterAttached("IsInputCaptureStarted", typeof(bool), typeof(KeyInputCapture), new PropertyMetadata(false));

        public static bool GetIsInputCaptureCompleted(FrameworkElement obj)
        {
            return (bool)obj.GetValue(IsInputCaptureCompletedProperty);
        }

        public static readonly DependencyProperty IsInputCaptureCompletedProperty =
            DependencyProperty.RegisterAttached("IsInputCaptureCompleted", typeof(bool), typeof(KeyInputCapture), new PropertyMetadata(false));

        public static int GetInputCaptureStartedCount(FrameworkElement obj)
        {
            return (int)obj.GetValue(InputCaptureStartedCountProperty);
        }

        public static readonly DependencyProperty InputCaptureStartedCountProperty =
            DependencyProperty.RegisterAttached("InputCaptureStartedCount", typeof(int), typeof(KeyInputCapture), new PropertyMetadata(0));

        public static int GetInputCaptureCompletedCount(FrameworkElement obj)
        {
            return (int)obj.GetValue(InputCaptureCompletedCountProperty);
        }

        public static readonly DependencyProperty InputCaptureCompletedCountProperty =
            DependencyProperty.RegisterAttached("InputCaptureCompletedCount", typeof(int), typeof(KeyInputCapture), new PropertyMetadata(0));

        public static readonly RoutedEvent InputCaptureStartedEvent = EventManager.RegisterRoutedEvent("InputCaptureStarted", RoutingStrategy.Direct, typeof(InputCaptureEventHandler), typeof(KeyInputCapture));

        public static void AddInputCaptureStartedHandler(FrameworkElement item, InputCaptureEventHandler handler)
        {
            if (item != null)
            {
                item.AddHandler(InputCaptureStartedEvent, handler);
            }
        }

        public static void RemoveInputCaptureStartedHandler(FrameworkElement item, InputCaptureEventHandler handler)
        {
            if (item != null)
            {
                item.RemoveHandler(InputCaptureStartedEvent, handler);
            }
        }

        public static readonly RoutedEvent InputCaptureCompletedEvent = EventManager.RegisterRoutedEvent("InputCaptureCompleted", RoutingStrategy.Direct, typeof(InputCaptureEventHandler), typeof(KeyInputCapture));

        public static void AddInputCaptureCompletedHandler(FrameworkElement item, InputCaptureEventHandler handler)
        {
            if (item != null)
            {
                item.AddHandler(InputCaptureCompletedEvent, handler);
            }
        }

        public static void RemoveInputCaptureCompletedHandler(FrameworkElement item, InputCaptureEventHandler handler)
        {
            if (item != null)
            {
                item.RemoveHandler(InputCaptureCompletedEvent, handler);
            }
        }

        static bool IsCaptureEventScope(FrameworkElement item)
        {
            return GetIsCaptureEventScope(item) == true;
        }

        private static void RaiseInputCaptureStartedEvent(FrameworkElement item, KeyInputCaptureItem capture)
        {
            KeyInputCaptureEventArgs args = new KeyInputCaptureEventArgs(InputCaptureStartedEvent, capture);

            foreach (FrameworkElement obj in FindElements(item, IsCaptureEventScope))
            {
                obj.RaiseEvent(args);
            }

            item.RaiseEvent(args);

            int count = GetInputCaptureStartedCount(item);
            item.SetValue(InputCaptureStartedCountProperty, ++count);
            item.SetValue(IsInputCaptureCompletedProperty, false);
            item.SetValue(IsInputCaptureStartedProperty, true);
        }

        private static void RaiseInputCaptureCompletedEvent(FrameworkElement item, KeyInputCaptureItem capture, string inputText)
        {
            KeyInputCaptureEventArgs args = new KeyInputCaptureEventArgs(InputCaptureCompletedEvent, capture, inputText);

            foreach (FrameworkElement obj in FindElements(item, IsCaptureEventScope))
            {
                obj.RaiseEvent(args);
            }

            item.RaiseEvent(args);

            int count = GetInputCaptureCompletedCount(item);
            item.SetValue(InputCaptureCompletedCountProperty, ++count);
            item.SetValue(IsInputCaptureStartedProperty, false);
            item.SetValue(IsInputCaptureCompletedProperty, true);
        }
    }
}
