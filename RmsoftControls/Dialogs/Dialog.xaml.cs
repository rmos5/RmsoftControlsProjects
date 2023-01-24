using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RmsoftControls.Dialogs
{
    /// <summary>
    /// Shows dialog.
    /// </summary>
    public partial class Dialog : Window
    {
        public DialogButtons Buttons
        {
            get { return (DialogButtons)GetValue(ButtonsProperty); }
            private set { SetValue(ButtonsProperty, value); }
        }

        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.Register("Buttons", typeof(DialogButtons), typeof(Dialog), new PropertyMetadata(default(DialogButtons)));

        public string OkButtonTitle
        {
            get { return (string)GetValue(OkButtonTitleProperty); }
            set { SetValue(OkButtonTitleProperty, value); }
        }

        public static readonly DependencyProperty OkButtonTitleProperty =
            DependencyProperty.Register("OkButtonTitle", typeof(string), typeof(Dialog), new PropertyMetadata(ControlResources.OkButtonTitle));

        public string CancelButtonTitle
        {
            get { return (string)GetValue(CancelButtonTitleProperty); }
            set { SetValue(CancelButtonTitleProperty, value); }
        }

        public static readonly DependencyProperty CancelButtonTitleProperty =
            DependencyProperty.Register("CancelButtonTitle", typeof(string), typeof(Dialog), new PropertyMetadata(ControlResources.CancelButtonTitle));

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(Dialog), new PropertyMetadata(new Style(typeof(Button)), OnButtonStyleChanged));

        private static void OnButtonStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Dialog obj = (Dialog)d;
            if (e.NewValue != null)
            {
                if (obj.Resources.Contains(typeof(Button)))
                    obj.Resources.Remove(typeof(Button));
                obj.Resources.Add(typeof(Button), e.NewValue);
            }
        }

        public Style DialogStyle
        {
            get { return (Style)GetValue(DialogStyleProperty); }
            set { SetValue(DialogStyleProperty, value); }
        }

        public static readonly DependencyProperty DialogStyleProperty =
            DependencyProperty.Register("DialogStyle", typeof(Style), typeof(Dialog), new PropertyMetadata(new Style(typeof(Dialog)), OnDialogStyleChanged));

        private static void OnDialogStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Dialog obj = (Dialog)d;
            if (e.NewValue != null)
            {
                if (obj.Resources.Contains(typeof(Dialog)))
                    obj.Resources.Remove(typeof(Dialog));
                obj.Resources.Add(typeof(Dialog), e.NewValue);
            }
        }

        internal Dialog()
        {
            InitializeComponent();
        }

        static Dialog Create(Window owner, string title, object content, DialogButtons dialogButtons, string okButtonTitle, string cancelButtonTitle, bool okButtonIsDefault, ImageSource icon, Style buttonStyle, Style dialogStyle)
        {
            Dialog result = new Dialog
            {
                Owner = owner,
                Icon = icon
            };

            result.headerIcon.Source = icon;
            result.Title = title;

            bool isTextContent = content as string != null;
            result.contentContainer.Visibility = isTextContent ? Visibility.Collapsed : Visibility.Visible;
            result.textContainer.Visibility = isTextContent ? Visibility.Visible : Visibility.Collapsed;
            if (isTextContent)
                result.textContainer.Text = (string)content;
            else
                result.contentContainer.Content = content;
            result.Buttons = dialogButtons;
            if (!string.IsNullOrWhiteSpace(okButtonTitle))
                result.OkButtonTitle = okButtonTitle;
            if (!string.IsNullOrWhiteSpace(cancelButtonTitle))
                result.CancelButtonTitle = cancelButtonTitle;
            result.okButton.IsDefault = okButtonIsDefault;
            result.cancelButton.IsDefault = !okButtonIsDefault;
            // Esc handling
            switch (dialogButtons)
            {
                case DialogButtons.Ok:
                    result.okButton.IsCancel = true;
                    break;
                default:
                    break;
            }

            if (buttonStyle != null)
            {
                result.ButtonStyle = buttonStyle;
            }

            if (dialogStyle != null)
            {
                result.DialogStyle = dialogStyle;
            }

            return result;
        }

        static bool? ShowDialog(Window owner, object content, string title, DialogButtons dialogButtons, string okButtonTitle, string cancelButtonTitle, bool okButtonIsDefault, ImageSource icon, Style buttonStyle, Style dialogStyle)
        {
            Dialog dialog = Create(owner, title, content, dialogButtons, okButtonTitle, cancelButtonTitle, okButtonIsDefault, icon, buttonStyle, dialogStyle);
            return dialog.ShowDialog();
        }

        public static bool? ShowInformation(Window owner, string content, string title = null, string okButtonTitle = null, ImageSource icon = null, Style buttonStyle = null, Style dialogStyle = null)
        {
            return ShowDialog(owner, content, title ?? ControlResources.InformationTitle, DialogButtons.Ok, okButtonTitle, null, true, icon ?? Icons.InformationIcon32, buttonStyle, dialogStyle);
        }

        public static bool? ShowWarning(Window owner, string content, string title = null, string okButtonTitle = null, ImageSource icon = null, Style buttonStyle = null, Style dialogStyle = null)
        {
            return ShowDialog(owner, content, title ?? ControlResources.WarningTitle, DialogButtons.Ok, okButtonTitle, null, true, icon ?? Icons.WarningIcon32, buttonStyle, dialogStyle);
        }

        public static bool? ShowError(Window owner, string content, string title = null, string okButtonTitle = null, ImageSource icon = null, Style buttonStyle = null, Style dialogStyle = null)
        {
            return ShowDialog(owner, content, title ?? ControlResources.ErrorTitle, DialogButtons.Ok, okButtonTitle, null, true, icon ?? Icons.ErrorIcon32, buttonStyle, dialogStyle);
        }

        public static bool? ShowQuestion(Window owner, string content, string title = null, string okButtonTitle = null, string cancelButtonTitle = null, bool okButtonIsDefault = false, ImageSource icon = null, Style buttonStyle = null, Style dialogStyle = null)
        {
            return ShowDialog(owner, content, title ?? ControlResources.QuestionTitle, DialogButtons.OkCancel, okButtonTitle, cancelButtonTitle, okButtonIsDefault, icon ?? Icons.QuestionIcon32, buttonStyle, dialogStyle);
        }

        public static bool? ShowCustom(Window owner, string content, string title, DialogButtons dialogButtons, string okButtonTitle = null, string cancelButtonTitle = null, bool okButtonIsDefault = false, ImageSource icon = null, Style buttonStyle = null, Style dialogStyle = null)
        {
            return ShowDialog(owner, content, title, dialogButtons, okButtonTitle, cancelButtonTitle, okButtonIsDefault, icon ?? Icons.InformationIcon32, buttonStyle, dialogStyle);
        }

        public static bool? ShowCustom(Window owner, FrameworkElement content, string title, DialogButtons dialogButtons = DialogButtons.None, string okButtonTitle = null, string cancelButtonTitle = null, bool okButtonIsDefault = false, ImageSource icon = null, Style buttonStyle = null, Style dialogStyle = null)
        {
            return ShowDialog(owner, content, title, dialogButtons, okButtonTitle, cancelButtonTitle, okButtonIsDefault, icon ?? Icons.InformationIcon32, buttonStyle, dialogStyle);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
