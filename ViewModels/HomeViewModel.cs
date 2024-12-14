using Newtonsoft.Json;
using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Stores;
using System.Collections.ObjectModel;
using System.IO;

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

                string image = files.First(x => Path.GetExtension(x).ToLower() == ".png");

                string priceHistoryFilePath = files.First(x => Path.GetFileNameWithoutExtension(x).Contains("Price_History"));

                string priceHistoryFile = File.ReadAllText(priceHistoryFilePath);

                PriceHistoryModel history = JsonConvert.DeserializeObject<PriceHistoryModel>(priceHistoryFile);

                KeyValuePair<DateTime, double> pair = history.PriceHistory.Last();
                string price = Math.Round(pair.Value, 2).ToString("N") + App.Settings.Currency;

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(name, image, price, new RelayCommand(execute => { _navigationStore.ViewModel = new DisplayItemDataViewModel(directory); }));
                _navigationItems.Add(steamItemNavigationItem);
            }
        }
    }
}
