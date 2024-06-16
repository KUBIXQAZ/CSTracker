using Newtonsoft.Json;
using SteamItemsStatsViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace SteamItemsStatsViewer.Commands
{
    public class RefreshDataCommand : CommandBase
    {
        private string _folderPath;
        private ISeries[] _iSeries;
        private Axis[] _xAxes;
        private ObservableCollection<DataGridItemModel> _itemsData;

        public RefreshDataCommand(string folderPath, ISeries[] iSeries, Axis[] xAxies, ObservableCollection<DataGridItemModel> itemsData)
        {
            _folderPath = folderPath;
            _iSeries = iSeries;
            _xAxes = xAxies;
            _itemsData = itemsData;
        }

        public override void Execute(object? parameter)
        {
            string priceHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Price_History.json";

            if (File.Exists(priceHistoryPath))
            {
                string file = File.ReadAllText(priceHistoryPath);
                PriceHistoryModel priceHistory = JsonConvert.DeserializeObject<PriceHistoryModel>(file);

                _iSeries[0].Values = priceHistory.Prices.Select(x => Double.Parse(x[1].Replace(".", ","))).ToList();
                _xAxes[0].Labels = priceHistory.Prices.Select(x => x[0].Replace(": +0", "")).ToList();
            }
            
            string itemDataPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}.json";

            if (File.Exists(itemDataPath))
            {
                _itemsData.Clear();

                var json = File.ReadAllText(itemDataPath);
                var itemsData = JsonConvert.DeserializeObject<List<ItemDataModel>>(json);
                itemsData.OrderBy(x => x.DataSaveDateTime);

                ItemDataModel lastItem = null;
                foreach (ItemDataModel item in itemsData)
                {
                    DataGridItemModel dataGridItem = new DataGridItemModel(item, lastItem);
                    _itemsData.Add(dataGridItem);

                    lastItem = item;
                }

                var reversedItems = new ObservableCollection<DataGridItemModel>(_itemsData.Reverse());
                _itemsData.Clear();
                foreach (var item in reversedItems)
                {
                    _itemsData.Add(item);
                }
            }
        }
    }
}
