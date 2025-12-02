using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
