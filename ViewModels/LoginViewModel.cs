using CSTracker.MVVM;
using CSTracker.Resources.Controls;
using System.Windows;

namespace CSTracker.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        public string WebViewSource => "https://cstracker.cloud/login-app";

        public RelayCommand NavigationCompletedCommand => new RelayCommand(execute => NavigationCompleted(execute));

        private Window _window;

        public LoginViewModel(Window window)
        {
            _window = window;
        }

        private void NavigationCompleted(object parameter)
        {
            WebView webView = parameter as WebView;

            string url = webView.Source.ToString();

            if (url.StartsWith("https://cstracker.cloud/login-app?token="))
            {
                string token = url.Substring(url.IndexOf("token=") + 6);

                _window.DialogResult = true;
                _window.Close();
            }
        }
    }
}