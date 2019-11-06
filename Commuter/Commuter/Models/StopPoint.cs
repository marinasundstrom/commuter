using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Commuter.Models
{
    public class StopPoint : ObservableCollection<Departure>
    {
        private string? name;

        public string? Name { get => name; set => SetProperty(ref name, value); }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (object.Equals(value, field))
            {
                return false;
            }

            field = value;

            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

            return true;

        }
    }
}
