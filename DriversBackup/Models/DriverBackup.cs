using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

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
        /// <returns>ArrayList containing driver information</returns>
        public List<DriverInformation> ListDrivers(bool showMicrosoft)
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
                var regDevice = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + deviceGuid);
                var regDeviceSubkeys = regDevice?.GetSubKeyNames();

               // For each driver, get the information on it (provider, type etc).
                
                foreach (var regDriverNumber in regDeviceSubkeys)
                {
                    string tmpProvider = "", tmpDesc = "";
                    try
                    {
                        var regDriver = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + deviceGuid + "\\" + regDriverNumber);

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
                            {
                                driverList.Add(
                                    new DriverInformation(tmpProvider, tmpDesc, deviceGuid, regDriverNumber)
                                );
                            }
                            else
                            {
                                if (showMicrosoft)
                                {
                                    driverList.Add(
                                        new DriverInformation(tmpProvider, tmpDesc, deviceGuid, regDriverNumber)
                                    );
                                }
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

            regDeviceGuiDs.Close();

            return driverList;
        }
        /// <summary>
        /// Starts the backup process 
        /// </summary>
        /// <param name="classGuid">Driver GUID</param>
        /// <param name="driverId">Driver ID {number}</param>
        /// <param name="backupLocation">Driver backup location (folder), every driver should have it's folder</param>
        public void BackupDriver(string classGuid, string driverId, string backupLocation)
        {
            string[] driverInfo = { classGuid, driverId, backupLocation };
            var backupThread = new Thread(BackupDriverExec);

            backupThread.Start(driverInfo);
        }
        /// <summary>
        /// Starts the backup process asynchronously
        /// </summary>
        /// <param name="driver">Driver to be saved</param>
        /// <param name="saveFolder">Folder for saving. Every driver creates a sub-folder for it self.</param>
        /// <returns></returns>
        public async Task BackupDriverAsync(DriverInformation driver, string saveFolder)
        {            
            await Task.Run(() =>
            {
                BackupDriver(driver.DriverDeviceGuid, driver.DriverId, saveFolder + "\\");
            });
        }
        /// <summary>
        /// Backs up multiple drivers asynchronously
        /// </summary>
        /// <param name="drivers">Drivers to be backed up</param>
        /// <param name="saveFolder">Folder for saving. Every driver creates a sub-folder for it self.</param>
        /// <returns></returns>
        public async Task BackupDriversAsync(IEnumerable<DriverInformation> drivers, string saveFolder)
        {
            await Task.Run(async () =>
            {
                foreach (var driver in drivers)
                {
                    await BackupDriverAsync(driver, saveFolder);
                }
            });
        }
        /// <summary>
        /// Backs up the given device driver.
        /// </summary>
        private void BackupDriverExec(object driverInfoObj)
        {
            // Driver Info.
             
            var driverInfoArray = (string[])driverInfoObj;
            var classGuid = driverInfoArray[0];
            var driverId = driverInfoArray[1];
            var backupLocation = driverInfoArray[2];

            var regDriverType = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + classGuid);
            var driverType = regDriverType?.GetValue("Class").ToString();

            var driverInfo = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Class\\" + classGuid + "\\" + driverId);
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
                File.Copy(infFilePath, backupLocation + driverType + "\\" + driverDesc + "\\" + infFile);
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
                            File.Copy(WindowsRoot + "Help\\" + driverFile, backupLocation + driverType + "\\" + driverDesc + "\\" + driverFile);
                            break;
                        case "sys":
                            File.Copy(SystemRoot + "drivers\\" + driverFile, backupLocation + driverType + "\\" + driverDesc + "\\" + driverFile);
                            break;
                        default:
                            File.Copy(SystemRoot + driverFile, backupLocation + driverType + "\\" + driverDesc + "\\" + driverFile);
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
        }
    }
}
