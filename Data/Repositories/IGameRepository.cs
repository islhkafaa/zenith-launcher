using System.Collections.Generic;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Data.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        Task<int> AddAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(int id);
    }
}
