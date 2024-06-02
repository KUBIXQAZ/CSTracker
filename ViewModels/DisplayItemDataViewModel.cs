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
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;

namespace SteamItemsStatsViewer.ViewModels
{
    public class DisplayItemDataViewModel : ViewModelBase
    {
        private ObservableCollection<DataGridItemModel> _itemsData = new ObservableCollection<DataGridItemModel>();

        public IEnumerable<DataGridItemModel> ItemsData => _itemsData;

        public ICommand RefreshDataCommand { get; set; }

        private string _filePath { get; set; }

        public ISeries[] Series { get; set; } = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = new SolidColorPaint(new SKColor(63, 77, 99)),
                Stroke = new SolidColorPaint(new SKColor(120, 152, 203)),
                LineSmoothness = 0,
                GeometrySize = 1,
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
            }
            _itemsData = new ObservableCollection<DataGridItemModel>(_itemsData.Reverse());

            //refresh price chart//
            Series[0].Values = _itemsData.Select(x => x.ItemData.Price).Reverse();
            OnPropertyChanged(nameof(Series));

            XAxes[0].Labels = _itemsData.Select(x => x.ItemData.DataSaveDateTime.ToString()).Reverse().ToList();
            OnPropertyChanged(nameof(XAxes));
        }
    }
}
