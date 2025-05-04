using Newtonsoft.Json;

namespace CSTracker.Models
{
    public class ItemDataModel
    {
        public string Name { get; set; }

        public byte[] Image { get; set; }

        [JsonIgnore]
        public string IconPath { get; set; }

        public Dictionary<DateTime, decimal> PriceHistory { get; set; }
        public Dictionary<DateTime, int> QuantityHistory { get; set; }
    }
}