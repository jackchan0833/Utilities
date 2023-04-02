using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the <see cref="BufferHandler"/>.
    /// </summary>
    public class BufferHandler
    {
        /// <summary>
        /// Merges two bytes into one bytes.
        /// </summary>
        /// <param name="buffer1">The buffer1.</param>
        /// <param name="buffer2">The buffer2.</param>
        /// <returns>The merged buffer.</returns>
        public static byte[] MergeBuffer(byte[] buffer1, byte[] buffer2)
        {
            if (buffer1 == null)
            {
                if (buffer2 == null)
                {
                    return null;
                }
                else
                {
                    var result = new Byte[buffer2.Length];
                    Buffer.BlockCopy(buffer2, 0, result, 0, buffer2.Length);
                    return result;
                }
            }
            else if (buffer2 == null)
            {
                var result = new Byte[buffer1.Length];
                Buffer.BlockCopy(buffer1, 0, result, 0, buffer1.Length);
                return result;
            }
            else
            {
                var result = new Byte[buffer1.Length + buffer2.Length];
                Buffer.BlockCopy(buffer1, 0, result, 0, buffer1.Length);
                Buffer.BlockCopy(buffer2, 0, result, buffer1.Length, buffer2.Length);
                return result;
            }
        }
        /// <summary>
        /// Converts bytes to Hex string.
        /// </summary>
        /// <param name="data">The bytes to convert.</param>
        /// <returns>The Hex string.</returns>
        public static string ConvertToHexString(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var b in data)
            {
                sb.Append(Convert.ToString(b, 16));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Converts the hex string to bytes.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns>The bytes.</returns>
        public static byte[] ConvertFromHexString(string hexString)
        {
            if (hexString == null)
            {
                return null;
            }
            if (hexString.Length % 2 != 0)
            {
                throw new InvalidOperationException("Invalid hex string.");
            }
            byte[] result = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length;)
            {
                result[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
                i = i + 2;
            }
            return result;
        }
    }
}
