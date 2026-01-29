using Microsoft.UI.Xaml.Controls;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.ViewModels;

namespace Zenith_Launcher.Views.Pages
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage()
        {
            InitializeComponent();
            ViewModel = ServiceLocator.GetService<SettingsViewModel>();
            DataContext = ViewModel;
        }
    }
}
