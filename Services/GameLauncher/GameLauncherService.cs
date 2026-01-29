using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Zenith_Launcher.Data.Repositories;
using Zenith_Launcher.Services.PlayTime;

namespace Zenith_Launcher.Services.GameLauncher
{
    public class GameLauncherService : IGameLauncherService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayTimeTracker _playTimeTracker;
        private readonly Dictionary<int, Process> _runningGames = new();

        public GameLauncherService(IGameRepository gameRepository, IPlayTimeTracker playTimeTracker)
        {
            _gameRepository = gameRepository;
            _playTimeTracker = playTimeTracker;
        }

        public async Task LaunchGameAsync(int gameId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new InvalidOperationException($"Game with ID {gameId} not found.");
            }

            if (_runningGames.ContainsKey(gameId))
            {
                return;
            }

            if (!File.Exists(game.InstallPath))
            {
                throw new FileNotFoundException($"Game executable not found at: {game.InstallPath}");
            }

            var processStartInfo = new ProcessStartInfo
            {
                FileName = game.InstallPath,
                UseShellExecute = true,
                WorkingDirectory = Path.GetDirectoryName(game.InstallPath)
            };

            var process = Process.Start(processStartInfo);
            if (process != null)
            {
                _runningGames[gameId] = process;

                game.LastPlayed = DateTime.Now;
                await _gameRepository.UpdateAsync(game);

                _playTimeTracker.StartTracking(gameId, process);

                process.EnableRaisingEvents = true;
                process.Exited += async (sender, args) =>
                {
                    await OnGameExited(gameId);
                };
            }
        }

        public bool IsGameRunning(int gameId)
        {
            return _runningGames.ContainsKey(gameId) &&
                   !_runningGames[gameId].HasExited;
        }

        private async Task OnGameExited(int gameId)
        {
            if (_runningGames.ContainsKey(gameId))
            {
                _runningGames[gameId].Dispose();
                _runningGames.Remove(gameId);
            }

            await _playTimeTracker.StopTrackingAsync(gameId);
        }
    }
}
