using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public class TextBoxEmptyDefaultValueBehavior : Behavior<TextBox>
    {
        public object DefaultValue
        {
            get { return GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        public static readonly DependencyProperty DefaultValueProperty =
            DependencyProperty.Register("DefaultValue", typeof(object), typeof(TextBoxEmptyDefaultValueBehavior), new PropertyMetadata(null));

        public bool SelectAllWhenDefaultValue
        {
            get { return (bool)GetValue(SelectAllWhenDefaultValueProperty); }
            set { SetValue(SelectAllWhenDefaultValueProperty, value); }
        }

        public static readonly DependencyProperty SelectAllWhenDefaultValueProperty =
            DependencyProperty.Register("SelectAllWhenDefaultValue", typeof(bool), typeof(TextBoxEmptyDefaultValueBehavior), new PropertyMetadata(true));

        protected override void OnAttached()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
            if (AssociatedObject.Text?.Length == 0)
                AssociatedObject.Text = DefaultValue?.ToString();
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text))
            {
                AssociatedObject.Text = DefaultValue?.ToString();
                if (SelectAllWhenDefaultValue)
                    AssociatedObject.SelectAll();
            }
        }
    }
}
