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

namespace Microsoft.Extensions.DependencyInjection
{
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
            var cultureInfo = new CultureInfo("zh-CN");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            services.AddDensenExtensions();
            services.AddSingleton<IIPAddressManager, IPAddressManager>();
            //fsql = new TestSqlite().test();
            //if (fsql != null) services.AddSingleton(fsql);
            return services;
        }

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
