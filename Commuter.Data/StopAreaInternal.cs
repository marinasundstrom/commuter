using System.Collections.Generic;

namespace Commuter.Data
{
    public struct StopAreaInternal : IStopArea
    {
        public int StopAreaId { get; set; }
        public IEnumerable<StopPoint> StopPoints { get; set; }
        public string? Name { get; set; }
        public int Distance { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public StopAreaInternal(int stopAreaId, string name, int distance, float x, float y, IEnumerable<StopPoint> stopPoints)
        {
            StopAreaId = stopAreaId;
            Name = name;
            Distance = distance;
            X = x;
            Y = y;
            StopPoints = stopPoints;
        }

        public override bool Equals(object? obj)
        {
            return obj is StopAreaInternal other &&
                   StopAreaId == other.StopAreaId &&
                   Name == other.Name &&
                   Distance == other.Distance &&
                   X == other.X &&
                   Y == other.Y &&
                   EqualityComparer<IEnumerable<StopPoint>>.Default.Equals(StopPoints, other.StopPoints);
        }

        public override int GetHashCode()
        {
            var hashCode = 650313786;
            hashCode = hashCode * -1521134295 + StopAreaId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Distance.GetHashCode();
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<StopPoint>>.Default.GetHashCode(StopPoints);
            return hashCode;
        }
    }
}
