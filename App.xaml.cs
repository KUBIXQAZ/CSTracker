using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Stores;
using SteamItemsStatsViewer.ViewModels;
using System.IO;
using System.Windows;

namespace SteamItemsStatsViewer
{
    public partial class App : Application
    {
        public static string MainDataFolder;
        public static string TempDataFolder;
        public static string IconFolder;

        public static SettingsModel Settings;
        public static CurrencyModel Currency;

        public static string BaseApiUrl = "http://192.168.31.71:5000/api/ItemsData/";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CreateAppDataFolders();

            Settings = new SettingsModel();
            Currency = new CurrencyModel();

            NavigationStore navigationStore = new NavigationStore();

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel(navigationStore);
            mainWindow.Show();
        }

        private void CreateAppDataFolders()
        {
            MainDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\";
            TempDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\Temp";
            IconFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\Icons";

            if (!Directory.Exists(MainDataFolder)) Directory.CreateDirectory(MainDataFolder);
            if (!Directory.Exists(TempDataFolder)) Directory.CreateDirectory(TempDataFolder);
            if (!Directory.Exists(IconFolder)) Directory.CreateDirectory(IconFolder);
        }
    }
}