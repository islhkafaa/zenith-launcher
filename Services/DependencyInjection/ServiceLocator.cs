using Microsoft.Extensions.DependencyInjection;
using System;

namespace Zenith_Launcher.Services.DependencyInjection
{
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceLocator has not been initialized.");
            }

            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
