using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RmsoftControls
{
    /// <summary>
    /// Selects a <see cref="TabItem"/> marked by attached state object based on container <see cref="TabControl"/> attached state object or the first TabItem.
    /// </summary>
    public static class TabControlStateSelector
    {
        public static object GetContainerState(TabControl obj)
        {
            return obj.GetValue(ContainerStateProperty);
        }

        public static void SetContainerState(TabControl obj, object value)
        {
            obj.SetValue(ContainerStateProperty, value);
        }

        public static readonly DependencyProperty ContainerStateProperty =
            DependencyProperty.RegisterAttached("ContainerState", typeof(object), typeof(TabControlStateSelector), new PropertyMetadata(null, OnContainerStateChanged));

        static IEnumerable<TabItem> GetTabItems(ItemCollection items)
        {
            foreach (TabItem obj in items)
                yield return obj;
        }

        private static void OnContainerStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TabControl tabControl = d as TabControl;

            if (tabControl != null)
            {
                TabItem tabItem = GetTabItems(tabControl.Items).FirstOrDefault(o => GetItemState(o)?.Equals(e.NewValue) == true);

                //select first tab if no state defined
                if (tabItem == null)
                    tabControl.SelectedIndex = 0;
                else
                {
                    RemoveTabItemEvents(tabItem);
                    tabControl.SelectedItem = tabItem;
                    AddTabItemEvents(tabItem);
                }
            }
        }
        public static object GetItemState(TabItem obj)
        {
            return obj.GetValue(ItemStateProperty);
        }

        public static void SetItemState(TabItem obj, object value)
        {
            obj.SetValue(ItemStateProperty, value);
        }

        public static readonly DependencyProperty ItemStateProperty =
            DependencyProperty.RegisterAttached("ItemState", typeof(object), typeof(TabControlStateSelector), new PropertyMetadata(null, OnItemStateChanged));

        private static void OnItemStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TabItem item = d as TabItem;

            if (e.OldValue != null)
            {
                RemoveTabItemEvents(item);
            }

            if (e.NewValue != null)
            {
                AddTabItemEvents(item);
            }

        }

        private static void AddTabItemEvents(TabItem item)
        {
            item.GotKeyboardFocus += TabItem_GotKeyboardFocus;
        }

        private static void RemoveTabItemEvents(TabItem item)
        {
            item.GotKeyboardFocus -= TabItem_GotKeyboardFocus;
        }

        private static void TabItem_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            TabItem item = (TabItem)sender;
            TabControl container = null;

            DependencyObject current = item;
            //find first parent TabControl
            while ((current = VisualTreeHelper.GetParent(current)) != null)
            {
                if (current is TabControl)
                {
                    container = (TabControl)current;
                    break;
                }
            }
            
            if (container != null)
            {
                item = GetTabItems(container.Items).FirstOrDefault(o => o.Equals(item));

                //is this TabItem container
                if (item != null)
                {
                    RemoveTabItemEvents(item);
                    SetContainerState(container, GetItemState(item));
                    AddTabItemEvents(item);
                }
            }
        }
    }
}
