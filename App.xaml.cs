using CSTracker.Models;
using CSTracker.Services;
using CSTracker.Stores;
using CSTracker.ViewModels;
using CSTracker.Views;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Windows;

namespace CSTracker
{
    public partial class App : Application
    {
        public static string MainDataFolder;
        public static string TempDataFolder;
        public static string IconFolder;
        public static string UserdataFolder;
        public static string InvestmentsFolder;

        public static SettingsModel Settings;
        public static CurrencyModel Currency;

        public static string BaseApiUrl = "https://api.cstracker.cloud/api/";

        public static string UserId = string.Empty;
        public static string Token = string.Empty;
        public static string ApiKey = string.Empty;

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CreateAppDataFolders();

            Settings = new SettingsModel();
            Currency = new CurrencyModel();

            NavigationStore navigationStore = new NavigationStore();

            await LoginUser();

            MainWindow mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel(navigationStore);
            mainWindow.Show();
        }

        private async Task LoginUser()
        {
            bool isLoggedIn = await IsUserLoggedIn();

            if (!isLoggedIn)
            {
                LoginWindow loginWindow = new LoginWindow();
                LoginViewModel loginViewModel = new LoginViewModel(loginWindow);
                loginWindow.DataContext = loginViewModel;
                loginWindow.ShowDialog();
            }

            Token = File.ReadAllText($"{UserdataFolder}\\Token.txt").Trim();

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(Token);

            UserId = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            ApiKey = await GetUserKey(UserId);
        }

        private async Task<string> GetUserKey(string userId)
        {
            using (HttpClient client = new HttpClientService().CreateHttpClient(BaseApiUrl, null, Token))
            {
                var answer = await client.GetAsync($"User/GetUserApiKey/{userId}");

                if (answer.IsSuccessStatusCode)
                {
                    string json = await answer.Content.ReadAsStringAsync();
                    var keyObj = JObject.Parse(json);
                    return (string)keyObj["apiKey"];
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<bool> IsUserLoggedIn()
        {
            if (!File.Exists($"{UserdataFolder}\\Token.txt")) return false;
            else
            {
                string token = File.ReadAllText($"{UserdataFolder}\\Token.txt").Trim();

                using (HttpClient client = new HttpClientService().CreateHttpClient(App.BaseApiUrl, null, token))
                {
                    var response = await client.GetAsync("user/validate-token");

                    if (response.IsSuccessStatusCode) return true;
                    else return false;
                }
            }
        }

        private void CreateAppDataFolders()
        {
            MainDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\CSTracker\\";
            TempDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\CSTracker\\Temp";
            IconFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\CSTracker\\Icons";
            UserdataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\CSTracker\\Userdata";
            InvestmentsFolder = $"{UserdataFolder}\\Investments";

            if (!Directory.Exists(MainDataFolder)) Directory.CreateDirectory(MainDataFolder);
            if (!Directory.Exists(TempDataFolder)) Directory.CreateDirectory(TempDataFolder);
            if (!Directory.Exists(IconFolder)) Directory.CreateDirectory(IconFolder);
            if (!Directory.Exists(UserdataFolder)) Directory.CreateDirectory(UserdataFolder);
            if (!Directory.Exists(InvestmentsFolder)) Directory.CreateDirectory(InvestmentsFolder);
        }
    }
}