using System.ComponentModel;
using System.Diagnostics;

namespace RmsoftControlsTestApp.Properties
{
    public sealed partial class Settings 
    {
        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine($"{nameof(OnPropertyChanged)}:PropertyName={e.PropertyName}", GetType().Name);
            base.OnPropertyChanged(sender, e);
            Save();
        }
    }
}
