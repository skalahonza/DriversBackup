using System.ComponentModel;
using System.Configuration;

namespace DriversBackup.MVVM
{
    public static class AppSettings
    {
        public static T Get<T>(string settingKey, bool throwException = true)
        {            
            var appSetting = ConfigurationManager.AppSettings[settingKey];
            var typeConverter = TypeDescriptor.GetConverter(typeof(T));

            return (T)(typeConverter.ConvertFromInvariantString(appSetting));
        }
    }
}