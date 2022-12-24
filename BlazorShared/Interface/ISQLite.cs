using System;
using System.Data.Common; 
 
namespace DemoShared.Services
{
	public interface ISQLite
	{
		//SqliteConnection GetConnection();
		DbConnection GetConnectionSqlite(string dbname);
	}
}

