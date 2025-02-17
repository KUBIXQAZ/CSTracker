namespace SteamItemsStatsViewer.Models
{
    public class ItemDataModel
    {
        public string Name { get; set; }

        public byte[] Icon { get; set; }
        public string IconPath { get; set; } = String.Empty;

        public Dictionary<DateTime, decimal> PriceHistory { get; set; }
        public Dictionary<DateTime, int> QuantityHistory { get; set; }
    }
}