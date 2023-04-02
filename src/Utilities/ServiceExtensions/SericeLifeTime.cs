using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.ServiceExtensions
{
    /// <summary>
    /// Represents the service life time.
    /// </summary>
    public enum ServiceLifeTime
    {
        Scope,
        Singleton
    }
}
