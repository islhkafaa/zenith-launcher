using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Services.StoreScanner
{
    public class GogScanner : IStoreScanner
    {
        public string StoreName => "GOG";

        public Task<bool> IsInstalledAsync()
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(@"Software\GOG.com\Games");
                return Task.FromResult(key != null);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Task<List<Game>> ScanLibraryAsync()
        {
            var games = new List<Game>();

            try
            {
                using var gamesKey = Registry.LocalMachine.OpenSubKey(@"Software\GOG.com\Games");
                if (gamesKey == null)
                    return Task.FromResult(games);

                foreach (var gameId in gamesKey.GetSubKeyNames())
                {
                    try
                    {
                        using var gameKey = gamesKey.OpenSubKey(gameId);
                        if (gameKey == null)
                            continue;

                        var gameName = gameKey.GetValue("gameName") as string;
                        var path = gameKey.GetValue("path") as string;

                        if (string.IsNullOrEmpty(gameName) || string.IsNullOrEmpty(path))
                            continue;

                        var game = new Game
                        {
                            Title = gameName,
                            Platform = "GOG",
                            InstallPath = path,
                            StoreId = gameId
                        };

                        games.Add(game);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }

            return Task.FromResult(games);
        }
    }
}
