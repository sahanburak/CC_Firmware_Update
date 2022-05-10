using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    class RT_Commands
    {
        public enum eCommands
        {
            APP_COMMS_CMD_ReadParam         = 0,
            APP_COMMS_CMD_WriteParam        = 1,
            APP_COMMS_CMD_Scope             = 2,
            APP_COMMS_CMD_SaveParam         = 3,
            APP_COMMS_CMD_Monitor           = 4,
            APP_COMMS_CMD_WatchList         = 5,
            APP_COMMS_CMD_ResetDevice       = 6,
            APP_COMMS_CMD_ReadDeviceInfo    = 7,
            APP_COMMS_CMD_Operation         = 8,
            APP_COMMS_CMD_RunModeChange     = 9,

            APP_COMMS_CMD_FirmwareUpdateStart= 0xF4,
            APP_COMMS_CMD_FirmwareUpdate     = 0xF5
        }

        public enum eReadDeviceInfoType
        {
            DEVICE_INFO_SUMMARY = 0,
            DEVICE_INFO_ADVANCE = 1
        }

        public enum eReadDeviceInfoOperation
        {
            DEVICE_INFO_OP_NONE = 0,
            DEVICE_INFO_OP_WRITE = 1,
            DEVICE_INFO_OP_READ = 2
        }

        public enum eReadDeviceInfoResponseType
        {
            DEVICE_INFO_RESP_SUCCESS = 0,
            DEVICE_INFO_RESP_ERROR = 1
        }

        public enum eSwLevel
        {
            SWLEVEL_NONE = 0,
            SWLEVEL_BOOTLOADER = 1,
            SWLEVEL_APPLICATION = 2
        }

        public enum eRunMode
        {
            RUNMODE_NONE = 0,
            RUNMODE_INIT = 1,
            RUNMODE_OPERATION = 2,
            RUNMODE_CONF = 3,
            RUNMODE_CALIBRATION = 4,
            RUNMODE_FW_UPDATE = 5
        }

        public enum eDeviceClass
        {
            DEVICE_NONE = 0,
            DEVICE_DAQ = 1,
            DEVICE_CONTROLLER = 2
        }

        public enum eHWUnit
        {
            HW_UNIT_NONE = 0,
            HW_UNIT_NETX = 1,
            HW_UNIT_STM32 = 2,
            HW_UNIT_HOST_MCU = 3,
        }

        public enum eFWType
        {
            DOWNLOAD_FW = 1,
            DOWNLOAD_CONFIG = 2,
            DOWNLOAD_FILE = 3,
            DOWNLOAD_BOOTLOADER = 4,
            DOWNLOAD_LIC = 5,
            DOWNLOAD_MODULE = 6,
            DOWNLOAD_FW_WITH_RST = 7,
        } 

        public RT_Commands() {
            
        }


        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        public byte[] getAdvanceDeviceInfoReadCommand()
        {
            byte[] cmd = new byte[12];
            RT_DeviceInfoCommand bodyCMD = new RT_DeviceInfoCommand();
            RT_Command_Header headerCMD = new RT_Command_Header();
            byte[] header = headerCMD.getHeaderArr((UInt16)eCommands.APP_COMMS_CMD_ReadDeviceInfo, 123,4);
            byte[] body = bodyCMD.getAdvanceDeviceInfoReadCommandBody();
            System.Buffer.BlockCopy(header, 0, cmd, 0, header.Length);
            System.Buffer.BlockCopy(body, 0, cmd, header.Length, body.Length);
            Console.WriteLine("Command Length: {0}",(header.Length+body.Length));
            return cmd;
        }

        public byte[] getDeviceInfoReadCommand()
        {
            byte[] cmd = new byte[12];
            RT_DeviceInfoCommand bodyCMD = new RT_DeviceInfoCommand();
            RT_Command_Header headerCMD = new RT_Command_Header();
            byte[] header = headerCMD.getHeaderArr((UInt16)eCommands.APP_COMMS_CMD_ReadDeviceInfo, 123, 4);
            byte[] body = bodyCMD.getDeviceInfoReadCommandBody();
            System.Buffer.BlockCopy(header, 0, cmd, 0, header.Length);
            System.Buffer.BlockCopy(body, 0, cmd, header.Length, body.Length);
            Console.WriteLine("Command Length: {0}", (header.Length + body.Length));
            return cmd;
        }

        public byte[] resetDeviceCommand(RT_Commands.eHWUnit hwUnit)
        {
            byte[] cmd = new byte[12];
            RT_DeviceReset bodyCMD = new RT_DeviceReset();
            RT_Command_Header headerCMD = new RT_Command_Header();
            byte[] header = headerCMD.getHeaderArr((UInt16)eCommands.APP_COMMS_CMD_ResetDevice, 123, 4);
            byte[] body = bodyCMD.getDeviceReset(hwUnit);
            System.Buffer.BlockCopy(header, 0, cmd, 0, header.Length);
            System.Buffer.BlockCopy(body, 0, cmd, header.Length, body.Length);
            Console.WriteLine("Command Length: {0}", (header.Length + body.Length));
            return cmd;
        }


    }
}
