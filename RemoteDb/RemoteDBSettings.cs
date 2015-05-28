using System;
using GDSQLite;
using Foundation;
using GoodDynamics;

namespace RemoteDb
{
	public class RemoteDBSettings
	{
		private IntPtr _database;

		const string SQL_CREATE_TABLE = "CREATE TABLE IF NOT EXISTS remoteSettings (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, serverHost  VARCHAR, serverPort  INTEGER, serverPriority  INTEGER, config      VARCHAR, copyPasteOn INTEGER, detailedLogOn INTEGER, userEmail  VARCHAR)";
		
		const string SQL_COUNT  = "SELECT COUNT(*) FROM remoteSettings";
		const string SQL_INSERT = "INSERT INTO remoteSettings (serverHost, serverPort, serverPriority, config, copyPasteOn, detailedLogOn, userEmail) VALUES (?, ?, ?, ?, ?, ?, ?)";
		const string SQL_UPDATE = "UPDATE remoteSettings SET serverHost = ?, serverPort = ?, serverPriority = ?, config = ?, copyPasteOn = ?, detailedLogOn = ?, userEmail = ?";
		const string SQL_GET_SETTINGS   = "SELECT serverHost, serverPort, serverPriority, config, copyPasteOn, detailedLogOn, userEmail FROM remoteSettings";
		const string DB_NAME = "remote_settings_db";

		public RemoteDBSettings ()
		{
			InitializeDB ();
		}

		~RemoteDBSettings()
		{
			SQLite3.Close (_database);
		}

		private SQLite3.Result GetResultFromIntPtr(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero) 
			{
				return SQLite3.Result.Error;
			}

			var ptrInt = System.Runtime.InteropServices.Marshal.ReadInt32 (ptr);

			return (SQLite3.Result)ptrInt;
		}

		private bool InitializeDB()
		{
			int result = -1;
			IntPtr db_namePtr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi (DB_NAME);
			result = GoodDynamics.Sqlite3Enc.sqlite3enc_open (db_namePtr, out _database); 

			if (result != 0) 
			{
				return false;
			}

			string query = SQL_CREATE_TABLE;
			IntPtr queryPtr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi (query);
			IntPtr statement = IntPtr.Zero;
			IntPtr errorPtr = IntPtr.Zero;

			var sqliteResult = SQLite3.Prepare2 (_database, queryPtr, query.Length, out statement, out errorPtr);
			if (sqliteResult != SQLite3.Result.OK) 
			{
				return false;
			}

			SQLite3.Result sqliteenum = (SQLite3.Result)SQLite3.Step (statement);
			if ((SQLite3.Result)sqliteResult != SQLite3.Result.OK) 
			{
				SQLite3.Finalize (statement);
				return false;
			}

			SQLite3.Finalize (statement);
			return true;
		}

		public bool SettingsAlreadyStored()
		{
			IntPtr statement = IntPtr.Zero;
			int count = 0;

			statement = SQLite3.Prepare2 (_database, SQL_COUNT);
			if (statement == IntPtr.Zero) 
			{
				return false;
			}

			var result = SQLite3.Step (statement);
			if (result == SQLite3.Result.Row) 
			{
				count = SQLite3.ColumnInt (statement, 0);
			}

			SQLite3.Finalize (statement);
			statement = IntPtr.Zero;

			return count > 0;
		}
			
		public bool UpdateRemoteSettings(string serverHost, int port, int priority, string config, int copyPasteOn, int detailedLogOn, string userEmail)
		{
			IntPtr statement = IntPtr.Zero;
			string sqlStatement = null;

			if (SettingsAlreadyStored ()) 
			{
				sqlStatement = SQL_UPDATE;
			} 
			else 
			{
				sqlStatement = SQL_INSERT;
			}

			statement = SQLite3.Prepare2 (_database, sqlStatement);
			if (statement == IntPtr.Zero) 
			{
				return false;
			}

			SQLite3.BindText (statement, 1, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(serverHost), -1, IntPtr.Zero);
			SQLite3.BindInt (statement, 2, port);
			SQLite3.BindInt (statement, 3, priority);
			SQLite3.BindText (statement, 4, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(config), -1, IntPtr.Zero);
			SQLite3.BindInt (statement, 5, copyPasteOn);
			SQLite3.BindInt (statement, 6, detailedLogOn);
			SQLite3.BindText (statement, 7, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(userEmail), -1, IntPtr.Zero);

			var result = SQLite3.Step (statement);
			if (result != SQLite3.Result.Done) 
			{
				SQLite3.Finalize (statement);
				return false;
			}

			result = SQLite3.Finalize (statement);
			if (result != SQLite3.Result.OK) 
			{
				return false;
			}

			return true;
		}

		public NSMutableDictionary GetConfigSettings()
		{
			NSMutableDictionary returnDictionary = new NSMutableDictionary ();

			IntPtr statement = IntPtr.Zero;
			statement = SQLite3.Prepare2 (_database, SQL_GET_SETTINGS);
			int detailedLogOn = -100;

			if (statement == IntPtr.Zero) 
			{
				return null;
			}

			var result = SQLite3.Step (statement);
			if (result == SQLite3.Result.Row) 
			{
				IntPtr serverHostPtr = SQLite3.ColumnText (statement, 0);
				string serverHost = System.Runtime.InteropServices.Marshal.PtrToStringAnsi (serverHostPtr);
				int port = SQLite3.ColumnInt (statement, 1);
				int priority = SQLite3.ColumnInt (statement, 2);
				IntPtr configPtr = SQLite3.ColumnText (statement, 3);
				string config = System.Runtime.InteropServices.Marshal.PtrToStringAnsi (configPtr);
				int copyPasteOn = SQLite3.ColumnInt (statement, 4);
				detailedLogOn = SQLite3.ColumnInt (statement, 5);
				IntPtr userEmailPtr = SQLite3.ColumnText (statement, 6);
				string userEmail = System.Runtime.InteropServices.Marshal.PtrToStringAnsi (userEmailPtr);

				if (!String.IsNullOrWhiteSpace (serverHost)) 
				{
					GDAppServer firstServer = new GDAppServer (serverHost, new NSNumber(port), new NSNumber(priority));
					returnDictionary.Add (GDiOS.GDAppConfigKeyServers, firstServer);
				}
				if (!String.IsNullOrWhiteSpace (config)) 
				{
					returnDictionary.Add (GDiOS.GDAppConfigKeyConfig, new NSString(config));
				}
				returnDictionary.Add (GDiOS.GDAppConfigKeyCopyPasteOn, new NSNumber(copyPasteOn));
				if (detailedLogOn != -100) 
				{
					returnDictionary.Add (GDiOS.GDAppConfigKeyDetailedLogsOn, new NSNumber(detailedLogOn));
				}
				if (!string.IsNullOrWhiteSpace(userEmail)) 
				{
					returnDictionary.Add (GDiOS.GDAppConfigKeyUserId, new NSString(userEmail));
				}
			}

			SQLite3.Finalize (statement);
			return returnDictionary;
		}
	}
}

