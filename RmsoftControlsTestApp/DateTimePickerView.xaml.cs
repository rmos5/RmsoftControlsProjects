using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RmsoftControlsTestApp
{
    /// <summary>
    /// Interaction logic for DateTimePickerView.xaml
    /// </summary>
    public partial class DateTimePickerView : UserControl
    {
        public DateTimePickerView()
        {
            InitializeComponent();
            dateTimePicker1.SelectedDateChanged += DateTimePicker1_SelectedDateChanged;
            dateTimePicker1.Loaded += DateTimePicker1_Loaded;
        }

        private void DateTimePicker1_Loaded(object sender, RoutedEventArgs e)
        {
            dateTimePicker1.SelectedDate = DateTime.Now;
        }

        private void DateTimePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine($"{nameof(DateTimePicker1_SelectedDateChanged)}:added1={e.AddedItems.Cast<DateTime?>()?.FirstOrDefault()};removed1{ e.RemovedItems?.Cast<DateTime?>()?.FirstOrDefault()}", GetType().Name);
        }

        private void NextDateButton_Click(object sender, RoutedEventArgs e)
        {
            dateTimePicker1.SelectedDate = dateTimePicker1.SelectedDate?.AddDays(1);
        }

        private void PreviousDateButton_Click(object sender, RoutedEventArgs e)
        {
            dateTimePicker1.SelectedDate = dateTimePicker1.SelectedDate?.AddDays(-1);
        }
    }
}
