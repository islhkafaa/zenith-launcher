using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DatabaseContext _context;

        public GameRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetAllAsync()
        {
            var games = new List<Game>();
            using var connection = _context.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Games";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                games.Add(MapToGame(reader));
            }

            return games;
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            using var connection = _context.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Games WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToGame(reader);
            }

            return null;
        }

        public async Task<int> AddAsync(Game game)
        {
            using var connection = _context.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Games (Title, Platform, InstallPath, CoverImagePath, LastPlayed, PlayTime, StoreId, LaunchParameters, Description, Developer, Publisher, ReleaseDate)
                VALUES (@title, @platform, @installPath, @coverImagePath, @lastPlayed, @playTime, @storeId, @launchParameters, @description, @developer, @publisher, @releaseDate);
                SELECT last_insert_rowid();
            ";

            AddParameters(command, game);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task UpdateAsync(Game game)
        {
            using var connection = _context.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Games
                SET Title = @title,
                    Platform = @platform,
                    InstallPath = @installPath,
                    CoverImagePath = @coverImagePath,
                    LastPlayed = @lastPlayed,
                    PlayTime = @playTime,
                    StoreId = @storeId,
                    LaunchParameters = @launchParameters,
                    Description = @description,
                    Developer = @developer,
                    Publisher = @publisher,
                    ReleaseDate = @releaseDate
                WHERE Id = @id
            ";

            command.Parameters.AddWithValue("@id", game.Id);
            AddParameters(command, game);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _context.GetConnection();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Games WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();
        }

        private void AddParameters(SqliteCommand command, Game game)
        {
            command.Parameters.AddWithValue("@title", game.Title);
            command.Parameters.AddWithValue("@platform", game.Platform);
            command.Parameters.AddWithValue("@installPath", game.InstallPath);
            command.Parameters.AddWithValue("@coverImagePath", game.CoverImagePath ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@lastPlayed", game.LastPlayed?.ToString("o") ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@playTime", (long)game.PlayTime.TotalSeconds);
            command.Parameters.AddWithValue("@storeId", game.StoreId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@launchParameters", game.LaunchParameters ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@description", game.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@developer", game.Developer ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@publisher", game.Publisher ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@releaseDate", game.ReleaseDate?.ToString("o") ?? (object)DBNull.Value);
        }

        private Game MapToGame(SqliteDataReader reader)
        {
            return new Game
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Platform = reader.GetString(2),
                InstallPath = reader.GetString(3),
                CoverImagePath = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                LastPlayed = reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
                PlayTime = TimeSpan.FromSeconds(reader.GetInt64(6)),
                StoreId = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                LaunchParameters = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                Developer = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                Publisher = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                ReleaseDate = reader.IsDBNull(12) ? null : DateTime.Parse(reader.GetString(12))
            };
        }
    }
}
