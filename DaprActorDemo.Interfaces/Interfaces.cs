using System;
using System.Threading.Tasks;

namespace DaprActorDemo.Interfaces
{
    public interface IStoreActor
    {
        Task<string> SetDataAsync(StoreInfo data);
        Task<StoreInfo> GetDataAsync();
        Task RegisterReminder();
        Task UnregisterReminder();
        Task RegisterTimer();
        Task UnregisterTimer();
    }

    public class StoreInfo
    {
        public string Id { get; set; }
        
        public int Latitude { get; set; }

        public int Longitude { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }
    }
}