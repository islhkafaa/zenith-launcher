using Microsoft.Data.Sqlite;

namespace Zenith_Launcher.Data
{
    public class DatabaseInitializer
    {
        private readonly DatabaseContext _context;

        public DatabaseInitializer(DatabaseContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            using var connection = _context.GetConnection();

            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS Games (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Platform TEXT NOT NULL,
                    InstallPath TEXT NOT NULL,
                    CoverImagePath TEXT,
                    LastPlayed TEXT,
                    PlayTime INTEGER,
                    StoreId TEXT,
                    LaunchParameters TEXT,
                    Description TEXT,
                    Developer TEXT,
                    Publisher TEXT,
                    ReleaseDate TEXT
                )
            ";
            createTableCommand.ExecuteNonQuery();
        }
    }
}
