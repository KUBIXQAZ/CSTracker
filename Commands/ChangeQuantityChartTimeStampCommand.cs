using SteamItemsStatsViewer.ViewModels;
using static SteamItemsStatsViewer.ViewModels.DisplayItemDataViewModel;

namespace SteamItemsStatsViewer.Commands
{
    public class ChangeQuantityChartTimeStampCommand : CommandBase
    {
        private DisplayItemDataViewModel _viewModel;
        private ChartTimeStamp _newChartTimeStamp;

        public ChangeQuantityChartTimeStampCommand(DisplayItemDataViewModel viewModel, ChartTimeStamp newChartTimeStamp)
        {
            _viewModel = viewModel;
            _newChartTimeStamp = newChartTimeStamp;
        }

        public override void Execute(object? parameter)
        {
            _viewModel.QuantityChartTimeStamp = _newChartTimeStamp;
        }
    }
}