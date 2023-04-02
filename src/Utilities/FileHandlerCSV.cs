using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace JC.Utilities
{
    public partial class FileHandler
    {
        /// <summary>
        /// Converts the specified row data collection to CSV text.
        /// </summary>
        /// <param name="headers">The CSV headers.</param>
        /// <param name="rowDatas">The CSV row data.</param>
        /// <param name="hasDoubleQuote">Whether to generate double quote.</param>
        /// <returns>The CSV text.</returns>
        public static string ConvertToCSVText(List<string> headers, List<List<string>> rowDatas, bool hasDoubleQuote = true)
        {
            StringBuilder sb = new StringBuilder();
            string strHeader = string.Join(",", headers);
            sb.AppendLine(strHeader);
            foreach(var dataRow in rowDatas)
            {
                List<string> formatedValues = dataRow.Select(th => FormartCSVField(th, hasDoubleQuote)).ToList();
                string strRow = string.Join(",", formatedValues);
                sb.AppendLine(strRow);
            }

            return sb.ToString();
        }
        private static string FormartCSVField(string data, bool hasDoubleQuote = true)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Empty;
            }
            if (hasDoubleQuote)
            {
                return string.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
            }
            else
            {
                return string.Format("{0}", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
            }
        }
    }
}
