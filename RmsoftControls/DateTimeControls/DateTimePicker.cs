using RmsoftControls.TextControls;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RmsoftControls.DateTimeControls
{
    /// <summary>
    /// Modified <see cref="DatePicker"/> showing picked date with time part.
    /// </summary>
    [TemplatePart(Name = ElementDateTimeTextBox, Type = typeof(TextBox))]
    public class DateTimePicker : DatePicker
    {
        public const string ElementDateTimeTextBox = "PART_DateTimeTextBox";

        private TextBoxPro dateTimeTextBox = null;

        public bool IsDateTimeTextBoxAcivated { get; private set; }

        public object Watermark
        {
            get { return (object)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(object), typeof(DateTimePicker), new PropertyMetadata(ControlResources.DefaultWatermarkText));

        public Visibility ClearButtonVisibility
        {
            get { return (Visibility)GetValue(ClearButtonVisibilityProperty); }
            set { SetValue(ClearButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ClearButtonVisibilityProperty =
            DependencyProperty.Register("ClearButtonVisibility", typeof(Visibility), typeof(DateTimePicker), new PropertyMetadata(Visibility.Hidden));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(false));

        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        public DateTimePicker()
        {
            Loaded += DateTimePicker_Loaded;
        }

        private void DateTimePicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsVisible)
                ActivateDateTimeTextBoxIfNotActivated();
            else
                IsVisibleChanged += DateTimePicker_IsVisibleChanged;
        }

        private void DateTimePicker_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IsVisibleChanged -= DateTimePicker_IsVisibleChanged;
            ActivateDateTimeTextBoxIfNotActivated();
        }

        private void ActivateDateTimeTextBoxIfNotActivated()
        {
            if (IsDateTimeTextBoxAcivated)
                return;

            dateTimeTextBox = GetTemplateChild(ElementDateTimeTextBox) as TextBoxPro;

            if (dateTimeTextBox != null)
            {
                Binding b = new Binding("SelectedDate");
                b.Source = this;
                b.StringFormat = "g";
                b.TargetNullValue = "";
                dateTimeTextBox.SetBinding(TextBox.TextProperty, b);
                b = new Binding("Watermark");
                b.Source = this;
                dateTimeTextBox.SetBinding(TextBoxPro.WatermarkProperty, b);
                dateTimeTextBox.PreviewLostKeyboardFocus += DateTimeTextBox_PreviewLostKeyboardFocus;
                dateTimeTextBox.MouseDoubleClick += DateTimeTextBox_MouseDoubleClick;
                IsDateTimeTextBoxAcivated = true;
            }
        }

        private void DateTimeTextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dateTimeTextBox?.SelectAll();
        }

        private void DateTimeTextBox_PreviewLostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (dateTimeTextBox.Text == "")
                SelectedDate = null;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ActivateDateTimeTextBoxIfNotActivated();
        }
    }
}
