using SteamItemsStatsViewer.MVVM;
using SteamItemsStatsViewer.Stores;

namespace SteamItemsStatsViewer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        public ViewModelBase ViewModel => _navigationStore.ViewModel;

        public RelayCommand HomeNavigationCommand => new RelayCommand(execute => NavigateHome());
        public RelayCommand SettingsNavigationCommand => new RelayCommand(execute => NavigateSettings());

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.ViewModelChanged += OnViewModelChanged;

            _navigationStore.ViewModel = new HomeViewModel(navigationStore);
        }

        private void OnViewModelChanged()
        {
            OnPropertyChanged(nameof(ViewModel));
        }

        private void NavigateHome()
        {
            _navigationStore.ViewModel = new HomeViewModel(_navigationStore);
        }

        private void NavigateSettings()
        {
            _navigationStore.ViewModel = new SettingsViewModel();
        }
    }
}