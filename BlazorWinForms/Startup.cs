using BlazorShared.Services;
using LibraryShared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace BlazorWinForms
{
    public class ConfigFake{}
    public static class Startup
    {
        public static IServiceProvider? Services { get; private set; }
        public static IConfiguration? Config;

        public static void Init()
        {
            //Config = new ConfigurationBuilder().AddUserSecrets<ConfigFake>().Build();
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
            services.AddWindowsFormsBlazorWebView();
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
