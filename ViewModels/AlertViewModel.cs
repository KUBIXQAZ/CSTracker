using CSTracker.Enums;
using CSTracker.MVVM;
using System.Windows;
using System.Windows.Media;

namespace CSTracker.ViewModels
{
    class AlertViewModel : ViewModelBase
    {
        private Window _informationWindow;

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public SolidColorBrush BorderColor { get; set; }

        public RelayCommand DragWindowCommand => new RelayCommand(execute => DragWindow());
        public RelayCommand CloseWindowCommand => new RelayCommand(execute => CloseWindow());

        private AlertTypeEnum _alertType;

        public AlertViewModel(Window window, string title, string text, AlertTypeEnum type)
        {
            _informationWindow = window;

            Title = title;
            Text = text;

            _alertType = type;

            if (_alertType == AlertTypeEnum.Info)
            {
                BorderColor = (SolidColorBrush)Application.Current.Resources["AccentColor"];
            }
            else if (_alertType == AlertTypeEnum.Error)
            {
                BorderColor = (SolidColorBrush)Application.Current.Resources["ErrorColor"];
            }
        }

        private void DragWindow()
        {
             _informationWindow.DragMove();
        }

        private void CloseWindow()
        {
            _informationWindow.Close();
        }
    }
}