using CSTracker.MVVM;
using CSTracker.ViewModels;

namespace CSTracker.Commands
{
    public class SaveSettingsCommand : CommandBase
    {
        private SettingsViewModel _viewModel;

        public SaveSettingsCommand(SettingsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async override void Execute(object? parameter)
        {
            App.Settings.Currency = _viewModel.SelectedCurrency;

            App.Settings.SaveSettings();
        }
    }
}