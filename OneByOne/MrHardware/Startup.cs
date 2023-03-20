using Densen.DataAcces.FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MrHardware;

public static class Startup
{
    public static IServiceProvider? Services { get; private set; }

    public static void Init()
    {
        var host = Host.CreateDefaultBuilder()
                       .ConfigureServices(WireupServices)
                       .Build();
        Services = host.Services;
    }

    private static void WireupServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddWpfBlazorWebView();
        //添加FreeSql服务
        services.AddFreeSql(option =>
        {
            option.UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=demo.db;")
#if DEBUG
                 //开发环境:自动同步实体
                 .UseAutoSyncStructure(true)
                 .UseNoneCommandParameter(true)
                 //调试sql语句输出
                 .UseMonitorCommand(cmd => System.Console.WriteLine(cmd.CommandText + Environment.NewLine))
#endif
            ;
        });

        //全功能版
        services.AddSingleton(typeof(FreeSqlDataService<>));

        services.AddDensenExtensions();

#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
    }
}
