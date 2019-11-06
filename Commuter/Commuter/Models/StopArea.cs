using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Commuter.Models
{

    public class StopArea : ObservableCollection<StopPoint>
    {
        private int id;
        private string? name;
        private float x;
        private float y;
        private int distance;

        public int StopAreaId { get => id; set => SetProperty(ref id, value); }
        public string? Name { get => name; set => SetProperty(ref name, value); }
        public float X { get => x; set => SetProperty(ref x, value); }
        public float Y { get => y; set => SetProperty(ref y, value); }

        public int Distance
        {
            get => distance; set
            {
                SetProperty(ref distance, value);
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Header)));
            }
        }

        public string Header => $"{Name} ({Distance} metres)";

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
