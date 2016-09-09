using System.Diagnostics;
using DriversBackup.MVVM;
using WpfViewModelBase;

namespace DriversBackup.ViewModels
{
    public class SettingsPageViewModel:ViewModelBase
    {
        #region Properties

        public bool ShowMicrosoftDrivers
        {
            get { return AppSettings.ShowMicrosoftDrivers; }
            set { AppSettings.ShowMicrosoftDrivers = value; }
        }

        #endregion

        #region Commands
        public RelayCommand GoBackCommand => new RelayCommand(() =>
        {
            if (AppContext.MainFrame.CanGoBack)
                AppContext.MainFrame.GoBack();
        });
        public GenericRelayCommand<string> GoToWeb => new GenericRelayCommand<string>(url => Process.Start(url));
        #endregion
    }
}