using CSTracker.MVVM;
using Newtonsoft.Json;

namespace CSTracker.Models
{
    internal class InvestmentModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }

        [JsonIgnore]
        public ItemDataModel ItemData { get; set; }

        [JsonIgnore]
        public string QuantityText => $"Quantity: {Quantity.ToString("N0")}";

        [JsonIgnore]
        public string BoughtAtText => $"Bought at: {PricePerItem.ToString("N2")}{App.Settings.Currency}";

        [JsonIgnore]
        public string CurrentValueText => $"Current value: {ItemData.PriceHistory.Last().Value}{App.Settings.Currency}";
        
        [JsonIgnore] 
        public string TotalSpentText => $"Total Spent: {PricePerItem * Quantity}{App.Settings.Currency}";

        [JsonIgnore]
        public string CurrentWorthText => $"Current Worth: {ItemData.PriceHistory.Last().Value * Quantity}{App.Settings.Currency}";

        [JsonIgnore]
        public RelayCommand DeleteInvestmentCommand => new RelayCommand(execute => RemoveInvestment());

        [JsonIgnore]
        public Action<string> RemoveInvestmentAction { get; set; }

        public InvestmentModel(string name, int quantity, decimal pricePerItem, ItemDataModel itemData, Action<string> removeInvestmentAction)
        {
            Name = name;
            Quantity = quantity;
            PricePerItem = pricePerItem;
            ItemData = itemData;
            RemoveInvestmentAction = removeInvestmentAction;
        }

        private void RemoveInvestment()
        {
            RemoveInvestmentAction?.Invoke(Name);
        }
    }
}