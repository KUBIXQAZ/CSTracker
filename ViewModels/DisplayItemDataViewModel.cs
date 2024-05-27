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

        public IEnumerable<DataGridItemModel> ItemsData => _itemsData.Reverse();

        public ICommand RefreshDataCommand { get; set; }

        private string filePath { get; set; }

        public DisplayItemDataViewModel(string parameter)
        {
            filePath = parameter;

            LoadData();

            RefreshDataCommand = new RefreshDataCommand(LoadData);
        }

        private void LoadData()
        {
            _itemsData.Clear();

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
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
