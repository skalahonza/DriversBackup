using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DriversBackup.Models;
using DriversBackup.MVVM;
using DriversBackup.Views;
using WpfViewModelBase;

namespace DriversBackup.ViewModels
{
    public class DriversBoxViewModel:ViewModelBase
    {
        /*
        private string search = "";
        private SortBy previousSortType;
        private readonly List<DriverInformation> allDrivers = new List<DriverInformation>();

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
            if (!Enum.TryParse(s, out sortType)) return;
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
        });

        public RelayCommand CancelSearch => new RelayCommand(() =>
        {
            Search = "";
        });

        #endregion
        */
    }
}
