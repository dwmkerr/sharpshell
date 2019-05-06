using System;
using System.Collections.Generic;
using SharpShell.Registry;

namespace SharpShell.ServiceRegistry
{
    /// <summary>
    /// Simple dependency injection container.
    /// </summary>
    public static class ServiceRegistry
    {
        private static readonly Dictionary<Type, Func<object>> ServiceProviders = new Dictionary<Type, Func<object>>();

        static ServiceRegistry()
        {
            Reset();
        }

        /// <summary>
        /// Resets all providers. Typically used only for testing.
        /// </summary>
        internal static void Reset()
        {
            ServiceProviders.Clear();

            //  The default registry provider is a new instance of the WindowsRegitry.
            ServiceProviders.Add(typeof(IRegistry), () => new WindowsRegistry());
        }

        /// <summary>
        /// Gets a service of type T.
        /// </summary>
        /// <typeparam name="T">The type of service to get.</typeparam>
        /// <returns>An instance of the service of type T.</returns>
        /// <exception cref="InvalidOperationException">Thrown if T is not registered.</exception>
        public static T GetService<T>() where T:class
        {
            //  Find the provider.
            if (!ServiceProviders.TryGetValue(typeof(T), out var provider)) throw new InvalidOperationException($"No provider has been registered for service type '{typeof(T).FullName}'");

            //  Use the provider to return the service instance.
            return provider() as T;
        }

        /// <summary>
        /// Registers a service.
        /// </summary>
        /// <typeparam name="T">The service type. Generally this must be the interface type, not the concrete type.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        public static void RegisterService<T>(Func<T> serviceProvider) where T:class
        {
            ServiceProviders[typeof(T)] = serviceProvider;
        }
    }
}
