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

        private string _filePath { get; set; }

        public DisplayItemDataViewModel(string parameter)
        {
            _filePath = parameter;

            LoadData(_filePath);

            RefreshDataCommand = new RefreshDataCommand(LoadData, _filePath);
        }

        private void LoadData(string filePath)
        {
            _itemsData.Clear();

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var itemsData = JsonConvert.DeserializeObject<List<ItemDataModel>>(json);
                itemsData.OrderBy(x => { return x.DataSaveDateTime; });

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
