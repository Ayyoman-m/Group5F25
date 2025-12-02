using Group5F25.APP.PageModels;

namespace Group5F25.APP.Pages;

public partial class TripHistoryPage : ContentPage
{
    private readonly TripHistoryViewModel _vm;

    public TripHistoryPage(TripHistoryViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadTripsAsync();
    }
}
