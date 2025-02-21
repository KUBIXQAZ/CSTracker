using Newtonsoft.Json;

namespace SteamItemsStatsViewer.DTOs
{
    public class ExchangeRatesModel
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("WARNING_UPGRADE_TO_V6")]
        public string WarningUpgradeToV6 { get; set; }

        [JsonProperty("terms")]
        public string Terms { get; set; }

        [JsonProperty("base")]
        public string BaseCurrency { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("time_last_updated")]
        public long TimeLastUpdated { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}