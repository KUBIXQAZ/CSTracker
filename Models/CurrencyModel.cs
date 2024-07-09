using Newtonsoft.Json;
using SteamItemsStatsViewer.DTOs;
using System.IO;

namespace SteamItemsStatsViewer.Models
{
    public class CurrencyModel
    {
        public List<string> Currencies { get; set; }

        public CurrencyModel()
        {
            Currencies = new List<string>();

            LoadCurrencies();
        }

        private void LoadCurrencies()
        {
            string fileName = "ExchangeRatesFile.json";
            string filePath = $"{App.TempDataFolder}\\{fileName}";

            if (File.Exists(filePath))
            {
                string file = File.ReadAllText(filePath);
                ExchangeRatesModel exchangeRates = JsonConvert.DeserializeObject<ExchangeRatesModel>(file);
                Currencies = exchangeRates.Rates.Keys.ToList();
            }
            else
            {
                throw new Exception("ExchangeRatesFile.json file does not exist");
            }
        }
    }
}
