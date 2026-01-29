using Microsoft.UI.Xaml.Controls;
using System;

namespace Zenith_Launcher.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;

        public void Initialize(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public bool CanGoBack => _frame?.CanGoBack ?? false;

        public void Navigate(Type pageType)
        {
            if (_frame == null)
            {
                throw new InvalidOperationException("NavigationService has not been initialized with a Frame.");
            }

            _frame.Navigate(pageType);
        }

        public void Navigate(Type pageType, object parameter)
        {
            if (_frame == null)
            {
                throw new InvalidOperationException("NavigationService has not been initialized with a Frame.");
            }

            _frame.Navigate(pageType, parameter);
        }

        public void GoBack()
        {
            if (_frame?.CanGoBack == true)
            {
                _frame.GoBack();
            }
        }
    }
}
