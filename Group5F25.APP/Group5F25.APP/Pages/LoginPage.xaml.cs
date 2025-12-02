using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        // This is the handler for the "Create Account" button
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("register");
        }
    }
}
