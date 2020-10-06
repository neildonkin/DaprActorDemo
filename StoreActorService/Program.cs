using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors.AspNetCore;
using Dapr.Actors.Runtime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoreActorService.Actors;

namespace StoreActorService
{
    public class Program
    {
        private const int AppChannelHttpPort = 3000;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseActors(actorRuntime =>
                        {
                            // Register MyActor actor type

                            // Actors with no constructor dependency arguments can be registered like this
                            // actorRuntime.RegisterActor<StoreActor>();
                            
                            // If you want to inject a dependency into an Actor, use this method
                            // TODO: inject ILogger into StoreActor
                            actorRuntime.RegisterActor<StoreActor>((type) =>
                                new ActorService(type,
                                    (actorService, actorId) =>
                                        new StoreActor(actorService, actorId)));
                        })
                        .UseUrls($"http://localhost:{AppChannelHttpPort}/");
                });
    }
}