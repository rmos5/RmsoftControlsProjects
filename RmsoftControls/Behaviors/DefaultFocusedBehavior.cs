using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public class DefaultFocusedBehavior : Behavior<TextBox>
    {
        public object ExternalFocusRequest
        {
            get { return GetValue(ExternalFocusRequestProperty); }
            set { SetValue(ExternalFocusRequestProperty, value); }
        }

        public static readonly DependencyProperty ExternalFocusRequestProperty =
            DependencyProperty.Register("ExternalFocusRequest", typeof(object), typeof(DefaultFocusedBehavior), new PropertyMetadata(null, OnExternalFocusRequestChanged));

        public object ExternalFocusRequest2
        {
            get { return GetValue(ExternalFocusRequest2Property); }
            set { SetValue(ExternalFocusRequest2Property, value); }
        }

        public static readonly DependencyProperty ExternalFocusRequest2Property =
            DependencyProperty.Register("ExternalFocusRequest2", typeof(object), typeof(DefaultFocusedBehavior), new PropertyMetadata(null, OnExternalFocusRequestChanged));


        private static void OnExternalFocusRequestChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DefaultFocusedBehavior obj = (DefaultFocusedBehavior)d;
            obj.TryToFocus();
        }

        protected override void OnAttached()
        {
            Debug.WriteLine($"{nameof(OnAttached)}", GetType().Name);

            if (AssociatedObject == null)
                return;

            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
            AssociatedObject.FocusableChanged += AssociatedObject_FocusableChanged;
            AssociatedObject.IsEnabledChanged += AssociatedObject_IsEnabledChanged;
            //AssociatedObject.TextInput += AssociatedObject_TextInput;
            //AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        protected override void OnDetaching()
        {
            Debug.WriteLine($"{nameof(OnDetaching)}", GetType().Name);

            if (AssociatedObject == null)
                return;

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.IsVisibleChanged -= AssociatedObject_IsVisibleChanged;
            AssociatedObject.FocusableChanged -= AssociatedObject_FocusableChanged;
            AssociatedObject.IsEnabledChanged -= AssociatedObject_IsEnabledChanged;
            //AssociatedObject.TextInput -= AssociatedObject_TextInput;
            //AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            TryToFocus();
        }

        private void AssociatedObject_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TryToFocus();
        }

        private void AssociatedObject_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                TryToFocus();
        }

        private void AssociatedObject_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                TryToFocus();
        }

        private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                TryToFocus();
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            TryToFocus();
        }

        private void TryToFocus()
        {
            if (AssociatedObject.IsVisible && AssociatedObject.IsEnabled && AssociatedObject.Focusable)
            {
                Debug.WriteLine($"{nameof(TryToFocus)}:{AssociatedObject}", GetType().Name);
                AssociatedObject.Focus();
            }
        }
    }
}
