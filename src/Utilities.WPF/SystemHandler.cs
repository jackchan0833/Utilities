using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace JC.Utilities.WPF
{
#pragma warning disable CA1416
    public class SystemHandler
    {
        /// <summary>
        /// Gets the disk drive serial number. Iff specified disk is null, then return it points to the disk of currrent application location.
        /// </summary>
        /// <returns>The disk drive serial number.</returns>
        public string GetDiskDriveSerialNo(string specifiedDisk = null)
        {
            string sysDisk;
            if (!string.IsNullOrWhiteSpace(specifiedDisk))
            {
                sysDisk = specifiedDisk;
            }
            else
            {
                sysDisk = FileHandler.GetAppInstallDisk();
            }
            ManagementObject disk = new ManagementObject($"win32_logicaldisk.deviceid=\"{sysDisk}:\"");
            disk.Get();
            string result = disk.GetPropertyValue("VolumeSerialNumber").ToString();
            return result;
        }
        /// <summary>
        /// Gets the first CPU serial number.
        /// </summary>
        public string GetCPUSerialNo()
        {
            string result = string.Empty;
            ManagementObjectSearcher Wmi = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject WmiObj in Wmi.Get())
            {
                result = WmiObj["ProcessorId"].ToString();
                break;
            }
            return result;
        }
        /// <summary>
        /// Gets the base board serial number.
        /// </summary>
        public string GetBaseBoardSerialNo()
        {
            string result = string.Empty;
            ManagementObjectSearcher Wmi = new ManagementObjectSearcher("SELECT * FROM WIN32_BaseBoard");
            foreach (ManagementObject WmiObj in Wmi.Get())
            {
                result = WmiObj["SerialNumber"].ToString();
                break;
            }
            return result;
        }

        /// <summary>
        /// Gets the harddisk id of Operation System.
        /// </summary>
        /// <param name="returnOriginalString">
        /// Whether return as orginal system value format. Win7 is hexadecimal string, but Win10 is number string.
        /// Default is false to process to number string.
        /// </param>
        public string GetOSHardDiskID(bool returnOriginalString = false)
        {
            string hardDiskID = null;
            string systemDisk = Path.GetPathRoot(Environment.SystemDirectory).TrimEnd('\\');
            using (var m1 = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + systemDisk + "'} WHERE ResultClass=Win32_DiskPartition"))
            {
                foreach (var i1 in m1.Get())
                {
                    using (var m2 = new ManagementObjectSearcher("ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + i1["DeviceID"] + "'} WHERE ResultClass=Win32_DiskDrive"))
                    {
                        foreach (var i2 in m2.Get())
                        {
                            hardDiskID = i2["SerialNumber"].ToString().Trim();
                            break;
                        }
                    }
                    break;
                }
            }
            if (!returnOriginalString)
            {
                hardDiskID = ProcessHardDiskIDByOSVersion(hardDiskID);
            }
            return hardDiskID;
        }
        private string ProcessHardDiskIDByOSVersion(string hardDiskID)
        {
            //convert win7 format (hexadecimal number format), example "2010202057202d44585731503341383030343435"; 
            if (hardDiskID.Length == 40)
            {
                try
                {
                    //win7 format is Hex number format, and the character is from right to left, and sequence is reverse.
                    if (StringHandler.TryConvertFromHexString(hardDiskID, out byte[] data))
                    {
                        var arrChars = Encoding.ASCII.GetString(data).Trim().ToCharArray();
                        for (int i = arrChars.Length; i > 0;)
                        {
                            var a = arrChars[i - 1];
                            var b = arrChars[i - 2];
                            arrChars[i - 1] = b;
                            arrChars[i - 2] = a;
                            i = i - 2;
                        }
                        var result = string.Join("", arrChars).Trim();
                        return result;
                    }
                    else
                    {
                        return hardDiskID;
                    }
                }
                catch
                {
                    return hardDiskID;
                }
            }
            else
            {
                return hardDiskID;
            }
        }
    }
#pragma warning restore CA1416
}
