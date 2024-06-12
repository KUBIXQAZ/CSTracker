using SteamItemsStatsViewer.Models;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Input;
using SteamItemsStatsViewer.Commands;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Net.Http;

namespace SteamItemsStatsViewer.ViewModels
{
    public class DisplayItemDataViewModel : ViewModelBase
    {
        private string _filePath { get; set; }

        //item data//
        private ObservableCollection<DataGridItemModel> _itemsData = new ObservableCollection<DataGridItemModel>();
        public IEnumerable<DataGridItemModel> ItemsData => _itemsData;

        //commands//
        public ICommand RefreshDataCommand { get; set; }

        //price chart//
        public ISeries[] Series { get; set; } = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = new SolidColorPaint(new SKColor(63, 77, 99)),
                Stroke = new SolidColorPaint(new SKColor(120, 152, 203)),
                LineSmoothness = 0,
                GeometrySize = 0,
                DataPadding = new LiveChartsCore.Drawing.LvcPoint(0,0),
            }
        };
        public Axis[] YAxes { get; set; } = new[]
        {
            new Axis
            {
                LabelsPaint = new SolidColorPaint(new SKColor(255,255,255)),
                TextSize = 15,
            }
        };
        public Axis[] XAxes { get; set; } = new[]
        {
            new Axis
            {
               LabelsRotation = 0,
               LabelsPaint = new SolidColorPaint(new SKColor(255,255,255)),
               IsVisible = true,
               TextSize = 10,
            }
        };

        public DisplayItemDataViewModel(string parameter)
        {
            _filePath = parameter;

            RefreshDataCommand = new RefreshDataCommand(LoadData, _filePath);

            LoadData(_filePath);
        }

        private void LoadData(string filePath)
        {
            string priceHistoryPath = _filePath.Replace(".json", "") + "_Price_History.json";

            if(File.Exists(priceHistoryPath))
            {
                string file = File.ReadAllText(priceHistoryPath);
                PriceHistoryModel priceHistory = JsonConvert.DeserializeObject<PriceHistoryModel>(file);

                Series[0].Values = priceHistory.Prices.Select(x => Double.Parse(x[1].Replace(".",","))).ToList();
                XAxes[0].Labels = priceHistory.Prices.Select(x => x[0].Replace(": +0","")).ToList();
            }

            //load data && auto refresh data table//
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
                _itemsData = new ObservableCollection<DataGridItemModel>(_itemsData.Reverse());
                OnPropertyChanged(nameof(ItemsData));
            }
        }
    }
}
