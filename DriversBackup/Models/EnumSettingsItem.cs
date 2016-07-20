using System;
using DriversBackup.MVVM;

namespace DriversBackup.Models
{
    public class EnumSettingsItem : SettingsBase
    {
        public enum BackupLocation
        {
            Local,
            Cloud
        }

        public BackupLocation SelectedBackupLocation
        {
            get
            {
                BackupLocation loc;
                if (Enum.TryParse(AppSettings.Get<string>(Key), out loc))
                    return loc;

                throw new ArgumentOutOfRangeException();
            }
            set
            {
                AppSettings.Set(Key,value);
                OnPropertyChanged();
                OnPropertyChanged("StringValue");
            }
        }

        //TODO add switch that loads localized strings
        public string StringValue => SelectedBackupLocation.ToString();

        public EnumSettingsItem(string text, string key) : base(text, key)
        {
        }
    }
}