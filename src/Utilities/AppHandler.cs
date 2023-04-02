using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the <see cref="AppHandler"/>.
    /// </summary>
    public class AppHandler
    {
        /// <summary>
        /// Gets current process's name.
        /// </summary>
        /// <returns>The current name of process.</returns>
        public static string GetCurrentProcessName()
        {
            return Process.GetCurrentProcess().ProcessName;
        }

        /// <summary>
        /// Check whether processes include specified process with name.
        /// </summary>
        /// <param name="processName">The specified name of process.</param>
        /// <returns>True if exists; False if not exists.</returns>
        public static bool CheckProcessIsExist(string processName)
        {
            var count = Process.GetProcessesByName(processName).Length;
            return count > 0;
        }
        /// <summary>
        /// Gets process id with specified process name.
        /// </summary>
        /// <param name="processName">The specified name of process.</param>
        /// <returns>The process id.</returns>
        public static List<int> GetPidByProcessName(string processName)
        {
            Process[] arrayProcess = Process.GetProcessesByName(processName);
            return arrayProcess.Select(th => th.Id).ToList();
        }
    }
}
