
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
            this.totalStatusProgressBar = new System.Windows.Forms.ProgressBar();
            this.hwUnitSelectGroupBox = new System.Windows.Forms.GroupBox();
            this.netxRadioButton = new System.Windows.Forms.RadioButton();
            this.auxMCURadioButton = new System.Windows.Forms.RadioButton();
            this.hostMCURadioButton = new System.Windows.Forms.RadioButton();
            this.operClassSelectGroupBox = new System.Windows.Forms.GroupBox();
            this.opc_AllRadioButton = new System.Windows.Forms.RadioButton();
            this.opc_ModuleRadioButton = new System.Windows.Forms.RadioButton();
            this.opc_Licence_RadioButton = new System.Windows.Forms.RadioButton();
            this.opc_CalibrationRadioButton = new System.Windows.Forms.RadioButton();
            this.opc_ConfigBLRadioButton = new System.Windows.Forms.RadioButton();
            this.opc_ConfigRadioButton = new System.Windows.Forms.RadioButton();
            this.opc_BootloaderRadioButton = new System.Windows.Forms.RadioButton();
            this.opc_FirmwareRadioButton = new System.Windows.Forms.RadioButton();
            this.changeProgressBarValueBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.CommandRunnerBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.hwUnitSelectGroupBox.SuspendLayout();
            this.operClassSelectGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Location = new System.Drawing.Point(75, 14);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(191, 20);
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
            this.searchDevicesButton.Location = new System.Drawing.Point(532, 261);
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
            this.generalPurposeProgressBar.Location = new System.Drawing.Point(11, 319);
            this.generalPurposeProgressBar.MarqueeAnimationSpeed = 1;
            this.generalPurposeProgressBar.Maximum = 10;
            this.generalPurposeProgressBar.Name = "generalPurposeProgressBar";
            this.generalPurposeProgressBar.Size = new System.Drawing.Size(690, 23);
            this.generalPurposeProgressBar.Step = 1;
            this.generalPurposeProgressBar.TabIndex = 7;
            // 
            // getDeviceInfoButton
            // 
            this.getDeviceInfoButton.Location = new System.Drawing.Point(532, 290);
            this.getDeviceInfoButton.Name = "getDeviceInfoButton";
            this.getDeviceInfoButton.Size = new System.Drawing.Size(169, 23);
            this.getDeviceInfoButton.TabIndex = 8;
            this.getDeviceInfoButton.Text = "Get Device Info";
            this.getDeviceInfoButton.UseVisualStyleBackColor = true;
            this.getDeviceInfoButton.Click += new System.EventHandler(this.getDeviceInfoButton_Click);
            // 
            // startFirmwareUpdateButton
            // 
            this.startFirmwareUpdateButton.Location = new System.Drawing.Point(532, 13);
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
            // totalStatusProgressBar
            // 
            this.totalStatusProgressBar.BackColor = System.Drawing.SystemColors.Control;
            this.totalStatusProgressBar.Location = new System.Drawing.Point(11, 348);
            this.totalStatusProgressBar.MarqueeAnimationSpeed = 1;
            this.totalStatusProgressBar.Maximum = 10;
            this.totalStatusProgressBar.Name = "totalStatusProgressBar";
            this.totalStatusProgressBar.Size = new System.Drawing.Size(690, 23);
            this.totalStatusProgressBar.Step = 1;
            this.totalStatusProgressBar.TabIndex = 12;
            // 
            // hwUnitSelectGroupBox
            // 
            this.hwUnitSelectGroupBox.Controls.Add(this.netxRadioButton);
            this.hwUnitSelectGroupBox.Controls.Add(this.auxMCURadioButton);
            this.hwUnitSelectGroupBox.Controls.Add(this.hostMCURadioButton);
            this.hwUnitSelectGroupBox.Location = new System.Drawing.Point(370, 12);
            this.hwUnitSelectGroupBox.Name = "hwUnitSelectGroupBox";
            this.hwUnitSelectGroupBox.Size = new System.Drawing.Size(156, 92);
            this.hwUnitSelectGroupBox.TabIndex = 13;
            this.hwUnitSelectGroupBox.TabStop = false;
            this.hwUnitSelectGroupBox.Text = "Hardware Unit";
            // 
            // netxRadioButton
            // 
            this.netxRadioButton.AutoSize = true;
            this.netxRadioButton.Location = new System.Drawing.Point(6, 65);
            this.netxRadioButton.Name = "netxRadioButton";
            this.netxRadioButton.Size = new System.Drawing.Size(49, 17);
            this.netxRadioButton.TabIndex = 2;
            this.netxRadioButton.Text = "NetX";
            this.netxRadioButton.UseVisualStyleBackColor = true;
            this.netxRadioButton.CheckedChanged += new System.EventHandler(this.netxRadioButton_CheckedChanged);
            // 
            // auxMCURadioButton
            // 
            this.auxMCURadioButton.AutoSize = true;
            this.auxMCURadioButton.Location = new System.Drawing.Point(6, 42);
            this.auxMCURadioButton.Name = "auxMCURadioButton";
            this.auxMCURadioButton.Size = new System.Drawing.Size(73, 17);
            this.auxMCURadioButton.TabIndex = 1;
            this.auxMCURadioButton.Text = "Aux. MCU";
            this.auxMCURadioButton.UseVisualStyleBackColor = true;
            this.auxMCURadioButton.CheckedChanged += new System.EventHandler(this.auxMCURadioButton_CheckedChanged);
            // 
            // hostMCURadioButton
            // 
            this.hostMCURadioButton.AutoSize = true;
            this.hostMCURadioButton.Checked = true;
            this.hostMCURadioButton.Location = new System.Drawing.Point(6, 19);
            this.hostMCURadioButton.Name = "hostMCURadioButton";
            this.hostMCURadioButton.Size = new System.Drawing.Size(74, 17);
            this.hostMCURadioButton.TabIndex = 0;
            this.hostMCURadioButton.TabStop = true;
            this.hostMCURadioButton.Text = "Host MCU";
            this.hostMCURadioButton.UseVisualStyleBackColor = true;
            this.hostMCURadioButton.CheckedChanged += new System.EventHandler(this.hostMCURadioButton_CheckedChanged);
            // 
            // operClassSelectGroupBox
            // 
            this.operClassSelectGroupBox.Controls.Add(this.opc_AllRadioButton);
            this.operClassSelectGroupBox.Controls.Add(this.opc_ModuleRadioButton);
            this.operClassSelectGroupBox.Controls.Add(this.opc_Licence_RadioButton);
            this.operClassSelectGroupBox.Controls.Add(this.opc_CalibrationRadioButton);
            this.operClassSelectGroupBox.Controls.Add(this.opc_ConfigBLRadioButton);
            this.operClassSelectGroupBox.Controls.Add(this.opc_ConfigRadioButton);
            this.operClassSelectGroupBox.Controls.Add(this.opc_BootloaderRadioButton);
            this.operClassSelectGroupBox.Controls.Add(this.opc_FirmwareRadioButton);
            this.operClassSelectGroupBox.Location = new System.Drawing.Point(370, 110);
            this.operClassSelectGroupBox.Name = "operClassSelectGroupBox";
            this.operClassSelectGroupBox.Size = new System.Drawing.Size(156, 203);
            this.operClassSelectGroupBox.TabIndex = 14;
            this.operClassSelectGroupBox.TabStop = false;
            this.operClassSelectGroupBox.Text = "Operation Class";
            // 
            // opc_AllRadioButton
            // 
            this.opc_AllRadioButton.AutoSize = true;
            this.opc_AllRadioButton.Enabled = false;
            this.opc_AllRadioButton.Location = new System.Drawing.Point(6, 180);
            this.opc_AllRadioButton.Name = "opc_AllRadioButton";
            this.opc_AllRadioButton.Size = new System.Drawing.Size(36, 17);
            this.opc_AllRadioButton.TabIndex = 7;
            this.opc_AllRadioButton.Text = "All";
            this.opc_AllRadioButton.UseVisualStyleBackColor = true;
            // 
            // opc_ModuleRadioButton
            // 
            this.opc_ModuleRadioButton.AutoSize = true;
            this.opc_ModuleRadioButton.Location = new System.Drawing.Point(6, 157);
            this.opc_ModuleRadioButton.Name = "opc_ModuleRadioButton";
            this.opc_ModuleRadioButton.Size = new System.Drawing.Size(60, 17);
            this.opc_ModuleRadioButton.TabIndex = 6;
            this.opc_ModuleRadioButton.Text = "Module";
            this.opc_ModuleRadioButton.UseVisualStyleBackColor = true;
            this.opc_ModuleRadioButton.CheckedChanged += new System.EventHandler(this.opc_ModuleRadioButton_CheckedChanged);
            // 
            // opc_Licence_RadioButton
            // 
            this.opc_Licence_RadioButton.AutoSize = true;
            this.opc_Licence_RadioButton.Location = new System.Drawing.Point(6, 134);
            this.opc_Licence_RadioButton.Name = "opc_Licence_RadioButton";
            this.opc_Licence_RadioButton.Size = new System.Drawing.Size(82, 17);
            this.opc_Licence_RadioButton.TabIndex = 5;
            this.opc_Licence_RadioButton.Text = "Licence File";
            this.opc_Licence_RadioButton.UseVisualStyleBackColor = true;
            this.opc_Licence_RadioButton.CheckedChanged += new System.EventHandler(this.opc_Licence_RadioButton_CheckedChanged);
            // 
            // opc_CalibrationRadioButton
            // 
            this.opc_CalibrationRadioButton.AutoSize = true;
            this.opc_CalibrationRadioButton.Location = new System.Drawing.Point(6, 111);
            this.opc_CalibrationRadioButton.Name = "opc_CalibrationRadioButton";
            this.opc_CalibrationRadioButton.Size = new System.Drawing.Size(93, 17);
            this.opc_CalibrationRadioButton.TabIndex = 4;
            this.opc_CalibrationRadioButton.Text = "Calibration File";
            this.opc_CalibrationRadioButton.UseVisualStyleBackColor = true;
            this.opc_CalibrationRadioButton.CheckedChanged += new System.EventHandler(this.opc_CalibrationRadioButton_CheckedChanged);
            // 
            // opc_ConfigBLRadioButton
            // 
            this.opc_ConfigBLRadioButton.AutoSize = true;
            this.opc_ConfigBLRadioButton.Location = new System.Drawing.Point(6, 88);
            this.opc_ConfigBLRadioButton.Name = "opc_ConfigBLRadioButton";
            this.opc_ConfigBLRadioButton.Size = new System.Drawing.Size(103, 17);
            this.opc_ConfigBLRadioButton.TabIndex = 3;
            this.opc_ConfigBLRadioButton.Text = "Configuration BL";
            this.opc_ConfigBLRadioButton.UseVisualStyleBackColor = true;
            this.opc_ConfigBLRadioButton.CheckedChanged += new System.EventHandler(this.opc_ConfigBLRadioButton_CheckedChanged);
            // 
            // opc_ConfigRadioButton
            // 
            this.opc_ConfigRadioButton.AutoSize = true;
            this.opc_ConfigRadioButton.Location = new System.Drawing.Point(6, 65);
            this.opc_ConfigRadioButton.Name = "opc_ConfigRadioButton";
            this.opc_ConfigRadioButton.Size = new System.Drawing.Size(87, 17);
            this.opc_ConfigRadioButton.TabIndex = 2;
            this.opc_ConfigRadioButton.Text = "Configuration";
            this.opc_ConfigRadioButton.UseVisualStyleBackColor = true;
            this.opc_ConfigRadioButton.CheckedChanged += new System.EventHandler(this.opc_ConfigRadioButton_CheckedChanged);
            // 
            // opc_BootloaderRadioButton
            // 
            this.opc_BootloaderRadioButton.AutoSize = true;
            this.opc_BootloaderRadioButton.Location = new System.Drawing.Point(6, 42);
            this.opc_BootloaderRadioButton.Name = "opc_BootloaderRadioButton";
            this.opc_BootloaderRadioButton.Size = new System.Drawing.Size(76, 17);
            this.opc_BootloaderRadioButton.TabIndex = 1;
            this.opc_BootloaderRadioButton.Text = "Bootloader";
            this.opc_BootloaderRadioButton.UseVisualStyleBackColor = true;
            this.opc_BootloaderRadioButton.CheckedChanged += new System.EventHandler(this.opc_BootloaderRadioButton_CheckedChanged);
            // 
            // opc_FirmwareRadioButton
            // 
            this.opc_FirmwareRadioButton.AutoSize = true;
            this.opc_FirmwareRadioButton.Checked = true;
            this.opc_FirmwareRadioButton.Location = new System.Drawing.Point(6, 19);
            this.opc_FirmwareRadioButton.Name = "opc_FirmwareRadioButton";
            this.opc_FirmwareRadioButton.Size = new System.Drawing.Size(67, 17);
            this.opc_FirmwareRadioButton.TabIndex = 0;
            this.opc_FirmwareRadioButton.TabStop = true;
            this.opc_FirmwareRadioButton.Text = "Firmware";
            this.opc_FirmwareRadioButton.UseVisualStyleBackColor = true;
            this.opc_FirmwareRadioButton.CheckedChanged += new System.EventHandler(this.opc_FirmwareRadioButton_CheckedChanged);
            // 
            // changeProgressBarValueBackgroundWorker
            // 
            this.changeProgressBarValueBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.changeProgressBarValueBackgroundWorker_DoWork);
            this.changeProgressBarValueBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.changeProgressBarValueBackgroundWorker_ProgressChanged);
            this.changeProgressBarValueBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.changeProgressBarValueBackgroundWorker_RunWorkerCompleted);
            // 
            // CommandRunnerBackgroundWorker
            // 
            this.CommandRunnerBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CommandRunnerBackgroundWorker_DoWork);
            this.CommandRunnerBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.CommandRunnerBackgroundWorker_ProgressChanged);
            this.CommandRunnerBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.CommandRunnerBackgroundWorker_RunWorkerCompleted);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(713, 379);
            this.Controls.Add(this.operClassSelectGroupBox);
            this.Controls.Add(this.hwUnitSelectGroupBox);
            this.Controls.Add(this.totalStatusProgressBar);
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
            this.hwUnitSelectGroupBox.ResumeLayout(false);
            this.hwUnitSelectGroupBox.PerformLayout();
            this.operClassSelectGroupBox.ResumeLayout(false);
            this.operClassSelectGroupBox.PerformLayout();
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
        private System.Windows.Forms.ProgressBar totalStatusProgressBar;
        private System.Windows.Forms.GroupBox hwUnitSelectGroupBox;
        private System.Windows.Forms.RadioButton netxRadioButton;
        private System.Windows.Forms.RadioButton auxMCURadioButton;
        private System.Windows.Forms.RadioButton hostMCURadioButton;
        private System.Windows.Forms.GroupBox operClassSelectGroupBox;
        private System.Windows.Forms.RadioButton opc_ConfigRadioButton;
        private System.Windows.Forms.RadioButton opc_BootloaderRadioButton;
        private System.Windows.Forms.RadioButton opc_FirmwareRadioButton;
        private System.Windows.Forms.RadioButton opc_AllRadioButton;
        private System.Windows.Forms.RadioButton opc_ModuleRadioButton;
        private System.Windows.Forms.RadioButton opc_Licence_RadioButton;
        private System.Windows.Forms.RadioButton opc_CalibrationRadioButton;
        private System.Windows.Forms.RadioButton opc_ConfigBLRadioButton;
        private System.ComponentModel.BackgroundWorker changeProgressBarValueBackgroundWorker;
        private System.ComponentModel.BackgroundWorker CommandRunnerBackgroundWorker;
    }
}

