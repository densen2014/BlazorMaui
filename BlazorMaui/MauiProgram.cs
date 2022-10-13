using BlazorShared.Services;
using LibraryShared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.Maui;
using System.Globalization;

namespace BlazorMaui
{
    public static class MauiProgram
    {
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
                }); ;

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSharedExtensions();
            builder.Services.AddOcrExtensions(builder.Configuration["AzureCvKey"], builder.Configuration["AzureCvUrl"]);
            builder.Services.AddAIFormExtensions(builder.Configuration["AzureAiFormKey"], builder.Configuration["AzureAiFormUrl"]);
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            builder.Services.AddSingleton<ITools, TestService>();

            return builder.Build();
        }


    }
}
