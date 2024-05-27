using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SteamItemsStatsViewer.Commands;
using SteamItemsStatsViewer.Models;
using SteamItemsStatsViewer.ViewModels;

namespace SteamItemsStatsViewer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _viewModel;
        public ViewModelBase ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                OnPropertyChanged(nameof(ViewModel));
            }
        }

        private ObservableCollection<SteamItemNavigationItemModel> _navigationItems = new ObservableCollection<SteamItemNavigationItemModel>();
        public IEnumerable<SteamItemNavigationItemModel> NavigationItems => _navigationItems;   
        
        public MainViewModel(ViewModelBase viewModel)
        {
            ViewModel = viewModel;

            LoadSteamItemsNavigationItems();
        }

        private void LoadSteamItemsNavigationItems()
        {
            string path = "D:\\PROGRAMMING-PROJECTS\\csharp-apps\\SteamMarketDataCollector\\bin\\Debug\\net8.0\\output";

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                string name = Path.GetFileName(file).Replace("_"," ").Replace(".json","");

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(name,new NavigateToCommand(this, () => { return new DisplayItemDataViewModel(file); }));
                _navigationItems.Add(steamItemNavigationItem);
            }
        }
    }
}
