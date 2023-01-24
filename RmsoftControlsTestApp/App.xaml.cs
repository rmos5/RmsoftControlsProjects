using RmsoftControlsTestApp.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace RmsoftControlsTestApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ListDictionary AppThemeResourceUris { get; } =
            new ListDictionary
            {
                { "Classic", new Uri("/PresentationFramework.Classic;component/themes/Classic.xaml", UriKind.Relative) },
                { "Luna", new Uri("/PresentationFramework.Luna;component/themes/Luna.NormalColor.xaml", UriKind.Relative) },
                { "Royale", new Uri("/PresentationFramework.Royale;component/themes/Royale.NormalColor.xaml", UriKind.Relative) },
                { "Aero", new Uri("/PresentationFramework.Aero;component/themes/Aero.NormalColor.xaml", UriKind.Relative) },
                { "Aero2", new Uri("/PresentationFramework.Aero2;component/themes/Aero2.NormalColor.xaml", UriKind.Relative) },
                { "Aero Lite", new Uri("/PresentationFramework.AeroLite;component/themes/AeroLite.NormalColor.xaml", UriKind.Relative) },
            };

        public App()
        {
            //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ApplyCultureInfo();
            ApplyAppTheme();
        }

        private void ApplyCultureInfo()
        {
            CultureInfo ci = Settings.Default.SelectedCultureInfo;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        private IEnumerable<Uri> GetAppThemeResourceUris(ListDictionary items)
        {
            foreach (DictionaryEntry entry in items)
                yield return (Uri)entry.Value;
        }

        private void ApplyAppTheme()
        {
            Settings.Default.AppThemeResourceUris = AppThemeResourceUris;
            List<Uri> themeUris = GetAppThemeResourceUris(AppThemeResourceUris).ToList();
            Uri themeUri = Settings.Default.AppThemeResourceUri;
            themeUri = themeUris.FirstOrDefault(o => o == themeUri) ?? (Uri)AppThemeResourceUris["Classic"];
            Settings.Default.AppThemeResourceUri = themeUri;
            Debug.WriteLine($"Adding app theme resource: {themeUri}", GetType().Name);
            ResourceDictionary themeResource = (ResourceDictionary)LoadComponent(themeUri);
            Resources.MergedDictionaries.Add(themeResource);
        }
    }
}
