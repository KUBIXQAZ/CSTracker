using SteamItemsStatsViewer.ViewModels;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SteamItemsStatsViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ViewModelBase ViewModel { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ViewModel = new HomeViewModel();

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel(ViewModel);
            mainWindow.Show();
        }
    }
}
