using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
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
        public ObservableCollection<SteamItemNavigationItemModel> NavigationItems
        {
            get => _navigationItems;
            set
            {
                _navigationItems = value;
                OnPropertyChanged(nameof(NavigationItems));
            }
        };   
        
        public MainViewModel(ViewModelBase viewModel)
        {
            ViewModel = viewModel;

            LoadSteamItemsNavigationItems();
        }

        private void LoadSteamItemsNavigationItems()
        {
            string path = "D:\\PROGRAMMING-PROJECTS\\csharp-apps\\SteamMarketDataCollector\\bin\\Debug\\net8.0\\output";

            string[] directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {
                string directoryName = Path.GetFileName(directory);
                string name = directoryName.Replace("_"," ");

                SteamItemNavigationItemModel steamItemNavigationItem = new SteamItemNavigationItemModel(name, new NavigateToCommand(this, () => { return new DisplayItemDataViewModel(directory); }));
                _navigationItems.Add(steamItemNavigationItem);
            }
        }
    }
}
