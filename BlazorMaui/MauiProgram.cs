using BlazorShared;
using BlazorShared.Services;
using LibraryShared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Globalization;
using System.Reflection;

namespace BlazorMaui
{
    public static class MauiProgram
    {
        public class ConfigFake { }
        public static MauiApp CreateMauiApp()
        {

            AppState _appState = new();
            var builder = MauiApp.CreateBuilder();

            #region Configuration
            //使用内置资源
            //需要在项目属性中设置生成操作为嵌入资源
            //<ItemGroup>
            //<EmbeddedResource Include="appsettings.json" />
            //</ItemGroup>            
            //try
            //{
            //    var a = Assembly.GetExecutingAssembly();
            //    using var stream = a.GetManifestResourceStream("BlazorMaui.appsettings.json");
            //    if (stream!=null) builder.Configuration.AddJsonStream(stream);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //使用 MauiAsset : appsettings.json
            //文件属性生成操作为 MauiAsset 和 不复制
            //<ItemGroup>
            //  <MauiAsset Include="appsettings.json">
            //    <CopyToOutputDirectory>Never</CopyToOutputDirectory>
            //  </MauiAsset>
            //</ItemGroup> 
            //需要在项目属性中 Remove 文件
            //<ItemGroup>
            //  <Content Remove="appsettings.json" />
            //</ItemGroup>
            try
            {
                var stream = LoadMauiAsset().Result; 
                if (stream!=null) builder.Configuration.AddJsonStream(stream);
            }
            catch (Exception e)
            {
                //TODO : ios error 'Stream was not readable'
                Console.WriteLine("builder.Configuration.AddJsonStream error=> " + e.Message);
            }
#if WINDOWS
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            //windows/ssr 平台可用 UserSecrets , 其他平台没用
            builder.Configuration.AddUserSecrets<ConfigFake>();
#endif

#endregion

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
            builder.Services.AddSingleton(_appState);
            builder.Services.AddSharedExtensions();
#if (IOS || MACCATALYST)
            string localFilePath = Path.Combine(FileSystem.CacheDirectory);
            Console.WriteLine("AzureCvKey:" + builder.Configuration["AzureCvKey"]);
            builder.Services.AddOcrExtensions(builder.Configuration["AzureCvKey"], builder.Configuration["AzureCvUrl"], localFilePath);
#else
            builder.Services.AddOcrExtensions();
#endif

            builder.Services.AddAIFormExtensions();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            builder.Services.AddSingleton<ITools, MauiService>();

            return builder.Build();
        }

        async static Task<Stream?> LoadMauiAsset()
        {
            try
            {

                using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
                using var reader = new StreamReader(stream);

                var contents = reader.ReadToEnd();
                Console.WriteLine("OpenAppPackageFileAsync => " + contents);
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                return memoryStream;
            }
            catch (Exception e)
            {
                Console.WriteLine("OpenAppPackageFileAsync Exception => " + e.Message);
            }
            return null;
        }
    }
}
