using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zenith_Launcher.Models;
using Zenith_Launcher.Services.GameLibrary;
using Zenith_Launcher.Services.GameLauncher;
using Zenith_Launcher.ViewModels.Base;

namespace Zenith_Launcher.ViewModels
{
    public partial class LibraryViewModel : ViewModelBase
    {
        private readonly IGameLibraryService _gameLibraryService;
        private readonly IGameLauncherService _gameLauncherService;

        [ObservableProperty]
        private ObservableCollection<Game> _games = new();

        [ObservableProperty]
        private bool _isLoading;

        public LibraryViewModel(IGameLibraryService gameLibraryService, IGameLauncherService gameLauncherService)
        {
            _gameLibraryService = gameLibraryService;
            _gameLauncherService = gameLauncherService;
        }

        public async Task LoadGamesAsync()
        {
            IsLoading = true;

            var games = await _gameLibraryService.GetAllGamesAsync();
            Games.Clear();

            foreach (var game in games)
            {
                Games.Add(game);
            }

            IsLoading = false;
        }

        [RelayCommand]
        private async Task LaunchGame(Game game)
        {
            if (game == null) return;

            try
            {
                await _gameLauncherService.LaunchGameAsync(game.Id);
            }
            catch (System.Exception)
            {
                // TODO: Show error dialog to user
            }
        }

        [RelayCommand]
        private async Task DeleteGame(Game game)
        {
            if (game == null) return;

            await _gameLibraryService.DeleteGameAsync(game.Id);
            Games.Remove(game);
        }
    }
}
