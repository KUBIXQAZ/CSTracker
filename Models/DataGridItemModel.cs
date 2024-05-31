using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SteamItemsStatsViewer.Models
{
    public class DataGridItemModel
    {
        public ItemDataModel ItemData { get; set; }
        public string PriceString { get; set; }
        public string PriceChangedByString { get; set; }
        public string QuantityChangedByString { get; set; }
        public int QuantityChangedBy { get; set; }
        public double PriceChangedBy { get; set; }
        public Brush PriceColor { get; set; } = new SolidColorBrush(Colors.White);
        public Brush QuantityColor { get; set; } = new SolidColorBrush(Colors.White);

        public DataGridItemModel(ItemDataModel itemData, ItemDataModel lastItemData)
        {
            ItemData = itemData;

            if (lastItemData != null)
            {
                QuantityChangedBy = ItemData.Quantity - lastItemData.Quantity;
                PriceChangedBy = ItemData.Price - lastItemData.Price;

                if (PriceChangedBy > 0) PriceColor = new SolidColorBrush(Color.FromRgb(0, 153, 0));
                if (PriceChangedBy < 0) PriceColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                if (QuantityChangedBy > 0) QuantityColor = new SolidColorBrush(Color.FromRgb(255,0,0));
                if (QuantityChangedBy < 0) QuantityColor = new SolidColorBrush(Color.FromRgb(0, 153, 0));
            }

            PriceString = $"{ItemData.Price}{ItemData.Currency}";
            PriceChangedByString = $"{Math.Round(PriceChangedBy,2)}{ItemData.Currency}";
            if (QuantityChangedBy.ToString()[0] != '-' && QuantityChangedBy != 0) QuantityChangedByString = $"+{QuantityChangedBy}";
            else QuantityChangedByString = QuantityChangedBy.ToString();
        }
    }
}
