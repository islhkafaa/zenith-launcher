using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Services.StoreScanner
{
    public class SteamScanner : IStoreScanner
    {
        public string StoreName => "Steam";

        public Task<bool> IsInstalledAsync()
        {
            return Task.FromResult(GetSteamPath() != null);
        }

        public async Task<List<Game>> ScanLibraryAsync()
        {
            var games = new List<Game>();
            var steamPath = GetSteamPath();

            if (string.IsNullOrEmpty(steamPath))
                return games;

            var libraryFolders = GetLibraryFolders(steamPath);

            foreach (var libraryPath in libraryFolders)
            {
                var steamAppsPath = Path.Combine(libraryPath, "steamapps");
                if (!Directory.Exists(steamAppsPath))
                    continue;

                var manifestFiles = Directory.GetFiles(steamAppsPath, "appmanifest_*.acf");

                foreach (var manifestFile in manifestFiles)
                {
                    try
                    {
                        var game = await ParseManifestAsync(manifestFile, libraryPath);
                        if (game != null)
                            games.Add(game);
                    }
                    catch
                    {
                    }
                }
            }

            return games;
        }

        private string? GetSteamPath()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
                return key?.GetValue("SteamPath") as string;
            }
            catch
            {
                return null;
            }
        }

        private List<string> GetLibraryFolders(string steamPath)
        {
            var folders = new List<string> { steamPath };

            try
            {
                var libraryFoldersFile = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                if (!File.Exists(libraryFoldersFile))
                    return folders;

                var content = File.ReadAllText(libraryFoldersFile);
                var pathMatches = Regex.Matches(content, @"""path""\s+""([^""]+)""");

                foreach (Match match in pathMatches)
                {
                    if (match.Groups.Count > 1)
                    {
                        var path = match.Groups[1].Value.Replace(@"\\", @"\");
                        if (Directory.Exists(path) && !folders.Contains(path))
                            folders.Add(path);
                    }
                }
            }
            catch
            {
            }

            return folders;
        }

        private Task<Game?> ParseManifestAsync(string manifestPath, string libraryPath)
        {
            try
            {
                var content = File.ReadAllText(manifestPath);
                var data = ParseVdf(content);

                if (!data.ContainsKey("name") || !data.ContainsKey("installdir"))
                    return Task.FromResult<Game?>(null);

                var appType = data.ContainsKey("type") ? data["type"].ToLower() : "";
                if (appType == "tool" || appType == "config" || appType == "dlc")
                    return Task.FromResult<Game?>(null);

                var name = data["name"];
                if (name.Contains("Dedicated Server", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("SDK", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Redist", StringComparison.OrdinalIgnoreCase) ||
                    name.Contains("Common Redistributables", StringComparison.OrdinalIgnoreCase))
                    return Task.FromResult<Game?>(null);

                var installDir = Path.Combine(libraryPath, "steamapps", "common", data["installdir"]);
                var appId = data.ContainsKey("appid") ? data["appid"] : "";

                var game = new Game
                {
                    Title = data["name"],
                    Platform = "Steam",
                    InstallPath = installDir,
                    StoreId = appId,
                    Developer = data.ContainsKey("developer") ? data["developer"] : "",
                    Publisher = data.ContainsKey("publisher") ? data["publisher"] : ""
                };

                return Task.FromResult<Game?>(game);
            }
            catch
            {
                return Task.FromResult<Game?>(null);
            }
        }

        private Dictionary<string, string> ParseVdf(string content)
        {
            var data = new Dictionary<string, string>();
            var matches = Regex.Matches(content, @"""([^""]+)""\s+""([^""]+)""");

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 2)
                {
                    var key = match.Groups[1].Value.ToLower();
                    var value = match.Groups[2].Value;
                    data[key] = value;
                }
            }

            return data;
        }
    }
}
