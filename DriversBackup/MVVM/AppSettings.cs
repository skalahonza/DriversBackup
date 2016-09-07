using System.ComponentModel;
using System.Configuration;

namespace DriversBackup.MVVM
{
    public static class AppSettings
    {        

        public static T Get<T>(string settingKey)
        {            
            var appSetting = ConfigurationManager.AppSettings[settingKey];
            var typeConverter = TypeDescriptor.GetConverter(typeof(T));

            return (T)(typeConverter.ConvertFromInvariantString(appSetting));
        }

        public static void Set(string settingKey, object value)
        {
            Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            oConfig.AppSettings.Settings[settingKey].Value = value.ToString();
            oConfig.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static bool ShowMicrosoftDrivers
        {
            get { return Get<bool>(nameof(ShowMicrosoftDrivers)); }
            set { Set(nameof(ShowMicrosoftDrivers), value); }
        }
    }
}