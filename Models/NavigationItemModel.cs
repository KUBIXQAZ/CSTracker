using SteamItemsStatsViewer.MVVM;

namespace SteamItemsStatsViewer.Models
{
    public class NavigationItemModel
    {
        public string Title { get; set; }
        public RelayCommand Command { get; set; }

        public NavigationItemModel(string title, RelayCommand command)
        {
            Title = title;
            Command = command;
        }
    }
}