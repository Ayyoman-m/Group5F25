using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomePageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is HomePageViewModel vm)
            {
                vm.RefreshUser();
            }

            // Simple bounce animation for the score card
            if (ScoreCard != null)
            {
                ScoreCard.Scale = 0.9;
                await ScoreCard.ScaleTo(1.03, 150, Easing.CubicOut);
                await ScoreCard.ScaleTo(1.0, 120, Easing.CubicIn);
            }
        }
    }
}
