using CSTracker.MVVM;
using CSTracker.Stores;
using System.Runtime.InteropServices;
using System.Windows;

namespace CSTracker.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        private Thickness _mainGridMargin;
        public Thickness MainGridMargin
        {
            get => _mainGridMargin;
            set
            {
                _mainGridMargin = value;
                OnPropertyChanged(nameof(MainGridMargin));
            }
        }

        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set
            {
                _windowState = value;
                OnPropertyChanged(nameof(WindowState));
                WindowStateChanged();
            }
        }

        private readonly NavigationStore _navigationStore;
        public ViewModelBase ViewModel => _navigationStore.ViewModel;

        public RelayCommand DragWindowCommand => new RelayCommand(execute => DragWindow());

        public RelayCommand MinimizeWindowCommand => new RelayCommand(execute => MinimizeWindow());
        public RelayCommand ChangeWindowStateCommand => new RelayCommand(execute => ChangeWindowState());
        public RelayCommand CloseWindowCommand => new RelayCommand(execute => CloseWindow()); 

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

        private void WindowStateChanged()
        {
            if (WindowState == WindowState.Normal) MainGridMargin = new Thickness(0);
            else if (WindowState == WindowState.Maximized) MainGridMargin = new Thickness(5);
        }

        private void DragWindow()
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Win32Point currentMousePosition = new Win32Point();
                GetCursorPos(ref currentMousePosition);

                double horizontalRatio = currentMousePosition.X / App.Current.MainWindow.ActualWidth;
                double verticalRatio = currentMousePosition.Y / App.Current.MainWindow.ActualHeight;

                double targetLeft = App.Current.MainWindow.RestoreBounds.Width * horizontalRatio;
                double targetTop = App.Current.MainWindow.RestoreBounds.Height * verticalRatio;

                ChangeWindowState();

                App.Current.MainWindow.Left = currentMousePosition.X - targetLeft;
                App.Current.MainWindow.Top = currentMousePosition.Y - targetTop;
            }

            App.Current.MainWindow.DragMove();
        }

        private void MinimizeWindow()
        {
            WindowState = WindowState.Minimized;
        }

        private void ChangeWindowState()
        {
            if (WindowState == WindowState.Normal) WindowState = WindowState.Maximized;
            else WindowState = WindowState.Normal;
        }

        private void CloseWindow()
        {
            App.Current.Shutdown();
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