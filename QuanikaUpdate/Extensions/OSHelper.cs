using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VPSetup.Extensions
{
    public static class OSHelper
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        /// <summary>
        /// Detect Operation System is 64bit
        /// </summary>
        /// <returns></returns>
        public static bool Is64BitOperatingSystem()
        {
            using (Process p = Process.GetCurrentProcess())
            {
                bool retVal;
                return (IntPtr.Size == 8 ||
                        (IntPtr.Size == 4 &&
                         IsWow64Process(p.Handle, out retVal) && retVal)
                       );
            }
        }

        /// <summary>
        /// Detect Process is 64bit
        /// </summary>
        /// <returns></returns>
        public static bool Is64BitProc()
        {
            return (IntPtr.Size == 8);
        }
    }
}
