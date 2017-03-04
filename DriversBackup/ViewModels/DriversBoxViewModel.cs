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
    public class DriversBoxViewModel : ViewModelBase
    {
        private string search = "";
        private SortBy previousSortType;
        private readonly List<DriverInformation> allDrivers;
        private ObservableCollection<DriverInformation> drivers = new ObservableCollection<DriverInformation>();
        private ObservableCollection<ActionButton> topButtons;
        private ObservableCollection<ActionButton> botButtons;

        public DriversBoxViewModel()
        {
        }

        public DriversBoxViewModel(ObservableCollection<DriverInformation> drivers)
        {
            allDrivers = new List<DriverInformation>(drivers);
            Drivers = drivers;
        }

        public DriversBoxViewModel(ObservableCollection<DriverInformation> drivers,
            ObservableCollection<ActionButton> topButtons, ObservableCollection<ActionButton> botButtons)
            : this(drivers)
        {
            TopButtons = topButtons;
            BotButtons = botButtons;
        }

        //Sort type for list-view of drivers
        enum SortBy
        {
            // ReSharper disable once UnusedMember.Local
            Undefined,
            Search,
            DriverId,
            Description,
            Backup
        }

        #region Properties

        public string Search
        {
            get { return search; }
            set
            {
                search = value;
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
            }
        }

        /// <summary>
        /// Buttons for the top command bar
        /// </summary>
        public ObservableCollection<ActionButton> TopButtons
        {
            get { return topButtons; }
            set
            {
                topButtons = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Buttons for the bottom command bar
        /// </summary>
        public ObservableCollection<ActionButton> BotButtons
        {
            get { return botButtons; }
            set
            {
                botButtons = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public RelayCommand SelectAll => new RelayCommand(() =>
        {
            //if all are selected, de-select them
            //if not select them all
            bool select = Drivers.Count != Drivers.Count(x => x.IsSelected);
            foreach (var driver in Drivers)
                driver.IsSelected = select;
        });

        public RelayCommand GoToSettings
            => new RelayCommand(() => { AppContext.MainFrame.Navigate(new SettingsPage()); });

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

        public RelayCommand CancelSearch => new RelayCommand(() => { Search = ""; });

        #endregion
    }
}