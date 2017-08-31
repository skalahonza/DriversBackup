using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using DriversBackup.Models;
using WpfViewModelBase;
using System.Threading.Tasks;
using DriversBackup.Properties;
using AppContext = DriversBackup.MVVM.AppContext;
using Application = System.Windows.Application;

namespace DriversBackup.ViewModels
{
    public class InstallPageViewModel : ViewModelBase
    {
        private ObservableCollection<DriverInformation> drivers = new ObservableCollection<DriverInformation>();
        private DriversBoxViewModel driversBox;
        private bool showInProgressDialog;
        private int driversToLoadCount;
        private int loadingPorgress;

        public InstallPageViewModel()
        {
            //top buttons
            var top = new ObservableCollection<ActionButton>
            {
                new ActionButton(Resources.DriverID, ActionButton.ButtonType.NoHighlight, "\xEA37",
                    "DriverId"),
                new ActionButton(Resources.Description, ActionButton.ButtonType.NoHighlight, "\xE7C3",
                    "Description"),
                new ActionButton(Resources.Install, ActionButton.ButtonType.NoHighlight, "\xE710", "Backup"),
            };
            //bot buttons
            var bot = new ObservableCollection<ActionButton>
            {
                new ActionButton(Resources.SelectAll, SelectAll, ActionButton.ButtonType.Deafult, "\xE133"),
                new ActionButton(Resources.InstallDrivers,
                    InstallSelectedDrivers,
                    ActionButton.ButtonType.Accept,
                    "\xE710"),
            };
            //init drivers box
            DriversBox = new DriversBoxViewModel(Drivers, top, bot);
        }

        public bool ShowInProgressDialog
        {
            get { return showInProgressDialog; }
            set
            {
                showInProgressDialog = value;
                OnPropertyChanged();
            }
        }

        public int DriversToLoadCount
        {
            get { return driversToLoadCount; }
            set
            {
                driversToLoadCount = value;
                OnPropertyChanged();
            }
        }

        public int LoadingPorgress
        {
            get { return loadingPorgress; }
            set
            {
                loadingPorgress = value;
                OnPropertyChanged();
            }
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
            var controller = new DriverInstall();
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
                var controller = new DriverInstall();
                Drivers.Clear();
                DriversBox.AllDrivers.Clear();
                ShowInProgressDialog = true;

                await Task.Run(() =>
                {
                    var driverFiles = controller.FindDriverFilesInFolder(dialog.SelectedPath);
                    DriversToLoadCount = driverFiles.Count;
                    LoadingPorgress = 0;
                    var factory = new DriverInformations();
                    foreach (var file in driverFiles)
                    {
                        var driver = factory.FromInfFile(file);
                        Application.Current.Dispatcher.Invoke(() => Drivers.Add(driver));
                        DriversBox.AllDrivers.Add(driver);
                        LoadingPorgress++;
                    }
                });
                
                ShowInProgressDialog = false;
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