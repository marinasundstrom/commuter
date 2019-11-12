using System;
using System.Threading.Tasks;
using Commuter;

namespace ClientTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            OpenApiClient client = new OpenApiClient(new System.Net.Http.HttpClient());
            var c = await client.GetNearestStopAreasAsync(55.707919, 13.186684, 400);
            Console.WriteLine(c.GetType());
        }
    }
}
