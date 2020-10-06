using System;
using System.Threading.Tasks;
using Dapr.Actors;

namespace DaprActorDemo.Interfaces
{
    public interface IStoreActor : IActor
    {
        Task<string> SetDataAsync(StoreInfo data);
        Task<StoreInfo> GetDataAsync();
        Task RegisterReminder();
        Task UnregisterReminder();
        Task RegisterTimer();
        Task UnregisterTimer();

        Task<float> RegisterPurchase(float amount);
    }

    public class StoreInfo
    {
        public int Latitude { get; set; }

        public int Longitude { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public float TotalPurchases { get; set; }
    }
}