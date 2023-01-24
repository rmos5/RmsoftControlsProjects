using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RmsoftControlsTestApp
{
    /// <summary>
    /// Interaction logic for CustomView.xaml
    /// </summary>
    public partial class CustomView : UserControl
    {
        public Func<bool, bool> CloseFunction { get; set; }

        public string ApplyButtonTitle
        {
            get { return (string)GetValue(ApplyButtonTitleProperty); }
            set { SetValue(ApplyButtonTitleProperty, value); }
        }

        public static readonly DependencyProperty ApplyButtonTitleProperty =
            DependencyProperty.Register("ApplyButtonTitle", typeof(string), typeof(CustomView), new PropertyMetadata(Properties.Resources.ApplyButtonTitle));

        public string IgnoreButtonTitle
        {
            get { return (string)GetValue(IgnoreButtonTitleProperty); }
            set { SetValue(IgnoreButtonTitleProperty, value); }
        }

        public static readonly DependencyProperty IgnoreButtonTitleProperty =
            DependencyProperty.Register("IgnoreButtonTitle", typeof(string), typeof(CustomView), new PropertyMetadata(Properties.Resources.IgnoreButtonTitle));


        public CustomView()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // do your stuff and call close at the end
            CloseFunction?.Invoke(true);
        }

        private void IgnoreButton_Click(object sender, RoutedEventArgs e)
        {
            // do your stuff and call close at the end
            CloseFunction?.Invoke(false);
        }
    }
}
