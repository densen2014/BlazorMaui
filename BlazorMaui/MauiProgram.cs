using LibraryShared;
using Microsoft.Extensions.Configuration;
namespace BlazorMaui
{
    public static class MauiProgram
    {
        public class ConfigFake { }
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .ConfigureEssentials(essentials =>
                {
                    essentials
                        .AddAppAction("app_info", "App Info", icon: "app_info_action_icon")
                        .AddAppAction("battery_info", "Battery Info")
                        .OnAppAction(App.HandleAppActions);
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
 
            return builder.Build();
        }

     }
}
