using DriversBackup.MVVM;

namespace DriversBackup.Models
{
    public class TriggerSettingsItem:SettingsBase
    {
        /// <summary>
        /// Is the settings triggered
        /// </summary>
        public bool Active
        {
            get { return !string.IsNullOrEmpty(Key) && AppSettings.Get<bool>(Key); }
            set
            {
                AppSettings.Set(Key,value);
                OnPropertyChanged();
            }
        }

        public TriggerSettingsItem(string text, string key) : base(text, key)
        {
            
        }
    }
}