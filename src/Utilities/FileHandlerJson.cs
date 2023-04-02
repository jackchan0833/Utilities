using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace JC.Utilities
{
    public partial class FileHandler
    {
        /// <summary>
        /// The Json serializer setting. Default is to ignore null value.
        /// </summary>
        public static JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
        };
        /// <summary>
        /// Serialize the specified object to JSON string with specified json serialize setting. 
        /// Null value will be as default setting, see <see cref="DefaultJsonSerializerSettings"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SerializeObject(object data, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (jsonSerializerSettings == null)
            {
                var text = JsonConvert.SerializeObject(data, DefaultJsonSerializerSettings);
                return text;
            }
            else
            {
                var text = JsonConvert.SerializeObject(data, jsonSerializerSettings);
                return text;
            }
        }
        /// <summary>
        /// Write the specified data as json format to specified file.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="storeFileInfo">The store file.</param>
        /// <param name="data">The data to write.</param>
        /// <param name="encoding">The encoding to write.</param>
        public static void WriteAsJson<TData>(StoreFileInfo storeFileInfo, TData data, Encoding encoding = null)
            where TData : class
        {
            string filePath = GetFilePath(storeFileInfo);
            WriteAsJson(filePath, data, encoding);
        }
        /// <summary>
        /// Write the specified data as json format to specified file.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="filePath">The file path to write.</param>
        /// <param name="data">The json data.</param>
        /// <param name="encoding">The encoding to write.</param>
        public static void WriteAsJson<TData>(string filePath, TData data, Encoding encoding = null)
            where TData : class
        {
            string text;
            if (data == null)
            {
                text = string.Empty;
            }
            else
            {
                text = JsonConvert.SerializeObject(data, DefaultJsonSerializerSettings);
            }
            var writeEncoding = encoding ?? DefaultEncoding;
            Write(filePath, text, writeEncoding);
        }
        /// <summary>
        /// Read the json data from specified store file.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="storeFileInfo">The store file.</param>
        /// <param name="encoding">The encoding to read.</param>
        /// <returns>The json data.</returns>
        public static TData ReadFromJson<TData>(StoreFileInfo storeFileInfo, Encoding encoding = null)
            where TData : class
        {
            string filePath = GetFilePath(storeFileInfo);
            return ReadFromJson<TData>(filePath, encoding);
        }
        /// <summary>
        /// Read the json data from specified file path.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="filePath">The specified file path to read.</param>
        /// <param name="encoding">The encoding to read.</param>
        /// <returns>The json data.</returns>
        public static TData ReadFromJson<TData>(string filePath, Encoding encoding = null)
           where TData : class
        {
            var readEncoding = encoding ?? DefaultEncoding;
            string text = ReadAllText(filePath, readEncoding);
            if (string.IsNullOrEmpty(text))
            {
                return default(TData);
            }
            return JsonConvert.DeserializeObject<TData>(text);
        }
    }
}
