using BlazorShared;
using BlazorShared.Services;
using LibraryShared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorWpf;

public static class Startup
{
    public class ConfigFake { }
    public static IServiceProvider? Services { get; private set; }
    public static IConfiguration? Config;

    public static readonly AppState _appState = new();
    public static void Init()
    {
        var host = Host.CreateDefaultBuilder()
                         .ConfigureAppConfiguration((hostingContext, config) =>
                         {
#if DEBUG
                             config.AddUserSecrets<ConfigFake>().Build();
#endif 
                             //config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                             Config = config.Build();
                         })
                       .ConfigureServices(WireupServices)
                       .Build();
        Services = host.Services;
    }

    private static void WireupServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddWpfBlazorWebView();
        services.AddSingleton(_appState);
        services.AddSharedExtensions();
        services.AddOcrExtensions();
        services.AddAIFormExtensions();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddSingleton<ITools, WpfService>();
    }
}
