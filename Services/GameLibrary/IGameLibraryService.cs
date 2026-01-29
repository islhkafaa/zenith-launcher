using System.Collections.Generic;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Services.GameLibrary
{
    public interface IGameLibraryService
    {
        Task<List<Game>> GetAllGamesAsync();
        Task<Game?> GetGameByIdAsync(int id);
        Task<int> AddGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task DeleteGameAsync(int id);
    }
}
