using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace RmsoftControls.TextControls
{
    /// <summary>
    /// <see cref="TextBox"/> with watermark and clear button when text is empty.
    /// </summary>
    [TemplatePart(Name = ElementContentName, Type = typeof(ContentControl))]
    [TemplatePart(Name = ButtonName, Type = typeof(Button))]
    public partial class TextBoxPro : TextBox
    {
        private const string ElementContentName = "PART_Watermark";

        private const string ButtonName = "PART_Button";

        #region GroupValidation
        /// <summary>
        /// Valid state
        /// </summary>
        public const string StateValid = "Valid";

        /// <summary>
        /// InvalidFocused state
        /// </summary>
        public const string StateInvalidFocused = "InvalidFocused";

        /// <summary>
        /// InvalidUnfocused state
        /// </summary>
        public const string StateInvalidUnfocused = "InvalidUnfocused";

        /// <summary>
        /// Validation state group
        /// </summary>
        public const string GroupValidation = "ValidationStates";
        #endregion GroupValidation

        #region GroupCommon
        /// <summary>
        /// Normal state
        /// </summary>
        public const string StateNormal = "Normal";

        /// <summary>
        /// MouseOver state
        /// </summary>
        public const string StateMouseOver = "MouseOver";

        /// <summary>
        /// Pressed state
        /// </summary>
        public const string StatePressed = "Pressed";

        /// <summary>
        /// Disabled state
        /// </summary>
        public const string StateDisabled = "Disabled";

        /// <summary>
        /// Readonly state
        /// </summary>
        public const string StateReadOnly = "ReadOnly";

        /// <summary>
        /// Transition into the Normal state in the ProgressBar template.
        /// </summary>
        internal const string StateDeterminate = "Determinate";

        /// <summary>
        /// Common state group
        /// </summary>
        public const string GroupCommon = "CommonStates";
        #endregion GroupCommon

        #region GroupFocus
        /// <summary>
        /// Unfocused state
        /// </summary>
        public const string StateUnfocused = "Unfocused";

        /// <summary>
        /// Focused state
        /// </summary>
        public const string StateFocused = "Focused";

        /// <summary>
        /// Focused and Dropdown is showing state
        /// </summary>
        public const string StateFocusedDropDown = "FocusedDropDown";

        /// <summary>
        /// Focus state group
        /// </summary>
        public const string GroupFocus = "FocusStates";
        #endregion GroupFocus

        #region GroupWatermark
        /// <summary>
        /// Unwatermarked state
        /// </summary>
        public const string StateUnwatermarked = "Unwatermarked";

        /// <summary>
        /// Watermarked state
        /// </summary>
        public const string StateWatermarked = "Watermarked";

        /// <summary>
        /// Watermark state group
        /// </summary>
        public const string GroupWatermark = "WatermarkStates";
        #endregion GroupWatermark

        private ContentControl elementContent;

        private Button button;

        public Visibility ClearButtonVisibility
        {
            get { return (Visibility)GetValue(ClearButtonVisibilityProperty); }
            set { SetValue(ClearButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ClearButtonVisibilityProperty =
            DependencyProperty.Register("ClearButtonVisibility", typeof(Visibility), typeof(TextBoxPro), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Watermark content
        /// </summary>
        /// <value>The watermark.</value>
        public object Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        /// <summary>
        /// Watermark dependency property
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(TextBoxPro), new PropertyMetadata(OnWatermarkPropertyChanged));

        public VerticalAlignment WatermarkVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(WatermarkVerticalAlignmentProperty); }
            set { SetValue(WatermarkVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty WatermarkVerticalAlignmentProperty =
            DependencyProperty.Register("WatermarkVerticalAlignment", typeof(VerticalAlignment), typeof(TextBoxPro), new PropertyMetadata(VerticalAlignment.Top));

        
        public HorizontalAlignment WatermarkHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(WatermarkHorizontalAlignmentProperty); }
            set { SetValue(WatermarkHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty WatermarkHorizontalAlignmentProperty =
            DependencyProperty.Register("WatermarkHorizontalAlignment", typeof(HorizontalAlignment), typeof(TextBoxPro), new PropertyMetadata(HorizontalAlignment.Left));

        /// <summary>
        /// Static constructor
        /// </summary>
        static TextBoxPro()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxPro), new FrameworkPropertyMetadata(typeof(TextBoxPro)));
            TextProperty.OverrideMetadata(typeof(TextBoxPro), new FrameworkPropertyMetadata(OnVisualStatePropertyChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxPro"/> class.
        /// </summary>
        public TextBoxPro()
        {
            SetCurrentValue(WatermarkProperty, ControlResources.DefaultWatermarkText);
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Called when template is applied to the control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            elementContent = GetTemplateChild(ElementContentName) as ContentControl;

            // We dont want to expose watermark property as public yet, because there
            // is a good chance in future that the implementation will change when
            // a WatermarkTextBox control gets implemented. This is mostly to
            // mimc SL. Hence setting the binding in code rather than in control template.
            if (elementContent != null)
            {
                Binding watermarkBinding = new Binding("Watermark")
                {
                    Source = this
                };
                elementContent.SetBinding(ContentControl.ContentProperty, watermarkBinding);
            }

            button = GetTemplateChild(ButtonName) as Button;

            if (button != null)
            {
                button.Click += Button_Click;
            }

            OnWatermarkChanged();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Text = "";
            Focus();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ApplyTemplate();
            ChangeVisualState(true);
        }

        internal static void OnVisualStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBoxPro control)
            {
                control.UpdateVisualState(true);
            }
        }

        /// <summary>
        /// Change to the correct visual state for the textbox.
        /// </summary>
        /// <param name="useTransitions">
        /// true to use transitions when updating the visual state, false to
        /// snap directly to the new visual state.
        /// </param>
        internal void ChangeVisualState(bool useTransitions)
        {
            // See ButtonBase.ChangeVisualState.
            // This method should be exactly like it, except we have a ReadOnly state instead of Pressed
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, StateDisabled, useTransitions);
            }
            else if (IsReadOnly)
            {
                VisualStateManager.GoToState(this, StateReadOnly, useTransitions);
            }
            else if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, StateMouseOver, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, StateNormal, useTransitions);
            }

            if (IsKeyboardFocused)
            {
                VisualStateManager.GoToState(this, StateFocused, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, StateUnfocused, useTransitions);
            }

            // Update the WatermarkStates group
            if (Watermark != null && string.IsNullOrEmpty(Text))
            {
                VisualStateManager.GoToState(this, StateWatermarked, useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, StateUnwatermarked, useTransitions);
            }

            ChangeValidationVisualState(useTransitions);
        }

        private void OnWatermarkChanged()
        {
            if (elementContent != null)
            {
                if (Watermark is Control watermarkControl)
                {
                    watermarkControl.IsTabStop = false;
                    watermarkControl.IsHitTestVisible = false;
                }
                else if (Watermark as string != null)
                {
                    //set margin for watermark text content
                    elementContent.Margin = new Thickness(3, 0, 0, 0);
                }
            }
        }

        internal enum ControlBoolFlags : ushort
        {
            ContentIsNotLogical = 0x0001,            // used in contentcontrol.cs
            IsSpaceKeyDown = 0x0002,            // used in ButtonBase.cs
            HeaderIsNotLogical = 0x0004,            // used in HeaderedContentControl.cs, HeaderedItemsControl.cs
            CommandDisabled = 0x0008,            // used in ButtonBase.cs, MenuItem.cs
            ContentIsItem = 0x0010,            // used in contentcontrol.cs
            HeaderIsItem = 0x0020,            // used in HeaderedContentControl.cs, HeaderedItemsControl.cs
            ScrollHostValid = 0x0040,            // used in ItemsControl.cs
            ContainsSelection = 0x0080,            // used in TreeViewItem.cs
            VisualStateChangeSuspended = 0x0100,            // used in Control.cs
        }

        internal ControlBoolFlags _controlBoolField;   // Cache valid bits

        internal bool ReadControlFlag(ControlBoolFlags reqFlag)
        {
            return (_controlBoolField & reqFlag) != 0;
        }

        internal void WriteControlFlag(ControlBoolFlags reqFlag, bool set)
        {
            if (set)
            {
                _controlBoolField |= reqFlag;
            }
            else
            {
                _controlBoolField &= (~reqFlag);
            }
        }

        internal bool VisualStateChangeSuspended
        {
            get { return ReadControlFlag(ControlBoolFlags.VisualStateChangeSuspended); }
            set { WriteControlFlag(ControlBoolFlags.VisualStateChangeSuspended, value); }
        }

        /// <summary>
        /// Update the current visual state of the control
        /// </summary>
        /// <param name="useTransitions">
        /// true to use transitions when updating the visual state, false to
        /// snap directly to the new visual state.
        /// </param>
        internal void UpdateVisualState(bool useTransitions)
        {
            //EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Info, EventTrace.Event.UpdateVisualStateStart);
            if (!VisualStateChangeSuspended)
            {
                ChangeVisualState(useTransitions);
            }
            //EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Info, EventTrace.Event.UpdateVisualStateEnd);
        }

        /// <summary>
        ///     Common code for putting a control in the validation state.  Controls that use the should register
        ///     for change notification of Validation.HasError.
        /// </summary>
        /// <param name="useTransitions"></param>
        internal void ChangeValidationVisualState(bool useTransitions)
        {
            if (Validation.GetHasError(this))
            {
                if (IsKeyboardFocused)
                {
                    VisualStateManager.GoToState(this, StateInvalidFocused, useTransitions);
                }
                else
                {
                    VisualStateManager.GoToState(this, StateInvalidUnfocused, useTransitions);
                }
            }
            else
            {
                VisualStateManager.GoToState(this, StateValid, useTransitions);
            }
        }

        /// <summary>
        /// Called when watermark property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnWatermarkPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TextBoxPro textBox = sender as TextBoxPro;
            Debug.Assert(textBox != null, $"The source is not an instance of {typeof(TextBoxPro).FullName}.");
            textBox.OnWatermarkChanged();
            textBox.UpdateVisualState(true);
        }
    }
}
