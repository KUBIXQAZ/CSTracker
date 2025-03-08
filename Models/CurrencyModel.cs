using Newtonsoft.Json;
using SteamItemsStatsViewer.DTOs;
using System.IO;
using System.Net.Http;

namespace SteamItemsStatsViewer.Models
{
    public class CurrencyModel
    {
        public Dictionary<string, decimal> Currencies = new Dictionary<string, decimal>();

        public decimal ExchangeRate;

        public CurrencyModel()
        {
            LoadCurrencies();

            UpdateExchangeRate();
        }

        public void UpdateExchangeRate()
        {
            if (!Currencies.ContainsKey(App.Settings.Currency)) throw new Exception("Invalid currency");
            ExchangeRate = Currencies.First(x => x.Key == App.Settings.Currency).Value;
        }

        private void LoadCurrencies()
        {
            string fileName = "ExchangeRatesFile.json";
            string filePath = $"{App.TempDataFolder}\\{fileName}";

            if (File.Exists(filePath))
            {
                string file = File.ReadAllText(filePath);
                ExchangeRatesModel exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesModel>(file);
                if (DateTime.Now.Date == exchangeRates.Date.Date)
                {
                    Currencies = exchangeRates.Rates;
                    return;
                }
            }

            using (HttpClient httpClient = new HttpClient())
            {
                string data = httpClient.GetStringAsync("https://api.exchangerate-api.com/v4/latest/usd").GetAwaiter().GetResult();
                ExchangeRatesModel exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesModel>(data);
                Currencies = exchangeRates.Rates;

                File.WriteAllText(filePath, data);
            }
        }
    }
}