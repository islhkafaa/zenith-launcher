using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Zenith_Launcher.Models;
using Zenith_Launcher.Services.StoreIntegration;
using Zenith_Launcher.ViewModels.Base;

namespace Zenith_Launcher.ViewModels
{
    public partial class StoresViewModel : ViewModelBase
    {
        private readonly IStoreIntegrationService _storeIntegrationService;

        [ObservableProperty]
        private bool _isScanning;

        [ObservableProperty]
        private string _scanStatus = string.Empty;

        [ObservableProperty]
        private ObservableCollection<StoreInfo> _stores = new();

        public StoresViewModel(IStoreIntegrationService storeIntegrationService)
        {
            _storeIntegrationService = storeIntegrationService;
            InitializeStores();
        }

        private void InitializeStores()
        {
            Stores.Add(new StoreInfo { Name = "Steam", IsInstalled = false, GamesFound = 0 });
            Stores.Add(new StoreInfo { Name = "Epic", IsInstalled = false, GamesFound = 0 });
            Stores.Add(new StoreInfo { Name = "GOG", IsInstalled = false, GamesFound = 0 });
        }

        public async Task CheckStoreInstallationsAsync()
        {
            foreach (var store in Stores)
            {
                store.IsInstalled = await _storeIntegrationService.IsStoreInstalledAsync(store.Name);
            }
        }

        [RelayCommand]
        private async Task ScanStore(string storeName)
        {
            if (IsScanning)
                return;

            try
            {
                IsScanning = true;
                ScanStatus = $"Scanning {storeName} library...";

                var games = await _storeIntegrationService.ScanStoreAsync(storeName);
                var store = Stores.FirstOrDefault(s => s.Name == storeName);

                if (store != null)
                {
                    store.GamesFound = games.Count;
                    store.ScannedGames = games;
                }

                ScanStatus = $"Found {games.Count} games in {storeName}";
            }
            catch
            {
                ScanStatus = $"Failed to scan {storeName}";
            }
            finally
            {
                IsScanning = false;
            }
        }

        [RelayCommand]
        private async Task ImportGames(string storeName)
        {
            var store = Stores.FirstOrDefault(s => s.Name == storeName);
            if (store?.ScannedGames == null || store.ScannedGames.Count == 0)
                return;

            try
            {
                IsScanning = true;
                ScanStatus = $"Importing {store.ScannedGames.Count} games from {storeName}...";

                var importedCount = await _storeIntegrationService.ImportGamesAsync(store.ScannedGames);

                ScanStatus = $"Imported {importedCount} new games from {storeName}";
                store.ScannedGames.Clear();
                store.GamesFound = 0;
            }
            catch
            {
                ScanStatus = $"Failed to import games from {storeName}";
            }
            finally
            {
                IsScanning = false;
            }
        }

        [RelayCommand]
        private async Task ScanAllStores()
        {
            if (IsScanning)
                return;

            try
            {
                IsScanning = true;
                ScanStatus = "Scanning all stores...";

                var results = await _storeIntegrationService.ScanAllStoresAsync();

                foreach (var result in results)
                {
                    var store = Stores.FirstOrDefault(s => s.Name == result.Key);
                    if (store != null)
                    {
                        store.GamesFound = result.Value.Count;
                        store.ScannedGames = result.Value;
                    }
                }

                var totalGames = results.Values.Sum(g => g.Count);
                ScanStatus = $"Found {totalGames} total games across all stores";
            }
            catch
            {
                ScanStatus = "Failed to scan stores";
            }
            finally
            {
                IsScanning = false;
            }
        }
    }

    public partial class StoreInfo : ObservableObject
    {
        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private bool _isInstalled;

        [ObservableProperty]
        private int _gamesFound;

        public List<Game> ScannedGames { get; set; } = new();
    }
}
