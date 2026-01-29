using System;
using System.Threading.Tasks;
using Zenith_Launcher.Data.Repositories;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Data
{
    public class SampleDataSeeder
    {
        private readonly IGameRepository _gameRepository;

        public SampleDataSeeder(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task SeedAsync()
        {
            var existingGames = await _gameRepository.GetAllAsync();
            if (existingGames.Count > 0)
            {
                return;
            }

            var sampleGames = new[]
            {
                new Game
                {
                    Title = "Cyberpunk 2077",
                    Platform = "Steam",
                    InstallPath = "C:\\Games\\Cyberpunk2077",
                    PlayTime = TimeSpan.FromHours(45)
                },
                new Game
                {
                    Title = "The Witcher 3",
                    Platform = "GOG",
                    InstallPath = "C:\\Games\\TheWitcher3",
                    PlayTime = TimeSpan.FromHours(120),
                    LastPlayed = DateTime.Now.AddDays(-5)
                },
                new Game
                {
                    Title = "Fortnite",
                    Platform = "Epic",
                    InstallPath = "C:\\Games\\Fortnite",
                    PlayTime = TimeSpan.FromHours(30),
                    LastPlayed = DateTime.Now.AddDays(-1)
                },
                new Game
                {
                    Title = "Red Dead Redemption 2",
                    Platform = "Steam",
                    InstallPath = "C:\\Games\\RDR2",
                    PlayTime = TimeSpan.FromHours(80)
                },
                new Game
                {
                    Title = "Hades",
                    Platform = "Epic",
                    InstallPath = "C:\\Games\\Hades",
                    PlayTime = TimeSpan.FromHours(25),
                    LastPlayed = DateTime.Now.AddDays(-3)
                },
                new Game
                {
                    Title = "Baldur's Gate 3",
                    Platform = "Steam",
                    InstallPath = "C:\\Games\\BG3",
                    PlayTime = TimeSpan.FromHours(95),
                    LastPlayed = DateTime.Now
                }
            };

            foreach (var game in sampleGames)
            {
                await _gameRepository.AddAsync(game);
            }
        }
    }
}
