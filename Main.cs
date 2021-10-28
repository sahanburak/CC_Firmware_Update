using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public Main()
        {
            InitializeComponent();
            waitDeviceBootBackgroundWorker.WorkerReportsProgress = true; 
            waitDeviceBootBackgroundWorker.WorkerSupportsCancellation = true;
            FirmwareUpdateBackgroundWorker.WorkerReportsProgress = true;
            FirmwareUpdateBackgroundWorker.WorkerSupportsCancellation = true;
            waitDeviceAPPBackgroundWorker.WorkerReportsProgress = true;
            waitDeviceAPPBackgroundWorker.WorkerSupportsCancellation = true;
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
            totalStatusProgressBar.Value = 0;
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Non Crypted Firmware|*.bin| Crypted Firmware |*.enc.bin" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog.FileName;
                }
                else {
                    return;
                }
            }
            totalStatusProgressBar.Value += 5;
            devIPAddress = ipAddressTextBox.Text;
            if (!connectAction(devIPAddress, CMD_PORT))
            {
                MessageBox.Show("Connection failure !!!");
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
                        ret = runModeChangeCommand.changeDeviceRunMode((UInt16)RT_Commands.eHWUnit.HW_UNIT_HOST_MCU, (UInt16)RT_Commands.eRunMode.RUNMODE_INIT);
                        totalStatusProgressBar.Value = 5;
                    }
                    else 
                    {
                        ret = (stat.SwLevel == (UInt16)RT_Commands.eSwLevel.SWLEVEL_BOOTLOADER) ? true : false;
                    }
                    
                }
                else
                {
                    MessageBox.Show("Device is in unknown mode.");

                }
            }
            else {
                MessageBox.Show("Device run mode is not recognized.");
            }

            if (ret) {
                RT_OperationCommand operationCmd = new RT_OperationCommand();
                ret = operationCmd.doOperationCommand((UInt16)RT_OperationCommand.eOperationClass.OPC_FIRMWARE, (UInt16)RT_OperationCommand.eOperationType.OPT_DOWNLOAD, (UInt16)RT_Commands.eHWUnit.HW_UNIT_HOST_MCU);
                if (ret)
                {
                    if (Program.isConnected) {
                        Program.Disconnect();
                        waitDeviceBootBackgroundWorker.RunWorkerAsync(devBootLoaderIPaddress);
                    }
                }
                else {
                    if (Program.isConnected)
                    {
                        disconnectAction();
                    }
                    MessageBox.Show("Update not started.");
                }
            }
            else {
                if (Program.isConnected)
                {
                    disconnectAction();
                }
                MessageBox.Show("Update mode not switchable.");
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
                firmwareUpdateStartCommand.startFirmwareUpdate((byte)RT_Commands.eHWUnit.HW_UNIT_HOST_MCU, (byte)RT_Commands.eFWType.DOWNLOAD_FW_WITH_RST, (UInt32)fsize);
                if (RT_FirmwareUpdateStartCommand.isUpdateStartable)
                {
                    firmwareFile = File.ReadAllBytes(fileName);// new StreamReader(fileName);
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
            }
            else if (e.Error != null)
            {
                statusTextLabel.Text = "Hata: " + e.Error.Message;
            }
            else
            {
                statusTextLabel.Text = statusMsg;
                disconnectAction();
            }
        }
    }
}
