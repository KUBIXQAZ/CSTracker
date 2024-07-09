using Newtonsoft.Json;
using SteamItemsStatsViewer.Models;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SteamItemsStatsViewer.ViewModels;

namespace SteamItemsStatsViewer.Commands
{
    public class RefreshDataCommand : CommandBase
    {
        private string _folderPath;
        private ISeries[] _iSeries;
        private Axis[] _xAxes;
        private DisplayItemDataViewModel _viewModel;
        private ItemDataModel _itemData;

        private CurrencyModel _currencies;
        private double rate;

        public RefreshDataCommand(string folderPath, ISeries[] iSeries, Axis[] xAxies, ItemDataModel itemData, DisplayItemDataViewModel viewModel)
        {
            _currencies = new CurrencyModel();
            rate = App.ExchangeRates.Where(x => x.Key == App.Settings.Currency).Select(x => x.Value).ToArray()[0];

            _folderPath = folderPath;
            _iSeries = iSeries;
            _xAxes = xAxies;
            _viewModel = viewModel;
            _itemData = itemData;
        }

        public override void Execute(object? parameter)
        {
            #region PRICE HISTORY
            string priceHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Price_History.json";

            if (File.Exists(priceHistoryPath))
            {
                string file = File.ReadAllText(priceHistoryPath);
                PriceHistoryModel priceHistory = JsonConvert.DeserializeObject<PriceHistoryModel>(file);

                _itemData.PriceHistory = priceHistory;

                _iSeries[0].Values = priceHistory.PriceHistory.Select(x => (x.Value * rate));
                _xAxes[0].Labels = priceHistory.PriceHistory.Select(x => x.Key.ToString()).ToArray();
            }
            #endregion

            #region QUANTITY HISTORY
            string quantityHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Quantity_History.json";

            if(File.Exists(quantityHistoryPath))
            {
                string file = File.ReadAllText(quantityHistoryPath);
                QuantityHistoryModel quantityHistory = JsonConvert.DeserializeObject<QuantityHistoryModel>(file);

                _itemData.QuantityHistory = quantityHistory;

                _viewModel.SeriesQuantity[0].Values = quantityHistory.QuantityHistory.Select(x => x.Value);
                _viewModel.XAxesQuantity[0].Labels = quantityHistory.QuantityHistory.Select(x => x.Key.ToString()).ToArray();
            }
            #endregion

            try
            {
                double price7Days = Math.Round(GetPriceFromLastDays(7) * rate, 2);

                _viewModel.Price7Days = price7Days > 0 ? $"+{price7Days.ToString("N")}" : price7Days.ToString("N");
                _viewModel.Price7Days = $"{_viewModel.Price7Days}{App.Settings.Currency}";
            } catch { _viewModel.Price7Days = "NO DATA"; }

            try
            {
                double price14Days = Math.Round(GetPriceFromLastDays(14) * rate, 2);

                _viewModel.Price14Days = price14Days > 0 ? $"+{price14Days.ToString("N")}" : price14Days.ToString("N");
                _viewModel.Price14Days = $"{_viewModel.Price14Days}{App.Settings.Currency}";
            }
            catch { _viewModel.Price14Days = "NO DATA"; }
                
            try
            {
                double price30Days = Math.Round(GetPriceFromLastDays(30) * rate, 2);

                _viewModel.Price30Days = price30Days > 0 ? $"+{price30Days.ToString("N")}" : price30Days.ToString("N");
                _viewModel.Price30Days = $"{_viewModel.Price30Days}{App.Settings.Currency}";
            } catch { _viewModel.Price30Days = "NO DATA"; }

            try
            {
                int quantity7Days = GetQuantityFromLastDays(7);

                _viewModel.Quantity7Days = quantity7Days > 0 ? $"+{quantity7Days.ToString("N0")}" : quantity7Days.ToString("N0");
            }
            catch { _viewModel.Quantity7Days = "NO DATA"; }

            try
            {
                int quantity14Days = GetQuantityFromLastDays(14);

                _viewModel.Quantity14Days = quantity14Days > 0 ? $"+{quantity14Days.ToString("N0")}" : quantity14Days.ToString("N0");
            } catch { _viewModel.Quantity14Days = "NO DATA"; }

            try
            {
                int quantity30Days = GetQuantityFromLastDays(30);

                _viewModel.Quantity30Days = quantity30Days > 0 ? $"+{quantity30Days.ToString("N0")}" : quantity30Days.ToString("N0");
            } catch { _viewModel.Quantity30Days = "NO DATA"; }

            _viewModel.CurrentPrice = $"{(_itemData.PriceHistory.PriceHistory.Last().Value * rate).ToString("N")}{App.Settings.Currency}";

            _viewModel.CurrentQuantity = _itemData.QuantityHistory.QuantityHistory.Last().Value.ToString("N0");
        }

        public double GetPriceFromLastDays(int days)
        {
            KeyValuePair<DateTime, double> firstKeyPair = _itemData.PriceHistory.PriceHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
            KeyValuePair<DateTime, double> secondKeyPair = _itemData.PriceHistory.PriceHistory.First(x => x.Key.Date == DateTime.Now.Date);

            return secondKeyPair.Value - firstKeyPair.Value;
        }

        public int GetQuantityFromLastDays(int days)
        {
            KeyValuePair<DateTime, int> firstKeyPair = _itemData.QuantityHistory.QuantityHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
            KeyValuePair<DateTime, int> secondKeyPair = _itemData.QuantityHistory.QuantityHistory.First(x => x.Key.Date == DateTime.Now.Date);
            return secondKeyPair.Value - firstKeyPair.Value;
        }
    }
}
