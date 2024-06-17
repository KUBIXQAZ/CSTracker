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
                Fill = new LinearGradientPaint(new [] { new SKColor(9, 76, 130,50), new SKColor(33, 150, 243,100) },new SKPoint(0, 0),new SKPoint(0, 1)),
                Stroke = new SolidColorPaint(new SKColor(33, 150, 243,50)),
                LineSmoothness = 1,
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

        //STATS//
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

        public DisplayItemDataViewModel(string parameter)
        {
            _folderPath = parameter;

            RefreshDataCommand = new RefreshDataCommand(_folderPath, Series, XAxes, _itemsData, this);
            RefreshDataCommand.Execute(this);
        }
    }
}