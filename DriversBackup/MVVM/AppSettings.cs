using System.ComponentModel;
using System.Configuration;

namespace DriversBackup.MVVM
{
    public static class AppSettings
    {        
        /// <summary>
        /// Finds a value in app.config and returns it's value as a specific type.
        /// </summary>
        /// <typeparam name="T">Required type.</typeparam>
        /// <param name="settingKey">Key of the settings. View app.config</param>
        /// <returns>Value of the settings in given Type</returns>
        public static T Get<T>(string settingKey)
        {            
            var appSetting = ConfigurationManager.AppSettings[settingKey];
            var typeConverter = TypeDescriptor.GetConverter(typeof(T));

            return (T)(typeConverter.ConvertFromInvariantString(appSetting));
        }

        /// <summary>
        /// Changes the value of settings in ap.config, based on a given key. Automatically save changes in app.config, once the change is done.
        /// </summary>
        /// <param name="settingKey">Given key</param>
        /// <param name="value">Value will be saved as string.</param>
        public static void Set(string settingKey, object value)
        {
            Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            oConfig.AppSettings.Settings[settingKey].Value = value.ToString();
            oConfig.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }

        #region App settings
        public static bool ShowMicrosoftDrivers
        {
            get { return Get<bool>(nameof(ShowMicrosoftDrivers)); }
            set { Set(nameof(ShowMicrosoftDrivers), value); }
        }
        #endregion
    }
}