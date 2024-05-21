using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamItemsStatsViewer.Models
{
    public class LiewViewItemModel
    {
        public ItemDataModel ItemData { get; set; }
        public int QuantityChangedBy { get; set; }
        public double PriceChangedBy { get; set; }

        public LiewViewItemModel(ItemDataModel itemData, ItemDataModel lastItemData)
        {
            ItemData = itemData;

            if (lastItemData != null)
            {
                QuantityChangedBy = ItemData.Quantity - lastItemData.Quantity;
                PriceChangedBy = ItemData.Price - lastItemData.Price;
            }
        }
    }
}
