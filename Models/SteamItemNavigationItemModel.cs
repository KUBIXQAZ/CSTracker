using Newtonsoft.Json;
using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Windows.Media;

namespace SteamItemsStatsViewer.Models
{
    public class SteamItemNavigationItemModel : INotifyPropertyChanged
    {
        private HomeViewModel viewModel;

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

        public SteamItemNavigationItemModel(string title, string image, string price, string priceThisWeek, SolidColorBrush priceThisWeekColor, RelayCommand command, bool favState, HomeViewModel viewModel)
        {
            this.viewModel = viewModel;

            Title = title;
            Image = image;
            Price = price;
            PriceThisWeek = priceThisWeek;
            PriceThisWeekColor = priceThisWeekColor;
            Command = command;
            FavState = favState;

            ToggleFavCommand = new RelayCommand(execute => ToggleFav());

            LoadFavImage();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                File.Create(filePath);
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

            viewModel.LoadSteamItemsNavigationItems();
        }
    }
}