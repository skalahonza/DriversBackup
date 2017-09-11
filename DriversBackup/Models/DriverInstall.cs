using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DriversBackup.MVVM;
using Newtonsoft.Json;

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

        public IEnumerable<string> FindDriverFilesInFolder(string folder)
        {
            return new DirectoryInfo(folder).GetFiles("*.inf", SearchOption.AllDirectories).Select(x => x.FullName);
        }

        public List<DriverInformation> GetDriversFromFolder(string path)
        {
            // special json found
            var jsonFilePath = Path.Combine(path, AppSettings.JsonInfoName);
            List<DriverInformation> drivers;
            if (File.Exists(jsonFilePath))
            {
                drivers =
                    JsonConvert.DeserializeObject<List<DriverInformation>>(File.ReadAllText(jsonFilePath));
                drivers.ForEach(di =>
                {
                    if (di.InfPath != null)
                        di.InfPath = Path.Combine(path, di.InfPath);
                });
            }

            //de-serialize info manually
            else
            {
                drivers = FindDriverFilesInFolder(path).Select(DriversInformation.FromInfFile).ToList();
            }
            
            return drivers;
        }

        public async Task<List<DriverInformation>> GetDriversFromFolderAsync(string path)
        {
            return await Task.Run(() => GetDriversFromFolder(path));
        }
    }
}