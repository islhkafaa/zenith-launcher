using Microsoft.UI.Xaml.Controls;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.ViewModels;

namespace Zenith_Launcher.Views.Pages
{
    public sealed partial class StoresPage : Page
    {
        public StoresViewModel ViewModel { get; }

        public StoresPage()
        {
            InitializeComponent();
            ViewModel = ServiceLocator.GetService<StoresViewModel>();
            DataContext = ViewModel;
        }
    }
}
