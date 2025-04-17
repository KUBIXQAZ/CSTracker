using CSTracker.MVVM;
using Microsoft.Web.WebView2.Wpf;
using System.Windows;

namespace CSTracker.Resources.Controls
{
    class WebView : WebView2
    {
        public RelayCommand NavigationCompletedCommand
        {
            get { return (RelayCommand)GetValue(NavigationCompletedCommandProperty); }
            set { SetValue(NavigationCompletedCommandProperty, value); }
        }

        public static readonly DependencyProperty NavigationCompletedCommandProperty =
            DependencyProperty.Register("NavigationCompletedCommand", typeof(RelayCommand), typeof(WebView), new PropertyMetadata(null));


        public WebView()
        {
            NavigationCompleted += OnNavigationCompleted;
        }

        public virtual void OnNavigationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            NavigationCompletedCommand?.Execute(this);
        }
    }
}