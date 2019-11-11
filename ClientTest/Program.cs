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
            var c = await client.GetNearestStopAreasAsync(55.608975, 12.9985393, 400);
            Console.WriteLine(c.GetType());
        }
    }
}
