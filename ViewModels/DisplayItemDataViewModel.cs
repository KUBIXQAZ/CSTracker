using SteamItemsStatsViewer.Models;
using System.Windows.Input;
using SteamItemsStatsViewer.Commands;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;

namespace SteamItemsStatsViewer.ViewModels
{
    public class DisplayItemDataViewModel : ViewModelBase
    {
        //ITEM DATA//
        private ItemDataModel _itemData = new ItemDataModel();
        public ItemDataModel ItemData
        {
            get => _itemData;
            set
            {
                _itemData = value;
                OnPropertyChanged(nameof(ItemData));
            }
        }

        //COMMANDS//
        public ICommand RefreshDataCommand { get; set; }

        public ICommand DayTimeStampPriceChartCommand { get; set; }
        public ICommand WeekTimeStampPriceChartCommand { get; set; }
        public ICommand MonthTimeStampPriceChartCommand { get; set; }
        public ICommand YearTimeStampPriceChartCommand { get; set; }
        public ICommand AllTimeStampPriceChartCommand { get; set; }

        public ICommand DayTimeStampQuantityChartCommand { get; set; }
        public ICommand WeekTimeStampQuantityChartCommand { get; set; }
        public ICommand MonthTimeStampQuantityChartCommand { get; set; }
        public ICommand YearTimeStampQuantityChartCommand { get; set; }
        public ICommand AllTimeStampQuantityChartCommand { get; set; }

        //PRICE CHART//
        public ISeries[] SeriesPrice { get; set; } = new ISeries[]
        {
            new LineSeries<double>
            {
                Fill = new LinearGradientPaint(new [] { new SKColor(33, 150, 243, 100), new SKColor(43, 43, 43, 0) },new SKPoint(0, 0),new SKPoint(0, 1)),
                Stroke = new SolidColorPaint(new SKColor(85, 177, 250,100)),
                LineSmoothness = 0,
                GeometrySize = 0,
                DataPadding = new LiveChartsCore.Drawing.LvcPoint(0,0),
            }
        };
        public Axis[] YAxesPrice { get; set; } = new[]
        {
            new Axis
            {
                LabelsPaint = new SolidColorPaint(new SKColor(255,255,255)),
                TextSize = 15,
            }
        };
        public Axis[] XAxesPrice { get; set; } = new[]
        {
            new Axis
            {
               LabelsRotation = 0,
               LabelsPaint = new SolidColorPaint(new SKColor(255,255,255)),
               IsVisible = true,
               TextSize = 10,
            }
        };

        public ISeries[] SeriesQuantity { get; set; } = new ISeries[]
        {
            new LineSeries<int>
            {
                Fill = new LinearGradientPaint(new [] { new SKColor(33, 150, 243, 100), new SKColor(43, 43, 43, 0) },new SKPoint(0, 0),new SKPoint(0, 1)),
                Stroke = new SolidColorPaint(new SKColor(85, 177, 250,100)),
                LineSmoothness = 0,
                GeometrySize = 0,
                DataPadding = new LiveChartsCore.Drawing.LvcPoint(0,0),
            }
        };
        public Axis[] YAxesQuantity { get; set; } = new[]
        {
            new Axis
            {
                LabelsPaint = new SolidColorPaint(new SKColor(255,255,255)),
                TextSize = 15,
            }
        };
        public Axis[] XAxesQuantity { get; set; } = new[]
        {
            new Axis
            {
               LabelsRotation = 0,
               LabelsPaint = new SolidColorPaint(new SKColor(255,255,255)),
               IsVisible = true,
               TextSize = 10,
            }
        };

        //STATS//
        private string _price7Days;
        public string Price7Days
        {
            get => _price7Days;
            set
            {
                _price7Days = value;
                OnPropertyChanged(nameof(Price7Days));
            }
        }
        private string _price14Days;
        public string Price14Days
        {
            get => _price14Days;
            set
            {
                _price14Days = value;
                OnPropertyChanged(nameof(Price14Days));
            }
        }
        private string _price30Days;
        public string Price30Days
        {
            get => _price30Days; 
            set 
            { 
                _price30Days = value;
                OnPropertyChanged(nameof(Price30Days));
            }
        }
        private string _quantity7Days;
        public string Quantity7Days
        {
            get => _quantity7Days;
            set
            {
                _quantity7Days = value;
                OnPropertyChanged(nameof(Quantity7Days));
            }
        }
        private string _quantity14Days;
        public string Quantity14Days
        {
            get => _quantity14Days;
            set
            {
                _quantity14Days = value;
                OnPropertyChanged(nameof(Quantity14Days));
            }
        }
        private string _quantity30Days;
        public string Quantity30Days
        {
            get => _quantity30Days;
            set
            {
                _quantity30Days = value;
                OnPropertyChanged(nameof(Quantity30Days));
            }
        }
        private string _currentQuantity;
        public string CurrentQuantity
        {
            get => _currentQuantity;
            set
            {
                _currentQuantity = value;
                OnPropertyChanged(nameof(CurrentQuantity));
            }
        }
        private string _currentPrice;
        public string CurrentPrice
        {
            get => _currentPrice;
            set
            {
                _currentPrice = value;
                OnPropertyChanged(nameof(CurrentPrice));
            }
        }

        public enum ChartTimeStamp
        {
            Day = 0,
            Week = 7,
            Month = 30,
            Year = 365,
            All = 100000
        }

        private ChartTimeStamp _priceChartTimeStamp;
        public ChartTimeStamp PriceChartTimeStamp
        {
            get => _priceChartTimeStamp;
            set
            {
                _priceChartTimeStamp = value;
                OnPropertyChanged(nameof(PriceChartTimeStamp));
                RefreshDataCommand.Execute(this);
            }
        }

        private ChartTimeStamp _quantityChartTimeStamp;
        public ChartTimeStamp QuantityChartTimeStamp
        {
            get => _quantityChartTimeStamp;
            set
            {
                _quantityChartTimeStamp = value;
                OnPropertyChanged(nameof(QuantityChartTimeStamp));
                RefreshDataCommand.Execute(this);
            }
        }

        public DisplayItemDataViewModel(ItemDataModel itemData)
        {
            ItemData = itemData;

            RefreshDataCommand = new RelayCommand(execute => RefreshData());

            DayTimeStampPriceChartCommand = new ChangePriceChartTimeStampCommand(this, ChartTimeStamp.Day);
            WeekTimeStampPriceChartCommand = new ChangePriceChartTimeStampCommand(this, ChartTimeStamp.Week);
            MonthTimeStampPriceChartCommand = new ChangePriceChartTimeStampCommand(this, ChartTimeStamp.Month);
            YearTimeStampPriceChartCommand = new ChangePriceChartTimeStampCommand(this, ChartTimeStamp.Year);
            AllTimeStampPriceChartCommand = new ChangePriceChartTimeStampCommand(this, ChartTimeStamp.All);

            DayTimeStampQuantityChartCommand = new ChangeQuantityChartTimeStampCommand(this, ChartTimeStamp.Day);
            WeekTimeStampQuantityChartCommand = new ChangeQuantityChartTimeStampCommand(this, ChartTimeStamp.Week);
            MonthTimeStampQuantityChartCommand = new ChangeQuantityChartTimeStampCommand(this, ChartTimeStamp.Month);
            YearTimeStampQuantityChartCommand = new ChangeQuantityChartTimeStampCommand(this, ChartTimeStamp.Year);
            AllTimeStampQuantityChartCommand = new ChangeQuantityChartTimeStampCommand(this, ChartTimeStamp.All);

            PriceChartTimeStamp = ChartTimeStamp.Day;
            QuantityChartTimeStamp = ChartTimeStamp.Day;
        }

        private void RefreshData()
        {
            //string priceHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Price_History.json";

            //if (File.Exists(priceHistoryPath))
            //{
            //    string file = File.ReadAllText(priceHistoryPath);
            //    PriceHistoryModel priceHistory = JsonConvert.DeserializeObject<PriceHistoryModel>(file);

            //    //_itemData.PriceHistory = priceHistory;
            //}

            //string quantityHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Quantity_History.json";

            //if (File.Exists(quantityHistoryPath))
            //{
            //    string file = File.ReadAllText(quantityHistoryPath);
            //    QuantityHistoryModel quantityHistory = JsonConvert.DeserializeObject<QuantityHistoryModel>(file);

            //    //_itemData.QuantityHistory = quantityHistory;
            //}

            decimal rate = App.ExchangeRates.First(x => x.Key == App.Settings.Currency).Value;

            SeriesPrice[0].Values = _itemData.PriceHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_priceChartTimeStamp) <= x.Key.Date).Select(x => (double)Math.Round(x.Value * rate, 2));
            XAxesPrice[0].Labels = _itemData.PriceHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_priceChartTimeStamp) <= x.Key.Date).Select(x => x.Key.ToString()).ToArray();

            XAxesPrice[0].MinLimit = 0;
            XAxesPrice[0].MaxLimit = XAxesPrice[0].Labels.Count - 1;

            SeriesQuantity[0].Values = _itemData.QuantityHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_quantityChartTimeStamp) <= x.Key.Date).Select(x => x.Value);
            XAxesQuantity[0].Labels = _itemData.QuantityHistory.Where(x => DateTime.Now.Date.AddDays(-(int)_quantityChartTimeStamp) <= x.Key.Date).Select(x => x.Key.ToString()).ToArray();

            XAxesQuantity[0].MinLimit = 0;
            XAxesQuantity[0].MaxLimit = XAxesQuantity[0].Labels.Count - 1;

            try
            {
                decimal price7Days = Math.Round(GetPriceFromLastDays(7) * rate, 2);

                Price7Days = price7Days > 0 ? $"+{price7Days.ToString("N")}" : price7Days.ToString("N");
                Price7Days = $"{Price7Days}{App.Settings.Currency}";
            }
            catch { Price7Days = "NO DATA"; }

            try
            {
                decimal price14Days = Math.Round(GetPriceFromLastDays(14) * rate, 2);

                Price14Days = price14Days > 0 ? $"+{price14Days.ToString("N")}" : price14Days.ToString("N");
                Price14Days = $"{Price14Days}{App.Settings.Currency}";
            }
            catch { Price14Days = "NO DATA"; }

            try
            {
                decimal price30Days = Math.Round(GetPriceFromLastDays(30) * rate, 2);

                Price30Days = price30Days > 0 ? $"+{price30Days.ToString("N")}" : price30Days.ToString("N");
                Price30Days = $"{Price30Days}{App.Settings.Currency}";
            }
            catch { Price30Days = "NO DATA"; }

            try
            {
                int quantity7Days = GetQuantityFromLastDays(7);

                Quantity7Days = quantity7Days > 0 ? $"+{quantity7Days.ToString("N0")}" : quantity7Days.ToString("N0");
            }
            catch { Quantity7Days = "NO DATA"; }

            try
            {
                int quantity14Days = GetQuantityFromLastDays(14);

                Quantity14Days = quantity14Days > 0 ? $"+{quantity14Days.ToString("N0")}" : quantity14Days.ToString("N0");
            }
            catch { Quantity14Days = "NO DATA"; }

            try
            {
                int quantity30Days = GetQuantityFromLastDays(30);

                Quantity30Days = quantity30Days > 0 ? $"+{quantity30Days.ToString("N0")}" : quantity30Days.ToString("N0");
            }
            catch { Quantity30Days = "NO DATA"; }

            CurrentPrice = $"{(_itemData.PriceHistory.Last().Value * rate).ToString("N")}{App.Settings.Currency}";

            CurrentQuantity = _itemData.QuantityHistory.Last().Value.ToString("N0");
        }

        public decimal GetPriceFromLastDays(int days)
        {
            KeyValuePair<DateTime, decimal> firstKeyPair = _itemData.PriceHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
            KeyValuePair<DateTime, decimal> secondKeyPair = _itemData.PriceHistory.First(x => x.Key.Date == DateTime.Now.Date);

            return secondKeyPair.Value - firstKeyPair.Value;
        }

        public int GetQuantityFromLastDays(int days)
        {
            KeyValuePair<DateTime, int> firstKeyPair = _itemData.QuantityHistory.First(x => x.Key.Date == DateTime.Now.AddDays(-days).Date);
            KeyValuePair<DateTime, int> secondKeyPair = _itemData.QuantityHistory.First(x => x.Key.Date == DateTime.Now.Date);

            return secondKeyPair.Value - firstKeyPair.Value;
        }
    }
}