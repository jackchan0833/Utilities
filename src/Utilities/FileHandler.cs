using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the file handler.
    /// </summary>
    public partial class FileHandler
    {
        /// <summary>
        /// Default file encoding as UTF8.
        /// </summary>
        public readonly static Encoding DefaultEncoding = Encoding.UTF8; //Encoding.UTF8;
        /// <summary>
        /// Gets the current application's directory path.
        /// </summary>
        /// <returns>The application's directory path, that doesn't has suffix "/".</returns>
        public static string GetAppDefaultDirectory()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            return basePath?.TrimEnd('/').TrimEnd('\\');
        }
        /// <summary>
        /// Gets the system disk.
        /// </summary>
        /// <returns>The system disk letter, return as "C" letter.</returns>
        public static string GetSystemDisk()
        {
            return Environment.SystemDirectory.Substring(0, 1);
        }
        /// <summary>
        /// Gets the disk of current application.
        /// </summary>
        /// <returns>The disk letter, retuan as "C" letter.</returns>
        public static string GetAppInstallDisk()
        {
            return Environment.CurrentDirectory.Substring(0, 1);
        }
        /// <summary>
        /// Writes the text into specified file path.
        /// </summary>
        /// <param name="filePath">The specified file path to write.</param>
        /// <param name="text">The text to write.</param>
        public static void Write(string filePath, string text)
        {
            Write(filePath, text, DefaultEncoding);
        }
        /// <summary>
        /// Writes the text into specified file path.
        /// </summary>
        /// <param name="filePath">The specified file path to write.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The encoding to write.</param>
        public static void Write(string filePath, string text, Encoding encoding)
        {
            CreateParentDirectory(filePath);
            byte[] dataBuffer = encoding.GetBytes(text);
            WriteAllBytes(filePath, dataBuffer);
        }
        /// <summary>
        /// Writes the data into specified file path.
        /// </summary>
        /// <param name="filePath">The specified file path to write.</param>
        /// <param name="dataBuffer">The data to write.</param>
        public static void WriteAllBytes(string filePath, byte[] dataBuffer)
        {
            CreateParentDirectory(filePath);
            if (dataBuffer == null)
            {
                WriteAllBytes(filePath, new byte[0]);
            }
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                fs.Write(dataBuffer, 0, dataBuffer.Length);
            }
        }
        /// <summary>
        /// Appends the text into specified file path with Default encoding (UTF8).
        /// </summary>
        /// <param name="filePath">The specified file path to append.</param>
        /// <param name="text">The text to append</param>
        public static void AppendAllText(string filePath, string text)
        {
            AppendAllText(filePath, text, DefaultEncoding);
        }
        /// <summary>
        /// Appends the text into specified file path with specified encoding.
        /// </summary>
        /// <param name="filePath">The specified file path to append.</param>
        /// <param name="text">The specified text.</param>
        /// <param name="encoding">The specified encoding.</param>
        public static void AppendAllText(string filePath, string text, Encoding encoding)
        {
            byte[] dataBuffer = encoding.GetBytes(text);
            AppendDataBuff(filePath, dataBuffer);
        }
        /// <summary>
        /// Append specified data to file.
        /// </summary>
        /// <param name="filePath">The specified file path to append.</param>
        /// <param name="dataBuffer">The specified data.</param>
        public static void AppendDataBuff(string filePath, byte[] dataBuffer)
        {
            if (dataBuffer == null)
                return;
            CreateParentDirectory(filePath);
            using (var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                fs.Write(dataBuffer, 0, dataBuffer.Length);
            }
        }
        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <param name="dirPath">The specified directory path.</param>
        public static void CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
        /// <summary>
        /// Gets parent directory path with specified file.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <returns>The parent directory path of file.</returns>
        public static string GetParentDirectory(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }
        /// <summary>
        /// Creates parent directory for specified file.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        public static void CreateParentDirectory(string filePath)
        {
            string parentDirectory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(parentDirectory) && !Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }
        }
        /// <summary>
        /// Gets the full path of specified file.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <returns>The full path of file.</returns>
        public static string GetFileFullPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }
            var path = filePath.TrimStart('/').TrimStart('\\');
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                string appDir = GetAppDefaultDirectory();
                return appDir + "\\" + path;
            }
        }
        /// <summary>
        /// Deletes the specified file if exists.
        /// </summary>
        /// <param name="filePath">The specified file to delete.</param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        /// <summary>
        /// Copy specified file as specified destionation file path.
        /// </summary>
        /// <param name="srcFilePath">The source file to copy.</param>
        /// <param name="destFilePath">The destionation file to copy to.</param>
        /// <param name="overwriteIfExist">Whether overwirte if destination file is existing already.</param>
        public static void CopyFile(string srcFilePath, string destFilePath, bool overwriteIfExist = true)
        {
            if (File.Exists(srcFilePath))
            {
                CreateParentDirectory(destFilePath);
                File.Copy(srcFilePath, destFilePath, overwriteIfExist);
            }
        }
        /// <summary>
        /// Move specified file to destination file.
        /// </summary>
        /// <param name="srcFilePath">The source file to move.</param>
        /// <param name="destFilePath">The destionation file path to copy to.</param>
        /// <param name="overwriteIfExist">Whether overwrite existing destionation file if exist already.</param>
        public static void MoveFile(string srcFilePath, string destFilePath, bool overwriteIfExist = true)
        {
            if (File.Exists(srcFilePath))
            {
                CreateParentDirectory(destFilePath);
                if (overwriteIfExist)
                {
                    if (File.Exists(destFilePath))
                    {
                        File.Delete(destFilePath);
                    }
                }
                File.Move(srcFilePath, destFilePath);
            }
        }
        /// <summary>
        /// Creates and get a temp file name with specified file extension.
        /// </summary>
        /// <param name="fileExtension">The specified file extension.</param>
        /// <returns>The temport file name.</returns>
        public static string GetTempFileName(string fileExtension = null)
        {
            if (string.IsNullOrWhiteSpace(fileExtension))
            {
                return Path.GetTempFileName();
            }
            else
            {
                var fileName = Path.GetTempFileName();
                string tempFileName = fileName + "." + fileExtension.TrimStart('.');
                File.Move(fileName, tempFileName);
                return tempFileName;
            }
        }
        /// <summary>
        /// Gets the folder path of specified file path.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <returns>The folder path of specified file.</returns>
        public static string GetFolerPath(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }
        /// <summary>
        /// Gets the file extension of specified file path.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <returns>The file extension</returns>
        public static string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath)?.ToLower();
        }
        /// <summary>
        /// Gets the file name of specified file with file extension.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <returns>The file name.</returns>
        public static string GetFileNameWithExtension(string filePath)
        {
            return Path.GetFileName(filePath)?.ToLower();
        }
        /// <summary>
        /// Gets file name of specified file without file extension.
        /// </summary>
        /// <param name="filePath">The specified file name.</param>
        /// <returns>The file name.</returns>
        public static string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath)?.ToLower();
        }
        /// <summary>
        /// Read all text of specified file with default encoding (UTF8).
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <returns>The file content.</returns>
        public static string ReadAllText(string filePath)
        {
            return ReadAllText(filePath, DefaultEncoding);
        }
        /// <summary>
        /// Read all text of specified file.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <param name="encoding">The encoding to read.</param>
        /// <returns>The file content.</returns>
        public static string ReadAllText(string filePath, Encoding encoding)
        {
            byte[] buffer = ReadAllBytes(filePath);
            if (buffer == null)
            {
                return null;
            }
            return encoding.GetString(buffer);
        }
        /// <summary>
        /// Read all bytes of specified file
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <returns>The file byte array.</returns>
        public static byte[] ReadAllBytes(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        /// <summary>
        /// Read file bytes with specified offset and count.
        /// </summary>
        /// <param name="fs">The file stream.</param>
        /// <param name="offset">The offset to read.</param>
        /// <param name="count">The read count.</param>
        /// <param name="returnNullIfLessCount">Whether return null when read count is smaller than read count.</param>
        /// <returns>The read byte array.</returns>
        public static byte[] ReadBytes(string filePath, int offset, int count, bool returnNullIfLessCount = false)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                if (returnNullIfLessCount)
                {
                    if (fs.Length < (offset + count))
                    {
                        return null;
                    }
                }
                byte[] buffer = new byte[count];
                fs.Read(buffer, offset, count);
                return buffer;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        /// <summary>
        /// Read file bytes with specified offset and length.
        /// </summary>
        /// <param name="fs">The file stream.</param>
        /// <param name="offset">The offset to read.</param>
        /// <param name="length">The read length.</param>
        /// <returns>The byte array to read.</returns>
        public static byte[] ReadBytes(FileStream fs, long offset, int length)
        {
            byte[] dataBuff = new byte[length];
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(dataBuff, 0, length);
            return dataBuff;
        }
        /// <summary>
        /// Gets all lines of specified file path by default encoding (UTF8).
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The all lines of file.</returns>
        public static string[] ReadAllLines(string filePath)
        {
            return ReadAllLines(filePath, DefaultEncoding);
        }
        /// <summary>
        /// Gets all lines of specified file path.
        /// </summary>
        /// <param name="filePath">The specified file path.</param>
        /// <param name="encoding">The encoding to read.</param>
        /// <returns>The all lines of file.</returns>
        public static string[] ReadAllLines(string filePath, Encoding encoding)
        {
            if (!File.Exists(filePath))
            {
                return new string[0];
            }
            var text = ReadAllText(filePath, encoding);
            if (text != null)
            {
                List<string> lines = new List<string>();
                using (StringReader reader = new StringReader(text))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
                return lines.ToArray();
            }
            return new string[0];
        }
        /// <summary>
        /// Gets the sub file full paths with specified file extension. 
        /// The fileExtension like ".xml", not include sub dir. When fileExtension is null, then return all.
        /// </summary>
        /// <param name="dirPath">The specified directory location.</param>
        /// <param name="fileExtension">The specified file extension.</param>
        /// <returns>The sub file full path.</returns>
        public static List<string> GetSubFiles(string dirPath, string fileExtension = null)
        {
            List<string> result = new List<string>();
            if (!Directory.Exists(dirPath))
            {
                return result;
            }
            string lowerFileExtension = fileExtension?.Trim().ToLower();
            bool checkFileExtension = !string.IsNullOrEmpty(lowerFileExtension);
            var dir = new DirectoryInfo(dirPath);
            foreach (var file in dir.GetFiles())
            {
                if (checkFileExtension)
                {
                    if (file.Extension?.ToLower() == lowerFileExtension)
                    {
                        result.Add(file.FullName);
                    }
                }
                else
                {
                    result.Add(file.FullName);
                }
            }
            return result;
        }
        /// <summary>
        /// Gets the sub file names with specified file extension. 
        /// The fileExtension like ".xml", not include sub dir. When fileExtension is null, then return all.
        /// </summary>
        /// <param name="dirPath">The specified directory location.</param>
        /// <param name="fileExtension">The specified file extension.</param>
        /// <returns>The sub file names.</returns>
        public static List<string> GetSubFileNames(string dirPath, string fileExtension = null)
        {
            List<string> result = new List<string>();
            if (!Directory.Exists(dirPath))
            {
                return result;
            }
            string lowerFileExtension = fileExtension?.Trim().ToLower();
            bool checkFileExtension = !string.IsNullOrEmpty(lowerFileExtension);
            var dir = new DirectoryInfo(dirPath);
            foreach (var file in dir.GetFiles())
            {
                if (checkFileExtension)
                {
                    if (file.Extension?.ToLower() == lowerFileExtension)
                    {
                        result.Add(file.Name);
                    }
                }
                else
                {
                    result.Add(file.Name);
                }
            }
            return result;
        }
        /// <summary>
        /// Gets the sub directories' name collection of specified directory location.
        /// </summary>
        /// <param name="dirPath">The specified directory location.</param>
        /// <returns>The collection of sub directories.</returns>
        public static List<string> GetSubFolderNames(string dirPath)
        {
            var result = new List<string>();
            if (!Directory.Exists(dirPath))
            {
                return result;
            }
            var dir = new DirectoryInfo(dirPath);
            foreach (var subDir in dir.GetDirectories())
            {
                result.Add(subDir.Name);
            }
            return result;
        }
        /// <summary>
        /// 
        /// Check whether exist the specified file.
        /// </summary>
        /// <param name="filePath">The specified file path to check.</param>
        /// <returns>True to have, otherwise false.</returns>
        public static bool ExistsFile(string filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// Check whether exist the specified directory.
        /// </summary>
        /// <param name="dirPath">The specified directory path to check.</param>
        /// <returns>True to have, otherwise false.</returns>
        public static bool ExistsDirectory(string dirPath)
        {
            return Directory.Exists(dirPath);
        }
        /// <summary>
        /// Check specified directory whether has specified file.
        /// </summary>
        /// <param name="dirPath">The specified directory path.</param>
        /// <param name="checkFileName">The file name to check. Case uninsensitive.</param>
        /// <returns>True to have, otherwise false.</returns>
        public static bool CheckIsExistSubFile(string dirPath, string checkFileName)
        {
            if (!Directory.Exists(dirPath) || string.IsNullOrWhiteSpace(checkFileName))
            {
                return false;
            }
            var dir = new DirectoryInfo(dirPath);
            foreach (var file in dir.GetFiles())
            {
                if (file.Name.ToLower() == checkFileName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
