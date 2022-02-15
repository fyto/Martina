using Prism.Navigation;

namespace Mobile.Prims.ViewModels
{
    public class AppTabbedPageViewModel : ViewModelBase
    {
        public AppTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //if (parameters.ContainsKey("product"))
            //{
            //    ProductResponse product = parameters.GetValue<ProductResponse>("product");
            //    Title = product.Name;
            //}
        }
    }
}
