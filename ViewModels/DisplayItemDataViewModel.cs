using SteamItemsStatsViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Input;
using SteamItemsStatsViewer.Commands;

namespace SteamItemsStatsViewer.ViewModels
{
    public class DisplayItemDataViewModel : ViewModelBase
    {
        private ObservableCollection<DataGridItemModel> _itemsData = new ObservableCollection<DataGridItemModel>();

        public IEnumerable<DataGridItemModel> ItemsData => _itemsData;

        public ICommand RefreshDataCommand { get; set; }

        public DisplayItemDataViewModel()
        {
            LoadData();

            RefreshDataCommand = new RefreshDataCommand(LoadData);
        }

        private void LoadData()
        {
            Debug.WriteLine("asdadadasasd");
            _itemsData.Clear();

            string dataFilePath = "D:\\PROGRAMMING-PROJECTS\\csharp-apps\\SteamMarketDataCollector\\bin\\Debug\\net8.0\\output\\Paris_2023_Legends_Sticker_Capsule.json";

            if (File.Exists(dataFilePath))
            {
                var json = File.ReadAllText(dataFilePath);
                List<ItemDataModel> itemsData = JsonConvert.DeserializeObject<List<ItemDataModel>>(json);

                ItemDataModel lastItem = null;
                foreach(ItemDataModel item in itemsData)
                {
                    DataGridItemModel dataGridItem = new DataGridItemModel(item, lastItem);
                    _itemsData.Add(dataGridItem);

                    lastItem = item;
                }
            }
        }
    }
}
