using System.Collections.Generic;
using System.Threading.Tasks;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Services.StoreScanner
{
    public interface IStoreScanner
    {
        string StoreName { get; }
        Task<bool> IsInstalledAsync();
        Task<List<Game>> ScanLibraryAsync();
    }
}
