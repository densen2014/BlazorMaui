using BlazorShared.Services;
using LibraryShared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorWpf
{
    public static class Startup
    {
        public class ConfigFake { }
        public static IServiceProvider? Services { get; private set; }
        public static IConfiguration? Config;

        public static void Init()
        {
            var host = Host.CreateDefaultBuilder()
                             .ConfigureAppConfiguration((hostingContext, config) =>
                             {
                                 config.AddUserSecrets<ConfigFake>().Build();
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
            services.AddSharedExtensions();
            services.AddFileSystemExtensions();
            services.AddOcrExtensions(Config["AzureCvKey"], Config["AzureCvUrl"]);
            services.AddAIFormExtensions(Config["AzureAiFormKey"], Config["AzureAiFormUrl"]);
#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif
            services.AddSingleton<ITools, TestService>();
        }
    }
}
