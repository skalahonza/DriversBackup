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
        private List<DriverInformation> drivers;

        public List<DriverInformation> Drivers
        {
            get { return drivers; }
            set
            {
                drivers = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NoDriversFound));
            }
        }

        public bool NoDriversFound => !Drivers.Any();

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

        #endregion
    }
}