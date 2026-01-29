using System.Collections.Generic;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Services.StoreIntegration
{
    public interface IStoreIntegrationService
    {
        Task<Dictionary<string, List<Game>>> ScanAllStoresAsync();
        Task<List<Game>> ScanStoreAsync(string storeName);
        Task<int> ImportGamesAsync(List<Game> games);
        Task<bool> IsStoreInstalledAsync(string storeName);
    }
}
