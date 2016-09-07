using System.Collections.ObjectModel;
using System.Diagnostics;
using DriversBackup.Models;
using DriversBackup.MVVM;
using WpfViewModelBase;

namespace DriversBackup.ViewModels
{
    public class SettingsPageViewModel:ViewModelBase
    {
        private ObservableCollection<TriggerSettingsItem> appSettingsItems = new ObservableCollection<TriggerSettingsItem>();
        private EnumSettingsItem backupLocationSettings;

        public SettingsPageViewModel()
        {
            //Initialize application settings 
            //AppSettingsItems.Add(new TriggerSettingsItem("Use dark theme", "darkTheme"));  
            AppSettingsItems.Add(new TriggerSettingsItem("Show Microsoft drivers", "showMicrosoft"));  
            //AppSettingsItems.Add(new TriggerSettingsItem("Backup drivers periodically", "backupPeriodically"));

            //Initialize backup settings
            //BackupLocationSettings = new EnumSettingsItem("Choose Backup location", "backupLocation");
        }

        #region Properties

        public ObservableCollection<TriggerSettingsItem> AppSettingsItems
        {
            get { return appSettingsItems; }
            set
            {
                appSettingsItems = value;
                OnPropertyChanged();
            }
        }

        public EnumSettingsItem BackupLocationSettings
        {
            get { return backupLocationSettings; }
            set
            {
                backupLocationSettings = value;
                OnPropertyChanged();
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(CloudSettingsSelected));
            }
        }

        public bool CloudSettingsSelected
            => BackupLocationSettings.SelectedBackupLocation != EnumSettingsItem.BackupLocation.Local;

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