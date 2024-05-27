using System.Windows.Input;

namespace SteamItemsStatsViewer.Models
{
    public class SteamItemNavigationItemModel
    {
        public string Title {  get; set; }
        public ICommand Command { get; set; }

        public SteamItemNavigationItemModel(string title, ICommand command)
        {
            Title = title;
            Command = command;
        }
    }
}
