using Newtonsoft.Json;
using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Stores;
using System.Collections.ObjectModel;
using System.IO;
using System.Printing;
using System.Windows.Media;

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

        public RelayCommand RefreshListCommand { get; set; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            RefreshListCommand = new RelayCommand(execute => LoadSteamItemsNavigationItems());

            LoadSteamItemsNavigationItems();
        }

        public void LoadSteamItemsNavigationItems()
        {
            NavigationItems.Clear();

            string path = "D:\\PROGRAMMING-PROJECTS\\csharp-apps\\SteamMarketDataCollector\\bin\\Debug\\net8.0\\output";
            string[] directories = Directory.GetDirectories(path);

            string[] directoriesDictionary = new string[directories.Length];

            string favItemsPath = App.MainDataFolder + "/FavouriteItems.json";
            if (Path.Exists(favItemsPath))
            {
                string f = File.ReadAllText(favItemsPath);
                string[] fav = JsonConvert.DeserializeObject<string[]>(f);

                string[]? matchedNames = directories.Where(x =>
                {
                    if(fav != null)
                    {
                        return fav.Contains(Path.GetFileName(x).Replace("_", " "));
                    } 
                    else
                    {
                        return false;
                    }
                    
                }).ToArray();
                string[] unmatchedNames = directories.Where(x =>
                {
                    if(fav != null)
                    {
                        return !fav.Contains(Path.GetFileName(x).Replace("_", " "));
                    } 
                    else
                    {
                        return true;
                    }
                }).ToArray();

                directoriesDictionary = matchedNames.Concat(unmatchedNames).ToArray();
            }
            else
            {
                directoriesDictionary = directories;
            }

            foreach (string directory in directoriesDictionary)
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

                bool isFav = false;
                
                string filePath = App.MainDataFolder + "/FavouriteItems.json";
                if(File.Exists(filePath))
                {
                    string file = File.ReadAllText(filePath);

                    List<string> favItems = JsonConvert.DeserializeObject<List<string>>(file);

                    if(favItems != null)
                    {
                        foreach (string item in favItems)
                        {
                            if (name == item) isFav = true;
                        }
                    }
                } else
                {
                    var f = File.Create(filePath);
                    f.Close();
                }

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(name, image, price, priceThisWeekP, priceThisWeekColor, new RelayCommand(execute => { _navigationStore.ViewModel = new DisplayItemDataViewModel(directory); }), isFav, this);
                _navigationItems.Add(steamItemNavigationItem);
            }
        }
    }
}
