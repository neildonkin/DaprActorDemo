using System;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Client;
using DaprActorDemo.Interfaces;
using StoreActorService.Actors;

namespace DaprActorDemo.Client
{
    class Program
    {
        // NOTE: you must run this console app once to create the default actor
        public static async Task Main(string[] args)
        {
            var actorType = "StoreActor";      // Registered Actor Type in Actor Service
            var actorId = new ActorId("1");

            // Create the local proxy by using the same interface that the service implements
            // By using this proxy, you can call strongly typed methods on the interface using Remoting.
            var proxy = ActorProxy.Create<IStoreActor>(actorId, actorType);
            var response = await proxy.SetDataAsync(new StoreInfo()
            {
                Address = "1 Main Street, London, W1",
                Latitude = 123456,
                Longitude = 67890,
                Telephone = "0171 123 4567"
            });
            
            Console.WriteLine(response);

            var savedData = await proxy.GetDataAsync();
            Console.WriteLine(savedData);
            
            
            /*****************************************
             *
             * Record a sale
             */
            var totalSales = await proxy.RegisterPurchase((float)1.37);
            Console.WriteLine($"Total sales: {totalSales}");



            /********************************************
             *
             * Read key to exit
             */
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}