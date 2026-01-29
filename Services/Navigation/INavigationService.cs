using System;

namespace Zenith_Launcher.Services.Navigation
{
    public interface INavigationService
    {
        void Navigate(Type pageType);
        void Navigate(Type pageType, object parameter);
        bool CanGoBack { get; }
        void GoBack();
    }
}
