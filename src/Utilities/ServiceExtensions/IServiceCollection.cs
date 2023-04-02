using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.ServiceExtensions
{
    /// <summary>
    /// Represents the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public interface IServiceCollection
    {
        /// <summary>
        /// Adds the service as scoped service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        void AddScoped<TService>() where TService : class;
        /// <summary>
        /// Adds the TService with implemention service as scoped service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        void AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class;

        /// <summary>
        /// Adds the service as singleton service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        void AddSingleton<TService>() where TService : class;
        /// <summary>
        /// Addes the service with implemented service as singleton service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        void AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class;
        /// <summary>
        /// Adds the service with specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceLifeTime"></param>
        void AddService<TService>(ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope) where TService : class;
        /// <summary>
        /// Adds the service with specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="serviceLifeTime"></param>
        void AddService<TService, TImplementation>(ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class
            where TImplementation : class;
        /// <summary>
        /// Adds the service with specified function service as scoped service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        void AddScoped<TService>(Func<IServiceCollection, object> func)
            where TService : class;
        /// <summary>
        /// Adds the service with specified function as singleton service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        void AddSingleton<TService>(Func<IServiceCollection, object> func)
            where TService : class;
        /// <summary>
        /// Adds the service with specified function and specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        /// <param name="serviceLifeTime"></param>
        void AddService<TService>(Func<IServiceCollection, object> func, ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class;
        /// <summary>
        /// Gets the specified service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService GetService<TService>() where TService : class;
        /// <summary>
        /// Gets the specified service with specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceLifeTime"></param>
        /// <returns></returns>
        TService GetService<TService>(ServiceLifeTime serviceLifeTime) where TService : class;
        /// <summary>
        /// Try to remove the specified service from added service list.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool TryRemoveService<TService>(out TService instance) where TService : class;
    }
}
