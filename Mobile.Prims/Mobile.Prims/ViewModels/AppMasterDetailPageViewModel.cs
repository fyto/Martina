using Common.Helpers;
using Common.Models;
using Mobile.Prims.ItemsViewModels;
using Mobile.Prims.Views;
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
        //private static GeneralMasterDetailPageViewModel _instance;
        public AppMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            //_instance = this;
            _navigationService = navigationService;

            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_person",
                    PageName = $"{nameof(EditUserPage)}",
                    Title = "Editar usuario"
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
