using Newtonsoft.Json;
using SteamItemsStatsViewer.DTOs;
using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.Stores;
using SteamItemsStatsViewer.ViewModels;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace SteamItemsStatsViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string MainDataFolder;
        public static string TempDataFolder;

        public static SettingsModel Settings {  get; set; }

        public static Dictionary<string, double> ExchangeRates = new Dictionary<string,double>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CreateAppDataFolders();
            GetExchangeRates();
            LoadExchangeRates();
            LoadSettings();

            NavigationStore navigationStore = new NavigationStore();

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel(navigationStore);
            mainWindow.Show();
        }

        private void CreateAppDataFolders()
        {
            MainDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\";
            TempDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\KUBIXQAZ\\SteamItemsStatsViewer\\Temp";

            if(!Directory.Exists(MainDataFolder)) Directory.CreateDirectory(MainDataFolder);
            if(!Directory.Exists(TempDataFolder)) Directory.CreateDirectory(TempDataFolder);
        }

        private void GetExchangeRates()
        {
            string fileName = "ExchangeRatesFile.json";
            string filePath = $"{TempDataFolder}\\{fileName}";

            if (File.Exists(filePath))
            {
                string file = File.ReadAllText(filePath);
                ExchangeRatesModel exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesModel>(file);
                if (DateTime.Now.Date == exchangeRates.Date.Date) return;
            }

            using (HttpClient httpClient = new HttpClient())
            {
                string data = httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/usd").GetAwaiter().GetResult();

                File.WriteAllText(filePath, data);
            }
        }

        private void LoadExchangeRates()
        {
            string fileName = "ExchangeRatesFile.json";
            string filePath = $"{TempDataFolder}\\{fileName}";

            if (File.Exists(filePath))
            {
                string file = File.ReadAllText(filePath);
                ExchangeRatesModel exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesModel>(file);
                if (exchangeRates == null) throw new Exception("exchangeRates is null");
                ExchangeRates = exchangeRates.Rates;
            }
            else
            {
                throw new Exception("ExchangeRatesFile.json file does not exist");
            }
        }

        private void LoadSettings()
        {
            string fileName = "Settings.json";
            string filePath = $"{MainDataFolder}\\{fileName}";

            if (File.Exists(filePath))
            {
                string file = File.ReadAllText(filePath);
                if (file == null) throw new Exception("Settings.json file is empty");
                Settings = JsonConvert.DeserializeObject<SettingsModel>(file);
            } 
            else
            {
                Settings = new SettingsModel("USD");
                Settings.SaveSettings();
            }
        }
    }
}
