using Group5F25.APP.PageModels;
using Microsoft.Maui.Storage;

namespace Group5F25.APP.Pages
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomePageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is HomePageViewModel vm)
            {
                // Prefer full display name, then username, then "User"
                var name = Preferences.Get("displayName",
                           Preferences.Get("username", "User"));

                vm.UserName = name;
            }
        }
    }
}
