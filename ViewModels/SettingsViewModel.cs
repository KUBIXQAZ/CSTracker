using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SteamItemsStatsViewer.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ObservableCollection<string> _currencies = new ObservableCollection<string>();
        public ObservableCollection<string> Currencies
        {
            get => _currencies;
            set
            {
                _currencies = value;
                OnPropertyChanged(nameof(Currencies));
            }
        }

        private string _selectedCurrency;
        public string SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                OnPropertyChanged(nameof(SelectedCurrency));
            }
        }

        public ICommand SaveSettingsCommand { get; set; }
        public ICommand ResetSettingsCommand { get; set; }

        public SettingsViewModel()
        {
            LoadCurrencies();
            _selectedCurrency = App.Settings.Currency;

            SaveSettingsCommand = new SaveSettingsCommand(this);
            ResetSettingsCommand = new RelayCommand(execute => ResetSettings());
        }

        private void LoadCurrencies()
        {
            CurrencyModel currencies = new CurrencyModel();
            foreach (string item in currencies.Currencies)
            {
                _currencies.Add(item);
            }
        }

        private void ResetSettings()
        {
            Directory.Delete(App.MainDataFolder, true);

            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
            Application.Current.Shutdown();
        }
    }
}