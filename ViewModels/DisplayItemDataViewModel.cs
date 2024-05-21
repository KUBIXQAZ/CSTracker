using SteamItemsStatsViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamItemsStatsViewer.ViewModels
{
    public class DisplayItemDataViewModel : ViewModelBase
    {
        private ObservableCollection<LiewViewItemModel> _itemsData = new ObservableCollection<LiewViewItemModel>();

        public IEnumerable<LiewViewItemModel> ItemsData => _itemsData;

        public DisplayItemDataViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            _itemsData.Clear();

            string dataFilePath = "D:\\PROGRAMMING-PROJECTS\\csharp-apps\\SteamMarketDataCollector\\bin\\Debug\\net8.0\\output\\Paris_2023_Legends_Sticker_Capsule.txt";

            if (File.Exists(dataFilePath))
            {
                using (StreamReader reader = new StreamReader(dataFilePath))
                {
                    ItemDataModel lastItemData = null;

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        int quantity = int.Parse(line.Split("Number of active listings: ")[1].Split(" ")[0]);
                        string priceString = line.Split("Median price (24H): ")[1].Split(" ")[0].Replace('.', ',');

                        int numberIndex = -1;
                        int index = 0;
                        foreach(char c in priceString)
                        {
                            if(int.TryParse(c.ToString(), out int cParse))
                            {
                                numberIndex = index;
                                break;
                            }
                            index++;
                        }

                        double price = double.Parse(priceString.Substring(numberIndex,priceString.Length - 1));
                        string currency = priceString.Replace(price.ToString(), "");

                        ItemDataModel itemData = new ItemDataModel
                        {
                            Quantity = quantity,
                            Price = price,
                            Currency = currency,
                        };

                        LiewViewItemModel liewViewItem = new LiewViewItemModel(itemData, lastItemData);

                        _itemsData.Add(liewViewItem);

                        lastItemData = itemData;
                    }
                }
            }
        }
    }
}
