﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public class SelectAllTextOnEnterKeyBehavior : Behavior<TextBox>
    {
        public bool SetFocusAsWell
        {
            get { return (bool)GetValue(SetFocusAsWellProperty); }
            set { SetValue(SetFocusAsWellProperty, value); }
        }

        public static readonly DependencyProperty SetFocusAsWellProperty =
            DependencyProperty.Register("SetFocusAsWell", typeof(bool), typeof(SelectAllTextOnEnterKeyBehavior), new PropertyMetadata(false));

        protected override void OnAttached()
        {
            if (AssociatedObject == null)
                return;
            AssociatedObject.KeyUp += AssociatedObject_KeyUp;
        }

        private void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SetFocusAsWell)
                {
                    AssociatedObject.Focus();
                }

                AssociatedObject.SelectAll();
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.KeyUp -= AssociatedObject_KeyUp;
        }
    }
}
