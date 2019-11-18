using System;
using System.Collections.ObjectModel;

namespace Commuter.Models
{
    public class DepartureViewModel : ObservableObject
    {
        private DateTime time;
        private int no;
        private string? name;
        private string? towards;
        private int runNo;
        private string? lineType;
        private DateTime? newTime;

        public int RunNo { get => runNo; set => SetProperty(ref runNo, value); }
        public string? LineType { get => lineType; set => SetProperty(ref lineType, value); }
        public int No { get => no; set => SetProperty(ref no, value); }

        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string? Towards
        {
            get => towards;
            set => SetProperty(ref towards, value);
        }

        public DateTime ActualTime => HasNewTime ? NewTime.GetValueOrDefault() : Time;

        public DateTime Time
        {
            get => time; set
            {
                time = value;
                OnPropertyChanged(nameof(Time));
                OnPropertyChanged(nameof(NewTime));
                OnPropertyChanged(nameof(ActualTime));
            }
        }

        public DateTime? NewTime
        {
            get => newTime;
            set
            {
                newTime = value;
                OnPropertyChanged(nameof(NewTime));
                OnPropertyChanged(nameof(HasNewTime));
                OnPropertyChanged(nameof(ActualTime));
            }
        }

        public bool HasNewTime => NewTime != null;

        public ObservableCollection<DeviationViewModel> Deviations { get; } = new ObservableCollection<DeviationViewModel>();
    }
}
