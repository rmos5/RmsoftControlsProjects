using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public abstract class KeyInputDetectBaseBehavior : Behavior<UIElement>
    {
        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(Key), typeof(KeyInputDetectBaseBehavior), new PropertyMetadata(null));

        public ModifierKeys ModifierKeys
        {
            get { return (ModifierKeys)GetValue(ModifierKeysProperty); }
            set { SetValue(ModifierKeysProperty, value); }
        }

        public static readonly DependencyProperty ModifierKeysProperty =
            DependencyProperty.Register("ModifierKeys", typeof(ModifierKeys), typeof(KeyInputDetectBaseBehavior), new PropertyMetadata(ModifierKeys.None));

        public KeyEventRoutes KeyEventRoute
        {
            get { return (KeyEventRoutes)GetValue(KeyEventRouteProperty); }
            set { SetValue(KeyEventRouteProperty, value); }
        }

        public static readonly DependencyProperty KeyEventRouteProperty =
            DependencyProperty.Register("KeyEventRoute", typeof(KeyEventRoutes), typeof(KeyInputDetectBaseBehavior), new PropertyMetadata(KeyEventRoutes.PreviewKeyDown));

        public bool HandleEvent
        {
            get { return (bool)GetValue(HandleEventProperty); }
            set { SetValue(HandleEventProperty, value); }
        }

        public static readonly DependencyProperty HandleEventProperty =
            DependencyProperty.Register("HandleEvent", typeof(bool), typeof(KeyInputDetectBaseBehavior), new PropertyMetadata(false));

        protected abstract void OnKeyInputDetected(UIElement sender, KeyEventArgs e);

        protected override void OnAttached()
        {
            Debug.WriteLine($"{nameof(OnAttached)}:element={AssociatedObject}", GetType().Name);

            if (AssociatedObject == null)
                return;

            Window window = Window.GetWindow(AssociatedObject);

            if (window == null)
            {
                AssociatedObject.IsVisibleChanged += (s, e) =>
                {
                    bool val = (bool)e.NewValue;
                    window = Window.GetWindow(AssociatedObject);

                    if (window != null)
                    {
                        if (val)
                        {
                            AddKeyEvents(window);
                        }
                        else
                        {
                            RemoveKeyEvents(window);
                        }
                    }
                };
            }
            else
            {
                AddKeyEvents(window);
            }
        }

        private void AddKeyEvents(Window window)
        {
            Debug.WriteLine($"{nameof(AddKeyEvents)}:window={window};{KeyEventRoute}", GetType().Name);

            switch (KeyEventRoute)
            {
                case KeyEventRoutes.KeyUp:
                    window.KeyUp += KeyEvent_Handler;
                    break;
                case KeyEventRoutes.KeyDown:
                    window.KeyDown += KeyEvent_Handler;
                    break;
                case KeyEventRoutes.PreviewKeyUp:
                    window.PreviewKeyUp += KeyEvent_Handler;
                    break;
                case KeyEventRoutes.PreviewKeyDown:
                    window.PreviewKeyDown += KeyEvent_Handler;
                    break;
                default:
                    break;
            }
        }

        private void RemoveKeyEvents(Window window)
        {
            Debug.WriteLine($"{nameof(RemoveKeyEvents)}:window={window};{KeyEventRoute}", GetType().Name);

            switch (KeyEventRoute)
            {
                case KeyEventRoutes.KeyUp:
                    window.KeyUp -= KeyEvent_Handler;
                    break;
                case KeyEventRoutes.KeyDown:
                    window.KeyDown -= KeyEvent_Handler;
                    break;
                case KeyEventRoutes.PreviewKeyUp:
                    window.PreviewKeyUp -= KeyEvent_Handler;
                    break;
                case KeyEventRoutes.PreviewKeyDown:
                    window.PreviewKeyDown -= KeyEvent_Handler;
                    break;
                default:
                    break;
            }
        }

        protected virtual bool IsKeyInputDetected(KeyEventArgs e)
        {
            return e.Key == Key && e.KeyboardDevice.Modifiers == ModifierKeys;
        }

        private void KeyEvent_Handler(object sender, KeyEventArgs e)
        {
            if (IsKeyInputDetected(e))
            {
                Debug.WriteLine($":e={e}", GetType().Name);
                e.Handled = HandleEvent;
                OnKeyInputDetected(sender as UIElement, e);
            }
        }

        protected override void OnDetaching()
        {
            Debug.WriteLine($"{nameof(OnDetaching)}:element={AssociatedObject}", GetType().Name);

            if (AssociatedObject == null)
                return;

            Window window = Window.GetWindow(AssociatedObject);

            if (window != null)
                RemoveKeyEvents(window);
        }
    }
}
