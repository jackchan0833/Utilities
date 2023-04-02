using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.ServiceExtensions
{
    /// <summary>
    /// Represents the service information.
    /// </summary>
    internal class ServiceInfo
    {
        /// <summary>
        /// The service type.
        /// </summary>
        public Type ServiceType { get; set; }
        /// <summary>
        /// The implementation service type.
        /// </summary>
        public Type ImplementType { get; set; }
        /// <summary>
        /// The implementation function.
        /// </summary>
        public Func<IServiceCollection, object> Func { get; set; }
        /// <summary>
        /// The service life time.
        /// </summary>
        public ServiceLifeTime ServiceLifeTime { get; set; }

    }
}
