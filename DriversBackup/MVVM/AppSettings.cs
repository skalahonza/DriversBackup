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

            return (T)typeConverter.ConvertFromInvariantString(appSetting);
        }
        public static void Set(string settingKey, object value)
        {
            var oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            oConfig.AppSettings.Settings[settingKey].Value = value.ToString();
            oConfig.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }
        public static bool ShowMicrosoftDrivers
        {
            get => Get<bool>(nameof(ShowMicrosoftDrivers));
            set => Set(nameof(ShowMicrosoftDrivers), value);
        }
        public static bool ZipRootFolder
        {
            get => Get<bool>(nameof(ZipRootFolder));
            set => Set(nameof(ZipRootFolder), value);
        }

        public static string JsonInfoName => Get<string>(nameof(JsonInfoName));
    }
}