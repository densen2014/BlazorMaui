// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using MauiApp7test.Data;
using Microsoft.Extensions.Logging;

namespace MauiApp7test
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
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSharedExtensions();

            return builder.Build();
        }
    }
}
