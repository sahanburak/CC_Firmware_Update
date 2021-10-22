using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    [Serializable()]
    class RT_Response
    {
        private RT_Command_Header header;
        private UInt32 err;
       
        public RT_Command_Header Header
        {
            get { return this.header; }
            set { this.header = value; }
        }

        public uint Error
        {
            get { return this.err; }
            set { this.err = value; }
        }


        public Boolean ResponseParser(byte[] responseArray) {
            RT_Response response = new RT_Response();

            response.Header = RT_Command_Header.HeaderTypeCasting(Utils.ByteSlicer(responseArray, 0, 8));
            Boolean ret = false;
            Boolean err = false;
            if ((response.Header.Command & 0x8000) == 0x8000) {
                err = true;
                UInt32 Error = BitConverter.ToUInt32(responseArray, 8);
                Console.WriteLine("Error Received:  {0:} Error Code: :  {0:}", (response.Header.Command & 0x00FF), Error);
            }
            if (!err) { 
                switch (response.Header.Command)
                {
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_ReadDeviceInfo:
                         ret = RT_Response_DeviceInfo.deviceInfoResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));                    
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_RunModeChange:
                        ret = RT_RunModeChangeCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_Operation:
                        ret = RT_OperationCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_FirmwareUpdateStart:
                        ret = RT_FirmwareUpdateStartCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_FirmwareUpdate:
                        ret = RT_FirmwareUpdateCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    default:
                        Console.WriteLine("Unknown command:  {0:}.", response.Header.Command);
                        ret = false;
                        break;
                }
            }

            return ret;
        }
    }
}
