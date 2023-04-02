using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the <see cref="RegularExpressions"/>.
    /// </summary>
    public class RegularExpressions
    {
        public const string UserName = @"^[a-zA-Z0-9_-]{3,16}$";
        public const string Password = @"^[a-zA-Z0-9\._]{6,50}$";
        /// <summary>
        /// Email regular expression.
        /// </summary>
        public const string Email = @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})";
        /// <summary>
        /// IPV4 regular expression.
        /// </summary>
        public const string IPAddress = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
        /// <summary>
        /// Color "#FFFFFF" or "FFFFFF".
        /// </summary>
        public const string ColorHexString = @"^#?[\d|A|B|C|D|E|F|a|b|c|d|e|f]{6}$";
        /// <summary>
        /// Hex string.
        /// </summary>
        public const string HexStringFormat = @"^[\d|A|B|C|D|E|F|a|b|c|d|e|f]{0}$";
    }
}
