using System;
using System.Collections;
using System.Windows.Forms;

namespace DriversBackup
{
    public partial class FormMain : Form
    {
        public Models.DriverBackup DriverBackup;

        public FormMain()
        {
            DriverBackup = new Models.DriverBackup();
            InitializeComponent();
        }

        /// <summary>
        /// Form OnLoad event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        /// <summary>
        /// Databinds to the ArrayList containing driver information.
        /// </summary>
        public void LoadDataGrid()
        {
            ArrayList driverList = DriverBackup.ListDrivers(false);

            dataDriverList.AutoGenerateColumns = false;
            dataDriverList.DataSource = driverList;
        }

        /// <summary>
        /// Select folder for saving of the drivers and save them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackup_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog driverSave = browseDriverSave;
            driverSave.ShowDialog();

            if (driverSave.SelectedPath.Length > 0)
            {
                /*
                 * Iterate through selected drivers and back them up.
                 */
                foreach (DataGridViewRow selectedDriver in dataDriverList.SelectedRows)
                {
                    string deviceGuid = selectedDriver.Cells[2].Value.ToString();
                    string driverId = selectedDriver.Cells[3].Value.ToString();

                    DriverBackup.BackupDriver(deviceGuid, driverId, driverSave.SelectedPath + "\\");
                }

                MessageBox.Show("Your drivers have been backed up to " + driverSave.SelectedPath);
            }
        }

        /// <summary>
        /// Selects all items in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAll_Click(object sender, EventArgs e)
        {
            dataDriverList.SelectAll();
        }
    }
}