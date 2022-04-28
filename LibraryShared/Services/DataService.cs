#if WINDOWS
using Windows.Storage;
#endif

namespace LibraryShared
{
    public class DataService
    {
        static IFreeSql fsql;

        public DataService()
        {
        }


        public IFreeSql Initfsql()
        {

#if WINDOWS
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
#elif ANDROID || IOS || MACCATALYST 
            string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sqliteSample.db");
#else 
            string dbpath = "sqliteSample.db";
#endif

            //Microsoft.Data.Sqlite.SqliteConnection _database = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={dbpath}");

            fsql ??= new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, $"Data Source={dbpath};")
            //.UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
            .UseAutoSyncStructure(true)
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
            .Build();
             
            return fsql;
        }
        public List<TaskItem> Init(IFreeSql fsqlt = null, bool initdemodatas = false)
        {

#if WINDOWS
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
#elif ANDROID || IOS || MACCATALYST 
            string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "sqliteSample.db");
#else 
            string dbpath = "sqliteSample.db";
#endif

            //Microsoft.Data.Sqlite.SqliteConnection _database = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={dbpath}");

            fsql ??= fsqlt ??= new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, $"Data Source={dbpath};")
            //.UseConnectionFactory(FreeSql.DataType.Sqlite, () => _database, typeof(FreeSql.Sqlite.SqliteProvider<>))
            .UseAutoSyncStructure(true)
            .UseMonitorCommand(cmd => Console.Write(cmd.CommandText))
            .Build();

            if (initdemodatas)
            {
                if (fsql.Select<TaskItem>().Count() < 4)
                {
                    var itemList = TaskItem.GenerateDatas();
                    fsql.Insert<TaskItem>().AppendData(itemList.Take(4)).ExecuteAffrows();
                } 
            }
            var ItemList = fsql.Select<TaskItem>().ToList();

            //var ItemList = TaskItem.GenerateDatas();
            Console.WriteLine("\r\n\r\nItemListCount: " + ItemList.Count());
            Console.WriteLine("\r\n\r\nLastItem: " + ItemList.Last()?.Text);

            return ItemList;
        }

        public TaskItem Add(string name)
        {
            var item = new TaskItem { Text = name, Description = name, Idu = Guid.NewGuid() };
            item.Id = (int)fsql.Insert(item).ExecuteIdentity();
            return item;
        }
        public bool Delete()
        {
            return fsql.Select<TaskItem>().Take(1).OrderByDescending(a=>a.Id).ToDelete().ExecuteAffrows() == 1;
        }
        public TaskItem Modify()
        {
            var item = fsql.Select<TaskItem>().First();
            item.Text = $"Modify {DateTime.Now.Second}";
            item.Description = $"Description {DateTime.Now.Second}";
            return fsql.Update<TaskItem>().SetSource(item).ExecuteAffrows() == 1 ? item : null;
        }
    }


}
