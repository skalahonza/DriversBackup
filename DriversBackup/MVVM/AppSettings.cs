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
            ConfigurationManager.AppSettings[settingKey] = (string) value;
        }
    }
}