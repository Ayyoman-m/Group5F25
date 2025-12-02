using Group5F25.APP.PageModels;

using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class HomePage : ContentPage
    {
        // 1. Inject the ViewModel into the constructor
        public HomePage(HomePageViewModel vm)
        {
            InitializeComponent();

            // 2. Connect the Page to the ViewModel
            BindingContext = vm;
        }
    }
}
