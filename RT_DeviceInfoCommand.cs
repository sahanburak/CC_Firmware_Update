using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    [Serializable()]
    class RT_DeviceInfoCommand
    {
        private UInt16 uPacketSize = 2;
        private byte bReadOrWrite;
        private byte bIsAdvanced;
        private RotaAdvanceDeviceInfo rotaAdvaceDeviceInfo;
        private RotaDeviceInfo rotaDeviceInfo;

        public UInt16 PacketSize
        {
            get { return uPacketSize; }
            set { this.uPacketSize = value; }
        }

        public byte ReadOrWrite
        {
            get { return bReadOrWrite; }
            set { this.bReadOrWrite = value; }
        }

        public byte IsAdvanced
        {
            get { return this.bIsAdvanced; }
            set { this.bIsAdvanced = value; }
        }

        public byte[] getDeviceInfoReadCommandBody() {
            byte[] cmd = new byte[4];
            byte[] size = BitConverter.GetBytes(this.PacketSize);
            cmd[0] = size[0];
            cmd[1] = size[1];
            cmd[2] = (byte)RT_Commands.eReadDeviceInfoOperation.DEVICE_INFO_OP_READ;
            cmd[3] = (byte)RT_Commands.eReadDeviceInfoType.DEVICE_INFO_SUMMARY;
            return cmd;
        }

        public byte[] getAdvanceDeviceInfoReadCommandBody()
        {
            byte[] cmd = new byte[4];
            byte[] size = BitConverter.GetBytes(this.PacketSize);
            cmd[0] = size[0];
            cmd[1] = size[1];
            cmd[2] = (byte)RT_Commands.eReadDeviceInfoOperation.DEVICE_INFO_OP_READ;
            cmd[3] = (byte)RT_Commands.eReadDeviceInfoType.DEVICE_INFO_ADVANCE;
            return cmd;
        }

        public String getDeviceInfo() {
            Program.rotaAdvanceDeviceInfo = null;
            Program.rotaAdvanceDeviceInfo = new RotaAdvanceDeviceInfo();
            RT_Commands deviceInfoCommand = new RT_Commands();
            byte[] cmd = deviceInfoCommand.getAdvanceDeviceInfoReadCommand();
            Boolean status = Program.SendData(cmd);
            if (!status) {
                return "";
            }
            byte[] rData = Program.ReceiveData();

            RT_Response response = new RT_Response();
            Boolean ret = response.ResponseParser(rData);
            if (ret)
            {
                if (!Encoding.Default.GetString(Program.rotaAdvanceDeviceInfo.TestDate).Replace("\0", string.Empty).Equals("")) /* Advance Info Received */
                {
                    String infoStr = RotaAdvanceDeviceInfo.getAdvanceInfoStringFormat(Program.rotaAdvanceDeviceInfo);
                    return infoStr;
                }
                else
                {
                    String infoStr = RotaDeviceInfo.getInfoStringFormat(Program.rotaDeviceInfo);
                    return infoStr;
                }
            }
            else
            {
                return "";
            }


            return "";
        }


        public static DeviceStatus readDeviceInfo()
        {
            DeviceStatus devStat = new DeviceStatus();
            Program.rotaAdvanceDeviceInfo = null;
            Program.rotaAdvanceDeviceInfo = new RotaAdvanceDeviceInfo();
            RT_Commands deviceInfoCommand = new RT_Commands();
            byte[] cmd = deviceInfoCommand.getAdvanceDeviceInfoReadCommand();
            Program.SendData(cmd);
            byte[] rData = Program.ReceiveData();

            RT_Response response = new RT_Response();
            Boolean ret = response.ResponseParser(rData);
            if (ret)
            {


                if (!Encoding.Default.GetString(Program.rotaAdvanceDeviceInfo.TestDate).Replace("\0", string.Empty).Equals("")) /* Advance Info Received */
                {
                    devStat.RunMode = Program.rotaAdvanceDeviceInfo.DeviceInfo.RunMode;
                    devStat.SwLevel = Program.rotaAdvanceDeviceInfo.DeviceInfo.SwLevel;
                }
                else
                {
                    devStat.RunMode = Program.rotaDeviceInfo.RunMode;
                    devStat.SwLevel = Program.rotaDeviceInfo.SwLevel;
                }
            }
            return devStat;

        }
    }
}
