using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    [Serializable()]
    class RT_RunModeChangeCommand
    {
        private RT_Command_Header header;
        private UInt16 uPacketSize = 0;
        private UInt16 uHwUnit = 0;
        private UInt16 uRunMode = 0;

        public RT_RunModeChangeCommand(){
            this.header = new RT_Command_Header();
        }
        public RT_Command_Header Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        public UInt16 PacketSize
        {
            get { return this.uPacketSize; }
            set { this.uPacketSize = value; }
        }

        public UInt16 HWUnit
        {
            get { return this.uHwUnit; }
            set { this.uHwUnit = value; }
        }

        public UInt16 RunMode
        {
            get { return this.uRunMode; }
            set { this.uRunMode = value; }
        }

        public int Size
        {
            get
            {
                int size = 0;
                size += this.header.Size;
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.uPacketSize);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.uHwUnit);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.uRunMode);
                return size;
            }
        }

        public byte[] commandToByteArray(RT_RunModeChangeCommand command) {
            byte[] runModeCommandArr = new byte[Size];
            byte[] temp = command.Header.getHeaderArr(command.Header);
            int startIndex = 0;
            System.Buffer.BlockCopy(temp, 0, runModeCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = null;
            temp = BitConverter.GetBytes(command.PacketSize);
            System.Buffer.BlockCopy(temp, 0, runModeCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = null;
            temp = BitConverter.GetBytes(command.HWUnit);
            System.Buffer.BlockCopy(temp, 0, runModeCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = null;
            temp = BitConverter.GetBytes(command.RunMode);
            System.Buffer.BlockCopy(temp, 0, runModeCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;
            return runModeCommandArr;
        }

        public byte[] getModeChangeCommand(UInt16 uHWUnit, UInt16 uRunMode)
        {
            RT_RunModeChangeCommand modeChangeCmd = new RT_RunModeChangeCommand();

            modeChangeCmd.Header.Command = (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_RunModeChange;
            modeChangeCmd.Header.RequestID = 123;
            modeChangeCmd.Header.MessgeLength = (UInt16)6;
            modeChangeCmd.PacketSize = (UInt16)4;
            modeChangeCmd.HWUnit = uHWUnit;
            modeChangeCmd.RunMode = uRunMode;

            byte[] cmd = modeChangeCmd.commandToByteArray(modeChangeCmd);

            Console.Write("Mode Change Command: [");
            for (int i = 0; i < cmd.Length; i++)
            {
                Console.Write(" 0x{0:X}", cmd[i]);
            }
            Console.WriteLine(" ]");

            return cmd;
        }

        public Boolean changeDeviceRunMode(UInt16 uHWUnit, UInt16 uRunMode)
        {
            byte[] cmd = getModeChangeCommand(uHWUnit, uRunMode);
           
            Boolean status = Program.SendData(cmd);
            if (!status)
            {
                return false;
            }
            byte[] rData = Program.ReceiveData();

            RT_Response response = new RT_Response();
            RT_Response.eResponseErrorCode ret = response.ResponseParser(rData);
            if (ret == RT_Response.eResponseErrorCode.APP_COMMS_CMD_SUCCESS)
                return true;
            return false;
        }


        public static RT_Response.eResponseErrorCode ResponseParser(byte[] response)
        {
            RT_RunModeChangeCommand runModeChangeResponse = new RT_RunModeChangeCommand();
            RT_Response.eResponseErrorCode ret = RT_Response.eResponseErrorCode.APP_COMMS_CMD_ERR_FORMAT;

            runModeChangeResponse.PacketSize = BitConverter.ToUInt16(response, 0);
            runModeChangeResponse.HWUnit = BitConverter.ToUInt16(response, 2);
            runModeChangeResponse.RunMode = BitConverter.ToUInt16(response, 4);
            Program.rotaAdvanceDeviceInfo.DeviceInfo.RunMode = runModeChangeResponse.RunMode;

            if (runModeChangeResponse.HWUnit == (UInt16)RT_Commands.eHWUnit.HW_UNIT_HOST_MCU)
            {
                if (runModeChangeResponse.RunMode == (UInt16)RT_Commands.eRunMode.RUNMODE_INIT) {
                    ret = RT_Response.eResponseErrorCode.APP_COMMS_CMD_SUCCESS;
                }
            }
            return ret;
        }
    }
}
