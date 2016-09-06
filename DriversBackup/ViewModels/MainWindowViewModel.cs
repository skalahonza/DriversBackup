using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using DriversBackup.Models;
using DriversBackup.MVVM;
using DriversBackup.Views;
using WpfViewModelBase;
using AppContext = DriversBackup.MVVM.AppContext;
using Application = System.Windows.Application;

namespace DriversBackup.ViewModels
{
    public class MainPageViewModel:ViewModelBase
    {
        private ObservableCollection<DriverInformation> drivers = new ObservableCollection<DriverInformation>();
        private SortBy previousSortType;
        private string search = "";
        // ReSharper disable once MemberInitializerValueIgnored
        private readonly List<DriverInformation> allDrivers = new List<DriverInformation>();
        private MessageDialogViewModel messageDialog;
        private bool showInProgressDialog;
        private int backingUpProgress;

        enum SortBy
        {
            // ReSharper disable once UnusedMember.Local
            Undefined,
            Search,
            DriverId,
            Description,
            Backup
        }

        public MainPageViewModel()
        {
            var controller = new DriverBackup();
            Drivers = new ObservableCollection<DriverInformation>(controller.ListDrivers(AppSettings.Get<bool>("showMicrosoft")));
            allDrivers = new List<DriverInformation>(Drivers);
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
        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged("SearchActive");
            }
        }

        public MessageDialogViewModel MessageDialog
        {
            get { return messageDialog; }
            set
            {
                messageDialog = value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(ShowMessage));
            }
        }

        public bool ShowMessage => MessageDialog != null;

        public bool ShowInProgressDialog
        {
            get { return showInProgressDialog; }
            set
            {
                showInProgressDialog = value;
                OnPropertyChanged();
            }
        }

        public int BackingUpProgress
        {
            get { return backingUpProgress; }
            set
            {
                backingUpProgress = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public RelayCommand SaveSelectedDrivers => new RelayCommand(async () =>
        {
            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() != DialogResult.OK) return;

            BackingUpProgress = 0;
            ShowInProgressDialog = true;
            await Task.Run(async () =>
            {
                try
                {
                    var controller = new DriverBackup();
                    //await controller.BackupDriversAsync(Drivers.Where(x => x.IsSelected), folder.SelectedPath);
                    foreach (DriverInformation t in Drivers)
                    {
                        await controller.BackupDriverAsync(t, folder.SelectedPath);
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(() => BackingUpProgress++));
                    }
                    MessageDialog =
                        new MessageDialogViewModel(
                            new ObservableCollection<ActionButton>(new List<ActionButton>
                            {
                                new ActionButton("Ok", () => MessageDialog = null, ActionButton.ButtonType.Accept)
                            }),
                            "Drivers saved", "Selected drivers have been successfully saved.");
                }
                catch (Exception e)
                {
                    MessageDialog =
                        new MessageDialogViewModel(
                            new ObservableCollection<ActionButton>(new List<ActionButton>
                            {
                                new ActionButton("Ok", () => MessageDialog = null, ActionButton.ButtonType.Accept)
                            }),
                            "Error", e.Message);
                }
                finally
                {
                    ShowInProgressDialog = false;
                }
            });
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

        public GenericRelayCommand<string> SortByCommand => new GenericRelayCommand<string>(s =>
        {
            SortBy sortType;
            if (Enum.TryParse(s, out sortType))
            {
                var driversList = new List<DriverInformation>(Drivers);
                //if the same sort type is used, just revers the list
                if (sortType == previousSortType && sortType != SortBy.Search)
                    driversList.Reverse();
                else  
                    switch (sortType)
                    {
                        case SortBy.DriverId:
                            driversList.Sort(
                                (a, b) => string.Compare(a.DriverProvider, b.DriverProvider, StringComparison.Ordinal));
                            break;
                        case SortBy.Description:
                            driversList.Sort(
                                (a, b) =>
                                    string.Compare(a.DriverDescription, b.DriverDescription, StringComparison.Ordinal));
                            break;
                        case SortBy.Backup:
                            driversList.Sort((a, b) => a.IsSelected.CompareTo(b.IsSelected));
                            break;
                        case SortBy.Search:
                            //empty drivers in GUI
                            driversList = allDrivers;
                            Drivers.Clear();

                            //search in driver provider
                            foreach (
                                var driverInformation in
                                    driversList.Where(x => x.DriverProvider.ToLower().Contains(Search.ToLower())))
                                Drivers.Add(driverInformation);
                            //search in driver description
                            foreach (
                                var driverInformation in
                                    driversList.Where(x => x.DriverDescription.ToLower().Contains(Search.ToLower())))
                                //preen redundant addition
                                if (!Drivers.Contains(driverInformation))
                                    Drivers.Add(driverInformation);
                            return;
                        case SortBy.Undefined:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                //Show newly sorted rivers on the UI
                Drivers = new ObservableCollection<DriverInformation>(driversList);
                //save sort type
                previousSortType = sortType;
            }
        });

        public RelayCommand CancelSearch => new RelayCommand(() =>
        {
            Search = "";
        });
        #endregion
    }
}