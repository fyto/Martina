using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Prims.ViewModels
{
    public class EditUserPageViewModel : ViewModelBase
    {
        public EditUserPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Editar usuario";
        }
    }
}
