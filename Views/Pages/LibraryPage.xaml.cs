using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Navigation;
using System;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.ViewModels;

namespace Zenith_Launcher.Views.Pages
{
    public sealed partial class LibraryPage : Page
    {
        public LibraryViewModel ViewModel { get; }
        public BoolToVisibilityInverseConverter BoolToVisibilityInverseConverter { get; } = new();

        public LibraryPage()
        {
            InitializeComponent();
            ViewModel = ServiceLocator.GetService<LibraryViewModel>();
            DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadGamesAsync();
        }

        public Visibility ShowEmptyState(int count, bool isLoading)
        {
            return !isLoading && count == 0 ? Visibility.Visible : Visibility.Collapsed;
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
