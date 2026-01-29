using CommunityToolkit.Mvvm.ComponentModel;
using Zenith_Launcher.ViewModels.Base;

namespace Zenith_Launcher.ViewModels
{
    public partial class ShellViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _title = "Zenith Launcher";
    }
}
