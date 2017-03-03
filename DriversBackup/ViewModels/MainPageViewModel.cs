using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
using StringResources = DriversBackup.Properties.Resources;

namespace DriversBackup.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<DriverInformation> drivers = new ObservableCollection<DriverInformation>();
        private string search = "";
        // ReSharper disable once MemberInitializerValueIgnored
        private MessageDialogViewModel messageDialog;
        private bool showInProgressDialog;
        private int backingUpProgress;

        //Sort type for listview of drivers

        public MainPageViewModel()
        {
            //Initialize collection of drivers
            var controller = new DriverBackup();
            Drivers =
                new ObservableCollection<DriverInformation>(controller.ListDrivers(AppSettings.ShowMicrosoftDrivers));
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

        private void OpenOutputFolder(string path)
        {            
            //Handle: Folder might have been compressed to zip - check and handle
            if (Directory.Exists(path))
                Process.Start(path);

            //older is a zip
            else if (File.Exists(path + ".zip"))
            {
                MessageDialog =
                    new MessageDialogViewModel(
                        new ObservableCollection<ActionButton>()
                        {
                            new ActionButton(StringResources.OK, () => MessageDialog = null, ActionButton.ButtonType.Accept)
                        },
                        StringResources.FolderCannotBeOpened, StringResources.FolderCannotBeOpenedLong);
            }

            //not found
            else
            {
                MessageDialog =
                    new MessageDialogViewModel(
                        new ObservableCollection<ActionButton>()
                        {
                            new ActionButton(StringResources.OK, () => MessageDialog = null, ActionButton.ButtonType.Accept)
                        },
                        StringResources.FolderCannotBeOpened, StringResources.FolderNotFound);
            }
        }

        private void CompressFolderAsZip(string path)
        {
            //TODO Initialize System.IO Compress stream or use NuGet instead
        }

        #region Commands

        public RelayCommand SaveSelectedDrivers => new RelayCommand(async () =>
        {
            //Update Drivers for backup count property
            OnPropertyChanged(nameof(DriversForBackpCount));
            MessageDialog = null;
            //check for empty selection
            if (!Drivers.Any(x => x.IsSelected))
            {
                MessageDialog =
                    new MessageDialogViewModel(new ObservableCollection<ActionButton>(new List<ActionButton>()
                    {
                        new ActionButton(StringResources.OK,
                            () =>
                            {
                                MessageDialog = null;
                            }, ActionButton.ButtonType.Deafult)
                    }), StringResources.NothingToSave, StringResources.NoDriversSelected);
                return;
            }

            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() != DialogResult.OK) return;
            string path = folder.SelectedPath;

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
                        await controller.BackupDriverAsync(t, path);
                        await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                            new Action(() => BackingUpProgress++));
                    }

                    //Zip folder if user wants it automatically
                    if (AppSettings.ZipRootFolder)
                    {

                    }

                    //Alert user when the job is done
                    MessageDialog =
                        new MessageDialogViewModel(
                            new ObservableCollection<ActionButton>(new List<ActionButton>
                            {
                                new ActionButton(StringResources.OK, () => MessageDialog = null, ActionButton.ButtonType.Accept),
                                new ActionButton(StringResources.OpenFolder, () => OpenOutputFolder(path),
                                    ActionButton.ButtonType.Deafult),
                            }),
                            StringResources.DriversSaved, StringResources.DriversSavedLong);

                    //Add compress folder as zip button if it is not automatic
                    if (!AppSettings.ZipRootFolder)
                    {
                        MessageDialog.ActionButtons.Add(new ActionButton(StringResources.ZipFolder,
                            () =>
                            {
                                CompressFolderAsZip(path);
                                MessageDialog.ActionButtons.Last().IsEnabled = false;
                            }, ActionButton.ButtonType.Deafult));
                    }
                }
                catch (Exception e)
                {
                    //Let user know about the error
                    MessageDialog =
                        new MessageDialogViewModel(
                            new ObservableCollection<ActionButton>(new List<ActionButton>
                            {
                                new ActionButton(StringResources.OK, () => MessageDialog = null, ActionButton.ButtonType.Accept)
                            }),
                            StringResources.Error, e.Message);
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
        #endregion
    }
}