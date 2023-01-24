using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RmsoftControlsTestApp
{
    public class SomeBindableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int intValue;

        public int IntValue
        {
            get { return intValue; }
            set
            {

                if (value != intValue)
                {
                    intValue = value;
                    OnPropertyChanged(nameof(IntValue));
                }
            }
        }

        private double doubleValue;

        public double DoubleValue
        {
            get { return doubleValue; }
            set
            {

                if (value != doubleValue)
                {
                    doubleValue = value;
                    OnPropertyChanged(nameof(DoubleValue));
                }
            }
        }

        private decimal decimalValue;

        public decimal DecimalValue
        {
            get { return decimalValue; }
            set
            {

                if (value != decimalValue)
                {
                    decimalValue = value;
                    OnPropertyChanged(nameof(DecimalValue));
                }
            }
        }

        private string stringValue;

        public string StringValue
        {
            get { return stringValue; }
            set
            {

                if (value != stringValue)
                {
                    stringValue = value;
                    OnPropertyChanged(nameof(StringValue));
                }
            }
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            h?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
