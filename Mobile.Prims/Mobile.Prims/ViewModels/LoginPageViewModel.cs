using Common.Models;
using Common.Models.Request;
using Common.Services;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;

namespace Mobile.Prims.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        private string _password;
        private string _pageReturn;

        private DelegateCommand _loginCommand;

        public LoginPageViewModel(INavigationService navigationService,
                                  IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
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
                await App.Current.MainPage.DisplayAlert("Error", "EmailError", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "PasswordError", "Accept");
                return;
            }

            //IsRunning = true;
            //IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                //IsRunning = false;
                //IsEnabled = true;
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
            //IsRunning = false;
            //IsEnabled = true;

            //if (!response.IsSuccess)
            //{
            //    await App.Current.MainPage.DisplayAlert("Error", "LoginError", "Accept");
            //    Password = string.Empty;
            //    return;
            //}

            //TokenResponse token = (TokenResponse)response.Result;
            //Settings.Token = JsonConvert.SerializeObject(token);
            //Settings.IsLogin = true;
            //Password = string.Empty;

            ////IsRunning = false;
            ////IsEnabled = true;

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
