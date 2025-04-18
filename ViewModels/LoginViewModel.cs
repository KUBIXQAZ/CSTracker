using CSTracker.MVVM;
using CSTracker.Resources.Controls;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace CSTracker.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        public string WebViewSource => "https://cstracker.cloud/login-app";

        public RelayCommand NavigationCompletedCommand => new RelayCommand(execute => NavigationCompleted(execute));
        public RelayCommand OnClosingCommand => new RelayCommand(execute => OnClosing());

        public string Token;

        private Window _window;

        public LoginViewModel(Window window)
        {
            _window = window;
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