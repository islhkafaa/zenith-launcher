using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Services.StoreScanner
{
    public class EpicScanner : IStoreScanner
    {
        public string StoreName => "Epic";

        public Task<bool> IsInstalledAsync()
        {
            var manifestPath = GetManifestPath();
            return Task.FromResult(Directory.Exists(manifestPath));
        }

        public async Task<List<Game>> ScanLibraryAsync()
        {
            var games = new List<Game>();
            var manifestPath = GetManifestPath();

            if (!Directory.Exists(manifestPath))
                return games;

            var manifestFiles = Directory.GetFiles(manifestPath, "*.item");

            foreach (var manifestFile in manifestFiles)
            {
                try
                {
                    var game = await ParseManifestAsync(manifestFile);
                    if (game != null)
                        games.Add(game);
                }
                catch
                {
                }
            }

            return games;
        }

        private string GetManifestPath()
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return Path.Combine(programData, "Epic", "EpicGamesLauncher", "Data", "Manifests");
        }

        private async Task<Game?> ParseManifestAsync(string manifestPath)
        {
            try
            {
                var json = await File.ReadAllTextAsync(manifestPath);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (!root.TryGetProperty("DisplayName", out var displayName) ||
                    !root.TryGetProperty("InstallLocation", out var installLocation))
                    return null;

                var catalogItemId = root.TryGetProperty("CatalogItemId", out var itemId) ? itemId.GetString() : "";
                var appName = root.TryGetProperty("AppName", out var name) ? name.GetString() : "";
                var displayNameStr = displayName.GetString() ?? "";

                if (displayNameStr.Contains("Launcher", StringComparison.OrdinalIgnoreCase) ||
                    displayNameStr.Contains("Epic Online Services", StringComparison.OrdinalIgnoreCase) ||
                    appName?.Contains("EpicGamesLauncher", StringComparison.OrdinalIgnoreCase) == true)
                    return null;

                var game = new Game
                {
                    Title = displayNameStr,
                    Platform = "Epic",
                    InstallPath = installLocation.GetString() ?? "",
                    StoreId = catalogItemId ?? appName ?? ""
                };

                return game;
            }
            catch
            {
                return null;
            }
        }
    }
}
