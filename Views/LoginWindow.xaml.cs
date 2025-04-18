using CSTracker.MVVM;
using System.ComponentModel;
using System.Windows;

namespace CSTracker.Views
{
    public partial class LoginWindow : Window
    {
        public RelayCommand OnClosingCommand
        {
            get { return (RelayCommand)GetValue(OnClosingCommandProperty); }
            set { SetValue(OnClosingCommandProperty, value); }
        }

        public static readonly DependencyProperty OnClosingCommandProperty =
            DependencyProperty.Register("OnClosingCommand", typeof(RelayCommand), typeof(LoginWindow), new PropertyMetadata(null));

        public LoginWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            OnClosingCommand?.Execute(null);
        }
    }
}