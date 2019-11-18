using System;
using System.IO;
using System.Net.Http;

using Commuter.Data;
using Commuter.Helpers;
using Commuter.Models;
using Commuter.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Polly;

using Xamarin.Essentials;

namespace Commuter
{
    public class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection>? nativeConfigureServices = null)
        {
            var systemDir = FileSystem.CacheDirectory;
            Utils.ExtractSaveResource("Commuter.appsettings.json", systemDir);
            var fullConfig = Path.Combine(systemDir, "Commuter.appsettings.json");

            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                                c.AddJsonFile(fullConfig, optional: true);
                            })
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices?.Invoke(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o =>
                            {
                                o.DisableColors = true;
                            }))
                            .Build();

            return host.Services.GetService<App>();
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                var world = ctx.Configuration["Hello"];
            }

            services.AddHttpClient<OpenApiClient>()
                 .ConfigurePrimaryHttpMessageHandler(() => HttpClientHandler())
                 .ConfigureHttpClient(httpClient => httpClient.Timeout = TimeSpan.FromSeconds(5))
                 .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                     {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10)
                     }));

            services
                .AddTransient<IStopAreaFetcher, StopAreaFetcher>()
                .AddTransient<IDepartureFetcher, DepartureFetcher>()
                .AddTransient<IDataFetcher, DataFetcher>()
                .AddTransient<IDepartureBoardPeriodicUpdater, DepartureBoardPeriodicUpdater>()
                .AddTransient<IGeoLocationService, GeoLocationFactory>();

            services.AddTransient<IStopAreaViewModelFactory, StopAreaViewModelFactory>()
                .AddTransient<IStopPointViewModelFactory, StopPointViewModelFactory>()
                .AddTransient<IDepartureViewModelFactory, DepartureViewModelFactory>()
                .AddTransient<IDeviationViewModelFactory, DeviationViewModelFactory>();

            services
                .AddSingleton<IAlertService, AlertService>()
                .AddSingleton<IThreadDispatcher, ThreadDispatcher>();

            services
              .AddSingleton<MainViewModel>()
              .AddSingleton<IDepartureBoard, DepartureBoardViewModel>();

            services
             .AddSingleton<App>()
             .AddTransient<MainPage>();
        }

        public static HttpClientHandler HttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                }
            };
            return handler;
        }
    }
}
