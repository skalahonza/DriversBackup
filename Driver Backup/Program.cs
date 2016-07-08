using System;
using System.Windows.Forms;

/*
 * CREDITS:
 *  Dave Hope
 *  Everything here (more or less)
 * 
 *  Kane Lean
 *  Reminding me to close the RegistryKey's, thanks Kane!
 * 
 */
namespace DriversBackup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}