using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenith_Launcher.Models;
using Zenith_Launcher.Services.GameLibrary;
using Zenith_Launcher.Services.StoreScanner;

namespace Zenith_Launcher.Services.StoreIntegration
{
    public class StoreIntegrationService : IStoreIntegrationService
    {
        private readonly IGameLibraryService _gameLibraryService;
        private readonly Dictionary<string, IStoreScanner> _scanners;

        public StoreIntegrationService(
            IGameLibraryService gameLibraryService,
            SteamScanner steamScanner,
            EpicScanner epicScanner,
            GogScanner gogScanner)
        {
            _gameLibraryService = gameLibraryService;
            _scanners = new Dictionary<string, IStoreScanner>
            {
                { "Steam", steamScanner },
                { "Epic", epicScanner },
                { "GOG", gogScanner }
            };
        }

        public async Task<Dictionary<string, List<Game>>> ScanAllStoresAsync()
        {
            var results = new Dictionary<string, List<Game>>();

            foreach (var scanner in _scanners.Values)
            {
                try
                {
                    if (await scanner.IsInstalledAsync())
                    {
                        var games = await scanner.ScanLibraryAsync();
                        results[scanner.StoreName] = games;
                    }
                    else
                    {
                        results[scanner.StoreName] = new List<Game>();
                    }
                }
                catch
                {
                    results[scanner.StoreName] = new List<Game>();
                }
            }

            return results;
        }

        public async Task<List<Game>> ScanStoreAsync(string storeName)
        {
            if (!_scanners.ContainsKey(storeName))
                return new List<Game>();

            var scanner = _scanners[storeName];

            try
            {
                if (await scanner.IsInstalledAsync())
                {
                    return await scanner.ScanLibraryAsync();
                }
            }
            catch
            {
            }

            return new List<Game>();
        }

        public async Task<int> ImportGamesAsync(List<Game> games)
        {
            var existingGames = await _gameLibraryService.GetAllGamesAsync();
            var importedCount = 0;

            foreach (var game in games)
            {
                var isDuplicate = existingGames.Any(g =>
                    g.Title.Equals(game.Title, System.StringComparison.OrdinalIgnoreCase) &&
                    g.Platform.Equals(game.Platform, System.StringComparison.OrdinalIgnoreCase));

                if (!isDuplicate)
                {
                    await _gameLibraryService.AddGameAsync(game);
                    importedCount++;
                }
            }

            return importedCount;
        }

        public async Task<bool> IsStoreInstalledAsync(string storeName)
        {
            if (!_scanners.ContainsKey(storeName))
                return false;

            try
            {
                return await _scanners[storeName].IsInstalledAsync();
            }
            catch
            {
                return false;
            }
        }
    }
}
