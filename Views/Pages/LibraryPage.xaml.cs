using Microsoft.UI.Xaml.Controls;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.ViewModels;

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
        }
    }
}
