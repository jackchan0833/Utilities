using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the validator, special for page validation.
    /// </summary>
    public class PageValidator
    {
        /// <summary>
        /// Check string wheter is under specified minium and maxium length.
        /// </summary>
        /// <param name="text">The string content to check.</param>
        /// <param name="minLength">The minumn length.</param>
        /// <param name="maxLength">The maxium length.</param>
        /// <returns></returns>
        public static bool IsString(string text, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            if (text.Length >= minLength && text.Length <= maxLength)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Check the specified text is ip address with regular expression <see cref="RegularExpressions.IPAddress"/>.
        /// </summary>
        /// <param name="ipAddr">The ip address to check</param>
        /// <returns>True if match, otherwise false.</returns>
        public static bool IsIPAddress(string ipAddr)
        {
            return IsRegularExpr(ipAddr, RegularExpressions.IPAddress);
        }
        /// <summary>
        /// Check the specified text is matching with specified regular expression.
        /// </summary>
        /// <param name="text">The specified text to check.</param>
        /// <param name="regular">The regular expression.</param>
        /// <returns>True if match, otherwise false.</returns>
        public static bool IsRegularExpr(string text, string regular)
        {
            Regex regex = new Regex(regular);
            return regex.IsMatch(text);
        }
        /// <summary>
        /// Check the specified text is matching with password expression <see cref="RegularExpressions.Password"/>.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="minLength">The minium length.</param>
        /// <param name="maxLength">The maxium length.</param>
        /// <returns></returns>
        public static bool IsPassword(string text, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            if (text.Length >= minLength && text.Length <= maxLength)
            {
                return IsRegularExpr(text, RegularExpressions.Password);
            }
            return false;
        }
        /// <summary>
        /// Check whether is a color with hexadecimal string. Example: #FF00FF
        /// </summary>
        /// <param name="text">The check text.</param>
        /// <returns>True is color with hexadecimal, otherwise false.</returns>
        public static bool IsColorHexString(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            return IsRegularExpr(text, RegularExpressions.ColorHexString);
        }
        /// <summary>
        /// Check whether specified text is hexadecimal string. Example: FF00FF00
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="hexLength">The specified hexadecimal length.</param>
        /// <returns>True if hexadecimal string, otherwise false.</returns>
        public static bool IsHexString(string text, int hexLength)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            string reg = @"^[\d|A|B|C|D|E|F|a|b|c|d|e|f]{" + hexLength + "}$";
            return IsRegularExpr(text, reg);
        }
    }
}
