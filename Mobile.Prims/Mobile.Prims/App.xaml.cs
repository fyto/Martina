using Common.Services;
using Mobile.Prims.ViewModels;
using Mobile.Prims.Views;
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

            await NavigationService.NavigateAsync($"{nameof(AppMasterDetailPage)}/NavigationPage/{nameof(MainPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();


            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<AppMasterDetailPage, AppMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<EditUserPage, EditUserViewModel>();
        }
    }
}
