using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamItemsStatsViewer.Models
{
    public class ItemDataModel
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Currency {  get; set; }
    }
}
