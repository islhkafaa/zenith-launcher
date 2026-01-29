using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Specialized;
using Zenith_Launcher.Models;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.ViewModels;
using Zenith_Launcher.Views.Dialogs;

namespace Zenith_Launcher.Views.Pages
{
    public sealed partial class LibraryPage : Page
    {
        public LibraryViewModel ViewModel { get; }

        public LibraryPage()
        {
            InitializeComponent();
            ViewModel = ServiceLocator.GetService<LibraryViewModel>();
            DataContext = ViewModel;

            ViewModel.FilteredGames.CollectionChanged += FilteredGames_CollectionChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LibraryViewModel.IsLoading))
            {
                UpdateEmptyState();
            }
        }

        private void FilteredGames_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateEmptyState();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadGamesAsync();
            UpdateEmptyState();
        }

        private void UpdateEmptyState()
        {
            if (GamesGridView == null || EmptyStatePanel == null) return;

            bool hasGames = ViewModel.FilteredGames.Count > 0;
            bool isLoading = ViewModel.IsLoading;

            GamesGridView.Visibility = (!isLoading && hasGames) ? Visibility.Visible : Visibility.Collapsed;
            EmptyStatePanel.Visibility = (!isLoading && !hasGames) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void OnAddGameClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditGameDialog();
            dialog.XamlRoot = this.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await ViewModel.SaveAddedGameAsync(dialog.Game);
            }
        }

        private async void OnGameLaunchRequested(object sender, Game game)
        {
            await ViewModel.LaunchGameCommand.ExecuteAsync(game);
        }

        private async void OnGameDeleteRequested(object sender, Game game)
        {
            await ViewModel.DeleteGameCommand.ExecuteAsync(game);
        }

        private async void OnGameEditRequested(object sender, Game game)
        {
            var dialog = new AddEditGameDialog(game);
            dialog.XamlRoot = this.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await ViewModel.SaveEditedGameAsync(dialog.Game);
            }
        }
    }

    public class BoolToVisibilityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is bool b && b ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
