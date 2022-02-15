using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Prims.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppTabbedPage : TabbedPage, INavigatedAware
    {
        public AppTabbedPage()
        {
            InitializeComponent();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                if (Children.Count == 1)
                {
                    return;
                }
                for (var pageIndex = 1; pageIndex < Children.Count; pageIndex++)
                {
                    var page = Children[pageIndex];
                    (page?.BindingContext as INavigationAware)?.OnNavigatedTo(parameters);
                }
            }
        }

    }
}
