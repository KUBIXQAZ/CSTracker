using CSTracker.Models;
using CSTracker.MVVM;
using CSTracker.Services;
using CSTracker.Stores;
using CSTracker.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;

namespace CSTracker.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public NavigationStore _navigationStore;

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

            RefreshListCommand = new RelayCommand(async execute => await LoadSteamItemsNavigationItems());
            
            RefreshListCommand.Execute(this);
        }

        public async Task LoadSteamItemsNavigationItems()
        {
            NavigationItems.Clear();

            List<ItemDataModel> itemsData = new List<ItemDataModel>();

            try
            {
                using (HttpClient client = new HttpClientService().CreateHttpClient(App.BaseApiUrl, App.ApiKey))
                {
                    client.BaseAddress = new Uri(App.BaseApiUrl);

                    var answer = await client.GetAsync("ItemData/GetItemsData");

                    if (answer.IsSuccessStatusCode)
                    {
                        string content = await answer.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(content)) return;

                        itemsData = JsonConvert.DeserializeObject<List<ItemDataModel>>(content)!;

                        foreach (var item in itemsData)
                        {
                            var updatedPriceHistory = item.PriceHistory
                                .ToDictionary(
                                    x => x.Key.ToLocalTime(),
                                    x => x.Value
                                );

                            item.PriceHistory = updatedPriceHistory;

                            var updatedQuantityHistory = item.QuantityHistory
                                .ToDictionary(
                                    x => x.Key.ToLocalTime(),
                                    x => x.Value
                                );

                            item.QuantityHistory = updatedQuantityHistory;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            } 
            catch (HttpRequestException)
            {
                AlertWindow alertWindow = new AlertWindow();
                alertWindow.Owner = App.Current.MainWindow;
                AlertViewModel vm = new AlertViewModel(alertWindow, "Error", "An unexpected issue occurred while loading items. Please try again.", Enums.AlertTypeEnum.Error);
                alertWindow.DataContext = vm;
                alertWindow.ShowDialog();
            }

            string favItemsPath = App.MainDataFolder + "/FavouriteItems.json";

            List<string> favItems = new List<string>();

            if (Path.Exists(favItemsPath))
            {
                string f = File.ReadAllText(favItemsPath);
                favItems = JsonConvert.DeserializeObject<List<string>>(f);
                
                ItemDataModel[]? matchedItems = itemsData.Where(x => favItems.Contains(x.Name)).ToArray();
                ItemDataModel[]? otherItems = itemsData.Where(x => !favItems.Contains(x.Name)).ToArray();

                itemsData = matchedItems.Concat(otherItems).ToList();
            }

            foreach (ItemDataModel itemData in itemsData)
            {
                bool isFav = favItems.Contains(itemData.Name);

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(itemData, isFav, this);
                NavigationItems.Add(steamItemNavigationItem);
            }
        }
    }
}