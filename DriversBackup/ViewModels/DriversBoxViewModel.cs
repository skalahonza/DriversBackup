using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using DriversBackup.Models;
using DriversBackup.MVVM;
using DriversBackup.Properties;
using DriversBackup.Views;
using WpfViewModelBase;
using Application = System.Windows.Application;

namespace DriversBackup.ViewModels
{
    public class DriversBoxViewModel:ViewModelBase
    {
        private ObservableCollection<DriverInformation> drivers;
        private string search = "";
        private SortBy previousSortType;

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
            }
        }

        #region Commands

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
                if (sortType == previousSortType && sortType != MainPageViewModel.SortBy.Search)
                    driversList.Reverse();
                else
                    switch (sortType)
                    {
                        case MainPageViewModel.SortBy.DriverId:
                            driversList.Sort(
                                (a, b) => string.Compare(a.DriverProvider, b.DriverProvider, StringComparison.Ordinal));
                            break;
                        case MainPageViewModel.SortBy.Description:
                            driversList.Sort(
                                (a, b) =>
                                    string.Compare(a.DriverDescription, b.DriverDescription, StringComparison.Ordinal));
                            break;
                        case MainPageViewModel.SortBy.Backup:
                            driversList.Sort((a, b) => a.IsSelected.CompareTo(b.IsSelected));
                            break;
                        case MainPageViewModel.SortBy.Search:
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
                        case MainPageViewModel.SortBy.Undefined:
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
