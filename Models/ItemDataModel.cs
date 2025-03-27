using Newtonsoft.Json;

namespace SteamItemsStatsViewer.Models
{
    public class ItemDataModel
    {
        public string Name { get; set; }

        public byte[] Icon { get; set; }

        [JsonIgnore]
        public string IconPath { get; set; }

        public Dictionary<DateTime, decimal> PriceHistory { get; set; }
        public Dictionary<DateTime, int> QuantityHistory { get; set; }
    }
}