using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    [Serializable()]
    class RT_Response_DeviceInfo
    {
        private UInt16 uPacketSize;
        private UInt16 uType;
        private UInt32 ulErrror;
        private UInt32 ulIsAdvanced;
        private RotaDeviceInfo deviceInfo;
        private RotaAdvanceDeviceInfo advanceDeviceInfo;

        public UInt16 PacketSize
        {
            get { return this.uPacketSize; }
            set { this.uPacketSize = value; }
        }

        public UInt16 InfoType
        {
            get { return this.uType; }
            set { this.uType = value; }
        }

        public UInt32 Error
        {
            get { return this.ulErrror; }
            set { this.ulErrror = value; }
        }

        public UInt32 IsAdvanced
        {
            get { return this.ulIsAdvanced; }
            set { this.ulIsAdvanced = value; }
        }

        public RotaDeviceInfo DeviceInfo
        {
            get { return this.deviceInfo; }
            set { this.deviceInfo = value; }
        }

        public RotaAdvanceDeviceInfo AdvanceDeviceInfo
        {
            get { return this.advanceDeviceInfo; }
            set { this.advanceDeviceInfo = value; }
        }

        public static Boolean deviceInfoResponseParser(byte[] response)
        {
            RT_Response_DeviceInfo deviceInfoResponse = new RT_Response_DeviceInfo();
            Boolean ret = false;
            deviceInfoResponse.PacketSize = BitConverter.ToUInt16(response, 0);
            deviceInfoResponse.InfoType = BitConverter.ToUInt16(response, 2);

            if (deviceInfoResponse.InfoType == (UInt16)RT_Commands.eReadDeviceInfoResponseType.DEVICE_INFO_RESP_SUCCESS)
            { /* Succes received*/
                deviceInfoResponse.IsAdvanced = BitConverter.ToUInt32(response, 4);
                if (deviceInfoResponse.IsAdvanced == (UInt16)RT_Commands.eReadDeviceInfoType.DEVICE_INFO_SUMMARY)
                {
                    byte[] data = Utils.ByteSlicer(response, 8, 8+deviceInfoResponse.PacketSize);
                    deviceInfoResponse.DeviceInfo = deviceInfoTypeCasting(data);
                    Console.WriteLine("Software Level: {0:}", deviceInfoResponse.DeviceInfo.SwLevel);
                    Console.WriteLine("Runmode Level: {0:}", deviceInfoResponse.DeviceInfo.RunMode);
                    ret = true;
                }
                else if (deviceInfoResponse.IsAdvanced == (UInt16)RT_Commands.eReadDeviceInfoType.DEVICE_INFO_ADVANCE)
                {
                    byte[] data = Utils.ByteSlicer(response, 8, 8 + deviceInfoResponse.PacketSize);
                    deviceInfoResponse.AdvanceDeviceInfo = advanceDeviceInfoTypeCasting(data);
                    Console.WriteLine("Software Level: {0:}", deviceInfoResponse.AdvanceDeviceInfo.DeviceInfo.SwLevel);
                    Console.WriteLine("Runmode Level: {0:}", deviceInfoResponse.AdvanceDeviceInfo.DeviceInfo.RunMode);
                    ret = true;
                }

            }
            else if (deviceInfoResponse.InfoType == (UInt16)RT_Commands.eReadDeviceInfoResponseType.DEVICE_INFO_RESP_ERROR)
            {  /* Error received */
                deviceInfoResponse.ulErrror = BitConverter.ToUInt32(response, 3);
                ret = false;
            }
            return ret;
        }

        public static RotaDeviceInfo deviceInfoTypeCasting(byte[] deviceInfoArray)
        {
            int index = 0;
            Program.rotaDeviceInfo.SwLevel = BitConverter.ToUInt16(deviceInfoArray, index);
            index += 2;
            Program.rotaDeviceInfo.RunMode = BitConverter.ToUInt16(deviceInfoArray, index);
            index += 2;
            Program.rotaDeviceInfo.ManufacturerID = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.DeviceClass = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;

            Program.rotaDeviceInfo.UniqueID[0] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.UniqueID[1] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.UniqueID[2] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.UniqueID[3] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;

            Program.rotaDeviceInfo.HWVersion = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;

            Program.rotaDeviceInfo.FWVersion = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;
            Program.rotaDeviceInfo.ProjectName = Utils.ByteSlicer(deviceInfoArray, index, (index + 100));
            index += 100;
            Program.rotaDeviceInfo.Manufacturer = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;
            Program.rotaDeviceInfo.SerialNumber = Utils.ByteSlicer(deviceInfoArray, index, (index + 100));
            index += 100;
            return Program.rotaDeviceInfo;
        }



        public static RotaAdvanceDeviceInfo advanceDeviceInfoTypeCasting(byte[] deviceInfoArray)
        {
            int index = 0;
            Program.rotaDeviceInfo.SwLevel = BitConverter.ToUInt16(deviceInfoArray, index);
            index += 2;
            Program.rotaDeviceInfo.RunMode = BitConverter.ToUInt16(deviceInfoArray, index);
            index += 2;
            Program.rotaDeviceInfo.ManufacturerID = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.DeviceClass = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;

            Program.rotaDeviceInfo.UniqueID[0] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.UniqueID[1] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.UniqueID[2] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;
            Program.rotaDeviceInfo.UniqueID[3] = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;

            Program.rotaDeviceInfo.HWVersion = BitConverter.ToUInt32(deviceInfoArray, index);
            index += 4;

            Program.rotaDeviceInfo.FWVersion = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;
            Program.rotaDeviceInfo.ProjectName = Utils.ByteSlicer(deviceInfoArray, index, (index + 100));
            index += 100;
            Program.rotaDeviceInfo.Manufacturer = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;
            Program.rotaDeviceInfo.SerialNumber = Utils.ByteSlicer(deviceInfoArray, index, (index + 100));
            index += 100;

            Program.rotaAdvanceDeviceInfo.DeviceInfo = Program.rotaDeviceInfo;
            Program.rotaAdvanceDeviceInfo.ProductionDate = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;
            Program.rotaAdvanceDeviceInfo.FwCompileDate = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;
            Program.rotaAdvanceDeviceInfo.CalibrationDate = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;
            Program.rotaAdvanceDeviceInfo.TestDate = Utils.ByteSlicer(deviceInfoArray, index, (index + 20));
            index += 20;

            return Program.rotaAdvanceDeviceInfo;
        }
    }
}
