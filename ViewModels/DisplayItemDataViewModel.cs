using SteamItemsStatsViewer.Models;
using System.IO;
using Newtonsoft.Json;
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
        private string _folderPath { get; set; }

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

        private string _itemName;
        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
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
                RefreshDataCommand = CreateRefreshDataCommand();
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
                RefreshDataCommand = CreateRefreshDataCommand();
                RefreshDataCommand.Execute(this);
            }
        }

        public DisplayItemDataViewModel(string parameter)
        {
            _folderPath = parameter;

            _itemName = Uri.UnescapeDataString(Path.GetFileName(parameter));

            RefreshDataCommand = CreateRefreshDataCommand();
            RefreshDataCommand.Execute(this);

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

            PriceChartTimeStamp = ChartTimeStamp.Week;
            QuantityChartTimeStamp = ChartTimeStamp.Week;
        }

        public void UpdateData()
        {
            string priceHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Price_History.json";

            if (File.Exists(priceHistoryPath))
            {
                string file = File.ReadAllText(priceHistoryPath);
                PriceHistoryModel priceHistory = JsonConvert.DeserializeObject<PriceHistoryModel>(file);

                //_itemData.PriceHistory = priceHistory;
            }

            string quantityHistoryPath = $"{_folderPath}\\{Path.GetFileName(_folderPath)}_Quantity_History.json";

            if (File.Exists(quantityHistoryPath))
            {
                string file = File.ReadAllText(quantityHistoryPath);
                QuantityHistoryModel quantityHistory = JsonConvert.DeserializeObject<QuantityHistoryModel>(file);

                //_itemData.QuantityHistory = quantityHistory;
            }
        }

        private RefreshDataCommand CreateRefreshDataCommand()
        {
            return new RefreshDataCommand(_folderPath, SeriesPrice, XAxesPrice, SeriesQuantity, XAxesQuantity, _priceChartTimeStamp, _quantityChartTimeStamp, ItemData, this);
        }
    }
}