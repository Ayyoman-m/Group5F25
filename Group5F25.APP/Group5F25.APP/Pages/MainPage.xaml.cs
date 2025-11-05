using Group5F25.APP.Models;
using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}