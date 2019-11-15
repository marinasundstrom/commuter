using System;
using System.Collections.Generic;

namespace Commuter.Data
{

    public class Departure
    {
        public int RunNo { get; set; }
        public string? LineType { get; set; }
        public int No { get; set; }
        public string? Name { get; set; }
        public string? Towards { get; set; }
        public DateTime DepartureTime { get; set; }
        public int? DepartureTimeDeviation { get; set; }
        public IEnumerable<Deviation>? Deviations { get; set; }
    }
}
