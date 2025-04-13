using CSTracker.ViewModels;

namespace CSTracker.Stores
{
    public class NavigationStore
    {
        public event Action ViewModelChanged;

        private ViewModelBase _viewModel;
        public ViewModelBase ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                OnViewModelChanged();
            }
        }

        private void OnViewModelChanged()
        {
            ViewModelChanged?.Invoke();
        }
    }
}