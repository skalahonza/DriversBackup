using System.Collections.Generic;
using System.Windows.Forms;
using DriversBackup.Models;
using DriversBackup.MVVM;
using WpfViewModelBase;

namespace DriversBackup.ViewModels
{
    public class InstallPageViewModel : ViewModelBase
    {
        private List<DriverInformation> drivers;

        public InstallPageViewModel()
        {
            
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

        #region Commands

        public RelayCommand GoBackCommand => new RelayCommand(() =>
        {
            if (AppContext.MainFrame.CanGoBack)
                AppContext.MainFrame.GoBack();
        });

        public RelayCommand InstallSelectedDrivers => new RelayCommand(() =>
        {
            
        });

        public RelayCommand ExploreFolder => new RelayCommand(() =>
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                
            }
        });        

        #endregion
    }
}