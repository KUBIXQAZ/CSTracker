using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamItemsStatsViewer.ViewModels;

namespace SteamItemsStatsViewer.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public ViewModelBase ViewModel { get; set; }

        public MainViewModel(ViewModelBase viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
