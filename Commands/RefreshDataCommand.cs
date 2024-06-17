using Newtonsoft.Json;
using SteamItemsStatsViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Globalization;
using SteamItemsStatsViewer.ViewModels;

namespace SteamItemsStatsViewer.Commands
{
    public class RefreshDataCommand : CommandBase
    {
        private string _folderPath;
        private ISeries[] _iSeries;
        private Axis[] _xAxes;
        private ObservableCollection<DataGridItemModel> _itemsData;
        private DisplayItemDataViewModel _viewModel;

        public RefreshDataCommand(string folderPath, ISeries[] iSeries, Axis[] xAxies, ObservableCollection<DataGridItemModel> itemsData, DisplayItemDataViewModel viewModel)
        {
            _folderPath = folderPath;
            _iSeries = iSeries;
            _xAxes = xAxies;
            _itemsData = itemsData;
            _viewModel = viewModel;
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

            _viewModel.SeriesQuantity[0].Values = _viewModel.ItemsData.Select(x => x.ItemData.Quantity).Reverse();
            _viewModel.XAxesQuantity[0].Labels = _viewModel.ItemsData.Select(x => x.ItemData.DataSaveDateTime.ToString()).Reverse().ToList();

            double price7Days = Math.Round(GetPriceFromLastDays(7), 2);
            _viewModel.Price7Days = price7Days > 0 ? $"+{price7Days}" : price7Days.ToString();

            double price14Days = Math.Round(GetPriceFromLastDays(14), 2);
            _viewModel.Price14Days = price14Days > 0 ? $"+{price14Days}" : price14Days.ToString();

            double price30Days = Math.Round(GetPriceFromLastDays(30), 2);
            _viewModel.Price30Days = price30Days > 0 ? $"+{price30Days}" : price30Days.ToString();

            int quantity7Days = GetQuantityFromLastDays(7);
            _viewModel.Quantity7Days = quantity7Days > 0 ? $"+{quantity7Days}" : quantity7Days.ToString();

            int quantity14Days = GetQuantityFromLastDays(14);
            _viewModel.Quantity14Days = quantity14Days > 0 ? $"+{quantity14Days}" : quantity14Days.ToString();

            int quantity30Days = GetQuantityFromLastDays(30);
            _viewModel.Quantity30Days = quantity30Days > 0 ? $"+{quantity30Days}" : quantity30Days.ToString();
        }

        public double GetPriceFromLastDays(int days)
        {
            string priceHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Price_History.json";

            if (File.Exists(priceHistoryPath))
            {
                string file = File.ReadAllText(priceHistoryPath);
                PriceHistoryModel priceHistory = JsonConvert.DeserializeObject<PriceHistoryModel>(file);

                DateTime[] dates = priceHistory.Prices.Select(x => DateTime.ParseExact(x[0].Replace(": +0", ""), "MMM dd yyyy HH", CultureInfo.InvariantCulture)).ToArray();
                double[] prices = priceHistory.Prices.Select(x => Double.Parse(x[1].Replace(".", ","))).ToArray();

                Dictionary<DateTime, double> data = new Dictionary<DateTime, double>();

                int i = 0;
                foreach (DateTime date in dates)
                {
                    data.Add(date, prices[i]);
                    i++;
                }

                KeyValuePair<DateTime, double> firstKeyPair = data.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
                KeyValuePair<DateTime, double> secondKeyPair;
                try
                {
                    secondKeyPair = data.First(x => x.Key.Date == DateTime.Now.Date);
                } catch (Exception)
                {
                    secondKeyPair = data.First(x => x.Key.Date == DateTime.Now.Date.AddDays(-1));
                }

                return secondKeyPair.Value - firstKeyPair.Value;
            }
            else return 0;
        }

        public int GetQuantityFromLastDays(int days)
        {
            DateTime[] dates = _itemsData.Select(x => x.ItemData.DataSaveDateTime).ToArray();
            int[] quantities = _itemsData.Select(x => x.ItemData.Quantity).ToArray();

            Dictionary<DateTime,int> data = new Dictionary<DateTime,int>();

            int i = 0;
            foreach(DateTime date in dates)
            {
                data.Add(date, quantities[i]);
                i++;
            }

            KeyValuePair<DateTime, int> firstKeyPair;
            try
            {
                firstKeyPair = data.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
            } catch (Exception)
            {
                firstKeyPair = data.Last();
            }
            KeyValuePair<DateTime, int> secondKeyPair;
            try
            {
                secondKeyPair = data.First(x => x.Key.Date == DateTime.Now.Date);
            } catch (Exception)
            {
                secondKeyPair = data.First(x => x.Key.Date == DateTime.Now.Date.AddDays(-1));
            }

            return secondKeyPair.Value - firstKeyPair.Value;
        }
    }
}
