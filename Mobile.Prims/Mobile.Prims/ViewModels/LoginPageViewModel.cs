using Common.Helpers;
using Common.Models;
using Common.Models.Request;
using Common.Models.Responses;
using Common.Services;
using Mobile.Prims.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;

namespace Mobile.Prims.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        private bool _isRunning;
        private bool _isEnabled;

        private string _password;
        private string _pageReturn;

        private DelegateCommand _loginCommand;

        public LoginPageViewModel(INavigationService navigationService,
                                  IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;

            IsEnabled = true;
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(LoginAsync));


        private async void LoginAsync()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar un usuario", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe ingresar una contraseña", "Aceptar");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "ConnectionError", "Accept");
                return;
            }

            string url = App.Current.Resources["UrlAPI"].ToString();
            TokenRequest request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            Response response = await _apiService.GetTokenAsync(url, "api", "/Account/CreateToken", request);
            
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                Password = string.Empty;
                return;
            }

            TokenResponse token = (TokenResponse)response.Result;
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.IsLogin = true;
            Password = string.Empty;

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync($"/{nameof(AppMasterDetailPage)}/NavigationPage/{nameof(AppTabbedPage)}");

            //if (string.IsNullOrEmpty(_pageReturn))
            //{
            //    await _navigationService.NavigateAsync($"/{nameof(AppMasterDetailPage)}/NavigationPage/{nameof(AppTabbedPage)}");
            //}
            //else
            //{
            //    await _navigationService.NavigateAsync($"/{nameof(AppMasterDetailPage)}/NavigationPage/{_pageReturn}");
            //}
        }
    }
}
