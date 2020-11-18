using System;
using Xamarin.Essentials;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PokeXamarin.Helpers;
using System.IO;
using PokeXamarin.ViewModels.Interfaces;
using PokeXamarin.ViewModels;
using PokeXamarin.Services;
using MonkeyCache.LiteDB;

namespace PokeXamarin
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            Barrel.ApplicationId = "PokeXamarin";

            var systemDir = FileSystem.CacheDirectory;
            ResourcesHelpers.ExtractSaveResource("PokeXamarin.appsettings.json", systemDir);
            var fullConfig = Path.Combine(systemDir, "PokeXamarin.appsettings.json");

            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                                c.AddJsonFile(fullConfig);
                            })
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o =>
                            {
                                o.DisableColors = true;
                            }))
                            .Build();

            App.ServiceProvider = host.Services;

            return App.ServiceProvider.GetService<App>();
        }


        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
            {

            }

            Infra.ApiURL = ctx.Configuration["ApiUrl"];

            services.AddTransient<IMainViewModel, MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddSingleton<IPokemonService, PokemonService>();
            services.AddSingleton<App>();
        }
    }
}
