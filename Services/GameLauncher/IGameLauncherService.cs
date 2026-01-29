using System.Threading.Tasks;

namespace Zenith_Launcher.Services.GameLauncher
{
    public interface IGameLauncherService
    {
        Task LaunchGameAsync(int gameId);
        bool IsGameRunning(int gameId);
    }
}
