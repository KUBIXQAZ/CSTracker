using SteamItemsStatsViewer.Commands;
using System.Windows.Media;

namespace SteamItemsStatsViewer.Models
{
    public class SteamItemNavigationItemModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string PriceThisWeek { get; set; }
        public SolidColorBrush PriceThisWeekColor {  get; set; }
        public RelayCommand Command { get; set; }

        public SteamItemNavigationItemModel(string title, string image, string price, string priceThisWeek, SolidColorBrush priceThisWeekColor, RelayCommand command)
        {
            Title = title;
            Image = image;
            Price = price;
            PriceThisWeek = priceThisWeek;
            PriceThisWeekColor = priceThisWeekColor;
            Command = command;
        }
    }
}