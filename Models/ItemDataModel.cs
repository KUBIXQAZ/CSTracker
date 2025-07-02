using LiveChartsCore.Geo;
using Newtonsoft.Json;
using System.Windows.Media.Imaging;

namespace CSTracker.Models
{
    public class ItemDataModel
    {
        public string Name { get; set; }

        private string _imageUrl; 
        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                _imageUrl = value;
                UpdateItemImageBitmap();
            }
        }

        [JsonIgnore] 
        public BitmapImage ImageBitmap { get; set; }

        public Dictionary<DateTime, decimal> PriceHistory { get; set; }
        public Dictionary<DateTime, int> QuantityHistory { get; set; }

        private void UpdateItemImageBitmap()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("https://cstracker.cloud" + ImageUrl, UriKind.Absolute);
            bitmap.EndInit();
            ImageBitmap = bitmap;
        }
    }
}