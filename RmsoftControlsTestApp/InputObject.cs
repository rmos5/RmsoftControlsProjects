using System;
using System.ComponentModel;

namespace RmsoftControlsTestApp
{
    public class InputObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; private set; }

        private int count = 1;

        public int Count
        {
            get { return count; }
            private set { count = value; OnPropertyChanged(nameof(Count)); }
        }

        private DateTime updated;

        public DateTime Updated
        {
            get { return updated; }
            private set { updated = value; OnPropertyChanged(nameof(Updated)); }
        }

        public InputObject(string name)
        {
            Name = name;
            Updated = DateTime.Now;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode()
                ^ Count.GetHashCode()
                ^ Updated.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj?.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Updated}\t{Name}\t{Count}";
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            h?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateCount()
        {
            Count += 1;
        }
    }
}
