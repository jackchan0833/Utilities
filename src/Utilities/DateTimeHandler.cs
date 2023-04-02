using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the DataTime handler extension.
    /// </summary>
    public class DateTimeHandler
    {
        /// <summary>
        /// The base start time when get seconds or milliseconds. Value is 2000/01/01.
        /// </summary>
        public static DateTime BaseStartTime = new DateTime(2000, 1, 1, 0, 0, 0);

        /// <summary>
        /// Try to convert string value to DataTime. Return null when val is null or empty.
        /// </summary>
        /// <param name="val">The value of string to convert.</param>
        /// <param name="dtTime">The output date time.</param>
        /// <returns>True when success; false when failure.</returns>
        public static bool TryConvertToDateTime(string val, out DateTime? dtTime)
        {
            dtTime = null;
            try
            {
                if (string.IsNullOrEmpty(val))
                    return true;
                dtTime = DateTime.Parse(val);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Convert the date time to specified string format. return null when time is null.
        /// </summary>
        /// <param name="dateTime">The specified date time.</param>
        /// <param name="format">The specified format.</param>
        /// <returns>The string result.</returns>
        public static string ConvertToString(DateTime? dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (dateTime == null)
                return null;
            return dateTime.Value.ToString(format);
        }

        /// <summary>
        /// Convert to seconds with based start year, month, day.
        /// </summary>
        /// <param name="dtTime">The date time.</param>
        /// <param name="startYear">The based start year.</param>
        /// <returns>The time seconds.</returns>
        public static double ConvertToTimeSeconds(DateTime? dtTime, int startYear = 2000)
        {
            if (dtTime == null)
            {
                return 0;
            }
            var dtStartTime = startYear == 2000 ? BaseStartTime : new DateTime(startYear, 1, 1, 0, 0, 0);
            var diffTime = dtTime.Value - dtStartTime;
            return (uint)diffTime.TotalSeconds;
        }
        /// <summary>
        /// Convert to milliseconds with based start year, month, day.
        /// </summary>
        /// <param name="dtTime">The date time.</param>
        /// <param name="startYear">The based start year.</param>
        /// <returns>The time milliseconds.</returns>
        public static double ConvertToTimeMilliseconds(DateTime? dtTime, int startYear = 2000)
        {
            if (dtTime == null)
            {
                return 0;
            }
            var dtStartTime = startYear == 2000 ? BaseStartTime : new DateTime(startYear, 1, 1, 0, 0, 0);
            var diffTime = dtTime.Value - dtStartTime;
            return diffTime.TotalMilliseconds - diffTime.TotalMilliseconds % 1;
        }
        /// <summary>
        /// Trys to convert milliseconds to date time with specified start year.
        /// </summary>
        /// <param name="totalTimeMilliseconds">The diff milliseconds.</param>
        /// <param name="dtTime">The output date time.</param>
        /// <param name="startYear">The base start year.</param>
        /// <returns>True when success; false when failure.</returns>
        public static bool TryConvertFromTimeMilliseconds(double totalTimeMilliseconds, out DateTime? dtTime, int startYear = 2000)
        {
            dtTime = null;
            try
            {
                var dtStartTime = startYear == 2000 ? BaseStartTime : new DateTime(startYear, 1, 1, 0, 0, 0);
                dtTime = dtStartTime.AddMilliseconds(totalTimeMilliseconds);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Trys to convert seconds to date time with specified start year.
        /// </summary>
        /// <param name="totalSeconds">The diff seconds.</param>
        /// <param name="dtTime">The output date time.</param>
        /// <param name="startYear">The base start year.</param>
        /// <returns>True when success; false when failure.</returns>
        public static bool TryConvertFromTimeSeconds(double totalSeconds, out DateTime? dtTime, int startYear = 2000)
        {
            dtTime = null;
            try
            {
                var dtStartTime = startYear == 2000 ? BaseStartTime : new DateTime(startYear, 1, 1, 0, 0, 0);
                dtTime = dtStartTime.AddSeconds(totalSeconds);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Gets the date time without milliseconds.
        /// </summary>
        /// <param name="dt">The specified date time.</param>
        /// <returns>The date time without milliseconds.</returns>
        public static DateTime? GetWithoutMilliseconds(DateTime? dt)
        {
            if (dt == null)
            {
                return dt;
            }
            var dtValue = dt.Value;
            return new DateTime(dtValue.Year, dtValue.Month, dtValue.Day, dtValue.Hour, dtValue.Minute, dtValue.Second, dtValue.Kind);
        }
        /// <summary>
        /// Gets the date time without seconds.
        /// </summary>
        /// <param name="dt">The date time.</param>
        /// <returns>The date time without seconds.</returns>
        public static DateTime? GetWithoutSeconds(DateTime? dt)
        {
            if (dt == null)
            {
                return dt;
            }
            var dtValue = dt.Value;
            return new DateTime(dtValue.Year, dtValue.Month, dtValue.Day, dtValue.Hour, dtValue.Minute, 0, dtValue.Kind);
        }
    }
}
