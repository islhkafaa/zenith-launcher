using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using Zenith_Launcher.Models;

namespace Zenith_Launcher.Views.Dialogs
{
    public sealed partial class AddEditGameDialog : ContentDialog
    {
        public Game Game { get; set; }

        public AddEditGameDialog()
        {
            Game = new Game
            {
                Platform = "Steam"
            };
            InitializeComponent();
            PlatformComboBox.SelectedIndex = 0;
        }

        public AddEditGameDialog(Game existingGame)
        {
            Game = new Game
            {
                Id = existingGame.Id,
                Title = existingGame.Title,
                Platform = existingGame.Platform,
                InstallPath = existingGame.InstallPath,
                CoverImagePath = existingGame.CoverImagePath,
                LastPlayed = existingGame.LastPlayed,
                PlayTime = existingGame.PlayTime
            };
            InitializeComponent();
            Title = "Edit Game";

            TitleTextBox.Text = Game.Title;
            InstallPathTextBox.Text = Game.InstallPath;
            CoverImageTextBox.Text = Game.CoverImagePath ?? string.Empty;
            PlatformComboBox.SelectedItem = Game.Platform;
        }

        private async void OnBrowseExecutableClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".exe");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                InstallPathTextBox.Text = file.Path;
            }
        }

        private async void OnBrowseCoverImageClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                CoverImageTextBox.Text = file.Path;
            }
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Game.Title = TitleTextBox.Text;
            Game.Platform = PlatformComboBox.SelectedItem?.ToString() ?? "Steam";
            Game.InstallPath = InstallPathTextBox.Text;
            Game.CoverImagePath = string.IsNullOrWhiteSpace(CoverImageTextBox.Text) ? null : CoverImageTextBox.Text;

            if (string.IsNullOrWhiteSpace(Game.Title))
            {
                args.Cancel = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(Game.InstallPath))
            {
                args.Cancel = true;
                return;
            }
        }

        private void OnSecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
