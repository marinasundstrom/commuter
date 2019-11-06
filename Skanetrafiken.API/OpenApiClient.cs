using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Commuter
{
    internal static class ApiVersion
    {
        public static readonly string V2_1 = "2.1";

        public static readonly string V2_2 = "2.2";
    }

    public class OpenApiClient : IDisposable
    {
        public HttpClient HttpClient { get; }

        public OpenApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient;

            httpClient.BaseAddress = new Uri($"http://www.labs.skanetrafiken.se/v{ApiVersion.V2_2}/");
        }

        public async Task<IEnumerable<Skanetrafiken.API.NearestStopArea.GetNearestStopAreaResponseGetNearestStopAreaResultNearestStopArea>> GetNearestStopAreasAsync(double x, double y, int radius)
        {
            var result = await Get<Skanetrafiken.API.NearestStopArea.Envelope>(
                new Uri($"neareststation.asp?x={x.ToString(System.Globalization.CultureInfo.InvariantCulture)}&y={y.ToString(System.Globalization.CultureInfo.InvariantCulture)}&Radius={radius}", UriKind.Relative));

            return result.Body.GetNearestStopAreaResponse.GetNearestStopAreaResult.NearestStopAreas;
        }

        public async Task<IEnumerable<Skanetrafiken.API.DepartureArrival.GetDepartureArrivalResponseGetDepartureArrivalResultLine>> GetGetDepartureArrivalsAsync(int stopAreaId, DateTime departureDateTime)
        {
            // &inpDate={departureDateTime.ToString("yyyy-mm-dd").Replace("-", "%E2%88%92")}&inpTime={departureDateTime.ToString("HH:mm").Replace(":", "%3A")}&selDirection=0

            var uri = new Uri($"stationresults.asp?selPointFrKey={stopAreaId}", UriKind.Relative);

            var result = await Get<Skanetrafiken.API.DepartureArrival.Envelope>(
                uri);

            return result.Body.GetDepartureArrivalResponse.GetDepartureArrivalResult.Lines;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }

        private async Task<T> Get<T>(Uri url)
        {
            using var stream = await HttpClient.GetStreamAsync(url);
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(stream);
        }
    }
}
