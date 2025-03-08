using Newtonsoft.Json;
using System.IO;

namespace SteamItemsStatsViewer.Models
{
    public class SettingsModel
    {
        private string _currency;
        public string Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                if (App.Currency != null) App.Currency.UpdateExchangeRate();
            }
        }

        public SettingsModel(string? currency = "USD")
        {
            Currency = currency;

            LoadSettings();
        }

        public void LoadSettings()
        {
            string fileName = "Settings.json";
            string filePath = $"{App.MainDataFolder}\\{fileName}";

            if (File.Exists(filePath))
            {
                string file = File.ReadAllText(filePath);
                if (file == null) throw new Exception("Settings.json file is empty");
                JsonConvert.PopulateObject(file, this);
            }
            else
            {
                SaveSettings();
            }
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