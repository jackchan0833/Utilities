using System;
using System.Collections.Generic;
using System.Text;

namespace JC.Utilities
{
    /// <summary>
    /// Represents a store file information.
    /// </summary>
    public class StoreFileInfo
    {
        /// <summary>
        /// The directory path of file.
        /// </summary>
        public string Dir { set; get; }
        /// <summary>
        /// The file name with file extension.
        /// </summary>
        public string FileName { set; get; }

        /// <summary>
        /// Constructs the <see cref="StoreFileInfo"/>.
        /// </summary>
        public StoreFileInfo() { }
        /// <summary>
        /// Constructs the <see cref="StoreFileInfo"/>.
        /// </summary>
        /// <param name="dir">The directory path of file.</param>
        /// <param name="fileName">The file name with file extension.</param>
        public StoreFileInfo(string dir, string fileName)
        {
            this.Dir = dir;
            this.FileName = fileName;
        }
    }
}
