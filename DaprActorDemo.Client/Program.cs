using System;
using System.Collections.Generic;
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

            // Create a proxy that will access the actor
            var proxy = ActorProxy.Create<IStoreActor>(actorId, actorType);
            
            // See if the store actor already exists
            try
            {
                var temp = await proxy.GetDataAsync();
            }
            catch (Exception)
            {
                // The store actor doesn't exist yet, so create it
                var response = await proxy.SetDataAsync(new StoreInfo()
                {
                    Address = "1 Main Street, London, W1",
                    Latitude = 123456,
                    Longitude = 67890,
                    Telephone = "0171 123 4567"
                });
                
                Console.WriteLine("Store created");
            }

            // Un-register any previous reminders
            await proxy.UnregisterReminder();
            
            var savedData = await proxy.GetDataAsync();
            Console.WriteLine(savedData);
            
            
            /*****************************************
             *
             * Register the store actor reminder
             */
            await proxy.RegisterReminder();
            
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