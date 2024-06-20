namespace SteamItemsStatsViewer.Models
{
    public class PriceHistoryModel
    {
        public string Currency { get; set; }
        public Dictionary<DateTime, double> PriceHistory { get; set; }
    }
}