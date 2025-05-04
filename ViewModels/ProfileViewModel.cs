using CSTracker.DTOs;
using CSTracker.Enums;
using CSTracker.Models;
using CSTracker.MVVM;
using CSTracker.Services;
using CSTracker.Views;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace CSTracker.ViewModels
{
    internal class ProfileViewModel : ViewModelBase
    {
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _currentPassword = string.Empty;
        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                _currentPassword = value;
                OnPropertyChanged(nameof(CurrentPassword));
            }
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        private string _newPasswordConfirm = string.Empty;
        public string NewPasswordConfirm
        {
            get => _newPasswordConfirm;
            set
            {
                _newPasswordConfirm = value;
                OnPropertyChanged(nameof(NewPasswordConfirm));
            }
        }

        public RelayCommand LogoutCommand => new RelayCommand(execute => Logout());
        public RelayCommand UpdateEmailCommand => new RelayCommand(execute => UpdateEmail());
        public RelayCommand UpdateUsernameCommand => new RelayCommand(execute => UpdateUsername());
        public RelayCommand UpdatePasswordCommand => new RelayCommand(execute => UpdatePassword());

        public ProfileViewModel()
        {
            LoadUserData();
        }

        private async void LoadUserData()
        {
            UserDataModel userData = null;

            try
            {
                using (HttpClient client = new HttpClientService().CreateHttpClient(App.BaseApiUrl, null, App.Token))
                {
                    var answer = await client.GetAsync($"User/GetUserData/{App.UserId}");

                    if (answer.IsSuccessStatusCode)
                    {
                        string content = await answer.Content.ReadAsStringAsync();
                        userData = JsonConvert.DeserializeObject<UserDataModel>(content);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }

            if (userData == null)
            {
                return;
            }

            Email = userData.Email;
            Username = userData.Username;

        }

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

        private async void UpdateEmail()
        {
            try
            {
                using (HttpClient client = new HttpClientService().CreateHttpClient(App.BaseApiUrl, null, App.Token))
                {
                    ChangeEmailRequest request = new ChangeEmailRequest
                    {
                        UserId = int.Parse(App.UserId),
                        Email = Email
                    };
                    string json = JsonConvert.SerializeObject(request);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var answer = await client.PutAsync("User/UpdateEmail", content);

                    string r = await answer.Content.ReadAsStringAsync();

                    if (answer.IsSuccessStatusCode)
                    {
                        AlertWindow alert = new AlertWindow();
                        AlertViewModel viewModel = new AlertViewModel(alert, "Success", r, AlertTypeEnum.Info);
                        alert.DataContext = viewModel;
                        alert.Owner = App.Current.MainWindow;
                        alert.ShowDialog();

                        LoadUserData();
                    }
                    else
                    {
                        AlertWindow alert = new AlertWindow();
                        AlertViewModel viewModel = new AlertViewModel(alert, "Error", r, AlertTypeEnum.Error);
                        alert.DataContext = viewModel;
                        alert.Owner = App.Current.MainWindow;
                        alert.ShowDialog();
                    }
                }

                LoadUserData();
            }
            catch (Exception)
            {
                AlertWindow alert = new AlertWindow();
                AlertViewModel viewModel = new AlertViewModel(alert, "Error", "Something went wrong, try again later.", AlertTypeEnum.Error);
                alert.DataContext = viewModel;
                alert.Owner = App.Current.MainWindow;
                alert.ShowDialog();
            }
        }

        private async void UpdateUsername()
        {
            try
            {
                using (HttpClient client = new HttpClientService().CreateHttpClient(App.BaseApiUrl, null, App.Token))
                {
                    ChangeUsernameRequest request = new ChangeUsernameRequest
                    {
                        UserId = int.Parse(App.UserId),
                        Username = Username
                    };
                    string json = JsonConvert.SerializeObject(request);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var answer = await client.PutAsync("User/UpdateUsername", content);

                    string r = await answer.Content.ReadAsStringAsync();

                    if (answer.IsSuccessStatusCode)
                    {
                        AlertWindow alert = new AlertWindow();
                        AlertViewModel viewModel = new AlertViewModel(alert, "Success", r, AlertTypeEnum.Info);
                        alert.DataContext = viewModel;
                        alert.Owner = App.Current.MainWindow;
                        alert.ShowDialog();
                    }
                    else
                    {
                        AlertWindow alert = new AlertWindow();
                        AlertViewModel viewModel = new AlertViewModel(alert, "Error", r, AlertTypeEnum.Error);
                        alert.DataContext = viewModel;
                        alert.Owner = App.Current.MainWindow;
                        alert.ShowDialog();
                    }
                }

                LoadUserData();
            }
            catch (Exception)
            {
                AlertWindow alert = new AlertWindow();
                AlertViewModel viewModel = new AlertViewModel(alert, "Error", "Something went wrong, try again later.", AlertTypeEnum.Error);
                alert.DataContext = viewModel;
                alert.Owner = App.Current.MainWindow;
                alert.ShowDialog();
            }
        }

        private async void UpdatePassword()
        {
            if (!NewPassword.Equals(NewPasswordConfirm))
            {
                AlertWindow alert = new AlertWindow();
                AlertViewModel viewModel = new AlertViewModel(alert, "Error", "Password and confirm password do not match.", AlertTypeEnum.Error);
                alert.DataContext = viewModel;
                alert.Owner = App.Current.MainWindow;
                alert.ShowDialog();

                return;
            }

            try
            {
                using (HttpClient client = new HttpClientService().CreateHttpClient(App.BaseApiUrl, null, App.Token))
                {
                    ChangePasswordRequest request = new ChangePasswordRequest
                    {
                        UserId = int.Parse(App.UserId),
                        OldPassword = CurrentPassword,
                        NewPassword = NewPassword
                    };
                    string json = JsonConvert.SerializeObject(request);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var answer = await client.PutAsync("User/UpdatePassword", content);

                    string r = await answer.Content.ReadAsStringAsync();

                    if (answer.IsSuccessStatusCode)
                    {
                        AlertWindow alert = new AlertWindow();
                        AlertViewModel viewModel = new AlertViewModel(alert, "Success", r, AlertTypeEnum.Info);
                        alert.DataContext = viewModel;
                        alert.Owner = App.Current.MainWindow;
                        alert.ShowDialog();

                        CurrentPassword = string.Empty;
                        NewPassword = string.Empty;
                        NewPasswordConfirm = string.Empty;
                    }
                    else
                    {
                        AlertWindow alert = new AlertWindow();
                        AlertViewModel viewModel = new AlertViewModel(alert, "Error", r, AlertTypeEnum.Error);
                        alert.DataContext = viewModel;
                        alert.Owner = App.Current.MainWindow;
                        alert.ShowDialog();
                    }
                }
            }
            catch (Exception)
            {
                AlertWindow alert = new AlertWindow();
                AlertViewModel viewModel = new AlertViewModel(alert, "Error", "Something went wrong, try again later.", AlertTypeEnum.Error);
                alert.DataContext = viewModel;
                alert.Owner = App.Current.MainWindow;
                alert.ShowDialog();
            }
        }
    }
}