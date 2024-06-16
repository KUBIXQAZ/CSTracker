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

namespace SteamItemsStatsViewer.ViewModels
{
    public class DisplayItemDataViewModel : ViewModelBase
    {
        private string _folderPath { get; set; }

        //ITEM DATA//
        private ObservableCollection<DataGridItemModel> _itemsData = new ObservableCollection<DataGridItemModel>();
        public ObservableCollection<DataGridItemModel> ItemsData
        {
            get => _itemsData;
            set
            {
                _itemsData = value;
                OnPropertyChanged(nameof(ItemsData));
            }
        }

        //COMMANDS//
        public ICommand RefreshDataCommand { get; set; }

        //PRICE CHART//
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
            _folderPath = parameter;
            
            RefreshDataCommand = new RefreshDataCommand(_folderPath,Series,XAxes,_itemsData);
            RefreshDataCommand.Execute(this);
        }
    }
}