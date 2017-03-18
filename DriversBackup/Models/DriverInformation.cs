using WpfViewModelBase;

namespace DriversBackup.Models
{
    /// <summary>
    /// Object setup for binding to our ArrayList.
    /// </summary>
    public class DriverInformation : ViewModelBase
    {
        private bool isSelected;

        public DriverInformation(string driverProvider, string driverDescription, string driverDeviceGuid, string driverId)
        {
            DriverProvider = driverProvider;
            DriverDescription = driverDescription;
            DriverDeviceGuid = driverDeviceGuid;
            DriverId = driverId;
        }
        public string DriverProvider { get; private set; }
        public string DriverDescription { get; private set; }
        public string DriverDeviceGuid { get; private set; }
        public string DriverId { get; private set; }
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