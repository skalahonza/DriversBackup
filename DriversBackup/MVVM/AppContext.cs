using System.Windows.Controls;

namespace DriversBackup.MVVM
{
    public static class AppContext
    {
        /// <summary>
        /// Used for MVVM in app navigation
        /// </summary>
         public static Frame MainFrame { get; set; } = new Frame();
    }
}