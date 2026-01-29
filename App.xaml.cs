using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using Zenith_Launcher.Data;
using Zenith_Launcher.Services.DependencyInjection;
using Zenith_Launcher.Services.Navigation;
using Zenith_Launcher.ViewModels;

namespace Zenith_Launcher
{
    public partial class App : Application
    {
        private Window? _window;
        private IServiceProvider? _serviceProvider;

        public App()
        {
            InitializeComponent();
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<DatabaseContext>();
            services.AddSingleton<DatabaseInitializer>();
            services.AddSingleton<Data.Repositories.IGameRepository, Data.Repositories.GameRepository>();
            services.AddSingleton<Services.GameLibrary.IGameLibraryService, Services.GameLibrary.GameLibraryService>();
            services.AddSingleton<Services.GameLauncher.IGameLauncherService, Services.GameLauncher.GameLauncherService>();
            services.AddSingleton<Services.PlayTime.IPlayTimeTracker, Services.PlayTime.PlayTimeTracker>();
            services.AddSingleton<Data.SampleDataSeeder>();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddTransient<ShellViewModel>();
            services.AddTransient<LibraryViewModel>();
            services.AddTransient<StoresViewModel>();
            services.AddTransient<SettingsViewModel>();

            _serviceProvider = services.BuildServiceProvider();
            ServiceLocator.Initialize(_serviceProvider);
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (_serviceProvider != null)
            {
                var dbInitializer = _serviceProvider.GetRequiredService<DatabaseInitializer>();
                dbInitializer.Initialize();

                var seeder = _serviceProvider.GetRequiredService<Data.SampleDataSeeder>();
                await seeder.SeedAsync();
            }

            _window = new MainWindow();
            _window.Activate();
        }
    }
}
