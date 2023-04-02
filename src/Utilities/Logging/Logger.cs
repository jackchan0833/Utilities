using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.Logging
{
    /// <summary>
    /// Represents the logger to write the log.
    /// </summary>
    public class Logger
    {
        private static string _LogFolderPath = string.Empty;
        /// <summary>
        /// Init the logger with specified log folder path. 
        /// If specified log folder path is null or emtpy, then will as application's location '{appDir}/Logs';
        /// </summary>
        /// <param name="logFolderPath">The specified log folder path (optional).</param>
        public static void Init(string logFolderPath = null)
        {
            if (!string.IsNullOrWhiteSpace(logFolderPath))
            {
                _LogFolderPath = logFolderPath;
            }
            else
            {
                _LogFolderPath = FileHandler.GetAppDefaultDirectory() + "/logs";
            }
        }
        /// <summary>
        /// Trace the message to save to current day's log file. The file is {yyyyMMdd}.trace.log.
        /// </summary>
        /// <param name="message">The message to trace.</param>
        public static void Trace(string message)
        {
            string logFilePath = string.Format("{0}/{1:yyyyMMdd}.trace.log", _LogFolderPath, DateTime.Today);
            string text =
                "--------------------------------------" + Environment.NewLine
                + "Timestamp: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine
                + message + Environment.NewLine;
            Write(logFilePath, message);
        }
        /// <summary>
        /// Writes the exception to current day's log file. The file is {yyyyMMdd}.log.
        /// </summary>
        /// <param name="ex">The exception to write.</param>
        public static void Write(Exception ex)
        {
            string logFilePath = string.Format("{0}/{1:yyyyMMdd}.log", _LogFolderPath, DateTime.Today);
            string message = 
                "--------------------------------------" + Environment.NewLine
                + "Timestamp: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine
                + "Exception: " + ex.Message + Environment.NewLine
                + "Stack Trace: " + Environment.NewLine
                + ex.ToString() + Environment.NewLine
                + "--------------------------------------" + Environment.NewLine;
            Write(logFilePath, message);
        }
        /// <summary>
        /// Writes the message to current day's log file. The file is {yyyyMMdd}.log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void Write(string message)
        {
            string logFilePath = string.Format("{0}/{1:yyyyMMdd}.log", _LogFolderPath, DateTime.Today);
            Write(logFilePath, message);
        }
        /// <summary>
        /// Writes the message to specified log file.
        /// </summary>
        /// <param name="filePath">The specified log file path (full path).</param>
        /// <param name="message">The message to write.</param>
        public static void Write(string filePath, string message)
        {
            try
            {
                FileHandler.AppendAllText(filePath, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
