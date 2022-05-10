using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    class RT_OperationCommand
    {
        public enum eOperationType
        {
            OPT_NONE = 0,
            OPT_UPLOAD = 1,
            OPT_DOWNLOAD = 2,
            OPT_RESET = 3,
            OPT_MANUAL = 4
        }
        
        public enum eOperationClass
        {
            OPC_NONE = 0,
            OPC_FIRMWARE = 1,
            OPC_BOOTLOADER = 2,
            OPC_CONFIG = 3,
            OPC_CONFIG_BL = 4,
            OPC_CALIBRATION = 5,
            OPC_LICENCE = 6,
            OPC_MODULE = 7,
            OPC_ALL = 0x10
        }
        
        private RT_Command_Header header;
        private UInt16 uPacketSize = 0;               /* Requested packet size */
        private UInt16 uOperClass;                /* Operation class flag. See eOperClass for more details.*/
        private UInt16 uOperType;                 /* Operation type flag. See eOperType for more details.*/
        private UInt16 uHwUnit;                   /* Hw unit type flag. See eHWUnit for more details.*/
        private byte[] operData;			      /* Operation data array */

        public RT_OperationCommand()
        {
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

        public UInt16 OperClass
        {
            get { return this.uOperClass; }
            set { this.uOperClass = value; }
        }

        public UInt16 OperType
        {
            get { return this.uOperType; }
            set { this.uOperType = value; }
        }

        public UInt16 HWUnit
        {
            get { return this.uHwUnit; }
            set { this.uHwUnit = value; }
        }

        public byte[] OperData
        {
            get { return this.operData; }
            set { this.operData = value; }
        }
        public int Size
        {
            get
            {
                int size = 0;
                size += this.header.Size;
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.uPacketSize);
                PacketSize += (UInt16)System.Runtime.InteropServices.Marshal.SizeOf(this.uOperClass);
                PacketSize += (UInt16)System.Runtime.InteropServices.Marshal.SizeOf(this.uOperType);
                PacketSize += (UInt16)System.Runtime.InteropServices.Marshal.SizeOf(this.uHwUnit);
                if(operData != null)
                    PacketSize += (UInt16)operData.Length;
                size += PacketSize;
                return size;
            }
        }

        public byte[] commandToByteArray(RT_OperationCommand command)
        {
            byte[] operationCommandArr = new byte[Size];
            byte[] temp = command.Header.getHeaderArr(command.Header);
            int startIndex = 0;
            System.Buffer.BlockCopy(temp, 0, operationCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = null;
            temp = BitConverter.GetBytes(command.PacketSize);
            System.Buffer.BlockCopy(temp, 0, operationCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = null;
            temp = BitConverter.GetBytes(command.OperClass);
            System.Buffer.BlockCopy(temp, 0, operationCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = null;
            temp = BitConverter.GetBytes(command.OperType);
            System.Buffer.BlockCopy(temp, 0, operationCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            temp = null;
            temp = BitConverter.GetBytes(command.HWUnit);
            System.Buffer.BlockCopy(temp, 0, operationCommandArr, startIndex, temp.Length);
            startIndex += temp.Length;

            return operationCommandArr;
        }

        public byte[] getOperationCommand(UInt16 operClass, UInt16 operType, UInt16 hwUnit)
        {
            RT_OperationCommand operationCmd = new RT_OperationCommand();

            operationCmd.Header.Command = (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_Operation;
            operationCmd.Header.RequestID = 123;

            operationCmd.OperClass = operClass;
            operationCmd.OperType = operType;
            operationCmd.HWUnit = hwUnit;
            
            operationCmd.Header.MessgeLength = (UInt32) (Size-operationCmd.Header.Size);


            byte[] cmd = operationCmd.commandToByteArray(operationCmd);

            Console.Write("Operation Command: [");
            for (int i = 0; i < cmd.Length; i++)
            {
                Console.Write(" 0x{0:X}", cmd[i]);
            }
            Console.WriteLine(" ]");

            return cmd;
        }

        public Boolean doOperationCommand(UInt16 operClass, UInt16 operType, UInt16 hwUnit)
        {
            byte[] cmd = getOperationCommand(operClass, operType, hwUnit);

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
            Console.Write("Operation Command Response: [");
            for (int i = 0; i < response.Length; i++)
            {
                Console.Write(" 0x{0:X}", response[i]);
            }
            Console.WriteLine(" ]");
            RT_OperationCommand operationResponse = new RT_OperationCommand();
            RT_Response.eResponseErrorCode ret = RT_Response.eResponseErrorCode.APP_COMMS_CMD_ERR_FORMAT;
            operationResponse.PacketSize = BitConverter.ToUInt16(response, 0);
            operationResponse.OperClass = BitConverter.ToUInt16(response, 2);
            operationResponse.OperType = BitConverter.ToUInt16(response, 4);
            operationResponse.HWUnit = BitConverter.ToUInt16(response, 6);
            if (operationResponse.OperClass == (UInt16) eOperationClass.OPC_FIRMWARE)
            {
                if (operationResponse.OperType == (UInt16) eOperationType.OPT_DOWNLOAD)
                {
                    ret = RT_Response.eResponseErrorCode.APP_COMMS_CMD_SUCCESS;
                }
            }
            return ret;
        }
    }
}
