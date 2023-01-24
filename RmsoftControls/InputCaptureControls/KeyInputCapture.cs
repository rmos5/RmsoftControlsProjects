using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using InputCaptureTimer = System.Tuple<System.Windows.FrameworkElement, System.Timers.Timer>;

namespace RmsoftControls.InputCaptureControls
{
    public delegate void InputCaptureEventHandler(object sender, KeyInputCaptureEventArgs e);

    public enum TargetUpdateModes
    {
        Default,
        SingleFocused,
        None
    }

    public class KeyInputCaptureItem
    {
        public Key StartKey { get; set; }

        public ModifierKeys StartKeyModifiers { get; set; } = ModifierKeys.None;

        public Key EndKey { get; set; }

        public ModifierKeys EndKeyModifiers { get; set; } = ModifierKeys.None;

        public bool HandleSingleKeyCaptureEvent { get; set; } = true;

        public bool IsSingleKeyCapture =>
            StartKey != Key.None
            && (StartKey == EndKey
            || EndKey == Key.None);


        public override string ToString()
        {
            return $"{StartKeyModifiers};{StartKey};{EndKeyModifiers};{EndKey}";
        }
    }

    public class KeyInputCaptureCollection : List<KeyInputCaptureItem>
    {
    }

    public static partial class KeyInputCapture
    {
        public static KeyInputCaptureCollection GetInputCaptureItems(FrameworkElement obj)
        {
            return (KeyInputCaptureCollection)obj.GetValue(InputCaptureItemsProperty);
        }

        public static void SetInputCaptureItems(FrameworkElement obj, KeyInputCaptureCollection value)
        {
            obj.SetValue(InputCaptureItemsProperty, value);
        }

        public static readonly DependencyProperty InputCaptureItemsProperty =
            DependencyProperty.RegisterAttached("InputCaptureItems", typeof(KeyInputCaptureCollection), typeof(KeyInputCapture), new PropertyMetadata(null, OnInputCaptureItemsChanged));


        public static KeyInputCaptureItem GetCurrentInputCaptureItem(FrameworkElement obj)
        {
            return (KeyInputCaptureItem)obj.GetValue(CurrentInputCaptureItemProperty);
        }

        private static void SetCurrentInputCaptureItem(FrameworkElement obj, KeyInputCaptureItem value)
        {
            obj.SetValue(CurrentInputCaptureItemProperty, value);
        }

        public static readonly DependencyProperty CurrentInputCaptureItemProperty =
            DependencyProperty.RegisterAttached("CurrentInputCaptureItem", typeof(KeyInputCaptureItem), typeof(KeyInputCapture), new PropertyMetadata(null));

        public static string GetInputText(FrameworkElement obj)
        {
            return (string)obj.GetValue(InputTextProperty);
        }

        private static void SetInputText(FrameworkElement obj, string value)
        {
            obj.SetValue(InputTextProperty, value);
        }

        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.RegisterAttached("InputText", typeof(string), typeof(KeyInputCapture), new PropertyMetadata(""));

        public static IList GetInputCaptures(FrameworkElement obj)
        {
            return (IList)obj.GetValue(InputCapturesProperty);
        }

        public static void SetInputCaptures(FrameworkElement obj, IList value)
        {
            obj.SetValue(InputCapturesProperty, value);
        }

        public static readonly DependencyProperty InputCapturesProperty =
            DependencyProperty.RegisterAttached("InputCaptures", typeof(IList), typeof(KeyInputCapture), new PropertyMetadata(null));

        public static int GetInputCapturesMaxLength(FrameworkElement obj)
        {
            return (int)obj.GetValue(InputCapturesMaxLengthProperty);
        }

        public static void SetInputCapturesMaxLength(FrameworkElement obj, int value)
        {
            obj.SetValue(InputCapturesMaxLengthProperty, value);
        }

        public static readonly DependencyProperty InputCapturesMaxLengthProperty =
            DependencyProperty.RegisterAttached("InputCapturesMaxLength", typeof(int), typeof(KeyInputCapture), new PropertyMetadata(100));

        public static int? GetInputCaptureTimeoutMilliseconds(FrameworkElement obj)
        {
            return (int)obj.GetValue(InputCaptureTimeoutMillisecondsProperty);
        }

        public static void SetInputCaptureTimeoutMilliseconds(FrameworkElement obj, int? value)
        {
            if (value <= 0)
                return;
            obj.SetValue(InputCaptureTimeoutMillisecondsProperty, value);
        }

        public static readonly DependencyProperty InputCaptureTimeoutMillisecondsProperty =
            DependencyProperty.RegisterAttached("InputCaptureTimeoutMilliseconds", typeof(int?), typeof(KeyInputCapture), new PropertyMetadata(null, OnInputCaptureTimeoutMillisecondsChanged));

        static List<InputCaptureTimer> InputCaptureTimers { get; } = new List<InputCaptureTimer>();

        public static TargetUpdateModes? GetTargetUpdateMode(FrameworkElement obj)
        {
            return (TargetUpdateModes?)obj.GetValue(TargetUpdateModeProperty);
        }

        public static void SetTargetUpdateMode(FrameworkElement obj, TargetUpdateModes value)
        {
            obj.SetValue(TargetUpdateModeProperty, value);
        }

        public static readonly DependencyProperty TargetUpdateModeProperty =
            DependencyProperty.RegisterAttached("TargetUpdateMode", typeof(TargetUpdateModes?), typeof(KeyInputCapture), new PropertyMetadata((TargetUpdateModes?)null));

        public static FrameworkElement GetSingleFocusedTarget(FrameworkElement obj)
        {
            return (FrameworkElement)obj.GetValue(SingleFocusedTargetProperty);
        }

        public static void SetSingleFocusedTarget(FrameworkElement obj, FrameworkElement value)
        {
            obj.SetValue(SingleFocusedTargetProperty, value);
        }

        public static readonly DependencyProperty SingleFocusedTargetProperty =
            DependencyProperty.RegisterAttached("SingleFocusedTarget", typeof(FrameworkElement), typeof(KeyInputCapture), new PropertyMetadata(null));

        public static IValueConverter GetTargetTextConverter(FrameworkElement obj)
        {
            return (IValueConverter)obj.GetValue(TargetTextConverterProperty);
        }

        public static void SetTargetTextConverter(FrameworkElement obj, IValueConverter value)
        {
            obj.SetValue(TargetTextConverterProperty, value);
        }

        public static readonly DependencyProperty TargetTextConverterProperty =
            DependencyProperty.RegisterAttached("TargetTextConverter", typeof(IValueConverter), typeof(KeyInputCapture), new PropertyMetadata(null));

        public static bool GetDisableUpdateTargets(FrameworkElement obj)
        {
            return (bool)obj.GetValue(DisableUpdateTargetsProperty);
        }

        public static void SetDisableUpdateTargets(FrameworkElement obj, bool value)
        {
            obj.SetValue(DisableUpdateTargetsProperty, value);
        }

        public static readonly DependencyProperty DisableUpdateTargetsProperty =
            DependencyProperty.RegisterAttached("DisableUpdateTargets", typeof(bool), typeof(KeyInputCapture), new PropertyMetadata(false));
        
        public static bool GetPassStartKeyToTargets(FrameworkElement obj)
        {
            return (bool)obj.GetValue(PassStartKeyToTargetsProperty);
        }

        public static void SetPassStartKeyToTargets(FrameworkElement obj, bool value)
        {
            obj.SetValue(PassStartKeyToTargetsProperty, value);
        }

        public static readonly DependencyProperty PassStartKeyToTargetsProperty =
            DependencyProperty.RegisterAttached("PassStartKeyToTargets", typeof(bool), typeof(KeyInputCapture), new PropertyMetadata(false));

        public static bool GetPassEndKeyToTargets(FrameworkElement obj)
        {
            return (bool)obj.GetValue(PassEndKeyToTargetsProperty);
        }

        public static void SetPassEndKeyToTargets(FrameworkElement obj, bool value)
        {
            obj.SetValue(PassEndKeyToTargetsProperty, value);
        }

        public static readonly DependencyProperty PassEndKeyToTargetsProperty =
            DependencyProperty.RegisterAttached("PassEndKeyToTargets", typeof(bool), typeof(KeyInputCapture), new PropertyMetadata(false));

        private static void OnInputCaptureItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement obj = d as FrameworkElement;

            if (obj == null)
                return;

            if (e.OldValue != null)
            {
                RemoveKeyInputCapturing(obj);
            }

            if (e.NewValue != null)
            {
                AddKeyInputCapturing(obj);
            }
        }

        private static void AddKeyInputCapturing(FrameworkElement obj)
        {
            obj.PreviewKeyDown += OnPreviewKeyDown;
            obj.PreviewTextInput += OnPreviewTextInput;
        }

        private static void RemoveKeyInputCapturing(FrameworkElement obj)
        {
            obj.PreviewKeyDown -= OnPreviewKeyDown;
            obj.PreviewTextInput -= OnPreviewTextInput;
        }

        private static void OnInputCaptureTimeoutMillisecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement obj = d as FrameworkElement;

            if (obj == null)
                return;

            int? val = (int?)e.OldValue;

            if (val.HasValue)
            {
                RemoveInputCaptureTimer(obj);
            }

            val = (int?)e.NewValue;

            if (val.HasValue)
            {
                AddInputCaptureTimer(obj, val.Value);
            }
        }

        private static void AddInputCaptureTimer(FrameworkElement obj, int timeoutMilliseconds)
        {
            Debug.WriteLine($"{nameof(AddInputCaptureTimer)}:{obj};{timeoutMilliseconds}", nameof(KeyInputCapture));
            Timer timer = new Timer(timeoutMilliseconds);
            InputCaptureTimers.Add(new InputCaptureTimer(obj, timer));
        }

        private static void RemoveInputCaptureTimer(FrameworkElement obj)
        {
            Debug.WriteLine($"{nameof(RemoveInputCaptureTimer)}:{obj}", nameof(KeyInputCapture));
            InputCaptureTimer item = InputCaptureTimers.FirstOrDefault(o => o.Item1 == obj);
            if (item != null)
            {
                item.Item2.Stop();
                InputCaptureTimers.Remove(item);
                item.Item2.Dispose();
            }
        }

        private static void StartInputReadingTimer(FrameworkElement obj)
        {
            Debug.WriteLine($"{nameof(StartInputReadingTimer)}:{obj}", nameof(KeyInputCapture));
            Timer timer = InputCaptureTimers.FirstOrDefault(o => o.Item1 == obj)?.Item2;
            if (timer != null)
            {
                timer.Elapsed += InputReadingTimer_Elapsed;
                timer.Start();
            }
        }

        private static void StopInputReadingTimer(FrameworkElement obj)
        {
            Debug.WriteLine($"{nameof(StopInputReadingTimer)}:{obj}", nameof(KeyInputCapture));
            Timer timer = InputCaptureTimers.FirstOrDefault(o => o.Item1 == obj)?.Item2;
            timer?.Stop();
        }

        private static void RestartInputReadingTimer(FrameworkElement obj)
        {
            Debug.WriteLine($"{nameof(RestartInputReadingTimer)}:{obj}", nameof(KeyInputCapture));
            Timer timer = InputCaptureTimers.FirstOrDefault(o => o.Item1 == obj)?.Item2;
            timer?.Stop();
            timer?.Start();
        }

        private static void InputReadingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine($"{nameof(InputReadingTimer_Elapsed)}", nameof(KeyInputCapture));
            Timer timer = (Timer)sender;
            timer.Elapsed -= InputReadingTimer_Elapsed;
            FrameworkElement obj = InputCaptureTimers.FirstOrDefault(o => o.Item2 == timer)?.Item1;
            obj?.Dispatcher.Invoke(() =>
            {
                StopInputReadingTimer(obj);
                ClearInputReadings(obj);
            });
        }

        private static void StartInputReading(FrameworkElement item, KeyInputCaptureItem capture, KeyEventArgs e)
        {
            Debug.WriteLine($"{nameof(StartInputReading)}:capture={capture}", nameof(KeyInputCapture));

            e.Handled = !GetPassStartKeyToTargets(item);

            SetInputText(item, "");
            SetCurrentInputCaptureItem(item, capture);

            TrySetSingleFocusedTarget(item);

            StartInputReadingTimer(item);

            RaiseInputCaptureStartedEvent(item, capture);
        }

        private static void EndInputReading(FrameworkElement item, KeyInputCaptureItem capture, KeyEventArgs e)
        {
            Debug.WriteLine($"{nameof(EndInputReading)}:capture={capture}", nameof(KeyInputCapture));

            StopInputReadingTimer(item);

            e.Handled = !GetPassEndKeyToTargets(item);

            string inputText = GetInputText(item);

            UpdateInputCaptures(item, capture, inputText);
            item.ClearValue(CurrentInputCaptureItemProperty);

            //target update is not disabled globally
            if (!GetDisableUpdateTargets(item))
            {
                FrameworkElement singleFocused = GetSingleFocusedTarget(item);

                if (singleFocused == null)
                {
                    //no single focused target
                    UpdateTargetsText(FindElements(item, HasTargetUpdateMode).Where(o => GetTargetUpdateMode(o) != TargetUpdateModes.None && o.IsVisible), inputText);
                }
                else
                {
                    UpdateTargetText(singleFocused, inputText);
                    SetSingleFocusedTarget(item, null);
                }
            }

            RaiseInputCaptureCompletedEvent(item, capture, inputText);
        }

        private static void ClearInputReadings(FrameworkElement obj)
        {
            Debug.WriteLine($"{nameof(ClearInputReadings)}:{obj}", nameof(KeyInputCapture));
            obj.ClearValue(CurrentInputCaptureItemProperty);
            SetInputText(obj, "");
            SetSingleFocusedTarget(obj, null);
        }

        private static void TrySetSingleFocusedTarget(FrameworkElement item)
        {
            //already focused might be suitable
            FrameworkElement singleFocused = Keyboard.FocusedElement as FrameworkElement ?? FocusManager.GetFocusedElement(item) as FrameworkElement;

            if (singleFocused != null
                && GetTargetUpdateMode(singleFocused) != TargetUpdateModes.SingleFocused)
            {
                //not suitable 
                singleFocused = null;
            }

            if (singleFocused == null)
            {
                //find first with TargetUpdateModes.SingleFocused and set focus
                singleFocused = FindElements(item, HasTargetUpdateMode).Where(o => GetTargetUpdateMode(o) == TargetUpdateModes.SingleFocused && o.IsVisible).FirstOrDefault();
                singleFocused?.Focus();
            }

            SetSingleFocusedTarget(item, singleFocused);
        }

        static bool HasTargetUpdateMode(FrameworkElement item)
        {
            return GetTargetUpdateMode(item).HasValue;
        }

        private static void FindElements(Visual item, IList<FrameworkElement> result, Func<FrameworkElement, bool> func)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
            {
                // Retrieve child visual at specified index value.
                Visual child = (Visual)VisualTreeHelper.GetChild(item, i);
                FrameworkElement element = child as FrameworkElement;

                if (element != null && func(element))
                {
                    result.Add(element);
                }

                Popup popup = child as Popup;

                if (popup?.IsOpen == true && popup?.Child is Visual)
                    FindElements(popup.Child, result, func);

                // Enumerate children of the child visual object.
                FindElements(child, result, func);
            }
        }

        private static List<FrameworkElement> FindElements(FrameworkElement item, Func<FrameworkElement, bool> func)
        {
            List<FrameworkElement> result = new List<FrameworkElement>();

            FindElements(item, result, func);

            return result;
        }

        private static void UpdateTargetText(FrameworkElement target, string text)
        {
            if (target == null)
                return;

            IValueConverter converter = GetTargetTextConverter(target);

            string convertedText = converter?.Convert(text, typeof(FrameworkElement), null, null) as string ?? text;

            if (target is TextBox)
            {
                TextBox textBox = (TextBox)target;
                textBox.Text = convertedText;
                textBox.CaretIndex = text?.Length ?? 0;
                //todo: revise, might be useful
                //textBox.SelectAll();
            }
            else if (target is TextBlock)
            {
                ((TextBlock)target).Text = convertedText;
            }
            else if (target is ContentControl)
            {
                ((ContentControl)target).Content = convertedText;
            }
        }

        private static void UpdateTargetsText(IEnumerable<FrameworkElement> items, string text)
        {
            foreach (FrameworkElement obj in items)
                UpdateTargetText(obj, text);
        }

        private static void UpdateInputCaptures(FrameworkElement item, KeyInputCaptureItem capture, string inputText)
        {
            IList inputCaptures = GetInputCaptures(item);

            if (inputCaptures == null)
                return;

            int inputCapturesMaxLength = GetInputCapturesMaxLength(item);

            if (inputCaptures.Count >= inputCapturesMaxLength)
                inputCaptures.RemoveAt(inputCaptures.Count - 1);

            dynamic record = new { StartKey = capture.StartKey.ToString(), StartKeyModifiers = capture.StartKeyModifiers.ToString(), EndKey = capture.EndKey.ToString(), EndKeyModifiers = capture.EndKeyModifiers.ToString(), UtcTimestamp = DateTime.UtcNow, Text = inputText };

            inputCaptures.Insert(0, record);
        }

        private static void HandleSingleKeyCapture(KeyEventArgs e, FrameworkElement obj, KeyInputCaptureItem capture)
        {
            string text = string.Empty;
            RaiseInputCaptureStartedEvent(obj, capture);
            e.Handled = capture.HandleSingleKeyCaptureEvent;
            UpdateInputCaptures(obj, capture, text);
            RaiseInputCaptureCompletedEvent(obj, capture, text);
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine($"{nameof(OnPreviewKeyDown)}:sender={sender};Key={e.Key};Modifiers={e.KeyboardDevice.Modifiers};{e.SystemKey}", nameof(KeyInputCapture));

            FrameworkElement obj = (FrameworkElement)sender;

            KeyInputCaptureCollection inputCaptures = GetInputCaptureItems(obj);

            if (inputCaptures == null)
                return;

            KeyInputCaptureItem currentInputCapture = GetCurrentInputCaptureItem(obj);

            Debug.WriteLine($"{nameof(currentInputCapture)}={currentInputCapture}", nameof(KeyInputCapture));

            //Alt key behaves differently
            //todo: revise system key behaviors
            Key key = e.Key == Key.System
                && e.KeyboardDevice.Modifiers == ModifierKeys.Alt
                ? e.SystemKey
                : e.Key;

            //not started, check for start
            if (currentInputCapture == null)
            {
                currentInputCapture = inputCaptures
                    .FirstOrDefault(o => o.StartKey == key && o.StartKeyModifiers == e.KeyboardDevice.Modifiers);

                if (currentInputCapture != null)
                {
                    if (currentInputCapture.IsSingleKeyCapture)
                    {
                        HandleSingleKeyCapture(e, obj, currentInputCapture);
                    }
                    else
                    {
                        StartInputReading(obj, currentInputCapture, e);
                    }
                }
            }
            else
            {
                RestartInputReadingTimer(obj);

                //test for end key/modifier
                if (currentInputCapture.EndKey == e.Key && currentInputCapture.EndKeyModifiers == e.KeyboardDevice.Modifiers)
                {
                    //end detected
                    EndInputReading(obj, currentInputCapture, e);
                }
                else if (currentInputCapture.StartKey == e.Key && currentInputCapture.StartKeyModifiers == e.KeyboardDevice.Modifiers)
                {
                    //restart reading
                    ClearInputReadings(obj);
                    currentInputCapture = inputCaptures.FirstOrDefault(o => o.StartKey == e.Key && o.StartKeyModifiers == e.KeyboardDevice.Modifiers);
                    StartInputReading(obj, currentInputCapture, e);
                }
            }
        }

        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"{nameof(OnPreviewTextInput)}:sender={sender};Text={e.Text}", nameof(KeyInputCapture));

            FrameworkElement obj = (FrameworkElement)sender;

            //input captures are not defined
            KeyInputCaptureCollection inputCaptures = GetInputCaptureItems(obj);

            //no captures defined
            if (inputCaptures == null)
                return;

            KeyInputCaptureItem currentInputCapture = GetCurrentInputCaptureItem(obj);

            //input reading
            if (currentInputCapture != null)
            {
                string inputText = GetInputText(obj) + e.Text;
                SetInputText(obj, inputText);
                e.Handled = true;
            }
        }
    }
}
