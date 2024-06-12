using Newtonsoft.Json;

namespace SteamItemsStatsViewer.Models
{
    public class PriceHistoryModel
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "price_prefix")]
        public string PricePrefix { get; set; }

        [JsonProperty(PropertyName = "price_suffix")]
        public string PriceSuffix { get; set; }

        [JsonProperty(PropertyName = "prices")]
        public List<List<string>> Prices { get; set; }
    }
}