using System.Collections.Generic;

namespace Commuter.Data
{
    public class StopPoint
    {
        public string? Name { get; set; }

        public List<Departure> Departures { get; set; } = new List<Departure>();
    }
}
