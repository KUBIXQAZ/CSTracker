using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Services;
using SteamItemsStatsViewer.Stores;
using SteamItemsStatsViewer.ViewModels;
using SteamItemsStatsViewer.Views;
using System.IO;
using System.IO.Packaging;
using System.Windows;

namespace SteamItemsStatsViewer
{
    public partial class App : Application
    {
        public static string MainDataFolder;
        public static string TempDataFolder;
        public static string IconFolder;
        public static string UserdataFolder;

        public static SettingsModel Settings;
        public static CurrencyModel Currency;

        public static string BaseApiUrl = "https://api.cstracker.cloud/api/ItemsData/";
        public static string ApiKey = string.Empty;

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CreateAppDataFolders();

            Settings = new SettingsModel();
            Currency = new CurrencyModel();

            NavigationStore navigationStore = new NavigationStore();

            await Task.Run(LoadUserdata);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.DataContext = new MainViewModel(navigationStore);
        }

        private void CreateAppDataFolders()
        {
            MainDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\";
            TempDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\Temp";
            IconFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\Icons";
            UserdataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\Userdata";

            if (!Directory.Exists(MainDataFolder)) Directory.CreateDirectory(MainDataFolder);
            if (!Directory.Exists(TempDataFolder)) Directory.CreateDirectory(TempDataFolder);
            if (!Directory.Exists(IconFolder)) Directory.CreateDirectory(IconFolder);
            if (!Directory.Exists(UserdataFolder)) Directory.CreateDirectory(UserdataFolder);
        }

        private void LoadUserdata()
        {
            string userdataPath = $"{UserdataFolder}\\Userdata.txt";

            if (File.Exists(userdataPath))
            {
                string content = File.ReadAllText(userdataPath).Trim();

                if (!string.IsNullOrEmpty(content))
                {
                    ApiKey = content;
                }
            }
            else
            {
                InputPromptWindow inputPromptWindow = new InputPromptWindow();
                inputPromptWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                InputPromptViewModel inputPromptViewModel = new InputPromptViewModel(inputPromptWindow, "License", "Enter your license:");
                inputPromptWindow.DataContext = inputPromptViewModel;
                inputPromptWindow.ShowDialog();

                if (inputPromptWindow.DialogResult == true)
                {
                    string inputText = (inputPromptViewModel.InputText ?? string.Empty).Trim();

                    if (!string.IsNullOrEmpty(inputText))
                    {
                        ApiKey = inputText;

                        File.WriteAllText(userdataPath, ApiKey);
                    }
                }
            }
        }
    }
}