using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages
{
    public partial class TripAnalyticsPage : ContentPage
    {
        public TripAnalyticsPage(TripAnalyticsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is TripAnalyticsViewModel vm)
                await vm.LoadAsync();
        }
    }
}
