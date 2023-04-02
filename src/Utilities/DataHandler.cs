using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the <see cref="DataHandler"/>.
    /// </summary>
    public class DataHandler
    {
        /// <summary>
        /// The Json serializer setting. Default is to ignore null value.
        /// </summary>
        public static JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
        };
        /// <summary>
        /// Default encoding as UTF8.
        /// </summary>
        public static Encoding DefaultEncoding = new UTF8Encoding(false);
        /// <summary>
        /// Serialize the object to json string, and encoding with default encoding (UTF8).
        /// </summary>
        /// <typeparam name="TData">The object type to serialize.</typeparam>
        /// <param name="data">The data to serialize.</param>
        /// <returns>The encoded byte array.</returns>
        public static byte[] SerializeAsBytes<TData>(TData data)
            where TData : class
        {
            return SerializeAsBytes(data, DefaultEncoding);
        }
        /// <summary>
        /// Serialize the object to json string, and encoding with UTF8.
        /// </summary>
        /// <typeparam name="TData">The object type to serialize.</typeparam>
        /// <param name="data">The data to serialize.</param>
        /// <returns>The encoded byte array.</returns>
        public static byte[] SerializeAsBytes<TData>(TData data, Encoding encoding)
            where TData : class
        {
            string text;
            if (data == null)
            {
                text = string.Empty;
            }
            else
            {
                text = JsonConvert.SerializeObject(data, JsonSerializerSettings);
            }
            return DefaultEncoding.GetBytes(text);
        }
        /// <summary>
        /// Deserialize the byte array data to specified object.
        /// </summary>
        /// <typeparam name="TData">The object type.</typeparam>
        /// <param name="data">The data to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static TData DeserializeToObject<TData>(byte[] data)
            where TData : class
        {
            return DeserializeToObject<TData>(data, DefaultEncoding);
        }
        /// <summary>
        /// Deserialize the byte array data to specified object.
        /// </summary>
        /// <typeparam name="TData">The object type.</typeparam>
        /// <param name="data">The data to deserialize.</param>
        /// <param name="encoding">The specified encoding.</param>
        /// <returns>The deserialized object.</returns>
        public static TData DeserializeToObject<TData>(byte[] data, Encoding encoding)
            where TData : class
        {
            if (data == null || data.Length == 0)
            {
                return default(TData);
            }
            var text = DefaultEncoding.GetString(data);
            return JsonConvert.DeserializeObject<TData>(text);
        }
        /// <summary>
        /// Convert the collection data to byte array by using <see cref="Marshal"/> processor.
        /// </summary>
        /// <typeparam name="TData">The object type of collection.</typeparam>
        /// <param name="listData">The collection data.</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] MarshalConvertToByteArray<TData>(List<TData> listData)
        //where TData : class
        {
            if (listData == null)
            {
                return null;
            }
            int objSize = Marshal.SizeOf(typeof(TData));
            int totalSize = listData.Count * objSize;
            byte[] dataBuffer = new byte[totalSize];
            int index = 0;
            foreach (var data in listData)
            {
                var objBuffer = MarshalConvertToByteArray(data, objSize);
                Buffer.BlockCopy(objBuffer, 0, dataBuffer, index, objSize);
                index += objSize;
            }
            return dataBuffer;
        }
        /// <summary>
        /// Convert the data to byte array by using <see cref="Marshal"/> processor.
        /// </summary>
        /// <param name="obj">The data to convert.</param>
        /// <param name="objSize">The data size (optional).</param>
        /// <returns>The converted byte array.</returns>
        public static byte[] MarshalConvertToByteArray(object obj, int? objSize = null)
        {
            if (objSize == null)
            {
                objSize = Marshal.SizeOf(obj);
            }
            byte[] data = null;
            IntPtr intPtr = IntPtr.Zero;
            try
            {
                intPtr = Marshal.AllocHGlobal(objSize.Value);
                Marshal.StructureToPtr(obj, intPtr, false);
                data = new byte[objSize.Value];
                Marshal.Copy(intPtr, data, 0, objSize.Value);
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return data;
        }
        /// <summary>
        /// Gets the object size by using <see cref="Marshal"/> processor.
        /// </summary>
        /// <typeparam name="TData">The specified data type.</typeparam>
        /// <returns>The object size.</returns>
        public static int GetObjectSizeByMarshal<TData>()
        {
            return Marshal.SizeOf<TData>();
        }
        /// <summary>
        /// Converts the byte array to specified data collection by using <see cref="Marshal"/> processor.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="dataBuff">The data byte array to convert.</param>
        /// <param name="onlyFirst">Whether is only convert the first one.</param>
        /// <returns>The data collection.</returns>
        public static List<TData> MarshalConvertToObject<TData>(byte[] dataBuff, bool onlyFirst = true)
        //where TData : class
        {
            List<TData> result = new List<TData>();
            if (dataBuff != null && dataBuff.Any())
            {
                int objSize = Marshal.SizeOf(typeof(TData));
                IntPtr intPtr = IntPtr.Zero;
                if (dataBuff.Length < objSize)
                {
                    var newDataBuff = new byte[objSize];
                    Buffer.BlockCopy(dataBuff, 0, newDataBuff, 0, dataBuff.Length);
                    try
                    {
                        intPtr = Marshal.AllocHGlobal(objSize);
                        Marshal.Copy(newDataBuff, 0, intPtr, objSize);
                        var dataObj = Marshal.PtrToStructure(intPtr, typeof(TData));
                        result.Add((TData)dataObj);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(intPtr);
                    }
                }
                else
                {
                    try
                    {
                        for (int i = 0; (i + objSize) <= dataBuff.Length;)
                        {
                            intPtr = Marshal.AllocHGlobal(objSize);
                            Marshal.Copy(dataBuff, i, intPtr, objSize);
                            var dataObj = Marshal.PtrToStructure(intPtr, typeof(TData));
                            result.Add((TData)dataObj);
                            if (onlyFirst)
                            {
                                break;
                            }
                            i = i + objSize;
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(intPtr);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Converts the byte array to specified object by using <see cref="Marshal"/>.
        /// </summary>
        /// <typeparam name="TObject">The specified object type.</typeparam>
        /// <param name="data">The byte array to convert.</param>
        /// <param name="objSize">The output object size.</param>
        /// <returns>The converted object.</returns>
        public static TObject MarshalConvertToObject<TObject>(byte[] data, out int objSize)
        {
            objSize = Marshal.SizeOf<TObject>();
            if (data.Length < objSize)
            {
                return default(TObject);
            }
            IntPtr intPtr = IntPtr.Zero;
            try
            {
                intPtr = Marshal.AllocHGlobal(objSize);
                Marshal.Copy(data, 0, intPtr, objSize);
                return Marshal.PtrToStructure<TObject>(intPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
        }
    }
}
