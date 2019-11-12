namespace Commuter.Models
{
    public class Deviation : ObservableObject
    {
        private string? shortText;
        private string? header;

        public string? Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        public string? ShortText
        {
            get => shortText;
            set => SetProperty(ref shortText, value);
        }

        public int Importance { get; set; }

        public int Influence { get; set; }

        public int Urgency { get; set; }
    }
}
