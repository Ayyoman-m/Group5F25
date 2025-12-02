using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class LeaderboardPage : ContentPage
    {
        public LeaderboardPage(LeaderboardViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is LeaderboardViewModel vm)
            {
                await vm.LoadAsync();
            }
        }
    }
}
