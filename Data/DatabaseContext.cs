using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace Zenith_Launcher.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ZenithLauncher"
            );

            Directory.CreateDirectory(appDataPath);

            var dbPath = Path.Combine(appDataPath, "zenith.db");
            _connectionString = $"Data Source={dbPath}";
        }

        public SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
