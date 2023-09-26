using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using VPSetup.Helpers;

namespace Setup.Extensions
{
    public class OSTypeHelper
    {
        // Declare the OSVERSIONINFO structure which will contain operating system version information
        public struct OSVERSIONINFO
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
        }

        [DllImport("kernel32.Dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern short GetVersionEx(ref OSVERSIONINFO o);
        static public string GetServicePack()
        {
            OSVERSIONINFO os = new OSVERSIONINFO();
            os.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFO));
            GetVersionEx(ref os);
            if (os.szCSDVersion == "")
                return "No Service Pack Installed";
            else
                return os.szCSDVersion;
        }

        public static bool IsServerVersion()
        {
            try
            {
                //using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
                //{
                //    foreach (ManagementObject managementObject in searcher.Get())
                //    {
                //        uint productType = (uint)managementObject.GetPropertyValue("ProductType");
                //        return productType != 1;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Helper.WriteLog(ex);
            }
            return false;
        }

        public static string GetOSType()
        {
            string osType = "";
            Version vs = Environment.OSVersion.Version;
            bool isServer = IsServerVersion();
            switch (vs.Major)
            {
                case 3:
                    osType = "Windows NT 3.51";
                    break;
                case 4:
                    osType = "Windows NT 4.0";
                    break;
                case 5:
                    if (vs.Minor == 0)
                        osType = "Windows 2000";
                    else if (vs.Minor == 1)
                        osType = "Windows XP";
                    else
                    {
                        if (isServer)
                        {
                            if (WindowsAPI.GetSystemMetrics(89) == 0)
                                osType = "Windows Server 2003";
                            else
                                osType = "Windows Server 2003 R2";
                        }
                        else
                            osType = "Windows XP";
                    }
                    break;
                case 6:
                    if (vs.Minor == 0)
                    {
                        if (isServer)
                            osType = "Windows Server 2008";
                        else
                            osType = "Windows Vista";
                    }
                    else if (vs.Minor == 1)
                    {
                        if (isServer)
                            osType = "Windows Server 2008 R2";
                        else
                            osType = "Windows 7";
                    }
                    else if (vs.Minor == 2)
                        osType = "Windows 8";
                    else
                    {
                        if (isServer)
                            osType = "Windows Server 2012 R2";
                        else
                            osType = "Windows 8.1";
                    }
                    break;
            }

            return osType;
        }

        private static void GetOSBit()
        {
            //if (Environment.Is64BitOperatingSystem)
            //{
            //    return "64-Bit";
            //}
            //else
            //{
            //    return "32-Bit";
            //}

            //if (Environment.Is64BitProcess)
            //{
            //    return "64-Bit";
            //}
            //else
            //{
            //    return "32-Bit";
            //}
        }
    }

    // Interop class to call GetSystemMetrics which will help us distinguish between Windows Server 2003 and 
    // Windows Server 2003 R2
    public partial class WindowsAPI
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetSystemMetrics(int smIndex);
    }
}
