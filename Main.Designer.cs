
namespace CC_Firmware_Update
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.ipAddressTitleLabel = new System.Windows.Forms.Label();
            this.StatusTitleLabel = new System.Windows.Forms.Label();
            this.statusTextLabel = new System.Windows.Forms.Label();
            this.searchDevicesButton = new System.Windows.Forms.Button();
            this.generalPurposeProgressBar = new System.Windows.Forms.ProgressBar();
            this.getDeviceInfoButton = new System.Windows.Forms.Button();
            this.startFirmwareUpdateButton = new System.Windows.Forms.Button();
            this.waitDeviceBootBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.FirmwareUpdateBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.waitDeviceAPPBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.totalStausProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Location = new System.Drawing.Point(75, 14);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(376, 20);
            this.ipAddressTextBox.TabIndex = 1;
            this.ipAddressTextBox.Text = "10.1.11.211";
            // 
            // ipAddressTitleLabel
            // 
            this.ipAddressTitleLabel.AutoSize = true;
            this.ipAddressTitleLabel.Location = new System.Drawing.Point(8, 18);
            this.ipAddressTitleLabel.Name = "ipAddressTitleLabel";
            this.ipAddressTitleLabel.Size = new System.Drawing.Size(64, 13);
            this.ipAddressTitleLabel.TabIndex = 2;
            this.ipAddressTitleLabel.Text = "IP Address: ";
            // 
            // StatusTitleLabel
            // 
            this.StatusTitleLabel.AutoSize = true;
            this.StatusTitleLabel.Location = new System.Drawing.Point(8, 50);
            this.StatusTitleLabel.Name = "StatusTitleLabel";
            this.StatusTitleLabel.Size = new System.Drawing.Size(40, 13);
            this.StatusTitleLabel.TabIndex = 4;
            this.StatusTitleLabel.Text = "Status:";
            // 
            // statusTextLabel
            // 
            this.statusTextLabel.AutoSize = true;
            this.statusTextLabel.Location = new System.Drawing.Point(75, 50);
            this.statusTextLabel.Name = "statusTextLabel";
            this.statusTextLabel.Size = new System.Drawing.Size(16, 13);
            this.statusTextLabel.TabIndex = 5;
            this.statusTextLabel.Text = "...";
            // 
            // searchDevicesButton
            // 
            this.searchDevicesButton.Location = new System.Drawing.Point(526, 45);
            this.searchDevicesButton.Name = "searchDevicesButton";
            this.searchDevicesButton.Size = new System.Drawing.Size(169, 23);
            this.searchDevicesButton.TabIndex = 6;
            this.searchDevicesButton.Text = "Search Devices";
            this.searchDevicesButton.UseVisualStyleBackColor = true;
            this.searchDevicesButton.Click += new System.EventHandler(this.searchDevicesButton_Click);
            // 
            // generalPurposeProgressBar
            // 
            this.generalPurposeProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.generalPurposeProgressBar.Location = new System.Drawing.Point(11, 109);
            this.generalPurposeProgressBar.MarqueeAnimationSpeed = 1;
            this.generalPurposeProgressBar.Maximum = 10;
            this.generalPurposeProgressBar.Name = "generalPurposeProgressBar";
            this.generalPurposeProgressBar.Size = new System.Drawing.Size(684, 23);
            this.generalPurposeProgressBar.Step = 1;
            this.generalPurposeProgressBar.TabIndex = 7;
            // 
            // getDeviceInfoButton
            // 
            this.getDeviceInfoButton.Location = new System.Drawing.Point(526, 77);
            this.getDeviceInfoButton.Name = "getDeviceInfoButton";
            this.getDeviceInfoButton.Size = new System.Drawing.Size(169, 23);
            this.getDeviceInfoButton.TabIndex = 8;
            this.getDeviceInfoButton.Text = "Get Device Info";
            this.getDeviceInfoButton.UseVisualStyleBackColor = true;
            this.getDeviceInfoButton.Click += new System.EventHandler(this.getDeviceInfoButton_Click);
            // 
            // startFirmwareUpdateButton
            // 
            this.startFirmwareUpdateButton.Location = new System.Drawing.Point(526, 13);
            this.startFirmwareUpdateButton.Name = "startFirmwareUpdateButton";
            this.startFirmwareUpdateButton.Size = new System.Drawing.Size(169, 23);
            this.startFirmwareUpdateButton.TabIndex = 11;
            this.startFirmwareUpdateButton.Text = "Start Firmware Update";
            this.startFirmwareUpdateButton.UseVisualStyleBackColor = true;
            this.startFirmwareUpdateButton.Click += new System.EventHandler(this.startFirmwareUpdateButton_Click);
            // 
            // waitDeviceBootBackgroundWorker
            // 
            this.waitDeviceBootBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.waitDeviceBootBackgroundWorker_DoWork);
            this.waitDeviceBootBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.waitDeviceBootBackgroundWorker_ProgressChanged);
            this.waitDeviceBootBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.waitDeviceBootBackgroundWorker_RunWorkerCompleted);
            // 
            // FirmwareUpdateBackgroundWorker
            // 
            this.FirmwareUpdateBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.FirmwareUpdateBackgroundWorker_DoWork);
            this.FirmwareUpdateBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.FirmwareUpdateBackgroundWorker_ProgressChanged);
            this.FirmwareUpdateBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.FirmwareUpdateBackgroundWorker_RunWorkerCompleted);
            // 
            // waitDeviceAPPBackgroundWorker
            // 
            this.waitDeviceAPPBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.waitDeviceAPPBackgroundWorker_DoWork);
            this.waitDeviceAPPBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.waitDeviceAPPBackgroundWorker_ProgressChanged);
            this.waitDeviceAPPBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.waitDeviceAPPBackgroundWorker_RunWorkerCompleted);
            // 
            // totalStausProgressBar
            // 
            this.totalStausProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.totalStausProgressBar.Location = new System.Drawing.Point(11, 141);
            this.totalStausProgressBar.MarqueeAnimationSpeed = 1;
            this.totalStausProgressBar.Maximum = 10;
            this.totalStausProgressBar.Name = "totalStausProgressBar";
            this.totalStausProgressBar.Size = new System.Drawing.Size(684, 23);
            this.totalStausProgressBar.Step = 1;
            this.totalStausProgressBar.TabIndex = 12;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(713, 170);
            this.Controls.Add(this.totalStausProgressBar);
            this.Controls.Add(this.startFirmwareUpdateButton);
            this.Controls.Add(this.getDeviceInfoButton);
            this.Controls.Add(this.generalPurposeProgressBar);
            this.Controls.Add(this.searchDevicesButton);
            this.Controls.Add(this.statusTextLabel);
            this.Controls.Add(this.StatusTitleLabel);
            this.Controls.Add(this.ipAddressTitleLabel);
            this.Controls.Add(this.ipAddressTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Compact Controller Slave Firmware Updater";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox ipAddressTextBox;
        private System.Windows.Forms.Label ipAddressTitleLabel;
        private System.Windows.Forms.Label StatusTitleLabel;
        private System.Windows.Forms.Label statusTextLabel;
        private System.Windows.Forms.Button searchDevicesButton;
        private System.Windows.Forms.ProgressBar generalPurposeProgressBar;
        private System.Windows.Forms.Button getDeviceInfoButton;
        private System.Windows.Forms.Button startFirmwareUpdateButton;
        private System.ComponentModel.BackgroundWorker waitDeviceBootBackgroundWorker;
        private System.ComponentModel.BackgroundWorker FirmwareUpdateBackgroundWorker;
        private System.ComponentModel.BackgroundWorker waitDeviceAPPBackgroundWorker;
        private System.Windows.Forms.ProgressBar totalStausProgressBar;
    }
}

