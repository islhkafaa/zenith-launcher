using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<Game> _filteredGames = new();

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private string _selectedPlatform = "All";

        [ObservableProperty]
        private string _sortBy = "Title";

        [ObservableProperty]
        private bool _sortDescending = false;

        public List<string> PlatformOptions { get; } = new() { "All", "Steam", "Epic", "GOG", "Other" };
        public List<string> SortOptions { get; } = new() { "Title", "PlayTime", "LastPlayed" };

        public LibraryViewModel(IGameLibraryService gameLibraryService, IGameLauncherService gameLauncherService)
        {
            _gameLibraryService = gameLibraryService;
            _gameLauncherService = gameLauncherService;
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFiltersAndSort();
        }

        partial void OnSelectedPlatformChanged(string value)
        {
            ApplyFiltersAndSort();
        }

        partial void OnSortByChanged(string value)
        {
            ApplyFiltersAndSort();
        }

        partial void OnSortDescendingChanged(bool value)
        {
            ApplyFiltersAndSort();
        }

        public async Task LoadGamesAsync()
        {
            try
            {
                IsLoading = true;
                var games = await _gameLibraryService.GetAllGamesAsync();

                Games.Clear();
                foreach (var game in games)
                {
                    Games.Add(game);
                }

                ApplyFiltersAndSort();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFiltersAndSort()
        {
            var filtered = Games.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(g => g.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedPlatform != "All")
            {
                filtered = filtered.Where(g => g.Platform == SelectedPlatform);
            }

            filtered = SortBy switch
            {
                "PlayTime" => SortDescending
                    ? filtered.OrderByDescending(g => g.PlayTime)
                    : filtered.OrderBy(g => g.PlayTime),
                "LastPlayed" => SortDescending
                    ? filtered.OrderByDescending(g => g.LastPlayed ?? DateTime.MinValue)
                    : filtered.OrderBy(g => g.LastPlayed ?? DateTime.MinValue),
                _ => SortDescending
                    ? filtered.OrderByDescending(g => g.Title)
                    : filtered.OrderBy(g => g.Title)
            };

            FilteredGames.Clear();
            foreach (var game in filtered)
            {
                FilteredGames.Add(game);
            }
        }

        [RelayCommand]
        private async Task LaunchGame(Game game)
        {
            if (game == null) return;

            try
            {
                await _gameLauncherService.LaunchGameAsync(game.Id);
                await LoadGamesAsync();
            }
            catch (Exception)
            {
            }
        }

        [RelayCommand]
        private async Task DeleteGame(Game game)
        {
            if (game == null) return;

            await _gameLibraryService.DeleteGameAsync(game.Id);
            Games.Remove(game);
            ApplyFiltersAndSort();
        }

        public Game? GameToAdd { get; set; }
        public Game? GameToEdit { get; set; }

        [RelayCommand]
        private void AddGame()
        {
            GameToAdd = new Game();
        }

        [RelayCommand]
        private void EditGame(Game game)
        {
            if (game == null) return;
            GameToEdit = game;
        }

        public async Task SaveAddedGameAsync(Game game)
        {
            await _gameLibraryService.AddGameAsync(game);
            await LoadGamesAsync();
        }

        public async Task SaveEditedGameAsync(Game game)
        {
            await _gameLibraryService.UpdateGameAsync(game);
            await LoadGamesAsync();
        }
    }
}
