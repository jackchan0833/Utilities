using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.ServiceExtensions
{
    /// <summary>
    /// Reprensents the service collection.
    /// </summary>
    public class ServiceCollection : IServiceCollection
    {
        private static ConcurrentDictionary<Type, ServiceInfo> _DictServices = new ConcurrentDictionary<Type, ServiceInfo>();
        private static ConcurrentDictionary<Type, object> _SingletonInstances = new ConcurrentDictionary<Type, object>();
        private static object _ServicerRemoveLock = new object();
        /// <summary>
        /// Adds the specified service as scoped service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        public void AddScoped<TService>()
            where TService : class
        {
            AddScoped<TService, TService>();
        }
        /// <summary>
        /// Adds the specified service as scoped service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public void AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class
        {
            AddService<TService, TImplementation>(ServiceLifeTime.Scope);
        }
        /// <summary>
        /// Adds the specified service as singleton service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        public void AddSingleton<TService>()
            where TService : class
        {
            AddSingleton<TService, TService>();
        }
        /// <summary>
        /// Adds the specified service as singleton service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public void AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class
        {
            AddService<TService, TImplementation>(ServiceLifeTime.Singleton);
        }
        /// <summary>
        /// Adds the specified service with specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceLifeTime"></param>
        public void AddService<TService>(ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class
        {
            AddService<TService, TService>();
        }
        /// <summary>
        /// Adds the specified service with specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="serviceLifeTime"></param>
        public void AddService<TService, TImplementation>(ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class
            where TImplementation : class
        {
            var serviceType = typeof(TService);
            var implementationService = typeof(TImplementation);
            ServiceInfo serviceInfo = new ServiceInfo()
            {
                ServiceType = serviceType,
                ImplementType = implementationService,
                ServiceLifeTime = serviceLifeTime
            };
            _DictServices.AddOrUpdate(serviceType, serviceInfo, (a, b) => { return serviceInfo; });
        }
        /// <summary>
        ///  Adds the specified service with specified function as scoped service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        public void AddScoped<TService>(Func<IServiceCollection, object> func)
            where TService : class
        {
            AddService<TService>(func, ServiceLifeTime.Scope);
        }
        /// <summary>
        /// Adds the specified service with specified function as singleton service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        public void AddSingleton<TService>(Func<IServiceCollection, object> func)
            where TService : class
        {
            AddService<TService>(func, ServiceLifeTime.Singleton);
        }
        /// <summary>
        /// Adds the specified service with specified function and <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="func"></param>
        /// <param name="serviceLifeTime"></param>
        public void AddService<TService>(Func<IServiceCollection, object> func, ServiceLifeTime serviceLifeTime = ServiceLifeTime.Scope)
            where TService : class
        {
            var serviceType = typeof(TService);
            ServiceInfo serviceInfo = new ServiceInfo()
            {
                ServiceType = serviceType,
                Func = func,
                ServiceLifeTime = serviceLifeTime
            };
            _DictServices.AddOrUpdate(serviceType, serviceInfo, (a, b) => { return serviceInfo; });
        }
        /// <summary>
        /// Gets the specified service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService GetService<TService>()
            where TService : class
        {
            var serviceType = typeof(TService);
            lock (_ServicerRemoveLock)
            {
                _DictServices.TryGetValue(serviceType, out ServiceInfo serviceInfo);
                if (serviceInfo == null)
                {
                    return default(TService);
                }
                else if (serviceInfo.ServiceLifeTime == ServiceLifeTime.Singleton)
                {
                    var obj = _SingletonInstances.GetOrAdd(serviceInfo.ServiceType, (a) =>
                    {
                        if (serviceInfo.Func != null)
                        {
                            return serviceInfo.Func(this);
                        }
                        else
                        {
                            object newInstance = Activator.CreateInstance(serviceInfo.ImplementType);
                            return newInstance;
                        }
                    });
                    return (TService)obj;
                }
                else if (serviceInfo.ServiceLifeTime == ServiceLifeTime.Scope)
                {
                    if (serviceInfo.Func != null)
                    {
                        var instance = serviceInfo.Func(this);
                        return (TService)instance;
                    }
                    else
                    {
                        //other, scope
                        var instance = Activator.CreateInstance(serviceInfo.ImplementType);
                        return (TService)instance;
                    }
                }
                else
                {
                    return default(TService);
                }
            }
        }
        /// <summary>
        /// Gets the specified service with specified <see cref="ServiceLifeTime"/>.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceLifeTime"></param>
        /// <returns></returns>
        public TService GetService<TService>(ServiceLifeTime serviceLifeTime)
            where TService : class
        {
            var serviceType = typeof(TService);
            lock (_ServicerRemoveLock)
            {
                _DictServices.TryGetValue(serviceType, out ServiceInfo serviceInfo);
                if (serviceInfo == null)
                {
                    return default(TService);
                }
                else if (serviceLifeTime == ServiceLifeTime.Singleton)
                {
                    var obj = _SingletonInstances.GetOrAdd(serviceInfo.ServiceType, (a) =>
                    {
                        if (serviceInfo.Func != null)
                        {
                            return serviceInfo.Func(this);
                        }
                        else
                        {
                            object newInstance = Activator.CreateInstance(serviceInfo.ImplementType);
                            return newInstance;
                        }
                    });
                    return (TService)obj;
                }
                else if (serviceLifeTime == ServiceLifeTime.Scope)
                {
                    if (serviceInfo.Func != null)
                    {
                        var instance = serviceInfo.Func(this);
                        return (TService)instance;
                    }
                    else
                    {
                        //other, scope
                        var instance = Activator.CreateInstance(serviceInfo.ImplementType);
                        return (TService)instance;
                    }
                }
                else
                {
                    return default(TService);
                }
            }
        }
        /// <summary>
        /// Try to remove the specified service.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public bool TryRemoveService<TService>(out TService instance)
            where TService : class
        {
            var serviceType = typeof(TService);
            lock (_ServicerRemoveLock)
            {
                if (_DictServices.TryRemove(serviceType, out ServiceInfo serviceInfo))
                {
                    if (serviceInfo.ServiceLifeTime == ServiceLifeTime.Singleton)
                    {
                        if (_SingletonInstances.TryRemove(serviceType, out object objVal))
                        {
                            instance = (TService)objVal;
                            return true;
                        }
                    }
                    instance = default(TService);
                    return true;
                }
                instance = default(TService);
                return false;
            }
        }
    }

}
