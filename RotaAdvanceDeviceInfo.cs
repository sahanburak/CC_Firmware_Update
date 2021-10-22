using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    [Serializable()]
    class RotaAdvanceDeviceInfo
    {
        private RotaDeviceInfo oDeviceInfo;
        private byte[] acProductionDate = new byte[20];
        private byte[] acFwCompileDate = new byte[20];
        private byte[] acCalibrationDate = new byte[20];
        private byte[] acTestDate = new byte[20];

        public RotaAdvanceDeviceInfo()
        {
            oDeviceInfo = new RotaDeviceInfo();
        }
        public RotaDeviceInfo DeviceInfo
        {
            get { return oDeviceInfo; }
            set { this.oDeviceInfo = value; }
        }

        public byte[] ProductionDate
        {
            get { return acProductionDate; }
            set { this.acProductionDate = value; }
        }

        public byte[] FwCompileDate
        {
            get { return acFwCompileDate; }
            set { this.acFwCompileDate = value; }
        }

        public byte[] CalibrationDate
        {
            get { return acCalibrationDate; }
            set { this.acCalibrationDate = value; }
        }

        public byte[] TestDate
        {
            get { return acTestDate; }
            set { this.acTestDate = value; }
        }

        public static String getAdvanceInfoStringFormat(RotaAdvanceDeviceInfo rotaAdvanceDeviceInfo)
        {
            RotaDeviceInfo deviceInfo = rotaAdvanceDeviceInfo.DeviceInfo;
            String infoStr = RotaDeviceInfo.getInfoStringFormat(deviceInfo);
            infoStr += "Production Date\t:\t" + Encoding.Default.GetString(rotaAdvanceDeviceInfo.ProductionDate).Replace("\0", string.Empty) + "\n\r";
            infoStr += "Compile Date\t:\t" + Encoding.Default.GetString(rotaAdvanceDeviceInfo.FwCompileDate).Replace("\0", string.Empty) + "\n\r";
            infoStr += "Calibration Date\t:\t" + Encoding.Default.GetString(rotaAdvanceDeviceInfo.CalibrationDate).Replace("\0", string.Empty) + "\n\r";
            infoStr += "Test Date\t\t:\t" + Encoding.Default.GetString(rotaAdvanceDeviceInfo.TestDate).Replace("\0", string.Empty) + "\n\r";

            return infoStr;
        }

    }
}
