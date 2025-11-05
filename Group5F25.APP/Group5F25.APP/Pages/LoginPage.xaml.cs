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
    }
}
