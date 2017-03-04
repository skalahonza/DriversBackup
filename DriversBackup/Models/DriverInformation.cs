using WpfViewModelBase;

namespace DriversBackup.Models
{
    /// <summary>
    /// Object setup for binding to our ArrayList.
    /// </summary>
    public class DriverInformation : ViewModelBase
    {
        private bool isSelected;
        private string infPath;

        public DriverInformation(string driverProvider, string driverDescription, string driverDeviceGuid,
            string driverId)
        {
            DriverProvider = driverProvider;
            DriverDescription = driverDescription;
            DriverDeviceGuid = driverDeviceGuid;
            DriverId = driverId;
        }

        public DriverInformation(string infPath)
        {
            InfPath = infPath;
            DriverProvider = "empty driver provider";
            DriverDescription = "empty driver description";
            DriverDeviceGuid = "empty device guid";
            DriverId = "empty driver id";
            //TODO extract driver info from the inf file
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

        /// <summary>
        /// Path to the INF file of the driver
        /// </summary>
        public string InfPath
        {
            get { return infPath; }
            set
            {
                infPath = value;
                OnPropertyChanged();
            }
        }
    }
}