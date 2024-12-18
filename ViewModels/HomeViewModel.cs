using Newtonsoft.Json;
using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Stores;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

            Dictionary<string,double> directoriesDictionary = new Dictionary<string,double>();

            foreach (string directory in directories)
            {
                string[] files = Directory.GetFiles(directory);

                string priceHistoryFilePath = files.First(x => Path.GetFileNameWithoutExtension(x).Contains("Price_History"));

                string priceHistoryFile = File.ReadAllText(priceHistoryFilePath);

                PriceHistoryModel history = JsonConvert.DeserializeObject<PriceHistoryModel>(priceHistoryFile);

                double price = history.PriceHistory.Last().Value;

                directoriesDictionary.Add(directory, price);
            }

            foreach (string directory in directoriesDictionary.OrderByDescending(x => x.Value).Select(x => x.Key))
            {
                string directoryName = Path.GetFileName(directory);
                string name = directoryName.Replace("_", " ");

                string[] files = Directory.GetFiles(directory);

                string image = files.First(x => Path.GetExtension(x).ToLower() == ".png");

                string priceHistoryFilePath = files.First(x => Path.GetFileNameWithoutExtension(x).Contains("Price_History"));

                string priceHistoryFile = File.ReadAllText(priceHistoryFilePath);

                PriceHistoryModel history = JsonConvert.DeserializeObject<PriceHistoryModel>(priceHistoryFile);

                KeyValuePair<DateTime, double> pair = history.PriceHistory.Last();
                string price = Math.Round(pair.Value * App.Settings.ExchangeRate, 2).ToString("N") + App.Settings.Currency;

                string priceThisWeekP = "NO DATA";
                SolidColorBrush priceThisWeekColor = new SolidColorBrush(Colors.White);
                try
                {
                    KeyValuePair<DateTime,double> thisWeek = history.PriceHistory.Last(x => x.Key.Date == DateTime.Now.Date);
                    double priceThisWeek = thisWeek.Value;

                    double priceLastWeek = history.PriceHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-7).Date && x.Key.TimeOfDay == thisWeek.Key.TimeOfDay).Value;

                    double p = (priceThisWeek / priceLastWeek) - 1;

                    priceThisWeekP = p.ToString("P");

                    if (p > 0) priceThisWeekColor = new SolidColorBrush(Colors.Green);
                    else if (p < 0) priceThisWeekColor = new SolidColorBrush(Colors.Red);
                    else priceThisWeekColor = new SolidColorBrush(Colors.Gray);
                } catch (Exception) { }

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(name, image, price, priceThisWeekP, priceThisWeekColor, new RelayCommand(execute => { _navigationStore.ViewModel = new DisplayItemDataViewModel(directory); }));
                _navigationItems.Add(steamItemNavigationItem);
            }
        }
    }
}
