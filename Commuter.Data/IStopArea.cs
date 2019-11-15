using System.Collections.Generic;

namespace Commuter.Data
{
    public interface IStopArea
    {
        int StopAreaId { get;}
        IEnumerable<StopPoint> StopPoints { get; }
        string? Name { get; }
        int Distance { get; }
        float X { get; }
        float Y { get; }
    }
}