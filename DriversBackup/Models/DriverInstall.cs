using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DriversBackup.Models
{
    public class DriverInstall
    {
        [DllImport("Setupapi.dll", EntryPoint = nameof(InstallHinfSection),
            CallingConvention = CallingConvention.StdCall)]
        private static extern void InstallHinfSection(
            [In] IntPtr hwnd,
            [In] IntPtr ModuleHandle,
            [In, MarshalAs(UnmanagedType.LPWStr)] string CmdLineBuffer,
            int nCmdShow);

        public void InstallDriver(DriverInformation driver)
        {
            InstallDriver(driver.InfPath);
        }

        public void InstallDriver(string infPath)
        {
            InstallHinfSection(IntPtr.Zero, IntPtr.Zero, infPath, 0);
        }

        public List<string> FindDriverFilesInFolder(string folder)
        {
            return new DirectoryInfo(folder).GetFiles("*.inf", SearchOption.AllDirectories).Select(x => x.FullName).ToList();
        }
    }
}