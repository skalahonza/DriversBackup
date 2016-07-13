using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DriversBackup.Models
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class DriverBackup
    {
        public string WindowsRoot;
        public string SystemRoot;

        /// <summary>
        /// Navigates to System32 folder
        /// </summary>
        public DriverBackup()
        {
            WindowsRoot = Environment.GetEnvironmentVariable("SystemRoot") + "\\";
            SystemRoot = WindowsRoot + "system32\\";
        }

        /// <summary>
        /// Returns a list of drivers registered on a system.
        /// </summary>
        /// <returns>ArrayList containing driver information</returns>
        public List<DriverInformation> ListDrivers(bool showMicrosoft)
        {
            var driverList = new List<DriverInformation>();

            //Get a list of device types.
            var regDeviceGuiDs = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\");
            var deviceGuiDs = regDeviceGuiDs?.GetSubKeyNames();

            // Iterate through devices.             
            foreach (var deviceGuid in deviceGuiDs)
            {
                // Get drivers assigned to each device type.                 
                var regDevice =
                    Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + deviceGuid);
                var regDeviceSubkeys = regDevice?.GetSubKeyNames();

                //For each driver, get the information on it (provider, type etc).                 
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
                            // Add information to our ArrayList.                             
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
                            if (tmpProvider != "Microsoft")
                                driverList.Add(
                                    new DriverInformation(tmpProvider, tmpDesc, deviceGuid, regDriverNumber));

                            //Add Microsoft drivers if user wanst them
                            else if (showMicrosoft)
                                driverList.Add(
                                    new DriverInformation(tmpProvider, tmpDesc, deviceGuid, regDriverNumber));
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
            regDeviceGuiDs.Close();
            return driverList;
        }

        /// <summary>
        /// Creates a thread of backupDriverExec.
        /// </summary>
        /// <param name="classGuid"></param>
        /// <param name="driverId"></param>
        /// <param name="backupLocation"></param>
        public async Task<bool> BackupDriver(string classGuid, string driverId, string backupLocation)
        {
            string[] driverInfo = {classGuid, driverId, backupLocation};
            bool succes = false;
            await Task.Run(async () =>
            {
                succes = await BackupDriverExec(driverInfo);
            });
            return succes;
        }

        /// <summary>
        /// Backups driver on the background thread
        /// </summary>
        /// <param name="driver">Driver to be saved</param>
        /// <returns>True: if successfull</returns>
        public async Task<bool> BackupDriverAsync(DriverInformation driver)
        {
            bool succes = false;
            await Task.Run(async () =>
            {
                succes = await BackupDriverExecAsync(driver);
            });
            return succes;
        }

        /// <summary>
        /// Backs up the given device driver.
        /// </summary>
        private async Task<bool> BackupDriverExec(object driverInfoObj)
        {
            // Driver Info
            var driverInfoArray = (string[]) driverInfoObj;
            var classGuid = driverInfoArray[0];
            var driverId = driverInfoArray[1];
            var backupLocation = driverInfoArray[2];

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

            if (driverType.Length > 0)
                Directory.CreateDirectory(backupLocation + driverType);

            Directory.CreateDirectory(backupLocation + driverType + "\\" + driverDesc);

            // Copy over inf file.            
            try
            {
                File.Copy(infFilePath, backupLocation + driverType + "\\" + driverDesc + "\\" + infFile);
            }
            catch (IOException)
            {
                //TODO Handel Exception
                return false;
            }

            // Backup driver files.             
            var driverIniFile = new IniFiles();
            var driverFiles = driverIniFile.GetKeys(infFilePath, "SourceDisksFiles");

            foreach (var driverFile in driverFiles)
            {
                try
                {
                    // El-Cheapo driver companies put weird things like %SYSFILE% in their inf files, no idea what this does. So ignore it.

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
                    return false;
                }
            }

            // Close registry.
            regDriverType?.Close();
            driverInfo?.Close();
            return true;
        }

        /// <summary>
        /// Backs up the given device driver into a selected fodler.
        /// </summary>
        /// <param name="driver">Driver for backup</param>
        /// <param name="saveFolder">Folder for saving</param>
        /// <returns></returns>
        private async Task<bool> BackupDriverExecAsync(DriverInformation driver, string saveFolder = "")
        {
            // Driver Info
            var deviceGuid = driver.DriverDeviceGuid;
            var driverId = driver.DriverId;

            var regDriverType =
                Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + deviceGuid);
            var driverType = regDriverType?.GetValue("Class").ToString();

            var driverInfo =
                Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + deviceGuid + "\\" +
                                                 driverId);
            var driverDesc = driverInfo?.GetValue("DriverDesc").ToString();
            var infFile = driverInfo?.GetValue("InfPath").ToString();
            var infFilePath = WindowsRoot + "inf\\" + infFile;

            //Create backup directory.
            if (driverType.Length > 0)
            {
                Directory.CreateDirectory(saveFolder + driverType);
                Directory.CreateDirectory(saveFolder + driverType + "\\" + driverDesc);
            }

            // Copy over inf file.            
            try
            {
                File.Copy(infFilePath, saveFolder + driverType + "\\" + driverDesc + "\\" + infFile);
            }
            catch (IOException)
            {
                //TODO Handel Exception
                return false;
            }

            // Backup driver files.             
            var driverIniFile = new IniFiles();
            var driverFiles = driverIniFile.GetKeys(infFilePath, "SourceDisksFiles");

            foreach (var driverFile in driverFiles)
            {
                try
                {
                    //HACK: El-Cheapo driver companies put weird things like %SYSFILE% in their inf files, no idea what this does. So ignore it.

                    if (driverFile.Split('.').Length <= 1) continue;
                    // Copy driver files from the right place.

                    switch (driverFile.Split('.')[1])
                    {
                        case "hlp":
                            File.Copy(WindowsRoot + "Help\\" + driverFile,
                                saveFolder + driverType + "\\" + driverDesc + "\\" + driverFile);
                            break;
                        case "sys":
                            File.Copy(SystemRoot + "drivers\\" + driverFile,
                                saveFolder + driverType + "\\" + driverDesc + "\\" + driverFile);
                            break;
                        default:
                            File.Copy(SystemRoot + driverFile,
                                saveFolder + driverType + "\\" + driverDesc + "\\" + driverFile);
                            break;
                    }
                }
                catch (IOException)
                {
                    //TODO Handle exception
                    return false;
                }
            }

            // Close registry.
            regDriverType?.Close();
            driverInfo?.Close();
            return true;
        }
    }
}
