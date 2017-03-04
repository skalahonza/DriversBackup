using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using DriversBackup.Models;
using DriversBackup.MVVM;
using WpfViewModelBase;
using System.Threading.Tasks;
using DriversBackup.Properties;
using DriversBackup.Views;

namespace DriversBackup.ViewModels
{
    public class InstallPageViewModel : ViewModelBase
    {
        private ObservableCollection<DriverInformation> drivers = new ObservableCollection<DriverInformation>();
        private DriversBoxViewModel driversBox;

        public InstallPageViewModel()
        {
            //top buttons
            var top = new ObservableCollection<ActionButton>()
            {
                new ActionButton(Resources.DriverID, ActionButton.ButtonType.NoHighlight, "\xEA37",
                    "DriverId"),
                new ActionButton(Resources.Description, ActionButton.ButtonType.NoHighlight, "\xE7C3",
                    "Description"),
                new ActionButton(Resources.Backup, ActionButton.ButtonType.NoHighlight, "\xE896", "Backup"),
            };
            //bot buttons
            var bot = new ObservableCollection<ActionButton>()
            {
                new ActionButton(Resources.SelectAll, SelectAll, ActionButton.ButtonType.Deafult, "\xE133"),
                new ActionButton(Resources.InstallDrivers,
                    InstallSelectedDrivers,
                    ActionButton.ButtonType.Accept,
                    "\xE133"),
            };
            //init drivers box
            DriversBox = new DriversBoxViewModel(Drivers, top, bot);
        }

        public ObservableCollection<DriverInformation> Drivers
        {
            get { return drivers; }
            set
            {
                drivers = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DriversBox.Drivers));
                OnPropertyChanged(nameof(NoDriversFound));
                OnPropertyChanged(nameof(AnyDrivers));
            }
        }

        public bool NoDriversFound => !Drivers.Any();
        public bool AnyDrivers => Drivers.Any();
        public int SelectedDriversCount => Drivers.Count(x => x.IsSelected);

        public DriversBoxViewModel DriversBox
        {
            get { return driversBox; }
            set
            {
                driversBox = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        public RelayCommand GoBackCommand => new RelayCommand(() =>
        {
            if (AppContext.MainFrame.CanGoBack)
                AppContext.MainFrame.GoBack();
        });

        public RelayCommand InstallSelectedDriversCommand => new RelayCommand(InstallSelectedDrivers);
        public RelayCommand SelectAllCommand => new RelayCommand(SelectAll);
        public RelayCommand SelectFolderCommand => new RelayCommand(SelectFolder);

        private void InstallSelectedDrivers()
        {
            var controller = new DriverBackup();
            var selectedDrivers = Drivers.Where(x => x.IsSelected);
            foreach (var driver in selectedDrivers)
            {
                controller.InstallDriver(driver);
            }
        }

        private async void SelectFolder()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var controller = new DriverBackup();
                Drivers.Clear();
                foreach (var driver in await controller.FindDriversInFolderAsync(dialog.SelectedPath))
                    Drivers.Add(driver);
            }
        }

        private void SelectAll()
        {
            //if all are selected, de-select them
            //if not select them all
            bool select = Drivers.Count != Drivers.Count(x => x.IsSelected);
            foreach (var driver in Drivers)
                driver.IsSelected = select;
        }

        #endregion
    }
}