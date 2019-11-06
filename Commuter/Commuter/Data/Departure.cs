using System;

namespace Commuter.Data
{

    public class Departure
    {
        public int RunNo { get; set; }
        public string? LineType { get; set; }
        public int Line { get; set; }
        public string? Name { get; set; }
        public string? Towards { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}
