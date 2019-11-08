using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Skanetrafiken.API.DepartureArrival;

namespace Commuter.Data
{
    public class DepartureFetcher
    {
        private readonly OpenApiClient client;

        public DepartureFetcher(OpenApiClient openApiClient)
        {
            client = openApiClient;
        }

        public async Task<IEnumerable<StopPoint>> GetDeparturesByStopPointAsync(int stopArea, DateTime departureTime = default, CancellationToken cancellationToken = default)
        {
            var departures = await client.GetGetDepartureArrivalsAsync(stopArea, departureTime);
            var departuresByStopPoint = departures.GroupBy(x => x.StopPoint);

            var stopPoints = new List<StopPoint>();

            // INFO: Create groups for Departures without a StopPoint

            var emptyStopPointGroup = departuresByStopPoint.FirstOrDefault(x => x.Key == string.Empty);
            if (emptyStopPointGroup != null)
            {
                foreach (var departure in emptyStopPointGroup)
                {
                    var stopPointName = departure?.RealTime?.RealTimeInfo?.NewDepPoint?.Trim() ?? "Unspecified";
                    var stopPoint = stopPoints.FirstOrDefault(x => x.Name == stopPointName);
                    if (stopPoint == null)
                    {
                        stopPoint = new StopPoint()
                        {
                            Name = stopPointName,
                        };
                        stopPoints.Add(stopPoint);
                    }

                    stopPoint.Departures.Add(CreateDeparture(departure!));
                }
            }

            // INFO: Create groups for Departures with StopPoints

            foreach (var stopPointGroup in departuresByStopPoint.Where(x => x.Key != string.Empty))
            {
                foreach (var departure in stopPointGroup!)
                {
                    var stopPoint = stopPoints.FirstOrDefault(x => x.Name == stopPointGroup.Key);
                    if (stopPoint == null)
                    {
                        stopPoint = new StopPoint()
                        {
                            Name = departure.StopPoint
                        };
                        stopPoints.Add(stopPoint);
                    }

                    stopPoint.Departures.Add(CreateDeparture(departure));
                }
            }

            return stopPoints;
        }

        private static Departure CreateDeparture(GetDepartureArrivalResponseGetDepartureArrivalResultLine departure)
        {
            return new Departure()
            {
                RunNo = departure.RunNo,
                Line = departure.No,
                Name = departure.Name,
                Towards = departure.Towards,
                LineType = departure.LineTypeName,
                DepartureTime = departure.JourneyDateTime
            };
        }
    }
}
