using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CC_Firmware_Update
{
    public partial class Main : Form
    {
        String statusMsg;
        RT_FirmwareUpdateCommand firmwareUpdateCommand;
        RT_FirmwareUpdateStartCommand firmwareUpdateStartCommand;
        byte[] firmwareFile;
        String fileName;
        String devIPAddress;
        String devBootLoaderIPaddress = "10.1.11.209";
        static int CMD_PORT = 1235;
        static int UPDATE_PORT = 1237;
        static int startIP = 205;
        static int endIP = 215;
        UInt16 uHwUnit = (UInt16)RT_Commands.eHWUnit.HW_UNIT_HOST_MCU;
        UInt16 uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_FIRMWARE;
        System.Diagnostics.Process p;
        int generalPurposeProgressBarVal = 0;
        int totalStatusProgressBarVal = 0;
        bool isProcessEnd = false;
        public Main()
        {
            InitializeComponent();
            waitDeviceBootBackgroundWorker.WorkerReportsProgress = true; 
            waitDeviceBootBackgroundWorker.WorkerSupportsCancellation = true;

            FirmwareUpdateBackgroundWorker.WorkerReportsProgress = true;
            FirmwareUpdateBackgroundWorker.WorkerSupportsCancellation = true;

            waitDeviceAPPBackgroundWorker.WorkerReportsProgress = true;
            waitDeviceAPPBackgroundWorker.WorkerSupportsCancellation = true;

            changeProgressBarValueBackgroundWorker.WorkerReportsProgress = true;
            changeProgressBarValueBackgroundWorker.WorkerSupportsCancellation = true;

            CommandRunnerBackgroundWorker.WorkerReportsProgress = true;
            CommandRunnerBackgroundWorker.WorkerSupportsCancellation = true;

            firmwareUpdateCommand = new RT_FirmwareUpdateCommand();
            firmwareUpdateStartCommand = new RT_FirmwareUpdateStartCommand();
            generalPurposeProgressBar.Minimum = 0;
            generalPurposeProgressBar.Maximum = 100;
            generalPurposeProgressBar.Step = 1;
            totalStatusProgressBar.Minimum = 0;
            totalStatusProgressBar.Maximum = 100;
            totalStatusProgressBar.Step = 1;
        }


        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void searchDevicesButton_Click(object sender, EventArgs e)
        {
            statusTextLabel.Text = "";
            searchDevicesButton.Enabled = false;
            List<String> devices = GetAvailableDevices();
            if (devices.Count > 0)
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    statusTextLabel.Text += devices[i] + "\n\r";
                }
            }
            searchDevicesButton.Enabled = true;
        }


        private List<String> GetAvailableDevices()
        {
            List<String> availableDevices = new List<String>();
            Ping p = new System.Net.NetworkInformation.Ping();
            generalPurposeProgressBar.Value = 0;
            for (int i = startIP; i < endIP; i++)
            {
                String ipAddress = "10.1.11." + i;
                generalPurposeProgressBar.Value = (endIP-startIP)*(i-startIP+1);
                PingReply reply = p.Send(ipAddress);
                if (reply.Status.ToString().Equals("Success"))
                {
                    availableDevices.Add(ipAddress);
                    Console.WriteLine("It is available device: " + ipAddress);
                }
                else
                {
                    Console.WriteLine("It is not available device: " + ipAddress);
                }
            }
            return availableDevices;
        }

        private void getDeviceInfoButton_Click(object sender, EventArgs e)
        {
            devIPAddress = ipAddressTextBox.Text;
            connectAction(devIPAddress, CMD_PORT);
            RT_DeviceInfoCommand   deviceInfoCommand = new RT_DeviceInfoCommand();
            String infoStr = deviceInfoCommand.getDeviceInfo();
            if (!infoStr.Equals(""))
            {
                MessageBox.Show(infoStr);
            }
            else {
                MessageBox.Show("Device Info read Error !!!");
            }
            disconnectAction();
        }

        private void startFirmwareUpdateButton_Click(object sender, EventArgs e)
        {
            switch (uHwUnit) {
                case (UInt16)RT_Commands.eHWUnit.HW_UNIT_HOST_MCU:
                    switch (uOperClass)
                    {
                        case (UInt16)RT_OperationCommand.eOperationClass.OPC_FIRMWARE:
                            HostMCU_FirmwareUpdate();
                            break;
                        case (UInt16)RT_OperationCommand.eOperationClass.OPC_BOOTLOADER:                        
                            HostMCU_BootloaderUpdate();
                            break;
                    }
                    break;
                case (UInt16)RT_Commands.eHWUnit.HW_UNIT_STM32:
                    
                    break;
                case (UInt16)RT_Commands.eHWUnit.HW_UNIT_NETX:

                    break;
                default:
                    break;
            }
        }

        public void ConnectionUIAction(Boolean status) {
            getDeviceInfoButton.Enabled = !status;
            searchDevicesButton.Enabled = !status;
        }
        public void disconnectAction() {
            Program.Disconnect();
            statusTextLabel.Text = "Disconnected";
            ConnectionUIAction(Program.isConnected);
            startFirmwareUpdateButton.Enabled = true;
        }

        public bool connectAction(String ipAddr, int port) {
            String statusMsg = Program.CheckAccess(ipAddr);
            if (statusMsg.Equals("Success"))
            {
                Boolean status = Program.Connect(ipAddr, port);
                if (status)
                {
                    statusMsg = "Connected to " + ipAddr + ":" + port;
                }
                else
                {
                    statusMsg = "Connection failure !!!";
                }
                ConnectionUIAction(Program.isConnected);
                if (!Program.isConnected)
                    MessageBox.Show(statusMsg);
                statusTextLabel.Text = statusMsg;
                return status;
            }
            return false;
        }



        private void waitDeviceBootBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    String ipAddr = e.Argument.ToString();
                    worker.ReportProgress(i*10);
                    statusMsg = Program.CheckAccess(ipAddr);
                    if (statusMsg.Equals("Success"))
                    {
                        Boolean status = Program.Connect(ipAddr, UPDATE_PORT);
                        if (status)
                        {
                            statusMsg = "Connected to "+ ipAddr+":"+ UPDATE_PORT;
                            waitDeviceBootBackgroundWorker.CancelAsync();
                        }
                        else
                        {
                            statusMsg = "Connection failure !!!";
                        }
                    }
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        
        private void waitDeviceBootBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusTextLabel.Text = "Waiting device for boot...";
            generalPurposeProgressBar.Value = e.ProgressPercentage;
            totalStatusProgressBar.Value = 10 + (e.ProgressPercentage / 5);
        }

       
        private void waitDeviceBootBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                statusTextLabel.Text = "Device in boot mode.";
                totalStatusProgressBar.Value = 30;
                generalPurposeProgressBar.Value = generalPurposeProgressBar.Maximum;
                ConnectionUIAction(Program.isConnected);
                FileInfo fileInfo = new FileInfo(fileName);
                long fsize = fileInfo.Length;
                firmwareUpdateStartCommand.startFirmwareUpdate((byte)uHwUnit, (byte)RT_Commands.eFWType.DOWNLOAD_FW_WITH_RST, (UInt32)fsize);
                if (RT_FirmwareUpdateStartCommand.isUpdateStartable)
                {
                    firmwareFile = File.ReadAllBytes(fileName);
                    Console.WriteLine("Array Len: {0:} Fsize: {0:}", firmwareFile.Length, fsize);
                    FirmwareUpdateBackgroundWorker.RunWorkerAsync(fsize);
                }
                else
                {
                    MessageBox.Show("Device nor ready for update mode.");
                }
            }
            else if (e.Error != null)
            {
                statusTextLabel.Text = "Hata: " + e.Error.Message;
                totalStatusProgressBar.Value = 0 ;
            }
            else
            {
                statusTextLabel.Text = statusMsg;
                ConnectionUIAction(Program.isConnected);
                totalStatusProgressBar.Value = 0;
            }
        }

        private void FirmwareUpdateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
           long fsize = (long) e.Argument;
            int loopSize = (int)fsize / RT_FirmwareUpdateCommand.APP_COMMS_MAX_MSG_SIZE;
            int lastPacketSize = (int)fsize % RT_FirmwareUpdateCommand.APP_COMMS_MAX_MSG_SIZE;
            if(lastPacketSize > 0)
            {
                loopSize += 1;
            }

            int startIndex = 0;
            int endIndex = 0;
            for (int i = 0; i < loopSize; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    Console.WriteLine("Progresss: {0:} {1:}/{2:}", ((i + 1) * 100 / loopSize),(i+1),loopSize);
                    worker.ReportProgress((i+1)*100/loopSize);   
                    if(i == (loopSize - 1)) { 
                        if (lastPacketSize > 0)
                        {
                            startIndex = endIndex;
                            endIndex += lastPacketSize;
                            byte[] data = Utils.ByteSlicer(firmwareFile, startIndex, endIndex);
                            firmwareUpdateCommand.firmwareUpdate((UInt16)(loopSize - i - 1), (UInt16)(i + 1), data);
                            Console.WriteLine("Waiting Packet Num: {0:} Packet Num: {1:} Start Index: {2:} End Index: {3:}", (loopSize - i -1), (i + 1), startIndex, endIndex);
                        }
                        else {
                            startIndex = endIndex;
                            endIndex += RT_FirmwareUpdateCommand.APP_COMMS_MAX_MSG_SIZE;
                            byte[] data = Utils.ByteSlicer(firmwareFile, startIndex, endIndex);
                            firmwareUpdateCommand.firmwareUpdate((UInt16)(loopSize - i - 1), (UInt16)(i + 1), data);
                            Console.WriteLine("Waiting Packet Num: {0:} Packet Num: {1:} Start Index: {2:} End Index: {3:}", (loopSize - i - 1), (i + 1), startIndex, endIndex);
                        }
                    }
                    else
                    {
                        startIndex = endIndex;
                        endIndex += RT_FirmwareUpdateCommand.APP_COMMS_MAX_MSG_SIZE;
                        byte[] data = Utils.ByteSlicer(firmwareFile, startIndex, endIndex);
                        firmwareUpdateCommand.firmwareUpdate((UInt16)(loopSize - i - 1), (UInt16)(i + 1), data);
                        Console.WriteLine("Waiting Packet Num: {0:} Packet Num: {1:} Start Index: {2:} End Index: {3:}", (loopSize - i - 1), (i + 1), startIndex, endIndex);
                    }
                    System.Threading.Thread.Sleep(10);

                }
            }
        }

        private void FirmwareUpdateBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusTextLabel.Text = "Firmware sending.";
            generalPurposeProgressBar.Value = e.ProgressPercentage;
            totalStatusProgressBar.Value = 30+(e.ProgressPercentage / 2);
        }

        private void FirmwareUpdateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                statusTextLabel.Text = statusMsg;
                totalStatusProgressBar.Value = 0;
            }
            else if (e.Error != null)
            {
                statusTextLabel.Text = "Hata: " + e.Error.Message;
                totalStatusProgressBar.Value = 0 ;
            }
            else
            {
                statusTextLabel.Text = statusMsg;
                totalStatusProgressBar.Value = 80;
                waitDeviceAPPBackgroundWorker.RunWorkerAsync(devIPAddress);
            }
            Program.Disconnect();
        }

        private void waitDeviceAPPBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    String ipAddr = e.Argument.ToString();
                    worker.ReportProgress(i * 10);
                    statusMsg = Program.CheckAccess(ipAddr);
                    if (statusMsg.Equals("Success"))
                    {
                        Boolean status = Program.Connect(ipAddr, CMD_PORT);
                        if (status)
                        {
                            statusMsg = "Connected to " + ipAddr + ":" + CMD_PORT;
                            waitDeviceAPPBackgroundWorker.CancelAsync();
                        }
                        else
                        {
                            statusMsg = "Connection failure !!!";
                        }
                    }
                    System.Threading.Thread.Sleep(1500);
                }
            }
        }

        private void waitDeviceAPPBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusTextLabel.Text = "Waiting device for firmware write operation...";
            generalPurposeProgressBar.Value = e.ProgressPercentage;
            totalStatusProgressBar.Value = 80 + (e.ProgressPercentage / 5);
        }

        private void waitDeviceAPPBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                disconnectAction();
                statusTextLabel.Text = "Device updated.";
                totalStatusProgressBar.Value = 100;
                generalPurposeProgressBar.Value = generalPurposeProgressBar.Maximum;
                startFirmwareUpdateButton.Enabled = true;
            }
            else if (e.Error != null)
            {
                statusTextLabel.Text = "Hata: " + e.Error.Message;
            }
            else
            {
                statusTextLabel.Text = statusMsg;
                disconnectAction();
                startFirmwareUpdateButton.Enabled = true;
            }
        }


        private void changeProgressBarValueBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (totalStatusProgressBar.Value != totalStatusProgressBar.Maximum) {

                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    int value = totalStatusProgressBarVal * 1000 + generalPurposeProgressBarVal;
                    worker.ReportProgress(value);
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        private void changeProgressBarValueBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusTextLabel.Text = "Waiting bootloader update...";
            generalPurposeProgressBar.Value = (e.ProgressPercentage % 1000);
            totalStatusProgressBar.Value = (e.ProgressPercentage / 1000);
            Console.WriteLine("sub task: "+ generalPurposeProgressBar.Value+"/100 main task: "+ totalStatusProgressBar.Value+"/100");
        }

        private void changeProgressBarValueBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                statusTextLabel.Text = "Bootloader update process error.";
                generalPurposeProgressBar.Value = generalPurposeProgressBar.Minimum;
                totalStatusProgressBar.Value = totalStatusProgressBar.Minimum;
            }
            else if (e.Error != null)
            {
                statusTextLabel.Text = "Hata: " + e.Error.Message;
            }
            else
            {
                statusTextLabel.Text = "Bootloader updated.";
                generalPurposeProgressBar.Value = generalPurposeProgressBar.Maximum;
                totalStatusProgressBar.Value = totalStatusProgressBar.Maximum;
            }
            startFirmwareUpdateButton.Enabled = true;
        }


        private void CommandRunnerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            generalPurposeProgressBarVal = 0;
            BackgroundWorker worker = sender as BackgroundWorker;
            p = new System.Diagnostics.Process();
            try
            {
                {
                    generalPurposeProgressBarVal += 5;
                    totalStatusProgressBarVal += 5;
                    worker.ReportProgress(25);
                    // set start info
                    p.StartInfo = new ProcessStartInfo(@"C:\Program Files (x86)\Atmel\Studio\7.0\atbackend\atprogram.exe")
                    {
                        RedirectStandardInput = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        Arguments = e.Argument.ToString()
                };

                    Console.WriteLine("Command running: "+e.Argument.ToString());
                    // start process
                    p.Start();
                    worker.ReportProgress(50);                    
                    totalStatusProgressBarVal += 25;
                    p.OutputDataReceived += (p_OutputDataReceived);
                    p.ErrorDataReceived += p_ErrorDataReceived;
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    //wait
                    p.WaitForExit();
                    p.Close();
                    worker.ReportProgress(100);
                    p = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void CommandRunnerBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        private void CommandRunnerBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Console.WriteLine("Bootloader updated with cancelled.");
            }
            else if (e.Error != null)
            {
                Console.WriteLine("Hata: " + e.Error.Message);
            }
            else
            {
                Console.WriteLine("Bootloader updated.");
            }
            if (!isProcessEnd)
            {
                generalPurposeProgressBarVal = 0;
                CommandRunnerBackgroundWorker.RunWorkerAsync(@"-t atmelice -i swd -d ATSAME70Q21B program -f " + fileName + " --verify");
                isProcessEnd = true;
            }
        }

        private void hostMCURadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (hostMCURadioButton.Checked) {
                uHwUnit = (UInt16)RT_Commands.eHWUnit.HW_UNIT_HOST_MCU;
            }
            else {
                uHwUnit = (UInt16)RT_Commands.eHWUnit.HW_UNIT_NONE;
            }
        }

        private void auxMCURadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (auxMCURadioButton.Checked)
            {
                uHwUnit = (UInt16)RT_Commands.eHWUnit.HW_UNIT_STM32;
            }
            else
            {
                uHwUnit = (UInt16)RT_Commands.eHWUnit.HW_UNIT_NONE;
            }
        }

        private void netxRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (netxRadioButton.Checked)
            {
                uHwUnit = (UInt16)RT_Commands.eHWUnit.HW_UNIT_NETX;
            }
            else
            {
                uHwUnit = (UInt16)RT_Commands.eHWUnit.HW_UNIT_NONE;
            }
        }

        private void opc_FirmwareRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (opc_FirmwareRadioButton.Checked)
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_FIRMWARE;
            }
            else
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_NONE;
            }
        }

        private void opc_BootloaderRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (opc_BootloaderRadioButton.Checked)
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_BOOTLOADER;
            }
            else
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_NONE;
            }
        }

        private void opc_ConfigRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (opc_ConfigRadioButton.Checked)
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_CONFIG;
            }
            else
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_NONE;
            }
        }

        private void opc_ConfigBLRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (opc_ConfigBLRadioButton.Checked)
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_CONFIG_BL;
            }
            else
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_NONE;
            }
        }

        private void opc_CalibrationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (opc_CalibrationRadioButton.Checked)
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_CALIBRATION;
            }
            else
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_NONE;
            }
        }

        private void opc_Licence_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (opc_Licence_RadioButton.Checked)
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_LICENCE;
            }
            else
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_NONE;
            }
        }

        private void opc_ModuleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (opc_ModuleRadioButton.Checked)
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_MODULE;
            }
            else
            {
                uOperClass = (UInt16)RT_OperationCommand.eOperationClass.OPC_NONE;
            }
        }

        void HostMCU_FirmwareUpdate()
        {
            totalStatusProgressBar.Value = 0;
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Non Crypted Firmware|*.bin| Crypted Firmware |*.enc.bin" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog.FileName;
                    startFirmwareUpdateButton.Enabled = false;
                }
                else
                {
                    return;
                }
            }
            totalStatusProgressBar.Value += 5;
            devIPAddress = ipAddressTextBox.Text;
            if (!connectAction(devIPAddress, CMD_PORT))
            {
                MessageBox.Show("Connection failure !!!");
                startFirmwareUpdateButton.Enabled = true;
                return;
            }

            DeviceStatus stat = RT_DeviceInfoCommand.readDeviceInfo();
            Boolean ret = false;
            if (stat.RunMode != (UInt16)RT_Commands.eRunMode.RUNMODE_FW_UPDATE && stat.RunMode != (UInt16)RT_Commands.eRunMode.RUNMODE_CALIBRATION)
            {

                if (stat.SwLevel != (UInt16)RT_Commands.eSwLevel.SWLEVEL_NONE)
                {
                    if (stat.RunMode != (UInt16)RT_Commands.eRunMode.RUNMODE_INIT)
                    {
                        RT_RunModeChangeCommand runModeChangeCommand = new RT_RunModeChangeCommand();
                        ret = runModeChangeCommand.changeDeviceRunMode(uHwUnit, (UInt16)RT_Commands.eRunMode.RUNMODE_INIT);
                        totalStatusProgressBar.Value = 5;
                    }
                    else
                    {
                        ret = (stat.SwLevel == (UInt16)RT_Commands.eSwLevel.SWLEVEL_BOOTLOADER) ? true : false;
                    }

                }
                else
                {
                    startFirmwareUpdateButton.Enabled = true;
                    MessageBox.Show("Device is in unknown mode.");

                }
            }
            else
            {
                startFirmwareUpdateButton.Enabled = true;
                MessageBox.Show("Device run mode is not recognized.");
            }

            if (ret)
            {
                RT_OperationCommand operationCmd = new RT_OperationCommand();
                ret = operationCmd.doOperationCommand(uOperClass, (UInt16)RT_OperationCommand.eOperationType.OPT_DOWNLOAD, uHwUnit);
                if (ret)
                {
                    if (Program.isConnected)
                    {
                        Program.Disconnect();
                        waitDeviceBootBackgroundWorker.RunWorkerAsync(devBootLoaderIPaddress);
                    }
                }
                else
                {
                    if (Program.isConnected)
                    {
                        disconnectAction();
                    }
                    MessageBox.Show("Update not started.");
                }
            }
            else
            {
                if (Program.isConnected)
                {
                    disconnectAction();
                }
                MessageBox.Show("Update mode not switchable.");
            }
        }

        void HostMCU_BootloaderUpdate()
        {
            isProcessEnd = false;
            totalStatusProgressBarVal = 0;
            totalStatusProgressBar.Value = totalStatusProgressBarVal;
            generalPurposeProgressBarVal = 0;
            generalPurposeProgressBar.Value = generalPurposeProgressBarVal;
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Firmware |*.elf" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }


            string[] lines = File.ReadAllLines(fileName);
            bool result = false;
            for (int i=0;i<lines.Length;i++) { 
                result = lines[i].Contains("Bootloader starting...");
                if (result)
                    break;
            }

            if (result)
            {
                startFirmwareUpdateButton.Enabled = false;
                changeProgressBarValueBackgroundWorker.RunWorkerAsync();
                CommandRunnerBackgroundWorker.RunWorkerAsync(@"-t atmelice -i swd -d ATSAME70Q21B chiperase");
            }
            else {
                MessageBox.Show("Please select bootloader firmware");
            }

        }


        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Process p = sender as Process;
            if (p == null)
                return;
            if(e.Data != null)
            {
                if (e.Data.Equals("[ERROR] Could not find tool."))
                {
                    changeProgressBarValueBackgroundWorker.CancelAsync();
                    CommandRunnerBackgroundWorker.CancelAsync();
                    MessageBox.Show("Please check Atmel ICE connection");
                }
                else if (e.Data.Equals("[ERROR] Could not establish connection to device. Please check input parameters, hardware connections, security bit, target power, and clock values."))
                {
                    changeProgressBarValueBackgroundWorker.CancelAsync();
                    CommandRunnerBackgroundWorker.CancelAsync();
                    MessageBox.Show("Could not establish connection to device.");
                }
                else
                {
                    Console.WriteLine("Error Msg: " + e.Data);
                    changeProgressBarValueBackgroundWorker.CancelAsync();
                    CommandRunnerBackgroundWorker.CancelAsync();
                }
            }
            
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Process p = sender as Process;
            if (p == null)
                return;
            if(e.Data != null) 
            {
                if (e.Data.Equals("Firmware check OK"))
                {
                    Console.WriteLine("Firmware OK:  " + e.Data);
                    generalPurposeProgressBarVal += 40;
                }
                else if (e.Data.Equals("Chiperase completed successfully"))
                {
                    Console.WriteLine("Erase ok.:  " + e.Data);
                    generalPurposeProgressBarVal = 100;
                    totalStatusProgressBarVal = 50;
                }
                else if (e.Data.Equals("Programming and verification completed successfully.")) {
                    generalPurposeProgressBarVal = 100;
                    totalStatusProgressBarVal = 100;
                }
                else
                {
                    Console.WriteLine("Data received: " + e.Data);
                    changeProgressBarValueBackgroundWorker.CancelAsync();
                    generalPurposeProgressBarVal = 0;
                }
            }
        }

    }
}
