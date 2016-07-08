namespace DriversBackup
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.dataDriverList = new System.Windows.Forms.DataGridView();
            this.DriverProvider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DriverDeviceGUID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DriverID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.browseDriverSave = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataDriverList)).BeginInit();
            this.SuspendLayout();
            // 
            // dataDriverList
            // 
            this.dataDriverList.AllowUserToAddRows = false;
            this.dataDriverList.AllowUserToDeleteRows = false;
            this.dataDriverList.AllowUserToOrderColumns = true;
            this.dataDriverList.AllowUserToResizeRows = false;
            this.dataDriverList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataDriverList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DriverProvider,
            this.Description,
            this.DriverDeviceGUID,
            this.DriverID});
            this.dataDriverList.Location = new System.Drawing.Point(12, 12);
            this.dataDriverList.Name = "dataDriverList";
            this.dataDriverList.ReadOnly = true;
            this.dataDriverList.RowHeadersVisible = false;
            this.dataDriverList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataDriverList.Size = new System.Drawing.Size(391, 431);
            this.dataDriverList.TabIndex = 0;
            // 
            // DriverProvider
            // 
            this.DriverProvider.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.DriverProvider.DataPropertyName = "DriverProvider";
            this.DriverProvider.HeaderText = "Provider";
            this.DriverProvider.Name = "DriverProvider";
            this.DriverProvider.ReadOnly = true;
            this.DriverProvider.Width = 72;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.DataPropertyName = "DriverDescription";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DriverDeviceGUID
            // 
            this.DriverDeviceGUID.DataPropertyName = "DriverDeviceGUID";
            this.DriverDeviceGUID.HeaderText = "DriverDeviceGUID";
            this.DriverDeviceGUID.Name = "DriverDeviceGUID";
            this.DriverDeviceGUID.ReadOnly = true;
            this.DriverDeviceGUID.Visible = false;
            // 
            // DriverID
            // 
            this.DriverID.DataPropertyName = "DriverID";
            this.DriverID.HeaderText = "DriverID";
            this.DriverID.Name = "DriverID";
            this.DriverID.ReadOnly = true;
            this.DriverID.Visible = false;
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(328, 450);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 1;
            this.btnBackup.Text = "Backup";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(247, 450);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 2;
            this.btnAll.Text = "Select All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // browseDriverSave
            // 
            this.browseDriverSave.Description = "Select the location to backup the selected driver(s) to";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 487);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.dataDriverList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(423, 519);
            this.MinimumSize = new System.Drawing.Size(423, 519);
            this.Name = "frmMain";
            this.Text = "Driver Backup";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataDriverList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataDriverList;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.DataGridViewTextBoxColumn DriverProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn DriverDeviceGUID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DriverID;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.FolderBrowserDialog browseDriverSave;
    }
}

