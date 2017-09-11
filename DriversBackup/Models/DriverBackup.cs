using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DriversBackup.MVVM;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace DriversBackup.Models
{
    public class DriverBackup
    {       
        public string WindowsRoot;
        public string SystemRoot;

        /// <summary>
        /// Constructor
        /// </summary>
        public DriverBackup()
        {
            // URI to the System32 folder.             
            WindowsRoot = Environment.GetEnvironmentVariable("SystemRoot") + "\\";
            SystemRoot = WindowsRoot + "system32\\";
        }

        /// <summary>
        /// Returns a list of drivers registered on a system.
        /// </summary>
        /// <returns>yList containing driver information</returns>
        public List<DriverInformation> ListDrivers(bool showMicrosoft = false)
        {
            var driverList = new List<DriverInformation>();

            // Open registry and get a list of device types.

            var regDeviceGuiDs = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\");
            var deviceGuiDs = regDeviceGuiDs?.GetSubKeyNames();
            // Iterate through devices.

            // ReSharper disable once PossibleNullReferenceException
            foreach (var deviceGuid in deviceGuiDs)
            {
                // Get drivers assigned to each device type.                 
                var regDevice =
                    Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + deviceGuid);
                var regDeviceSubkeys = regDevice?.GetSubKeyNames();

                // For each driver, get the information on it (provider, type etc).

                foreach (var regDriverNumber in regDeviceSubkeys)
                {
                    string tmpProvider = "", tmpDesc = "";
                    try
                    {
                        var regDriver =
                            Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + deviceGuid +
                                                             "\\" + regDriverNumber);

                        try
                        {
                            // Add information to result List.
                            tmpDesc = regDriver.GetValue("DriverDesc").ToString();
                            tmpProvider = regDriver.GetValue("ProviderName").ToString();
                        }
                        catch
                        {
                            //TODO Handle exception
                        }

                        // If any of the information checks out as rubbish, discard this driver.

                        if (tmpProvider.Length > 0 && tmpDesc.Length > 0)
                        {
                            // add non microsoft driver
                            if (tmpProvider != "Microsoft")
                            {
                                driverList.Add(
                                    new DriverInformation(tmpProvider, tmpDesc, deviceGuid, regDriverNumber)
                                    );
                            }
                            //if microsoft drivers ae required, add them too
                            else if (showMicrosoft)
                            {
                                driverList.Add(
                                    new DriverInformation(tmpProvider, tmpDesc, deviceGuid, regDriverNumber)
                                    );
                            }

                        }
                        regDriver.Close();
                    }
                    catch
                    {
                        // TODO Handle exception
                    }

                    regDevice.Close();
                }
            }

            // free resources
            regDeviceGuiDs.Close();
            return driverList;
        }     

        /// <summary>
        /// Saves drivers, works on background thread
        /// </summary>
        public async Task BackupDriverAsync(DriverInformation driver, string saveFolder)
        {
            await Task.Run(() =>
            {
                // Driver Info.
                var classGuid = driver.DriverDeviceGuid;
                var driverId = driver.DriverId;
                var backupLocation = saveFolder + "\\";

                var regDriverType =
                    Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + classGuid);
                var driverType = regDriverType?.GetValue("Class").ToString();

                var driverInfo =
                    Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + classGuid + "\\" +
                                                     driverId);
                var driverDesc = driverInfo?.GetValue("DriverDesc").ToString();
                var infFile = driverInfo?.GetValue("InfPath").ToString();
                var infFilePath = WindowsRoot + "inf\\" + infFile;

                //Create backup directory.

                if (!string.IsNullOrEmpty(driverType))
                {
                    Directory.CreateDirectory(backupLocation + driverType);
                }
                Directory.CreateDirectory(backupLocation + driverType + "\\" + driverDesc);                


                // Copy over inf file.

                try
                {
                    File.Copy(infFilePath, backupLocation + driverType + "\\" + driverDesc + "\\" + infFile,true);
                    driver.InfPath = Path.Combine(driverType, driverDesc, infFile);
                }
                catch (IOException)
                {
                    //TODO Handle exception
                }

                // Backup driver files.
                var driverIniFile = new IniFiles();
                var driverFiles = driverIniFile.GetKeys(infFilePath, "SourceDisksFiles");

                foreach (var driverFile in driverFiles)
                {
                    try
                    {
                        /*
                          El-Cheapo driver companies put weird things like %SYSFILE%
                          in their inf files, no idea what this does. So ignore it.
                         */
                        if (driverFile.Split('.').Length <= 1) continue;
                        // Copy driver files from the right place.

                        switch (driverFile.Split('.')[1])
                        {
                            case "hlp":
                                File.Copy(WindowsRoot + "Help\\" + driverFile,
                                    backupLocation + driverType + "\\" + driverDesc + "\\" + driverFile);
                                break;
                            case "sys":
                                File.Copy(SystemRoot + "drivers\\" + driverFile,
                                    backupLocation + driverType + "\\" + driverDesc + "\\" + driverFile);
                                break;
                            default:
                                File.Copy(SystemRoot + driverFile,
                                    backupLocation + driverType + "\\" + driverDesc + "\\" + driverFile);
                                break;
                        }
                    }
                    catch (IOException)
                    {
                        //TODO Handle exception
                    }
                }

                // Close registry.             
                regDriverType?.Close();
                driverInfo?.Close();
            });
        }

        public void SaveDriversInfo(IEnumerable<DriverInformation> drivers, string folder)
        {
            string json = JsonConvert.SerializeObject(drivers);
            File.WriteAllText(Path.Combine(folder, AppSettings.JsonInfoName), json);
        }
    }
}
