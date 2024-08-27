using SteamItemsStatsViewer.Commands;

namespace SteamItemsStatsViewer.Models
{
    public class SteamItemNavigationItemModel
    {
        public string Title {  get; set; }
        public string Image {  get; set; }
        public RelayCommand Command { get; set; }

        public SteamItemNavigationItemModel(string title, string image, RelayCommand command)
        {
            Title = title;
            Image = image;
            Command = command;
        }
    }
}