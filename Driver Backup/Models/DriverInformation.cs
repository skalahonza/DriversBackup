namespace DriversBackup.Models
{
    /// <summary>
    /// Object setup for binding to our ArrayList.
    /// </summary>
    public class DriverInformation
    {
        public DriverInformation(string driverProvider, string driverDescription, string driverDeviceGuid, string driverId)
        {
            DriverProvider = driverProvider;
            DriverDescription = driverDescription;
            DriverDeviceGuid = driverDeviceGuid;
            DriverId = driverId;
        }
        public string DriverProvider { get; }    
        public string DriverDescription { get; }
        public string DriverDeviceGuid { get; }
        public string DriverId { get; }
    }
}