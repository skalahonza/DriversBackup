using System.Collections.Generic;
using System.Windows.Forms;
using DriversBackup.Models;
using WpfViewModelBase;

namespace DriversBackup.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        private List<DriverInformation> drivers = new List<DriverInformation>();

        public MainWindowViewModel()
        {
            var controlelr = new DriverBackup();
            Drivers = controlelr.ListDrivers(false);
        }

        public List<DriverInformation> Drivers
        {
            get { return drivers; }
            set
            {
                drivers = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveSelectedDrivers => new RelayCommand(() =>
        {
            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                foreach (var driver in Drivers)
                {

                    var controller = new DriverBackup();
                    controller.BackupDriver(driver.DriverDeviceGuid, driver.DriverId, folder.SelectedPath + "\\");
                }
                MessageBox.Show("Drivers hopefully saved correctly");
            }
        });
    }
}