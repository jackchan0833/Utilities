using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the enum handler.
    /// </summary>
    public class EnumHandler
    {
        /// <summary>
        /// Converts int to specified enum type.
        /// </summary>
        /// <typeparam name="TData">The enum type.</typeparam>
        /// <param name="value">The int value to convert.</param>
        /// <returns>The converted enum value.</returns>
        public static TData ConvertToEnum<TData>(int value)
            where TData : struct
        {
            return (TData)Enum.ToObject(typeof(TData), value);
        }
        /// <summary>
        /// Converts int to specified enum type.
        /// </summary>
        /// <typeparam name="TData">The enum type.</typeparam>
        /// <param name="value">The int value to convert.</param>
        /// <returns>The converted enum value.</returns>
        public static TData ConvertToEnum<TData>(short value)
            where TData : struct
        {
            return (TData)Enum.ToObject(typeof(TData), value);
        }
        /// <summary>
        /// Try to convert specified string value to enum.
        /// </summary>
        /// <typeparam name="TData">The enum type.</typeparam>
        /// <param name="strVal">The string value.</param>
        /// <param name="result">The output enum result.</param>
        /// <returns>True when success; false when failure.</returns>
        public static bool TryConvertToEnum<TData>(string strVal, out TData result)
            where TData : struct
        {
            return Enum.TryParse<TData>(strVal, out result);
        }
    }
}
