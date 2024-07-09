using Newtonsoft.Json;
using SteamItemsStatsViewer.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamItemsStatsViewer.Models
{
    public class SettingsModel
    {
        public string Currency;

        public SettingsModel(string currency)
        {
            Currency = currency;
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
