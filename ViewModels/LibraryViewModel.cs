using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Zenith_Launcher.Models;
using Zenith_Launcher.Services.GameLibrary;
using Zenith_Launcher.ViewModels.Base;

namespace Zenith_Launcher.ViewModels
{
    public partial class LibraryViewModel : ViewModelBase
    {
        private readonly IGameLibraryService _gameLibraryService;

        [ObservableProperty]
        private ObservableCollection<Game> _games = new();

        [ObservableProperty]
        private bool _isLoading;

        public LibraryViewModel(IGameLibraryService gameLibraryService)
        {
            _gameLibraryService = gameLibraryService;
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
    }
}
