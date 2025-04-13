using CSTracker.MVVM;

namespace CSTracker.Models
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