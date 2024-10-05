using SteamItemsStatsViewer.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SteamItemsStatsViewer.Resources.Controls
{
    public class MainScrollViewer : ScrollViewer
    {
        public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            "ViewModel",
            typeof(ViewModelBase),
            typeof(MainScrollViewer)
        );

        public ViewModelBase ViewModel
        {
            get 
            { 
                return (ViewModelBase)GetValue(ViewModelProperty); 
            }
            set 
            { 
                SetValue(ViewModelProperty, value); 
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if(ViewModel is DisplayItemDataViewModel)
            {
                return;  
            } 
            else
            {
                base.OnMouseWheel(e);
            }
        }
    }
}