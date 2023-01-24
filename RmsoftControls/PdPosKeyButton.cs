using RmsoftControls.TextControls;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RmsoftControls
{
    public class PdPosKeyButton
    {
        public static Key GetKeyboardKey(DependencyObject obj)
        {
            return (Key)obj.GetValue(KeyboardKeyProperty);
        }

        public static void SetKeyboardKey(DependencyObject obj, Key value)
        {
            obj.SetValue(KeyboardKeyProperty, value);
        }

        public static readonly DependencyProperty KeyboardKeyProperty =
            DependencyProperty.RegisterAttached("KeyboardKey", typeof(Key), typeof(PdPosKeyButton), new PropertyMetadata(Key.None, OnButtonKeyboardKeyChanged));

        private static void OnButtonKeyboardKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonBase item = (ButtonBase)d;
            if (e.OldValue != null)
                RemoveButtonEvents(item);

            if (e.NewValue != null)
                AddButtonEvents(item);
        }

        static void AddButtonEvents(ButtonBase item)
        {
            item.Click += Button_Click;
        }

        static void RemoveButtonEvents(ButtonBase item)
        {
            item.Click -= Button_Click;
        }


        static void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonBase item = (ButtonBase)sender;
            Key key = GetKeyboardKey(item);

            IInputElement element = InputManager.Current.PrimaryKeyboardDevice?.FocusedElement;

            if (element is TextBoxBase textBoxBase)
            {
                KeyConverter converter = new KeyConverter();
                string text = null;
                switch (key)
                {
                    case Key.D0:
                    case Key.D1:
                    case Key.D2:
                    case Key.D3:
                    case Key.D4:
                    case Key.D5:
                    case Key.D6:
                    case Key.D7:
                    case Key.D8:
                    case Key.D9:
                        text = converter.ConvertToInvariantString(key);
                        break;
                    case Key.Multiply:
                        text = "*";
                        break;
                    case Key.Decimal:
                    case Key.OemComma:
                    case Key.OemPeriod:
                        text = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        break;
                    case Key.Clear:
                    case Key.OemClear:
                        ClearText(textBoxBase);
                        break;
                    default:
                        SendKeyboardKeyEvent(key);
                        break;
                }

                if (text?.Length > 0)
                {
                    SendTextCompositionEvent(text);
                }
            }
        }

        private static void ClearText(TextBoxBase item)
        {
            if (item is TextBox textBox)
                textBox.Clear();
            else if (item is TextBoxPro textBoxPro)
                textBoxPro.Clear();
            else if (item is RichTextBox richTextBox)
            {
                richTextBox.SelectAll();
                richTextBox.Selection.Text = "";
            }
        }

        static void SendTextCompositionEvent(string text)
        {
            TextCompositionEventArgs e = new TextCompositionEventArgs(Keyboard.PrimaryDevice, new TextComposition(InputManager.Current, Keyboard.FocusedElement, text)) { RoutedEvent = UIElement.TextInputEvent };
            SendInputRoutedEvent(e);
        }

        static void SendKeyboardKeyEvent(Key key)
        {
            KeyEventArgs e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key) { RoutedEvent = Keyboard.KeyDownEvent };
            InputManager.Current.ProcessInput(e);

            e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key) { RoutedEvent = Keyboard.KeyUpEvent };
            SendInputRoutedEvent(e);
        }

        static void SendInputRoutedEvent(InputEventArgs e)
        {
            InputManager.Current.ProcessInput(e);
        }
    }
}
