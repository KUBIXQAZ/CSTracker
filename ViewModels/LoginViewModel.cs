using CSTracker.MVVM;
using CSTracker.Resources.Controls;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace CSTracker.ViewModels
{
    internal class LoginViewModel : ViewModelBase
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

        public RelayCommand DragWindowCommand => new RelayCommand(execute => DragWindow());

        public RelayCommand MinimizeWindowCommand => new RelayCommand(execute => MinimizeWindow());
        public RelayCommand ChangeWindowStateCommand => new RelayCommand(execute => ChangeWindowState());
        public RelayCommand CloseWindowCommand => new RelayCommand(execute => CloseWindow());

        public RelayCommand NavigationCompletedCommand => new RelayCommand(execute => NavigationCompleted(execute));
        public RelayCommand OnClosingCommand => new RelayCommand(execute => OnClosing());

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
        private Window _window;

        public string WebViewSource => "https://cstracker.cloud/login-app";

        public string Token;

        public LoginViewModel(Window window)
        {
            _window = window;
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

        private void OnClosing()
        {
            if (!_window.DialogResult == true) Environment.Exit(0);
        }

        private void NavigationCompleted(object parameter)
        {
            WebView webView = parameter as WebView;

            string url = webView.Source.ToString();

            if (url.StartsWith("https://cstracker.cloud/login-app?token="))
            {
                Token = url.Substring(url.IndexOf("token=") + 6);

                File.WriteAllText($"{App.UserdataFolder}\\Token.txt", Token);

                _window.DialogResult = true;
                _window.Close();
            }
        }
    }
}