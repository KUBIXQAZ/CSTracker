using SteamItemsStatsViewer.MVVM;
using System.Windows.Media;
using System.Windows;

namespace SteamItemsStatsViewer.ViewModels
{
    class InputPromptViewModel : ViewModelBase
    {
        private Window _inputPromptWindow;

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

        private string _inputText;
        public string InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged(nameof(InputText));
            }
        }

        public SolidColorBrush BorderColor { get; set; }

        public RelayCommand DragWindowCommand => new RelayCommand(execute => DragWindow());
        public RelayCommand CloseWindowCommand => new RelayCommand(execute => CloseWindow());
        public RelayCommand OkCommand => new RelayCommand(execute => Ok());

        public InputPromptViewModel(Window window, string title, string text)
        {
            _inputPromptWindow = window;

            Title = title;
            Text = text;
        }

        private void DragWindow()
        {
            _inputPromptWindow.DragMove();
        }

        private void CloseWindow()
        {
            _inputPromptWindow.DialogResult = false;
            _inputPromptWindow.Close();
        }

        private void Ok()
        {
            _inputPromptWindow.DialogResult = true;
            _inputPromptWindow.Close();
        }
    }
}