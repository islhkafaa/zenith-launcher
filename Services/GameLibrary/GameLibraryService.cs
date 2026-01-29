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

        public Task<List<Game>> GetAllGamesAsync()
        {
            return _gameRepository.GetAllAsync();
        }

        public Task<Game?> GetGameByIdAsync(int id)
        {
            return _gameRepository.GetByIdAsync(id);
        }

        public Task<int> AddGameAsync(Game game)
        {
            return _gameRepository.AddAsync(game);
        }

        public Task UpdateGameAsync(Game game)
        {
            return _gameRepository.UpdateAsync(game);
        }

        public Task DeleteGameAsync(int id)
        {
            return _gameRepository.DeleteAsync(id);
        }
    }
}
