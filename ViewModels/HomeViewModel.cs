using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Stores;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows.Data;
using System.Xml.Linq;

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

        public async void LoadSteamItemsNavigationItems()
        {
            NavigationItems.Clear();

            List<ItemDataModel> itemsData = new List<ItemDataModel>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.31.71:5000/api/ItemsData/");

                var answer = await client.GetAsync("GetItemsData");

                if(answer.IsSuccessStatusCode)
                {
                    string content = await answer.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(content)) return;

                    itemsData = JsonConvert.DeserializeObject<List<ItemDataModel>>(content)!;
                }
            }

            try
            {
                foreach (ItemDataModel item in itemsData)
                {
                    Random rng = new Random();

                    item.IconPath = $"{App.IconFolder}\\{rng.Next(int.MaxValue)}.png";

                    File.WriteAllBytes(item.IconPath, item.Icon);
                }
            }
            catch (Exception) { }

            foreach (ItemDataModel item in itemsData)
            {
                string price = Math.Round(item.PriceHistory.Last().Value * App.Settings.ExchangeRate, 2).ToString("N") + App.Settings.Currency;

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(item.Name, item.IconPath, price, new RelayCommand(execute => { _navigationStore.ViewModel = new DisplayItemDataViewModel(); }), this);
                NavigationItems.Add(steamItemNavigationItem);
            }

            //foreach(string name in names)
            //{
            //    SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(name, image, price, priceThisWeekP, priceThisWeekColor, new RelayCommand(execute => { _navigationStore.ViewModel = new DisplayItemDataViewModel(directory); }), isFav, this);
            //    _navigationItems.Add(steamItemNavigationItem);
            //}

            /*
            string path = "D:\\Windows\\Desktop\\output\\";
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
                        return fav.Contains(Path.GetFileName(x));
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
                        return !fav.Contains(Path.GetFileName(x));
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
                string name = WebUtility.UrlDecode(directoryName);

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
                            if (Uri.EscapeDataString(name) == item) isFav = true;
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
            */
        }
    }
}
