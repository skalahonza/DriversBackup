using DriversBackup.MVVM;

namespace DriversBackup.Models
{
    /// <summary>
    /// Class model for binary settings item. Used in a flow. 
    /// </summary>
    public class TriggerSettingsItem:SettingsBase
    {
        /// <summary>
        /// Is the settings triggered. Can be true or false. When this property is changed, the property with the same Key is Changed in app.config.
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