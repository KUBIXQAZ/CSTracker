using CSTracker.MVVM;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace CSTracker.ViewModels
{
    internal class ProfileViewModel : ViewModelBase
    {
        public RelayCommand LogoutCommand => new RelayCommand(execute => Logout());

        private void Logout()
        {
            string path = $"{App.UserdataFolder}\\token.txt";

            if (File.Exists(path))
            {
                File.Delete(path);

                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
        }
    }
}
