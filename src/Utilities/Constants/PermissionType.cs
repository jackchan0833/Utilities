using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities
{
    /// <summary>
    /// Permission control by Power(2,n). Cannot change the position if defined.
    /// Example to generate permissions: 0001 | 0010 = 0011
    /// Check permission: 0011 & 0001 = 0001
    /// Check permission: 0011 & 0010 = 0010
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// No permission.
        /// </summary>
        None = 0,
        /// <summary>
        /// Trial version with 30 days.
        /// </summary>
        ThirtyDaysTrial = 1,
        /// <summary>
        /// Standard version
        /// </summary>
        SimpleVersion = 2,
        /// <summary>
        /// Enterprise version.
        /// </summary>
        EnterpriseVersion = 4,
    }
}
