using WpfViewModelBase;

namespace DriversBackup.Models
{
    /// <summary>
    /// Object setup for binding to our ArrayList.
    /// </summary>
    public class DriverInformation:ViewModelBase
    {
        private bool isSelected;

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