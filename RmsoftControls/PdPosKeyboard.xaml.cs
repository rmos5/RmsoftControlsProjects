using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace RmsoftControls
{
    /// <summary>
    /// Interaction logic for PdPosKeyboard.xaml
    /// </summary>
    public partial class PdPosKeyboard : UserControl
    {
        public PdPosKeyboard()
        {
            InitializeComponent();
        }

        public string NumberDecimalSeparator { get; } = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;

        public string EnterKeyTitle
        {
            get { return (string)GetValue(EnterKeyTitleProperty); }
            set { SetValue(EnterKeyTitleProperty, value); }
        }

        public static readonly DependencyProperty EnterKeyTitleProperty =
            DependencyProperty.Register("EnterKeyTitle", typeof(string), typeof(PdPosKeyboard), new PropertyMetadata("Enter"));

        public string ClearKeyTitle
        {
            get { return (string)GetValue(ClearKeyTitleProperty); }
            set { SetValue(ClearKeyTitleProperty, value); }
        }

        public static readonly DependencyProperty ClearKeyTitleProperty =
            DependencyProperty.Register("ClearKeyTitle", typeof(string), typeof(PdPosKeyboard), new PropertyMetadata("C"));

        public string BackKeyTitle
        {
            get { return (string)GetValue(BackKeyTitleProperty); }
            set { SetValue(BackKeyTitleProperty, value); }
        }

        public static readonly DependencyProperty BackKeyTitleProperty =
            DependencyProperty.Register("BackKeyTitle", typeof(string), typeof(PdPosKeyboard), new PropertyMetadata("Back"));
    }
}
