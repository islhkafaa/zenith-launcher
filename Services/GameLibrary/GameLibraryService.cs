using System.Collections.Generic;
using System.Threading.Tasks;
using Zenith_Launcher.Data.Repositories;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Services.GameLibrary
{
    public class GameLibraryService : IGameLibraryService
    {
        private readonly IGameRepository _gameRepository;

        public GameLibraryService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<Game>> GetAllGamesAsync()
        {
            return await _gameRepository.GetAllAsync();
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _gameRepository.GetByIdAsync(id);
        }

        public async Task<int> AddGameAsync(Game game)
        {
            return await _gameRepository.AddAsync(game);
        }

        public async Task UpdateGameAsync(Game game)
        {
            await _gameRepository.UpdateAsync(game);
        }

        public async Task DeleteGameAsync(int id)
        {
            await _gameRepository.DeleteAsync(id);
        }
    }
}
