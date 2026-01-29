using Microsoft.UI.Xaml;
using Zenith_Launcher.Views;

namespace Zenith_Launcher
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Title = "Zenith Launcher";

            AppWindow.Resize(new Windows.Graphics.SizeInt32(1400, 900));

            RootFrame.Navigate(typeof(ShellPage));
        }
    }
}
