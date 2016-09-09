using WpfViewModelBase;

namespace DriversBackup.Models
{
    /// <summary>
    /// Object setup for binding to our ArrayList.
    /// </summary>
    public class DriverInformation:ViewModelBase
    {
        private bool isSelected;
        /// <summary>
        /// Class model for a driver, used in flow. All given parameters should be obtained using DriversBackup class
        /// </summary>
        /// <param name="driverProvider">Name of the driver provider {Microsoft,Intel, etc}</param>
        /// <param name="driverDescription">Driver description</param>
        /// <param name="driverDeviceGuid">Driver GUID obtained from registers</param>
        /// <param name="driverId">Driver ID {4 numbers}</param>
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
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}