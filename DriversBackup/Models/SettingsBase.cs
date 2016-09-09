using WpfViewModelBase;

namespace DriversBackup.Models
{
    public class SettingsBase:ViewModelBase
    {
        private string text;
        private string key = "";

        /// <summary>
        /// Class model for app settings. Used for classes TriggerSettingsItem and EnumSettingsItem. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        public SettingsBase(string text, string key)
        {
            Text = text;
            Key = key;
        }

        /// <summary>
        /// Text of the settings. Is displayed in flow, for example next to a radiobutton
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Matching key in app.config. Used for interaction with AppSettings class. When the Value property of inheriting classes is changed, this key is used for changing the value of the actual settings item in app.config. 
        /// </summary>
        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged();
            }
        }
    }
}