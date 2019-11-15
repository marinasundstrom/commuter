using System;
namespace Commuter.Services
{
    public struct Location
    {
        public Location(double latitude, double longitude, double? altitude = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }
        public double? Altitude { get; }
    }
}
