using RmsoftControls.InputCaptureControls;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RmsoftControlsTestApp
{
    /// <summary>
    /// Interaction logic for KeyInputCaptureView.xaml
    /// </summary>
    public partial class KeyInputCaptureView : UserControl
    {
        class KeyInputCaptureCommandImpl : ICommand
        {
            public event EventHandler CanExecuteChanged;

            KeyInputCaptureView Context { get; }

            public KeyInputCaptureCommandImpl(KeyInputCaptureView context)
            {
                Context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public bool CanExecute(object parameter)
            {
                string param = parameter as string ?? "";
                return param.Length > 0;
            }

            public void Execute(object parameter)
            {
                Context.ExecuteKeyInputCaptureCommand((string)parameter);
            }

            internal void RaiseCanExecuteChanged()
            {
                EventHandler h = CanExecuteChanged;
                h?.Invoke(this, EventArgs.Empty);
            }
        }

        public ObservableCollection<dynamic> InputCaptures { get; } = new ObservableCollection<dynamic>();

        public ICommand KeyInputCaptureCommand { get; }

        public KeyInputCaptureView()
        {
            InitializeComponent();
            KeyInputCaptureCommand = new KeyInputCaptureCommandImpl(this);
            InputCaptures.CollectionChanged += InputCaptures_CollectionChanged;
        }

        private void InputCaptures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            Dispatcher.Invoke(() =>
            {
                dynamic selectedItem = InputCaptures.FirstOrDefault();

                if (InputCaptures.Count > 20)
                {
                    ResetInputCaptures(selectedItem);
                }

                inputCapturesSelector.SelectedItem = selectedItem;
            });
        }

        private void ResetInputCaptures(dynamic selectedItem)
        {
            InputCaptures.CollectionChanged -= InputCaptures_CollectionChanged;

            InputCaptures.Clear();

            if (selectedItem != null)
                InputCaptures.Add(selectedItem);

            InputCaptures.CollectionChanged += InputCaptures_CollectionChanged;
        }

        private void ExecuteKeyInputCaptureCommand(string parameter)
        {
            bool isDisabled = KeyInputCapture.GetDisableUpdateTargets(Application.Current.MainWindow);
            if (!isDisabled)
                KeyInputCapture.SetDisableUpdateTargets(Application.Current.MainWindow, true);
            RmsoftControls.Dialogs.Dialog.ShowInformation(Application.Current.MainWindow, $"{nameof(ExecuteKeyInputCaptureCommand)}:{parameter}", "KeyInputCapture");
            if (!isDisabled)
                KeyInputCapture.SetDisableUpdateTargets(Application.Current.MainWindow, false);
        }

        private void UserControl_InputCaptureStarted(object sender, KeyInputCaptureEventArgs e)
        {
            Debug.WriteLine($"{nameof(UserControl_InputCaptureStarted)}:{e}");
        }

        private void UserControl_InputCaptureCompleted(object sender, KeyInputCaptureEventArgs e)
        {
            Debug.WriteLine($"{nameof(UserControl_InputCaptureCompleted)}:{e}");
        }
    }
}
