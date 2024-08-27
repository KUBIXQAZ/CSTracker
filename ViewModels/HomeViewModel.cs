using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace SteamItemsStatsViewer.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        private ObservableCollection<SteamItemNavigationItemModel> _navigationItems = new ObservableCollection<SteamItemNavigationItemModel>();
        public ObservableCollection<SteamItemNavigationItemModel> NavigationItems
        {
            get => _navigationItems;
            set
            {
                _navigationItems = value;
                OnPropertyChanged(nameof(NavigationItems));
            }
        }

        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            LoadSteamItemsNavigationItems();
        }

        private void LoadSteamItemsNavigationItems()
        {
            string path = "D:\\PROGRAMMING-PROJECTS\\csharp-apps\\SteamMarketDataCollector\\bin\\Debug\\net8.0\\output";

            string[] directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {
                string directoryName = Path.GetFileName(directory);
                string name = directoryName.Replace("_", " ");

                string[] files = Directory.GetFiles(directory);
                string image = $"{directory}\\{directoryName}_Image.png";

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(name, image, new RelayCommand(execute => { _navigationStore.ViewModel = new DisplayItemDataViewModel(directory); }));
                _navigationItems.Add(steamItemNavigationItem);
            }
        }
    }
}
