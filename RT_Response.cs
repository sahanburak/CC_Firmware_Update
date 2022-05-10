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

        public enum eResponseErrorCode
        {
            APP_COMMS_CMD_SUCCESS = 0,
            APP_COMMS_CMD_ERR_MALFORMED_REQ = 1,
            APP_COMMS_CMD_ERR_IL_GROUP = 2,
            APP_COMMS_CMD_ERR_IL_OFFSET = 3,
            APP_COMMS_CMD_INVALID_STATE = 4,
            APP_COMMS_CMD_ERR_NOMEM = 5,
            APP_COMMS_CMD_SCOPE_INVALID_STATE = 6,
            APP_COMMS_CMD_SCOPE_ALREADY_INSTATE = 7,
            APP_COMMS_CMD_SCOPE_CH_CONF_ERR = 8,
            APP_COMMS_DATA_SAVE_ERROR = 9,
            APP_COMMS_CMD_MONITOR_INVALID_STATE = 10,
            APP_COMMS_CMD_MONITOR_ALREADY_INSTATE = 11,
            APP_COMMS_CMD_MONITOR_CH_CONF_ERR = 12,

            APP_COMMS_CMD_RUNMODE_ERR = 13,
            APP_COMMS_CMD_SWMODE_ERR = 14,
            APP_COMMS_CMD_RESET_ERR = 15,

            APP_COMMS_CMD_ERR_WRONG_PACKAGE = 0xF0,
            APP_COMMS_CMD_ERR_DROPPED_PACKAGE = 0xF1,
            APP_COMMS_CMD_ERR_FORMAT = 0xFF,
        }

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


        public RT_Response.eResponseErrorCode ResponseParser(byte[] responseArray) {
            RT_Response response = new RT_Response();
            byte[] resp = responseArray;
            response.Header = RT_Command_Header.HeaderTypeCasting(Utils.ByteSlicer(responseArray, 0, 8));
            RT_Response.eResponseErrorCode errorCode = RT_Response.eResponseErrorCode.APP_COMMS_CMD_SUCCESS;
            Boolean err = false;
            if ((response.Header.Command & 0x8000) == 0x8000) {
                err = true;
                errorCode = (RT_Response.eResponseErrorCode) BitConverter.ToUInt32(resp, 8);
                Console.WriteLine("Error Received:  CMD: {0:} Error Code: :  {1:}", (response.Header.Command & 0x00FF), Error);
            }
            if (!err) { 
                switch (response.Header.Command)
                {
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_ReadDeviceInfo:
                        errorCode = RT_Response_DeviceInfo.deviceInfoResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));                    
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_RunModeChange:
                        errorCode = RT_RunModeChangeCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_Operation:
                        errorCode = RT_OperationCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_FirmwareUpdateStart:
                        errorCode = RT_FirmwareUpdateStartCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_FirmwareUpdate:
                        errorCode = RT_FirmwareUpdateCommand.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;

                    case (UInt16)RT_Commands.eCommands.APP_COMMS_CMD_ResetDevice:
                        errorCode = RT_DeviceReset.ResponseParser(Utils.ByteSlicer(responseArray, 8, responseArray.Length));
                        break;
                    default:
                        Console.WriteLine("Unknown command:  {0:}.", response.Header.Command);
                        errorCode = RT_Response.eResponseErrorCode.APP_COMMS_CMD_ERR_FORMAT;
                        break;
                }
            }

            return errorCode;
        }
    }
}
