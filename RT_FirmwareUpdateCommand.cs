using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    class RT_FirmwareUpdateHeader
    {
        UInt16 mwaiting_packet_num;
        UInt16 mpacket_num;

        public UInt16 WaitingPacketNumber
        {
            get { return this.mwaiting_packet_num; }
            set { this.mwaiting_packet_num = value; }
        }

        public UInt16 PacketNumber
        {
            get { return this.mpacket_num; }
            set { this.mpacket_num = value; }
        }

        public int Size
        {
            get
            {
                int size = 0;
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.mwaiting_packet_num);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.mpacket_num);
                return size;
            }
        }

        public byte[] getHeaderArr(RT_FirmwareUpdateHeader header)
        {
            byte[] hdr = new byte[header.Size];
            int startIndex = 0;            
            byte[] temp = BitConverter.GetBytes(header.mwaiting_packet_num);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, temp.Length);
            startIndex += temp.Length;
            temp = BitConverter.GetBytes(header.mpacket_num);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, temp.Length);
            startIndex += temp.Length;
            return hdr;
        }


    }

    class RT_FirmwareUpdateStartCommand
    {
        public static int BOARD_SDRAM_ADDR = 0x70000000;
        public static int SAMPLER_BUFFER_SIZE = (1 * 1024 * 1024);
        public static int HAL_FIRMWARE_BASE = ((BOARD_SDRAM_ADDR + SAMPLER_BUFFER_SIZE));
        public static int MAX_FIRMWARE_SIZE = (1 * 1024 * 1024);

        public static bool isUpdateStartable = false;
        RT_Command_Header header;
        byte majorType;  // which device firmware netx=1; STM32=2; HOST_MCU=3
        byte minorType;  // which type of config =0; file =1; firmware =2; licence =3
        UInt16 reserved = 0;
        UInt32 fw_size;
        UInt32 baseAddr;


        public RT_FirmwareUpdateStartCommand()
        {
            header = new RT_Command_Header();
        }

        public RT_Command_Header Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        public byte MajorType
        {
            get { return this.majorType; }
            set { this.majorType = value; }
        }
        public byte MinorType
        {
            get { return this.minorType; }
            set { this.minorType = value; }
        }
        public UInt32 FW_SIZE
        {
            get { return this.fw_size; }
            set { this.fw_size = value; }
        }
        public UInt32 BaseAddr
        {
            get { return this.baseAddr; }
            set { this.baseAddr = value; }
        }

        public int Size
        {
            get
            {
                int size = 0;
                size += this.header.Size;
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.majorType);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.minorType);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.reserved);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.fw_size);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.baseAddr);
                return size;
            }
        }

        public byte[] commandToByteArray(RT_FirmwareUpdateStartCommand command)
        {
            byte[] firmwareUpdateCmdArr = new byte[Size];
            int startIndex = 0;
            
            byte[] temp = command.Header.getHeaderArr(command.Header);
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;

            Array.Resize(ref temp, 1);
            temp[0] = (command.majorType);
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;

            Array.Resize(ref temp, 1);
            temp[0] = (command.minorType);
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = BitConverter.GetBytes(command.reserved);
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = BitConverter.GetBytes(command.fw_size);
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = BitConverter.GetBytes(command.baseAddr);
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;

            return firmwareUpdateCmdArr;
        }

        public byte[] getFirmwareUpdateStartCommand(byte uHWUnit, byte minorCommandType, UInt32 fwSize)
        {
            RT_FirmwareUpdateStartCommand firmwareUpdateStartCmd = new RT_FirmwareUpdateStartCommand();
            firmwareUpdateStartCmd.Header.Command = (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_FirmwareUpdateStart;
            firmwareUpdateStartCmd.Header.RequestID = 123;
            firmwareUpdateStartCmd.Header.MessgeLength = (UInt16)(Size - firmwareUpdateStartCmd.Header.Size);
            firmwareUpdateStartCmd.MajorType = uHWUnit;
            firmwareUpdateStartCmd.MinorType = minorCommandType;
            firmwareUpdateStartCmd.FW_SIZE = fwSize;
            firmwareUpdateStartCmd.BaseAddr = (UInt32)HAL_FIRMWARE_BASE;

            byte[] cmd = firmwareUpdateStartCmd.commandToByteArray(firmwareUpdateStartCmd);

            Console.Write("Firmware update Start Command: [");
            for (int i = 0; i < cmd.Length; i++)
            {
                Console.Write(" 0x{0:X}", cmd[i]);
            }
            Console.WriteLine(" ]");

            return cmd;
        }

        public Boolean startFirmwareUpdate(byte uHWUnit, byte minorCommandType, UInt32 fwSize)
        {
            byte[] cmd = getFirmwareUpdateStartCommand(uHWUnit, minorCommandType, fwSize);

            Boolean status = Program.SendData(cmd);
            if (!status)
            {
                return false;
            }
            byte[] rData = Program.ReceiveData();

            RT_Response response = new RT_Response();
            Boolean ret = response.ResponseParser(rData);
            return ret;
        }


        public static Boolean ResponseParser(byte[] response)
        {
            RT_Response rtResponse = new RT_Response();
            Boolean ret = false;

            rtResponse.Error = BitConverter.ToUInt32(response, 0);
            Console.WriteLine("Response Update Start Command: {0:}", rtResponse.Error);

            if (rtResponse.Error == 0) {
                isUpdateStartable = true;
            }
            else
            {
                isUpdateStartable = false;
            }

            return isUpdateStartable;
        }
    }

    class RT_FirmwareUpdateCommand
    {
        public static int APP_COMMS_MAX_MSG_SIZE = 1388;
        RT_Command_Header header;
        RT_FirmwareUpdateHeader updateHeader;
        byte[] msg;


        public RT_FirmwareUpdateCommand()
        {
            header = new RT_Command_Header();
            updateHeader = new RT_FirmwareUpdateHeader();
        }

        public RT_Command_Header Header
        {
            get { return this.header; }
            set { this.header = value; }
        }
        public RT_FirmwareUpdateHeader FirmwareUpdateHeader
        {
            get { return this.updateHeader; }
            set { this.updateHeader = value; }
        }
        public byte[] Message
        {
            get { return this.msg; }
            set { this.msg = value; }
        }
       

        public int Size
        {
            get
            {
                int size = 0;
                size += this.header.Size;
                size += this.updateHeader.Size;
                if(Message != null)
                {
                    size += Message.Length;
                }
                return size;
            }
        }

        public byte[] commandToByteArray(RT_FirmwareUpdateCommand command)
        {
            byte[] firmwareUpdateCmdArr = new byte[Size];
            byte[] temp = command.Header.getHeaderArr(command.Header);
            int startIndex = 0;
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;
            
            temp = null;
            temp = command.FirmwareUpdateHeader.getHeaderArr(command.FirmwareUpdateHeader);
            System.Buffer.BlockCopy(temp, 0, firmwareUpdateCmdArr, startIndex, temp.Length);
            startIndex += temp.Length;

            if (Message != null)
            {
                System.Buffer.BlockCopy(Message, 0, firmwareUpdateCmdArr, startIndex, Message.Length);
                startIndex += Message.Length;
            }
            return firmwareUpdateCmdArr;
        }

        public byte[] getFirmwareUpdateCommand(UInt16 totalPacketSize, UInt16 packetNumber,byte[] data)
        {
            RT_FirmwareUpdateCommand firmwareUpdateCmd = new RT_FirmwareUpdateCommand();

            firmwareUpdateCmd.Header.Command = (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_FirmwareUpdate;
            firmwareUpdateCmd.Header.RequestID = 123;
            firmwareUpdateCmd.updateHeader.WaitingPacketNumber = totalPacketSize;
            firmwareUpdateCmd.updateHeader.PacketNumber = packetNumber;
            firmwareUpdateCmd.Message = data;
            firmwareUpdateCmd.Header.MessgeLength = (UInt32) (data.Length + firmwareUpdateCmd.updateHeader.Size);

            byte[] cmd = firmwareUpdateCmd.commandToByteArray(firmwareUpdateCmd);

            Console.Write("Firmware Update Command: [");
            for (int i = 0; i < cmd.Length; i++)
            {
                Console.Write(" 0x{0:X}", cmd[i]);
            }
            Console.WriteLine(" ]");

            return cmd;
        }

        public Boolean firmwareUpdate(UInt16 totalPacketSize, UInt16 packetNumber, byte[] data)
        {
            byte[] cmd = getFirmwareUpdateCommand(totalPacketSize, packetNumber, data);

            Boolean status = Program.SendData(cmd);
            if (!status)
            {
                return false;
            }
            byte[] rData = Program.ReceiveData();

            RT_Response response = new RT_Response();
            Boolean ret = response.ResponseParser(rData);
            return ret;
        }


        public static Boolean ResponseParser(byte[] response)
        {

            RT_Response rtResponse = new RT_Response();
            Boolean ret = false;

            rtResponse.Error = BitConverter.ToUInt32(response, 0);

            return true;
        }
    }
}
