using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Group5F25.APP.Pages;
using Font = Microsoft.Maui.Font;

namespace Group5F25.APP
{
    public partial class AppShell : Shell
    {
        // Inject both pages so VM binding works
        public AppShell(LoginPage loginPage, HomePage homePage)
        {
            InitializeComponent();

            Items.Clear();

            Items.Add(new ShellContent
            {
                Title = "Login",
                Route = "login",
                Content = loginPage
            });

            Items.Add(new ShellContent
            {
                Title = "Home",
                Route = "home",
                Content = homePage
            });

            // NEW: route for RegisterPage (navigation only, NOT a tab)
            Routing.RegisterRoute("register", typeof(RegisterPage));

            // NEW: route for RegistrationSuccessPage
            Routing.RegisterRoute("registrationSuccess", typeof(RegistrationSuccessPage));

            Routing.RegisterRoute("tripHistory", typeof(TripHistoryPage));

            Routing.RegisterRoute("profile", typeof(ProfilePage));

            Routing.RegisterRoute("analytics", typeof(TripAnalyticsPage));

            Routing.RegisterRoute("leaderboard", typeof(LeaderboardPage));



        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            // Skip Snackbar on Windows to avoid Toolkit setup requirements
            if (OperatingSystem.IsWindows())
                return;

            var cts = new CancellationTokenSource();
            var snackbar = Snackbar.Make(
                message,
                visualOptions: new SnackbarOptions
                {
                    BackgroundColor = Color.FromArgb("#FF3300"),
                    TextColor = Colors.White,
                    ActionButtonTextColor = Colors.Yellow,
                    CornerRadius = new CornerRadius(0),
                    Font = Font.SystemFontOfSize(18),
                    ActionButtonFont = Font.SystemFontOfSize(14)
                });
            await snackbar.Show(cts.Token);
        }


        public static async Task DisplayToastAsync(string message)
        {
            if (OperatingSystem.IsWindows()) return;
            var toast = Toast.Make(message, textSize: 18);
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }
    }
}
