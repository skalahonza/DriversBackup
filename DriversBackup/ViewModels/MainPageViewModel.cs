using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<DriverInformation> drivers = new ObservableCollection<DriverInformation>();
        private SortBy previousSortType;
        private string search = "";
        // ReSharper disable once MemberInitializerValueIgnored
        private readonly List<DriverInformation> allDrivers = new List<DriverInformation>();
        private MessageDialogViewModel messageDialog;
        private bool showInProgressDialog;
        private int backingUpProgress;

        //Sort type for listview of drivers
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
            //Initialize collection of drivers
            var controller = new DriverBackup();
            Drivers =
                new ObservableCollection<DriverInformation>(controller.ListDrivers(AppSettings.ShowMicrosoftDrivers));
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

        /// <summary>
        /// Search query
        /// </summary>
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

        /// <summary>
        /// ViewModel for the message dialog control
        /// </summary>
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

        /// <summary>
        /// Determines the message dialog visibility
        /// </summary>
        public bool ShowMessage => MessageDialog != null;

        /// <summary>
        /// Determines the in progress dialog visibility
        /// </summary>
        public bool ShowInProgressDialog
        {
            get { return showInProgressDialog; }
            set
            {
                showInProgressDialog = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Represents the amount of completed operations - used for progress bar
        /// </summary>
        public int BackingUpProgress
        {
            get { return backingUpProgress; }
            set
            {
                backingUpProgress = value;
                OnPropertyChanged();
            }
        }

        public int DriversForBackpCount => Drivers.Count(x => x.IsSelected);
        #region Commands

        public RelayCommand SaveSelectedDrivers => new RelayCommand(async () =>
        {
            //Update Drivers for backup count property
            OnPropertyChanged(nameof(DriversForBackpCount));

            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() != DialogResult.OK) return;

            BackingUpProgress = 0;
            ShowInProgressDialog = true;
            await Task.Run(async () =>
            {
                try
                {
                    var controller = new DriverBackup();
                    foreach (var t in Drivers.Where(x => x.IsSelected))
                    {
                        //Backup drivers one by one on background thread and show progress to the user
                        await controller.BackupDriverAsync(t, folder.SelectedPath);
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(() => BackingUpProgress++));
                    }
                    //Alert user when the job is done
                    MessageDialog =
                        new MessageDialogViewModel(
                            new ObservableCollection<ActionButton>(new List<ActionButton>
                            {
                                new ActionButton("Ok", () => MessageDialog = null, ActionButton.ButtonType.Accept),
                                new ActionButton("Open folder", () => Process.Start(folder.SelectedPath), ActionButton.ButtonType.Deafult),
                            }),
                            "Drivers saved", "Selected drivers have been successfully saved.");
                }
                catch (Exception e)
                {
                    //Let user know about the error
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
            //if not select them all
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
                //if the same sort type is used, just reverse the list
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
                //Show newly sorted drivers on the UI
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