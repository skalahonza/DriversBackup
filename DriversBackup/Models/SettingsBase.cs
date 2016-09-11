using WpfViewModelBase;

namespace DriversBackup.Models
{
    public class SettingsBase:ViewModelBase
    {
        private string text;
        private string key = "";

        public SettingsBase(string text, string key)
        {
            Text = text;
            Key = key;
        }

        /// <summary>
        /// Text of the settings
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
        /// Matching key in app.config
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