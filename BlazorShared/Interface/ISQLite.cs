using System;
using System.Data.Common; 
 
namespace BlazorShared.Services
{
	public interface ISQLite
	{
		//SqliteConnection GetConnection();
		DbConnection GetConnectionSqlite(string dbname);
	}
}

