using Common.Helpers;
using Common.Models;
using Common.Models.Responses;
using Mobile.Prims.ItemsViewModels;
using Mobile.Prims.Views;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Mobile.Prims.ViewModels
{
    public class AppMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        
        private static AppMasterDetailPageViewModel _instance;

        private UserResponse _user;

        public AppMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _instance = this;

            _navigationService = navigationService;

            LoadMenus();
            LoadUser();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        private void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
            }
        }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_person",
                    PageName = $"{nameof(EditUserPage)}",
                    Title = "Editar usuario",
                    IsLoginRequired = true                    
                },
                new Menu
                {
                    Icon = "ic_exit",
                    PageName = $"{nameof(LoginPage)}",
                    Title = Settings.IsLogin ? "Cerrar sesión" : "Login"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired = m.IsLoginRequired
                }).ToList());
        }

    }
}
