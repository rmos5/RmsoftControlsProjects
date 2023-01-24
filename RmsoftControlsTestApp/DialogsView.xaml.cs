using RmsoftControls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RmsoftControlsTestApp
{
    /// <summary>
    /// Interaction logic for DialogsView.xaml
    /// </summary>
    public partial class DialogsView : UserControl
    {
        private Window MainWindow => Application.Current.MainWindow;

        public IEnumerable<string> DialogTypes { get; } =
            new[]
            {
                "Information",
                "Warning",
                "Error",
                "Question",
                "CustomText",
                "CustomView"
            };

        public IEnumerable<DialogButtons> DialogButtons { get; } =
            new[]
            {
                RmsoftControls.Dialogs.DialogButtons.None,
                RmsoftControls.Dialogs.DialogButtons.Ok,
                RmsoftControls.Dialogs.DialogButtons.Cancel,
                RmsoftControls.Dialogs.DialogButtons.OkCancel
            };

        public string DialogTitle
        {
            get { return (string)GetValue(DialogTitleProperty); }
            set { SetValue(DialogTitleProperty, value); }
        }

        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register("DialogTitle", typeof(string), typeof(DialogsView), new PropertyMetadata(null));

        public string DialogText
        {
            get { return (string)GetValue(DialogTextProperty); }
            set { SetValue(DialogTextProperty, value); }
        }

        public static readonly DependencyProperty DialogTextProperty =
            DependencyProperty.Register("DialogText", typeof(string), typeof(DialogsView), new PropertyMetadata(null));

        public string OkButtonTitle
        {
            get { return (string)GetValue(OkButtonTitleProperty); }
            set { SetValue(OkButtonTitleProperty, value); }
        }

        public static readonly DependencyProperty OkButtonTitleProperty =
            DependencyProperty.Register("OkButtonTitle", typeof(string), typeof(DialogsView), new PropertyMetadata(null));

        public string CancelButtonTitle
        {
            get { return (string)GetValue(CancelButtonTitleProperty); }
            set { SetValue(CancelButtonTitleProperty, value); }
        }

        public static readonly DependencyProperty CancelButtonTitleProperty =
            DependencyProperty.Register("CancelButtonTitle", typeof(string), typeof(DialogsView), new PropertyMetadata(null));

        public string SelectedDialogType
        {
            get { return (string)GetValue(SelectedDialogTypeProperty); }
            set { SetValue(SelectedDialogTypeProperty, value); }
        }

        public static readonly DependencyProperty SelectedDialogTypeProperty =
            DependencyProperty.Register("SelectedDialogType", typeof(string), typeof(DialogsView), new PropertyMetadata(Properties.Settings.Default.SelectedDialogType));

        public DialogButtons SelectedDialogButtons
        {
            get { return (DialogButtons)GetValue(SelectedDialogButtonsProperty); }
            set { SetValue(SelectedDialogButtonsProperty, value); }
        }

        public static readonly DependencyProperty SelectedDialogButtonsProperty =
            DependencyProperty.Register("SelectedDialogButtons", typeof(DialogButtons), typeof(DialogsView), new PropertyMetadata(Properties.Settings.Default.SelectedDialogButtons));

        public bool? DialogResult
        {
            get { return (bool?)GetValue(DialogResultProperty); }
            private set { SetValue(DialogResultProperty, value); }
        }

        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("DialogResult", typeof(bool?), typeof(DialogsView), new PropertyMetadata(null));

        public bool OkButtonIsDefault
        {
            get { return (bool)GetValue(OkButtonIsDefaultProperty); }
            set { SetValue(OkButtonIsDefaultProperty, value); }
        }

        public static readonly DependencyProperty OkButtonIsDefaultProperty =
            DependencyProperty.Register("OkButtonIsDefault", typeof(bool), typeof(DialogsView), new PropertyMetadata(false));

        public Style SelectedDialogWindowStyle
        {
            get { return (Style)GetValue(SelectedDialogWindowStyleProperty); }
            set { SetValue(SelectedDialogWindowStyleProperty, value); }
        }

        public static readonly DependencyProperty SelectedDialogWindowStyleProperty =
            DependencyProperty.Register("SelectedDialogWindowStyle", typeof(Style), typeof(DialogsView), new PropertyMetadata(null));

        public Style SelectedDialogButtonStyle
        {
            get { return (Style)GetValue(SelectedDialogButtonStyleProperty); }
            set { SetValue(SelectedDialogButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty SelectedDialogButtonStyleProperty =
            DependencyProperty.Register("SelectedDialogButtonStyle", typeof(Style), typeof(DialogsView), new PropertyMetadata(null));

        public DialogsView()
        {
            InitializeComponent();
        }

        private void ShowDialogButton_Click(object sender, RoutedEventArgs e)
        {
            switch (SelectedDialogType)
            {
                case "Information":
                    DialogResult = Dialog.ShowInformation(MainWindow, DialogText ?? "Information text goes here...", DialogTitle, OkButtonTitle, null, SelectedDialogButtonStyle, SelectedDialogWindowStyle);
                    break;
                case "Warning":
                    DialogResult = Dialog.ShowWarning(MainWindow, DialogText ?? "Warning text goes here...", DialogTitle, OkButtonTitle, null, SelectedDialogButtonStyle, SelectedDialogWindowStyle);
                    break;
                case "Error":
                    DialogResult = Dialog.ShowError(MainWindow, DialogText ?? "Error text goes here...", DialogTitle, OkButtonTitle, null, SelectedDialogButtonStyle, SelectedDialogWindowStyle);
                    break;
                case "Question":
                    DialogResult = Dialog.ShowQuestion(MainWindow, DialogText ?? "Question text goes here...", DialogTitle, OkButtonTitle, CancelButtonTitle, OkButtonIsDefault, null, SelectedDialogButtonStyle, SelectedDialogWindowStyle);
                    break;
                case "CustomText":
                    DialogResult = Dialog.ShowCustom(MainWindow, DialogText ?? "Custom text goes here...", DialogTitle ?? "Custom message dialog", SelectedDialogButtons, OkButtonTitle, CancelButtonTitle, OkButtonIsDefault, null, SelectedDialogButtonStyle, SelectedDialogWindowStyle);
                    break;
                case "CustomView":
                    CustomView view = new CustomView();
                    DialogResult = Dialog.ShowCustom(MainWindow, view, DialogTitle ?? "Custom view dialog", SelectedDialogButtons, OkButtonTitle, CancelButtonTitle, OkButtonIsDefault, null, SelectedDialogButtonStyle, SelectedDialogWindowStyle);
                    break;
                default:
                    break;
            }
        }
    }
}
