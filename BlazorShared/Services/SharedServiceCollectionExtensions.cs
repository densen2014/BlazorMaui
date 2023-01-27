// ********************************** 
// Densen Informatica 中讯科技 
// 作者：Alex Chow
// e-mail:zhouchuanglin@gmail.com 
// **********************************

using BlazorShared.Services;
using BlazorShared.Models;
using Shared.DependencyServices;
//using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using BlazorShared.Data;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 服务扩展类
/// </summary>
public static class SharedServiceCollectionExtensions
{

    //public static IFreeSql fsql;

    /// <summary>
    /// 服务扩展类,<para></para>
    /// 包含各平台差异实现
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSharedExtensions(this IServiceCollection services)
    {
        string UploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "uploads");
        if (!Directory.Exists(UploadPath)) Directory.CreateDirectory(UploadPath);

        var cultureInfo = new CultureInfo("zh-CN");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        services.AddDensenExtensions();
        services.ConfigureJsonLocalizationOptions(op =>
        {
            // 忽略文化信息丢失日志
            op.IgnoreLocalizerMissing = true;

            // 附加自己的 json 多语言文化资源文件 如 zh-TW.json
            op.AdditionalJsonAssemblies = new Assembly[]
            {
                //typeof(BootstrapBlazor.Shared.App).Assembly,
                typeof(BootstrapBlazor.Components.Chart).Assembly,
                //typeof(BootstrapBlazor.Components.SignaturePad).Assembly
            };
        });

        services.AddSingleton<IIPAddressManager, IPAddressManager>();
        services.AddSingleton<ITools, FakeService>();

        //据说已经修复
        //2022/8/11 测试fsql是不是这个问题
        services.AddSingleton<IErrorBoundaryLogger, MyErrorBoundaryLogger>();

        //fsql = new TestSqlite().test();
        //if (fsql != null) services.AddSingleton(fsql);

        //            builder.Services.AddFreeSql(option =>
        //            {
        //                //demo演示的是Sqlite驱动,FreeSql支持多种数据库，MySql/SqlServer/PostgreSQL/Oracle/Sqlite/Firebird/达梦/神通/人大金仓/翰高/华为GaussDB/MsAccess
        //                option.UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=test.db;")  //也可以写到配置文件中
        //#if DEBUG
        //                     //开发环境:自动同步实体
        //                     .UseAutoSyncStructure(true)
        //                     .UseNoneCommandParameter(true)
        //                     //调试sql语句输出
        //                     .UseMonitorCommand(cmd => System.Console.WriteLine(cmd.CommandText))
        //#endif
        //                ;
        //            });

        // 增加 Table 数据服务操作类
        //services.AddTableDemoDataService();
        return services;
    }

}




//public class TestSqlite
//{

//    public IFreeSql test()
//    {
//        IFreeSql fsql;
//        List<Item> items;


//        items = new List<Item>()
//            {
//                new Item { Id = Guid.NewGuid().ToString(), Text = "假装 First item", Description="This is an item description." },
//                new Item { Id = Guid.NewGuid().ToString(), Text = "的哥 Second item", Description="This is an item description." },
//                new Item { Id = Guid.NewGuid().ToString(), Text = "四风 Third item", Description="This is an item description." },
//                new Item { Id = Guid.NewGuid().ToString(), Text = "加州 Fourth item", Description="This is an item description." },
//                new Item { Id = Guid.NewGuid().ToString(), Text = "阳光 Fifth item", Description="This is an item description." },
//                new Item { Id = Guid.NewGuid().ToString(), Text = "孔雀 Sixth item", Description="This is an item description." }
//            };



//        try
//        {
//            #region mssql测试没问题

//            //        fsql = new FreeSql.FreeSqlBuilder()
//            //.UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=192.168.1.100;Initial Catalog=demo;Persist Security Info=True;MultipleActiveResultSets=true;User ID=sa;Password=a123456;Connect Timeout=30;min pool size=1;connection lifetime=15")
//            //.UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
//            //.UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
//            //.Build();

//            #endregion

//            #region Sqlite 多平台实现



//            //WPF,winforms,UWP C:\Users\Alex\Documents\demomaui.db
//            //WASM '/home/web_user/demomaui.db'
//            var dbPathuwp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "demomaui.db");
//            //var connv3 = new Mono.Data.Sqlite.SqliteConnection($"Data Source={MicrosoftdbPath};");

//            //WPF,winforms,ssr,UWP C:\Users\Alex\AppData\Local\demomaui.db
//            //UWP最后执行 C:\Users\Alex\AppData\Local\Packages\31A0A253-CFA3-4CF9-AB61-727488C167C2_9zz4h110yvjzm\LocalCache\Local\demomaui.db
//            //WASM 'demomaui.db'
//            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "demomaui.db");
//            Debug.WriteLine(dbPath);

//#if (ANDROID || IOS || MACCATALYST)

//            fsql = SQLiteProv2(dbPath);
//#else
//#if (WINDOWS)
//            //uwp要指定可读写的路径
//            fsql = new FreeSql.FreeSqlBuilder()
//                        .UseConnectionString(FreeSql.DataType.Sqlite, $"Data Source={dbPath}; Pooling=true;Min Pool Size=1")
//                        .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
//                        .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
//                        .Build();
//#else
//            fsql = new FreeSql.FreeSqlBuilder()
//                        .UseConnectionString(FreeSql.DataType.Sqlite, "Data Source=demomaui.db; Pooling=true;Min Pool Size=1")
//                        .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
//                        .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
//                        .Build();

//#endif
//#endif

//            #endregion

//            #region mysql使用 reeSql.Provider.MySqlConnector , debug和release都设置为不链接即可

//            //fsql = new FreeSql.FreeSqlBuilder()
//            //        .UseConnectionString(FreeSql.DataType.MySql, "Data Source=192.168.1.100;Port=3306;User ID=root;Password=a123456; Initial Catalog=test;Charset=utf8; SslMode=none;Min pool size=1")
//            //        .UseAutoSyncStructure(true) //自动同步实体结构【开发环境必备】
//            //        .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
//            //        .Build();
//            #endregion

//            fsql.CodeFirst.SyncStructure<Item>();
//            if (fsql.Select<Item>().Count() < 10) fsql.Insert<Item>().AppendData(items).ExecuteAffrows();
//            var res = fsql.Select<Item>().ToList(a => a.Text);
//            res.ForEach(a =>
//            {
//                Debug.WriteLine(" <== 测试测试测试 ==> " + a);
//            });
//            return fsql;
//        }
//        catch (Exception ex)
//        {
//            Debug.WriteLine(ex);
//        }
//        return null;
//    }

//    private static IFreeSql SQLiteProv2(string dbPath)
//    {
//        IFreeSql fsql;
//        SQLiteConnection _database;

//        _database = new SQLiteConnection($"Data Source={dbPath}");

//        fsql = new FreeSql.FreeSqlBuilder()
//           .UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
//           .UseAutoSyncStructure(true)
//           //.UseMonitorCommand(cmd => Console.WriteLine(cmd.CommandText))
//           .UseNoneCommandParameter(true)
//           .Build();
//        return fsql;
//    }
//}
