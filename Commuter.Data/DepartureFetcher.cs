using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Skanetrafiken.API.DepartureArrival;

namespace Commuter.Data
{
    public class DepartureFetcher : IDepartureFetcher
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

            // INFO: Create groups for Departures without a set StopPoint

            var emptyStopPointGroup = departuresByStopPoint.FirstOrDefault(x => x.Key == string.Empty);
            if (emptyStopPointGroup != null)
            {
                foreach (var departure in emptyStopPointGroup)
                {
                    var stopPointName = departure?.RealTime?.RealTimeInfo?.NewDepPoint?.Trim();
                    if(stopPointName == null)
                    {
                        // INFO: Ignore departures without a StopPoint (Position or Track)
                        continue;
                    }
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
            var d = new Departure()
            {
                RunNo = departure.RunNo,
                No = GetLineNo(departure),
                Name = departure.Name,
                Towards = departure.Towards,
                LineType = departure.LineTypeName,
                DepartureTime = departure.JourneyDateTime,
            };

            d.DepartureTimeDeviation = departure?.RealTime?.RealTimeInfo?.DepTimeDeviation ?? null;

            if(d.DepartureTimeDeviation == 0)
            {
                d.DepartureTimeDeviation = null;
            }

            var deviations = new List<Deviation>();
            foreach(var deviation in departure.Deviations)
            {
                deviations.Add(new Deviation()
                {
                    Header = deviation.Header,
                    ShortText = deviation.Details,
                    Importance = deviation.Importance,
                    Urgency = deviation.Urgency,
                    Influence = deviation.Influence
                });
            }
            d.Deviations = deviations;

            return d;
        }

        private static int GetLineNo(GetDepartureArrivalResponseGetDepartureArrivalResultLine departure)
        {
            if(int.TryParse(departure.Name, out var value))
            {
                //Stadsbuss
                return value;
            }

            if(departure.LineTypeId == 2)
            {
                // SkåneExpressen
                return int.Parse(departure.Name.Split(' ').Last());
            }

            return departure.LineTypeId == 32 || departure.LineTypeId == 128 ? departure.TrainNo : departure.No;
        }
    }
}
