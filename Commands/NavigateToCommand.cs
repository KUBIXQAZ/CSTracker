using SteamItemsStatsViewer.ViewModels;
using System;

namespace SteamItemsStatsViewer.Commands
{
    public class NavigateToCommand : CommandBase
    {
        private MainViewModel _mainViewModel { get; set; }
        private Func<ViewModelBase> _funcGoToViewModel {  get; set; }

        public NavigateToCommand(MainViewModel mainViewModel, Func<ViewModelBase> func)
        {
            _mainViewModel = mainViewModel;
            _funcGoToViewModel = func;
        }

        public override void Execute(object? parameter)
        {
            _mainViewModel.ViewModel = _funcGoToViewModel.Invoke();
        }
    }
}
