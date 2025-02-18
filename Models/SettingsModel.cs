using Newtonsoft.Json;
using System.IO;

namespace SteamItemsStatsViewer.Models
{
    public class SettingsModel
    {
        public string Currency;
        public decimal ExchangeRate;

        public SettingsModel(string currency)
        {
            Currency = currency;

            ExchangeRate = App.ExchangeRates.Where(x => x.Key == Currency).ToArray()[0].Value;
        }

        public void SaveSettings()
        {
            string fileName = "Settings.json";
            string filePath = $"{App.MainDataFolder}\\{fileName}";

            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, json);
        }
    }
}