// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BlazorMauiLite.Data;
using Microsoft.Extensions.Logging;
using BlazorShared;
using BlazorShared.Services;
using LibraryShared;

namespace BlazorMauiLite
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            AppState _appState = new();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton(_appState);
            builder.Services.AddSharedExtensions();
            builder.Services.AddOcrExtensions();
            builder.Services.AddAIFormExtensions();
            builder.Services.AddSingleton<ITools, MauiService>();

            builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
 
            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}
