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
        /// Gets the full file path with specified store file base on current application location.
        /// </summary>
        /// <param name="storeFileInfo">The specified store file.</param>
        /// <returns>The full file path.</returns>
        public static string GetFilePath(StoreFileInfo storeFileInfo)
        {
            string filePath = string.Format("{0}/{1}/{2}", GetAppDefaultDirectory(), storeFileInfo.Dir, storeFileInfo.FileName);
            return filePath;
        }
        /// <summary>
        /// Gets the content of specified store file.
        /// </summary>
        /// <param name="storeFileInfo">The specified store file.</param>
        /// <returns>The content of file.</returns>
        public static string ReadAllText(StoreFileInfo storeFileInfo)
        {
            string fileFullPath = GetFilePath(storeFileInfo);
            return ReadAllText(fileFullPath);
        }
        /// <summary>
        /// Write the specified text to specified store file with overwrite.
        /// </summary>
        /// <param name="storeFileInfo">The store file to write.</param>
        /// <param name="text">The specified text.</param>
        public static void Write(StoreFileInfo storeFileInfo, string text)
        {
            string filePath = GetFilePath(storeFileInfo);
            Write(filePath, text);
        }
    }
}
