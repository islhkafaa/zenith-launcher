using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.Services.Navigation;
using Zenith_Launcher.ViewModels;
using Zenith_Launcher.Views.Pages;

namespace Zenith_Launcher.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; }
        private readonly INavigationService _navigationService;

        public ShellPage()
        {
            InitializeComponent();
            ViewModel = ServiceLocator.GetService<ShellViewModel>();
            DataContext = ViewModel;

            _navigationService = ServiceLocator.GetService<INavigationService>();
            if (_navigationService is NavigationService navService)
            {
                navService.Initialize(ContentFrame);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];
            _navigationService.Navigate(typeof(LibraryPage));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item)
            {
                string tag = item.Tag?.ToString();
                Type pageType = tag switch
                {
                    "Library" => typeof(LibraryPage),
                    "Stores" => typeof(StoresPage),
                    "Settings" => typeof(SettingsPage),
                    _ => null
                };

                if (pageType != null)
                {
                    _navigationService.Navigate(pageType);
                }
            }
        }
    }
}
