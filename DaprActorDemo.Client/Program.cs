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
             * Record some sales
             */
            var salesData = new float[]
            {
                1.37f,
                2.24f,
                3.99f
            };
            
            foreach (var thisSale in salesData)
            {
                Console.WriteLine($"Registering sale: {thisSale}");
                var totalSales = await proxy.RegisterPurchase(thisSale);
                
            }

            var storeState = await proxy.GetDataAsync();
            Console.WriteLine($"Total sales: {storeState.TotalPurchases}");



            /********************************************
             *
             * Finished
             */
            Console.WriteLine("Store Actor demo finished");
        }
    }
}