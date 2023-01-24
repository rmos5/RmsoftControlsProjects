using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public class DecimalInputBehavior : Behavior<TextBox>
    {
        public const string NegativeSign = "-";

        static string[] Numbers { get; } =
        {
            "0","1","2","3","4","5","6","7","8","9"
        };

        public string DecimalSeparator { get; set; } = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;

        public bool DisableNegativeNumbers { get; set; }

        public int DecimalDigits { get; set; } = 2;

        protected override void OnAttached()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
                return;

            AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
        }

        private void ClearBindingErrors()
        {
            BindingExpression bx = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            if (bx?.HasValidationError == true)
            {
                Validation.ClearInvalid(bx);
            }
        }

        private void AssociatedObject_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string originalText = AssociatedObject.Text ?? "";
            string finalText = originalText;

            if (AssociatedObject.SelectionLength > 0)
                finalText = finalText.Remove(AssociatedObject.SelectionStart, AssociatedObject.SelectionLength);

            int insertIndex = AssociatedObject.SelectionLength > 0 ? AssociatedObject.SelectionStart : AssociatedObject.CaretIndex;
            finalText = finalText.Insert(insertIndex, e.Text);

            if (e.Text == DecimalSeparator)
            {
                e.Handled = DecimalDigits == 0 || finalText.Length == 0 || originalText.IndexOf(DecimalSeparator) > 0;

                if (e.Handled)
                {
                    AssociatedObject.CaretIndex = finalText.IndexOf(DecimalSeparator) + 1;
                }
            }
            else if (NegativeSign == e.Text)
            {
                e.Handled = DisableNegativeNumbers || AssociatedObject.CaretIndex > 0;
            }
            else
            {
                // detect number and disable adding beyond DecimalDigits value
                if (Numbers.Contains(e.Text))
                {
                    int separatorIndex = originalText.IndexOf(DecimalSeparator);
                    int caretIndex = AssociatedObject.CaretIndex;

                    if (DecimalDigits > 0 && separatorIndex > 0 && caretIndex > separatorIndex + DecimalDigits)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void AssociatedObject_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine($"{nameof(AssociatedObject_TextChanged)}:Text={AssociatedObject.Text}");

            string text = AssociatedObject.Text ?? "";

            //negative sign should not raise validation error
            if (NegativeSign == text)
            {
                ClearBindingErrors();
            }
        }
    }
}
