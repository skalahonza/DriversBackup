using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DriversBackup.Models;
using DriversBackup.MVVM;
using WpfViewModelBase;

namespace DriversBackup.ViewModels
{
    public class InstallPageViewModel : ViewModelBase
    {
        private List<DriverInformation> drivers = new List<DriverInformation>();

        public List<DriverInformation> Drivers
        {
            get { return drivers; }
            set
            {
                drivers = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoDriversFound));
                OnPropertyChanged(nameof(AnyDrivers));
            }
        }

        public bool NoDriversFound => !Drivers.Any();
        public bool AnyDrivers => Drivers.Any();
        public int SelectedDriversCount => Drivers.Count(x => x.IsSelected);

        #region Commands

        public RelayCommand GoBackCommand => new RelayCommand(() =>
        {
            if (AppContext.MainFrame.CanGoBack)
                AppContext.MainFrame.GoBack();
        });

        public RelayCommand InstallSelectedDrivers => new RelayCommand(() =>
        {
            var controller = new DriverBackup();
            var selectedDrivers = Drivers.Where(x => x.IsSelected);
            foreach (var driver in selectedDrivers)
            {
                controller.InstallDriver(driver);
            }
        });

        public RelayCommand SelectFolder => new RelayCommand(async () =>
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var controller = new DriverBackup();
                Drivers = await controller.FindDriversInFolderAsync(dialog.SelectedPath);
            }
        });

        public RelayCommand SelectAll => new RelayCommand(() =>
        {
            //if all are selected, de-select them
            //if not select them all
            bool select = Drivers.Count != Drivers.Count(x => x.IsSelected);
            foreach (var driver in Drivers)
                driver.IsSelected = select;
        });

        #endregion
    }
}