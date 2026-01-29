using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Zenith_Launcher.Data.Repositories;

namespace Zenith_Launcher.Services.PlayTime
{
    public class PlayTimeTracker : IPlayTimeTracker
    {
        private readonly IGameRepository _gameRepository;
        private readonly Dictionary<int, DateTime> _startTimes = new();

        public PlayTimeTracker(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public void StartTracking(int gameId, Process process)
        {
            _startTimes[gameId] = DateTime.Now;
        }

        public async Task StopTrackingAsync(int gameId)
        {
            if (!_startTimes.ContainsKey(gameId))
            {
                return;
            }

            var startTime = _startTimes[gameId];
            var endTime = DateTime.Now;
            var sessionDuration = endTime - startTime;

            _startTimes.Remove(gameId);

            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game != null)
            {
                game.PlayTime += sessionDuration;
                await _gameRepository.UpdateAsync(game);
            }
        }
    }
}
