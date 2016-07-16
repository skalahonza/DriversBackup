using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using DriversBackup.Models;
using DriversBackup.Views;
using WpfViewModelBase;
using AppContext = DriversBackup.MVVM.AppContext;

namespace DriversBackup.ViewModels
{
    public class MainPageViewModel:ViewModelBase
    {
        private ObservableCollection<DriverInformation> drivers = new ObservableCollection<DriverInformation>();

        public MainPageViewModel()
        {
            var controlelr = new DriverBackup();
            Drivers = new ObservableCollection<DriverInformation>(controlelr.ListDrivers(false));
        }

        public ObservableCollection<DriverInformation> Drivers
        {
            get { return drivers; }
            set
            {
                drivers = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public RelayCommand SaveSelectedDrivers => new RelayCommand(async () =>
        {
            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() != DialogResult.OK) return;
            var controller = new DriverBackup();

            await controller.BackupDriversAsync(Drivers.Where(x => x.IsSelected), folder.SelectedPath);
            MessageBox.Show("Drivers saved.");
        });
        public RelayCommand SelectAll => new RelayCommand(() =>
        {
            //if all are selected, de-select them
            //if not select all
            bool select = Drivers.Count != Drivers.Count(x => x.IsSelected);
            foreach (var driver in Drivers)
                driver.IsSelected = select;
        });
        public RelayCommand GoToSettings => new RelayCommand(() =>
        {
            AppContext.MainFrame.Navigate(new SettingsPage());
        });
        #endregion
    }
}