using Newtonsoft.Json;
using SteamItemsStatsViewer.MVVM;
using SteamItemsStatsViewer.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;

namespace SteamItemsStatsViewer.Models
{
    public class SteamItemNavigationItemModel : INotifyPropertyChanged
    {
        private HomeViewModel _viewModel {  get; set; }
        private ItemDataModel _itemData {  get; set; }

        public string Title { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string PriceThisWeek { get; set; }
        public SolidColorBrush PriceThisWeekColor {  get; set; }
        public RelayCommand Command { get; set; }

        public bool FavState { get; set; }
        private string favImageSource;
        public string FavImageSource
        {
            get => favImageSource;
            set
            {
                favImageSource = value;
                OnPropertyChanged(nameof(FavImageSource));
            }
        }
        public RelayCommand ToggleFavCommand { get; set; }

        public SteamItemNavigationItemModel(ItemDataModel itemData, bool favState, HomeViewModel viewModel)
        {
            _viewModel = viewModel;
            _itemData = itemData;

            Title = itemData.Name;
            Image = itemData.IconPath;

            Price = Math.Round(itemData.PriceHistory.Last().Value * App.Currency.ExchangeRate, 2).ToString("N") + App.Settings.Currency;
            PriceThisWeek = GetPriceThisWeek().Key;
            PriceThisWeekColor = GetPriceThisWeek().Value;
            FavState = favState;

            Command = new RelayCommand(execute => LoadItemPage());
            ToggleFavCommand = new RelayCommand(execute => ToggleFav());

            LoadFavImage();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private KeyValuePair<string, SolidColorBrush> GetPriceThisWeek()
        {
            string price = String.Empty;
            SolidColorBrush color = new SolidColorBrush(Colors.Transparent);

            KeyValuePair<string, SolidColorBrush> pair = new KeyValuePair<string, SolidColorBrush>(price, color);

            try
            {
                KeyValuePair<DateTime, decimal> thisWeek = _itemData.PriceHistory.Last(x => x.Key.Date == DateTime.Now.Date);
                decimal priceThisWeek = thisWeek.Value;

                decimal priceLastWeek = _itemData.PriceHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-7).Date && x.Key.Hour == thisWeek.Key.Hour).Value;

                decimal p = (priceThisWeek / priceLastWeek) - 1;
                price = p.ToString("P");

                if (p > 0)
                {
                    color = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    price = "+" + price;
                }
                else if (p < 0) color = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                else if (p == 0) color = new SolidColorBrush(Colors.Gray);

                pair = new KeyValuePair<string, SolidColorBrush>(price, color);
                return pair;
            }
            catch (Exception)
            {
                return pair;
            }
        }

        private void LoadItemPage()
        {
            _viewModel._navigationStore.ViewModel = new DisplayItemDataViewModel(_itemData);
        }

        private void LoadFavImage()
        {
            if (FavState)
            {
                FavImageSource = "../Resources/Images/star_fill.png";
            }
            else
            {
                FavImageSource = "../Resources/Images/star.png";
            }
        }

        private void ToggleFav()
        {
            FavState = !FavState;

            string filePath = App.MainDataFolder + "/FavouriteItems.json";
            
            if(!Path.Exists(filePath))
            {
                var f = File.Create(filePath);
                f.Close();
            }

            string file = File.ReadAllText(filePath);

            List<string> items = JsonConvert.DeserializeObject<List<string>>(file);
            if (items == null) items = new List<string>();

            if(FavState)
            {
                items.Add(Title);
            } 
            else
            {
                items.Remove(Title);
            }

            string json = JsonConvert.SerializeObject(items);

            File.WriteAllText(filePath, json);

            LoadFavImage();

            _viewModel.LoadSteamItemsNavigationItems();
        }
    }
}