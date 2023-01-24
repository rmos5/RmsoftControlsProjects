using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace RmsoftControls
{
    public static class ToggleButtonGroup
    {
        public static string GetGroup(ToggleButton obj)
        {
            return (string)obj.GetValue(GroupProperty);
        }

        public static void SetGroup(ToggleButton obj, string value)
        {
            obj.SetValue(GroupProperty, value);
        }

        public static readonly DependencyProperty GroupProperty =
            DependencyProperty.RegisterAttached("Group", typeof(string), typeof(ToggleButtonGroup), new PropertyMetadata(null, OnGroupChanged));

        private static void OnGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToggleButton obj = d as ToggleButton;

            if (obj != null)
            {
                string value = e.OldValue as string;

                if (!string.IsNullOrWhiteSpace(value))
                    obj.Checked -= ToggleButton_Checked;

                value = e.NewValue as string;

                if (!string.IsNullOrWhiteSpace(value))
                    obj.Checked += ToggleButton_Checked;
            }
        }

        private static void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton obj = (ToggleButton)sender;
            Window window = Window.GetWindow(obj);
            string groupName = GetGroup(obj);
            List<ToggleButton> group = FindGroup(window, groupName);

            foreach (ToggleButton obj2 in group.Where(o => o != obj))
            {
                obj2.IsChecked = false;
            }
        }

        private static void FindGroup(Visual item, IList<ToggleButton> result, string groupName)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
            {
                // Retrieve child visual at specified index value.
                Visual child = (Visual)VisualTreeHelper.GetChild(item, i);

                FrameworkElement element = child as FrameworkElement;
                ToggleButton toggleButton = element as ToggleButton;

                if (toggleButton != null && GetGroup(toggleButton) == groupName)
                {
                    result.Add(toggleButton);
                }

                Popup popup = child as Popup;

                if (popup?.IsOpen == true && (popup?.Child != null))
                    FindGroup(popup.Child, result, groupName);

                // Enumerate children of the child visual object.
                FindGroup(child, result, groupName);
            }
        }

        private static List<ToggleButton> FindGroup(Window item, string groupName)
        {
            List<ToggleButton> result = new List<ToggleButton>();

            FindGroup(item, result, groupName);

            return result;
        }
    }
}
