using CSTracker.Models;
using CSTracker.MVVM;
using CSTracker.Services;
using CSTracker.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace CSTracker.ViewModels
{
    internal class InvestmentsViewModel : ViewModelBase
    {
        private List<ItemDataModel> _itemsData = new List<ItemDataModel>();
        public List<ItemDataModel> ItemsData
        {
            get => _itemsData;
            set
            {
                _itemsData = value;
                OnPropertyChanged(nameof(ItemsData));
                OnPropertyChanged(nameof(ItemNames));
            }
        }

        public List<string> ItemNames => _itemsData?.Select(x => x.Name).ToList();

        private ObservableCollection<InvestmentModel> _investments = new ObservableCollection<InvestmentModel>();
        public ObservableCollection<InvestmentModel> Investments
        {
            get => _investments;
            set
            {
                _investments = value;
                OnPropertyChanged(nameof(Investments));
            }
        }

        private string _investmentName;
        public string InvestmentName
        {
            get => _investmentName;
            set
            {
                _investmentName = value;
                OnPropertyChanged(nameof(InvestmentName));
            }
        }

        private string _investmentQuantity;
        public string InvestmentQuantity
        {
            get => _investmentQuantity;
            set
            {
                _investmentQuantity = value;
                OnPropertyChanged(nameof(InvestmentQuantity));
            }
        }

        private string _investmentPrice;
        public string InvestmentPrice
        {
            get => _investmentPrice;
            set
            {
                _investmentPrice = value;
                OnPropertyChanged(nameof(InvestmentPrice));
            }
        }

        private Visibility _showInvestmentPanel = Visibility.Collapsed;
        public Visibility ShowInvestmentPanel
        {
            get => _showInvestmentPanel;
            set
            {
                _showInvestmentPanel = value;
                OnPropertyChanged(nameof(ShowInvestmentPanel));
            }
        }

        private string _showAddInvestmentPanelButtonText = "Add";
        public string ShowAddInvestmentPanelButtonText
        {
            get => _showAddInvestmentPanelButtonText;
            set
            {
                _showAddInvestmentPanelButtonText = value;
                OnPropertyChanged(nameof(ShowAddInvestmentPanelButtonText));
            }
        }

        public RelayCommand ShowAddInvestmentPanelCommand => new RelayCommand(execute => ShowAddInvestmentPanel());
        public RelayCommand AddInvestmentCommand => new RelayCommand(execute => AddInvestment());

        public InvestmentsViewModel()
        {
            Task.Run(async () =>
            {
                await LoadItemData();
                LoadInvestments();
            });      
        }

        private async Task LoadItemData()
        {
            try
            {
                using (HttpClient client = new HttpClientService().CreateHttpClient(App.BaseApiUrl, App.ApiKey))
                {
                    client.BaseAddress = new Uri(App.BaseApiUrl);

                    var answer = await client.GetAsync("ItemData/GetItemsData");

                    if (answer.IsSuccessStatusCode)
                    {
                        string content = await answer.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(content)) return;

                        var data = JsonConvert.DeserializeObject<List<ItemDataModel>>(content);

                        foreach (var item in data)
                        {
                            var updatedPriceHistory = item.PriceHistory
                                .ToDictionary(
                                    x => x.Key.ToLocalTime(),
                                    x => x.Value
                                );

                            item.PriceHistory = updatedPriceHistory;

                            var updatedQuantityHistory = item.QuantityHistory
                                .ToDictionary(
                                    x => x.Key.ToLocalTime(),
                                    x => x.Value
                                );

                            item.QuantityHistory = updatedQuantityHistory;
                        }

                        ItemsData = data;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (HttpRequestException)
            {
                AlertWindow alertWindow = new AlertWindow();
                alertWindow.Owner = App.Current.MainWindow;
                AlertViewModel vm = new AlertViewModel(alertWindow, "Error", "An unexpected issue occurred while loading items. Please try again.", Enums.AlertTypeEnum.Error);
                alertWindow.DataContext = vm;
                alertWindow.ShowDialog();
            }
        }

        private void ShowAddInvestmentPanel()
        {
            if (_showInvestmentPanel == Visibility.Visible)
            {
                ShowInvestmentPanel = Visibility.Collapsed;
                ShowAddInvestmentPanelButtonText = "Add";
            }
            else
            {
                ShowInvestmentPanel = Visibility.Visible;
                ShowAddInvestmentPanelButtonText = "Hide";
            }
        }

        private void LoadInvestments()
        {
            string path = App.InvestmentsFolder + "\\Investments.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                if (!string.IsNullOrEmpty(json)) Investments = JsonConvert.DeserializeObject<ObservableCollection<InvestmentModel>>(json)!;

                foreach (var item in Investments)
                {
                    item.ItemData = ItemsData.First(x => x.Name == item.Name);
                    if (item.ItemData.Image != null) item.ItemData.IconPath = $"{App.IconFolder}\\{item.ItemData.Image.Select(x => (int)x).ToArray().Sum()}.png";
                    item.RemoveInvestmentAction = RemoveInvestment;
                }
            }
        }

        private void RemoveInvestment(string name)
        {
            Investments.Remove(Investments.First(x => x.Name == name));

            SaveInvestments();
        }

        public void SaveInvestments()
        {
            string path = App.InvestmentsFolder + "\\Investments.json";
            string json = JsonConvert.SerializeObject(Investments);
            File.WriteAllText(path, json);
        }
        
        private void AddInvestment()
        {
            ItemDataModel itemData = ItemsData.First(x => x.Name == InvestmentName);
            itemData.IconPath = $"{App.IconFolder}\\{itemData.Image.Select(x => (int)x).ToArray().Sum()}.png";

            InvestmentModel investment = new InvestmentModel(InvestmentName, int.Parse(InvestmentQuantity), decimal.Parse(InvestmentPrice), itemData, RemoveInvestment);

            Investments.Add(investment);

            SaveInvestments();

            InvestmentName = string.Empty;
            InvestmentQuantity = string.Empty;
            InvestmentPrice = string.Empty;
        }
    }
}