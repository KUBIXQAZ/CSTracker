namespace SteamItemsStatsViewer.Models
{
    public class ItemDataModel
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Currency {  get; set; }

        public DateTime DataSaveDateTime { get; set; }
    }
}
