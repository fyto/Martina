using Common.Helpers;
using Common.Models;
using Mobile.Prims.Views;
using Prism.Commands;
using Prism.Navigation;


namespace Mobile.Prims.ItemsViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if (PageName == nameof(LoginPage) && Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.Token = null;
            }

            if (IsLoginRequired && !Settings.IsLogin)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe iniciar sesión para completar esta acción.", "Aceptar");
                NavigationParameters parameters = new NavigationParameters
                {
                    { "pageReturn", PageName }
                };

                await _navigationService.NavigateAsync($"/{nameof(AppMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}", parameters);
            }
            else
            {
                await _navigationService.NavigateAsync($"/{nameof(AppMasterDetailPage)}/NavigationPage/{PageName}");
            }


            //if (PageName == nameof(LoginPage) && Settings.IsLogin)
            //{
            //    Settings.IsLogin = false;
            //    Settings.Token = null;
            //}

            //await _navigationService.NavigateAsync($"/{nameof(AppMasterDetailPage)}/NavigationPage/{PageName}");



            //if (IsLoginRequired && !Settings.IsLogin)
            //{
            //    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginFirstMessage, Languages.Accept);
            //    NavigationParameters parameters = new NavigationParameters
            //    {
            //        { "pageReturn", PageName }
            //    };

            //    await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{nameof(LoginPage)}", parameters);
            //}
            //else
            //{
            //    await _navigationService.NavigateAsync($"/{nameof(OnSaleMasterDetailPage)}/NavigationPage/{PageName}");
            //}
        }
    }
}
