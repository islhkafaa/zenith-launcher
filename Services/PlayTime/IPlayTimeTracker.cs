using System.Diagnostics;
using System.Threading.Tasks;

namespace Zenith_Launcher.Services.PlayTime
{
    public interface IPlayTimeTracker
    {
        void StartTracking(int gameId, Process process);
        Task StopTrackingAsync(int gameId);
    }
}
