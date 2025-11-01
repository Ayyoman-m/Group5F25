using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;

namespace Group5F25.APP
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
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
