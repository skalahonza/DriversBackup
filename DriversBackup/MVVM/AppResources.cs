using System;
using System.Windows;

namespace DriversBackup.MVVM
{
    public static class AppResources
    {
        public static string Hello => ReadResource("test");

        private static string ReadResource(string key, bool throwExceptions = true)
        {
            try
            {
                return Application.Current.FindResource("Hello") as string;
            }
            catch (Exception ex)
            {
                if (throwExceptions)
                    throw ex;
                return default(string);
            }
        }
    }
}