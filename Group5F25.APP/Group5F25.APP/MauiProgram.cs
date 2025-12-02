using CommunityToolkit.Maui;
using Group5F25.APP;
using Group5F25.APP.PageModels;
using Group5F25.APP.Pages;
using Group5F25.APP.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using System;
using System.Net.Http;


namespace Group5F25.APP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
#if IOS || MACCATALYST
    				handlers.AddHandler<Microsoft.Maui.Controls.CollectionView, Microsoft.Maui.Controls.Handlers.Items2.CollectionViewHandler2>();
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily);
                });

#if DEBUG
    		builder.Logging.AddDebug();
    		builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            builder.Services.AddSingleton<ProjectRepository>();
            builder.Services.AddSingleton<TaskRepository>();
            builder.Services.AddSingleton<CategoryRepository>();
            builder.Services.AddSingleton<TagRepository>();
            builder.Services.AddSingleton<SeedDataService>();
            builder.Services.AddSingleton<ModalErrorHandler>();
            builder.Services.AddSingleton<MainPageModel>();
            builder.Services.AddSingleton<ProjectListPageModel>();
            builder.Services.AddSingleton<ManageMetaPageModel>();

            builder.Services.AddSingleton<ProfilePageViewModel>();
            builder.Services.AddSingleton<ProfilePage>();


            builder.Services.AddSingleton<TripAnalyticsViewModel>();
            builder.Services.AddSingleton<TripAnalyticsPage>();

            builder.Services.AddSingleton<LeaderboardViewModel>();
            builder.Services.AddSingleton<LeaderboardPage>();


            builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
            builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");
            

            // Register Login Page and ViewModel
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddSingleton<AppShell>();

            // Register the ViewModel so HomePage can inject it
            builder.Services.AddTransient<HomePageViewModel>();
            builder.Services.AddTransient<HomePage>();

            builder.Services.AddTransient<RegisterViewModel>();

            builder.Services.AddTransient<RegisterPage>();

       
            builder.Services.AddTransient<TripHistoryViewModel>();
            builder.Services.AddTransient<TripHistoryPage>();


            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri(ApiConfig.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(15);
            });


            builder.Services.AddHttpClient<ITripService, TripService>(client =>
            {
                client.BaseAddress = new Uri(ApiConfig.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(15);
            });

            return builder.Build();
        }
    }
}
