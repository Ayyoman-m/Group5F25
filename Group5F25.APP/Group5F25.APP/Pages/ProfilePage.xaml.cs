using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(ProfilePageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProfilePageViewModel vm)
                vm.LoadFromPreferences();
        }
    }
}
