using Newtonsoft.Json;
using SteamItemsStatsViewer.Models;
using System.IO;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SteamItemsStatsViewer.ViewModels;
using System.Diagnostics;
using static SteamItemsStatsViewer.ViewModels.DisplayItemDataViewModel;
using LiveChartsCore.Kernel.Sketches;

namespace SteamItemsStatsViewer.Commands
{
    public class RefreshDataCommand : CommandBase
    {
        private string _folderPath;

        private ISeries[] _iSeriesPrice;
        private Axis[] _xAxesPrice;
        private ChartTimeStamp _priceChartTimeStamp;

        private ISeries[] _iSeriesQuantity;
        private Axis[] _xAxesQuantity;
        private ChartTimeStamp _quantityChartTimeStamp;

        private DisplayItemDataViewModel _viewModel;
        private ItemDataModel _itemData;

        private CurrencyModel _currencies;
        private decimal rate;

        public RefreshDataCommand(string folderPath, ISeries[] iSeriesPrice, Axis[] xAxesPrice, ISeries[] iSeriesQuantity, Axis[] xAxesQuantity, ChartTimeStamp priceChartTimeStamp, ChartTimeStamp quantityChartTimeStamp, ItemDataModel itemData, DisplayItemDataViewModel viewModel)
        {
            _currencies = new CurrencyModel();
            rate = App.ExchangeRates.Where(x => x.Key == App.Settings.Currency).Select(x => x.Value).ToArray()[0];

            _folderPath = folderPath;

            _iSeriesPrice = iSeriesPrice;
            _xAxesPrice = xAxesPrice;
            _priceChartTimeStamp = priceChartTimeStamp;

            _iSeriesQuantity = iSeriesQuantity;
            _xAxesQuantity = xAxesQuantity;
            _quantityChartTimeStamp = quantityChartTimeStamp;

            _viewModel = viewModel;
            _itemData = itemData;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.UpdateData();

            //_iSeriesPrice[0].Values = _itemData.PriceHistory.PriceHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_priceChartTimeStamp) <= x.Key.Date).Select(x => Math.Round(x.Value * rate, 2));
            //_xAxesPrice[0].Labels = _itemData.PriceHistory.PriceHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_priceChartTimeStamp) <= x.Key.Date).Select(x => x.Key.ToString()).ToArray();

            _xAxesPrice[0].MinLimit = 0;
            _xAxesPrice[0].MaxLimit = _xAxesPrice[0].Labels.Count - 1;

            //_iSeriesQuantity[0].Values = _itemData.QuantityHistory.QuantityHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_quantityChartTimeStamp) <= x.Key.Date).Select(x => x.Value);
            //_xAxesQuantity[0].Labels = _itemData.QuantityHistory.QuantityHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_quantityChartTimeStamp) <= x.Key.Date).Select(x => x.Key.ToString()).ToArray();

            _xAxesQuantity[0].MinLimit = 0;
            _xAxesQuantity[0].MaxLimit = _xAxesQuantity[0].Labels.Count - 1;

            try
            {
                decimal price7Days = Math.Round(GetPriceFromLastDays(7) * rate, 2);

                _viewModel.Price7Days = price7Days > 0 ? $"+{price7Days.ToString("N")}" : price7Days.ToString("N");
                _viewModel.Price7Days = $"{_viewModel.Price7Days}{App.Settings.Currency}";
            } catch { _viewModel.Price7Days = "NO DATA"; }

            try
            {
                decimal price14Days = Math.Round(GetPriceFromLastDays(14) * rate, 2);

                _viewModel.Price14Days = price14Days > 0 ? $"+{price14Days.ToString("N")}" : price14Days.ToString("N");
                _viewModel.Price14Days = $"{_viewModel.Price14Days}{App.Settings.Currency}";
            }
            catch { _viewModel.Price14Days = "NO DATA"; }
                
            try
            {
                decimal price30Days = Math.Round(GetPriceFromLastDays(30) * rate, 2);

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

            //_viewModel.CurrentPrice = $"{(_itemData.PriceHistory.PriceHistory.Last().Value * rate).ToString("N")}{App.Settings.Currency}";

            //_viewModel.CurrentQuantity = _itemData.QuantityHistory.QuantityHistory.Last().Value.ToString("N0");
        }

        public decimal GetPriceFromLastDays(int days)
        {
            //KeyValuePair<DateTime, double> firstKeyPair = _itemData.PriceHistory.PriceHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
            //KeyValuePair<DateTime, double> secondKeyPair = _itemData.PriceHistory.PriceHistory.First(x => x.Key.Date == DateTime.Now.Date);

            //return secondKeyPair.Value - firstKeyPair.Value;

            return 0;
        }

        public int GetQuantityFromLastDays(int days)
        {
            //KeyValuePair<DateTime, int> firstKeyPair = _itemData.QuantityHistory.QuantityHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
            //KeyValuePair<DateTime, int> secondKeyPair = _itemData.QuantityHistory.QuantityHistory.First(x => x.Key.Date == DateTime.Now.Date);
            //return secondKeyPair.Value - firstKeyPair.Value;
            return 0;
        }
    }
}
