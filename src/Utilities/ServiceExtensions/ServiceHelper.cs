using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.ServiceExtensions
{
    /// <summary>
    /// Represents the <see cref="ServiceHelper"/>.
    /// </summary>
    public class ServiceHelper
    {
        private static IServiceCollection _ServiceCollection = new ServiceCollection();
        /// <summary>
        /// Adds the specified service with scoped life time.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        public static void AddScoped<TService>()
            where TService : class
        {
            _ServiceCollection.AddScoped<TService>();
        }
        /// <summary>
        /// Adds the specified service with scoped life time.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementation">The implementation service.</typeparam>
        public static void AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class
        {
            _ServiceCollection.AddScoped<TService, TImplementation>();
        }
        /// <summary>
        /// Adds the specified service with specified service life time.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="serviceLifeTime">The service life time.</param>
        public static void AddService<TService>(ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class
        {
            _ServiceCollection.AddService<TService>(serviceLifeTime);
        }
        /// <summary>
        /// Adds the specified service with specified service life time.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementation">The implementation service.</typeparam>
        /// <param name="serviceLifeTime">The specified life time.</param>
        public static void AddService<TService, TImplementation>(ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class
            where TImplementation : class
        {
            _ServiceCollection.AddService<TService, TImplementation>(serviceLifeTime);
        }
        /// <summary>
        /// Adds the specified service with specified funtion as scoped service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="func">The implementation function.</param>
        public static void AddScoped<TService>(Func<IServiceCollection, object> func)
            where TService : class
        {
            _ServiceCollection.AddService<TService>(func, ServiceLifeTime.Scope);
        }
        /// <summary>
        /// Adds the specified service as singleton service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        public static void AddSingleton<TService>()
            where TService : class
        {
            _ServiceCollection.AddSingleton<TService>();
        }
        /// <summary>
        /// Adds the specified service with specified funtion as singleton service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="func">The implementation function.</param>
        public static void AddSingleton<TService>(Func<IServiceCollection, object> func)
            where TService : class
        {
            _ServiceCollection.AddSingleton<TService>(func);
        }
        /// <summary>
        /// Adds the specified service as singleton service.
        /// </summary>
        /// <typeparam name="TService">The specified service.</typeparam>
        /// <typeparam name="TImplementation">The implementation service.</typeparam>
        public static void AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class
        {
            _ServiceCollection.AddSingleton<TService, TImplementation>();
        }
        /// <summary>
        /// Adds the specified service with specified funtion and <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="func">The specified funtion.</param>
        /// <param name="serviceLifeTime">The specified service life time.</param>
        public static void AddService<TService>(Func<IServiceCollection, object> func, ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class
        {
            _ServiceCollection.AddService<TService>(func, serviceLifeTime);
        }
        /// <summary>
        /// Gets the specified service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>The specified service instance.</returns>
        public static TService GetService<TService>()
            where TService : class
        {
            return _ServiceCollection.GetService<TService>();
        }
        /// <summary>
        /// Gets the specified service with specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="serviceLifeTime">The specified service life time.</param>
        /// <returns>The specified service instance.</returns>
        public static TService GetService<TService>(ServiceLifeTime serviceLifeTime) where TService : class
        {
            return _ServiceCollection.GetService<TService>(serviceLifeTime);
        }
    }

}
