using Common.Helpers;
using Common.Services;
using Mobile.Prims.Helpers;
using Mobile.Prims.ViewModels;
using Mobile.Prims.Views;
using Mobile.Prims.Views.Login;
using Prism;
using Prism.Ioc;
using Syncfusion.Licensing;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;


namespace Mobile.Prims
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("NTgwNTUwQDMxMzkyZTM0MmUzMGVKcTNGdmZUajdXRCtaWkNqZzc3SG5Jb2d4YnFQWk5sSlR5VmxhdVVJWHM9");

            InitializeComponent();

            await NavigationService.NavigateAsync($"{nameof(AppMasterDetailPage)}/NavigationPage/{nameof(AppTabbedPage)}");
            
            //await NavigationService.NavigateAsync($"/NavigationPage/{nameof(LoginPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFileHelper, FileHelper>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<AppMasterDetailPage, AppMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<EditUserPage, EditUserPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<SearchPage, SearchPageViewModel>();
            containerRegistry.RegisterForNavigation<AppTabbedPage, AppTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
        }
    }
}
