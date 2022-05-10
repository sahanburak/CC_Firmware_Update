using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    [Serializable()]
    class RT_DeviceReset
    {
        private UInt16 uPacketSize = 2;

        public UInt16 PacketSize
        {
            get { return uPacketSize; }
            set { this.uPacketSize = value; }
        }

        public byte[] getDeviceReset(RT_Commands.eHWUnit hwUnit)
        {
            byte[] cmd = new byte[4];
            byte[] size = BitConverter.GetBytes(this.PacketSize);
            cmd[0] = size[0];
            cmd[1] = size[1];
            byte[] unit = BitConverter.GetBytes((UInt16) hwUnit);
            cmd[2] = unit[0];
            cmd[3] = unit[1];
            return cmd;
        }

       


        public RT_Response.eResponseErrorCode resetDevice(RT_Commands.eHWUnit hwUnit)
        {
            RT_Commands command = new RT_Commands();
            byte[] cmd = command.resetDeviceCommand(hwUnit);
            Boolean status = Program.SendData(cmd);
            RT_Response.eResponseErrorCode ret = RT_Response.eResponseErrorCode.APP_COMMS_CMD_RESET_ERR;

            if (!status)
            {
                return ret;
            }
            byte[] rData = Program.ReceiveData();

            RT_Response response = new RT_Response();
            ret = response.ResponseParser(rData);
            return ret;

        }

        public static RT_Response.eResponseErrorCode ResponseParser(byte[] response)
        {

            RT_Response rtResponse = new RT_Response();
            Boolean ret = false;

            rtResponse.Error = BitConverter.ToUInt32(response, 0);
            if (rtResponse.Error != 0)
            {
                int errorCode = BitConverter.ToUInt16(response, 4);
                return (RT_Response.eResponseErrorCode)errorCode;
            }
            else {
                return RT_Response.eResponseErrorCode.APP_COMMS_CMD_SUCCESS;
            }
        }
    }
}
