using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.Services.Navigation;
using Zenith_Launcher.ViewModels;

namespace Zenith_Launcher
{
    public partial class App : Application
    {
        private Window? _window;

        public App()
        {
            InitializeComponent();
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddTransient<ShellViewModel>();
            services.AddTransient<LibraryViewModel>();
            services.AddTransient<StoresViewModel>();
            services.AddTransient<SettingsViewModel>();

            var serviceProvider = services.BuildServiceProvider();
            ServiceLocator.Initialize(serviceProvider);
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }
    }
}
