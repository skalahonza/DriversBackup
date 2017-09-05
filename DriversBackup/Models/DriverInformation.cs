using InfHelper.Models.Attributes;
using WpfViewModelBase;

namespace DriversBackup.Models
{
    /// <inheritdoc />
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

        public DriverInformation()
        {
        }

        [InfKeyValue("Version", "Provider",true)]
        public string DriverProvider { get; set; }
        public string DriverDescription { get; set; }

        [InfKeyValue("Version", "ClassGuid",true)]
        public string DriverDeviceGuid { get; set; }
        public string DriverId { get; set; }

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