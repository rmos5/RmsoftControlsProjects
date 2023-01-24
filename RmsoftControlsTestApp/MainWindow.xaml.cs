using RmsoftControls.InputCaptureControls;
using RmsoftControlsTestApp.Properties;
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace RmsoftControlsTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CollectionViewSource CultureDataViewSource => Resources["cultureDataViewSource"] as CollectionViewSource;

        bool IsInitializing = false;

        public MainWindow()
        {
            InitializeComponent();

            IsInitializing = true;

            CultureDataViewSource.Source = new CultureInfo[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("fi-FI"),
                new CultureInfo("et-EE"),
            };
            languageSelector.SelectedItem = Settings.Default.SelectedCultureInfo;
            Language = XmlLanguage.GetLanguage(Settings.Default.SelectedCultureInfo.TextInfo.CultureName);
            fontSizeSelector.SelectedItem = Settings.Default.SelectedFontSize;

            IsInitializing = false;
        }

        bool AskForApplicationClose(string message)
        {
            return RmsoftControls.Dialogs.Dialog.ShowQuestion(this, message) == true;
        }

        private void UpdateLanguage(CultureInfo cultureInfo)
        {

            if (cultureInfo != null && cultureInfo != Settings.Default.SelectedCultureInfo)
            {
                if (AskForApplicationClose("Application language change requires application restart. Restart application?"))
                {
                    Settings.Default.SelectedCultureInfo = cultureInfo;
                    Language = XmlLanguage.GetLanguage(cultureInfo.TextInfo.CultureName);
                    Close();
                }
                else
                {
                    languageSelector.SelectedItem = Settings.Default.SelectedCultureInfo;
                }
            }
        }

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine($"{nameof(LanguageSelector_SelectionChanged)}: value={languageSelector.SelectedValue}", GetType().Name);
            if (IsInitializing)
                return;

            UpdateLanguage(languageSelector.SelectedItem as CultureInfo);
        }

        bool appThemeChangeCancel = false;
        private void AppThemeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            if (appThemeChangeCancel)
            {
                appThemeChangeCancel = false;
                return;
            }

            DictionaryEntry? added = e.AddedItems.Cast<DictionaryEntry>().FirstOrDefault();
            DictionaryEntry? removed = e.RemovedItems.Cast<DictionaryEntry>().FirstOrDefault();

            if (AskForApplicationClose("Application theme change requires application restart. Please restart application."))
            {
                Settings.Default.AppThemeResourceUri = (Uri)added.Value.Value;
                Close();
            }
            else
            {
                appThemeChangeCancel = true;
                appThemeSelector.SelectedItem = removed;
            }
        }

        private void Window_InputCaptureStarted(object sender, KeyInputCaptureEventArgs e)
        {
            Debug.WriteLine($"{nameof(Window_InputCaptureStarted)}:{e}");
        }

        private void Window_InputCaptureCompleted(object sender, KeyInputCaptureEventArgs e)
        {
            Debug.WriteLine($"{nameof(Window_InputCaptureCompleted)}:{e}");
        }
    }
}
