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
    /// Represents the <see cref="StringHandler"/>.
    /// </summary>
    public class StringHandler
    {
        private static Random random = new Random();
        public static Char[] RdChars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public static int RdCharsLength = RdChars.Length;
        /// <summary>
        /// Gets the random string with specified length.
        /// </summary>
        /// <param name="length">The specified length to generate.</param>
        /// <returns>The generated random string.</returns>
        public static string GetRandomString(int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var rdIndex = random.Next(1, RdCharsLength) - 1;
                sb.Append(RdChars[rdIndex]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Gets the index of specified char in the char array.
        /// </summary>
        /// <param name="chs">The char array.</param>
        /// <param name="ch">The specified char to find.</param>
        /// <returns>The index of specified char. If not found, return -1.</returns>
        public static int GetIndexOfChar(char[] chs, char ch)
        {
            if (chs == null)
            {
                return -1;
            }
            for (int i = 0; i < chs.Length; i++)
            {
                if (chs[i] == ch)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Converts the array of Unicode characters to string, and ignore the rest characters of end '\0' character.
        /// </summary>
        /// <param name="chs">The array of unicode characters.</param>
        /// <returns>The string.</returns>
        public static string ConvertFromChars(char[] chs)
        {
            if (chs == null)
                return null;
            var pos = GetIndexOfChar(chs, '\0');
            if (pos >= 0)
            {
                return new string(chs, 0, pos);
            }
            else
            {
                return new String(chs);
            }
        }
        /// <summary>
        /// Split the string content with maxium character count.
        /// </summary>
        /// <param name="content">The content to split.</param>
        /// <param name="maxLineCharCount">The specified maxium character count.</param>
        /// <returns>The splitted string array.</returns>
        public static List<string> SplitString(string content, int maxLineCharCount)
        {
            List<string> result = new List<string>();
            if (content == null)
                return result;
            int index = 0;
            for (; (index + maxLineCharCount) < content.Length;)
            {
                var subStr = content.Substring(index, maxLineCharCount);
                result.Add(subStr);
                index += maxLineCharCount;
            }
            if (index < content.Length)
            {
                result.Add(content.Substring(index));
            }
            return result;
        }
        /// <summary>
        /// Gets the MD5 hash value (Hex) of specified text. If null value, return null.
        /// </summary>
        /// <param name="text">The specified text to compute the MD5 hash value..</param>
        /// <returns>The MD5 hash value with hexadecimal format.</returns>
        public static string MD5Encrypt32(string text)
        {
            if (text == null)
                return null;
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            string result = BitConverter.ToString(s);
            return result.Replace("-", "");
        }
        /// <summary>
        /// Converts the hexadecimal string to byte array.
        /// </summary>
        /// <param name="hexString">The hexadecimal string.</param>
        /// <returns>The array bytes.</returns>
        public static byte[] ConvertToHexBytes(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
            {
                throw new InvalidOperationException("Invalid hex string: " + hexString);
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
        /// <summary>
        /// Converts the specified number by specified format. Default format is 8 chars like 000000AB
        /// </summary>
        /// <param name="val">The specified number to convert.</param>
        /// <param name="format">The specified format.</param>
        /// <returns>The converted string result.</returns>
        public static string ConvertToHexString(int val, string format = "X8")
        {
            return val.ToString(format);
        }
        /// <summary>
        /// Try to convert the specified Base64 content to byte array.
        /// </summary>
        /// <param name="content">The specified Base4 content.</param>
        /// <param name="dataBuff">The output byte array.</param>
        /// <returns>True to success, false to fail to convert.</returns>
        public static bool TryConvertFromBase64String(string content, out byte[] dataBuff)
        {
            try
            {
                dataBuff = Convert.FromBase64String(content);
            }
            catch
            {
                dataBuff = null;
                return false;
            }
            return true;
        }
        /// <summary>
        /// Try to convert the specified hexadecimal content to byte array.
        /// </summary>
        /// <param name="content">The specified hexadecimal content.</param>
        /// <param name="dataBuff">The output byte array.</param>
        /// <returns>True to success, false to fail to convert.</returns>
        public static bool TryConvertFromHexString(string content, out byte[] dataBuff)
        {
            try
            {
                dataBuff = BufferHandler.ConvertFromHexString(content);
            }
            catch
            {
                dataBuff = null;
                return false;
            }
            return true;
        }

        #region calcualte formula
        /// <summary>
        /// Try to calculate the specified expression. Like Excel expression.
        /// </summary>
        /// <param name="expression">The specified expression.</param>
        /// <param name="value">The ouput value.</param>
        /// <returns>True to success, false to failure.</returns>
        public static bool TryCalculate(string expression, out decimal value)
        {
            value = 0;
            try
            {
                var dt = new System.Data.DataTable();
                value = Convert.ToDecimal(dt.Compute(expression, ""));
                //string className = "CustomCalcExpression";
                //string methodName = "Run";

                //// 创建编译器实例。
                //var complier = (new Microsoft.CSharp.CSharpCodeProvider());
                //// 设置编译参数。
                //var paras = new System.CodeDom.Compiler.CompilerParameters();
                //paras.GenerateExecutable = false;
                //paras.GenerateInMemory = true;

                //// 创建动态代码。
                //StringBuilder classSource = new StringBuilder();
                //classSource.Append("public class " + className + "\n");
                //classSource.Append("{\n");
                //classSource.Append(" public object " + methodName + "()\n");
                //classSource.Append(" {\n");
                //classSource.Append(" return " + expression + " * 1.0;\n");
                //classSource.Append(" }\n");
                //classSource.Append("}");

                //// 编译代码。
                //var compileResult = complier.CompileAssemblyFromSource(paras, classSource.ToString());

                //// 获取编译后的程序集。
                //var assembly = compileResult.CompiledAssembly;

                //// 动态调用方法。
                //object eval = assembly.CreateInstance(className);
                //var method = eval.GetType().GetMethod(methodName);
                //object reobj = method.Invoke(eval, null);
                //GC.Collect();
                //value = Convert.ToDecimal(reobj);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
