using Common.Models;
using Common.Models.Request;
using Common.Services;
using Mobile.Prims.Helpers;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mobile.Prims.ViewModels
{
    public class RecoverPasswordPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private readonly IRegexHelper _regexHelper;
        private bool _isRunning;
        private bool _isEnabled;
        private string _email;

        private DelegateCommand _recoverCommand;

        public RecoverPasswordPageViewModel(
            INavigationService navigationService,
            IApiService apiService,
            IRegexHelper regexHelper)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _regexHelper = regexHelper;
            Title = "RecoverPassword";
            IsEnabled = true;
        }

        public DelegateCommand RecoverCommand => _recoverCommand ?? (_recoverCommand = new DelegateCommand(RecoverAsync));

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("email"))
            {
                Email = parameters.GetValue<string>("email");
            }
        }


        private async void RecoverAsync()
        {
            bool isValid = await ValidateData();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "ConnectionError", "Aceptar");
                return;
            }

            EmailRequest request = new EmailRequest { Email = Email };
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.RecoverPasswordAsync(url, "/api", "/Account/RecoverPassword", request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                if (response.Message == "Error001")
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Error001", "Accept");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                }

                return;
            }

            await App.Current.MainPage.DisplayAlert("Confirmación", "Las instrucciones para el cambio de contraseña han sido enviadas a su email.", "Aceptar");
            await _navigationService.GoBackAsync();
        }

        private async Task<bool> ValidateData()
        {
            if (string.IsNullOrEmpty(Email) || !_regexHelper.IsValidEmail(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "EmailError", "Accept");
                return false;
            }

            return true;
        }



    }
}
