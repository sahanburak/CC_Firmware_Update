using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    class DeviceStatus
    {
        private UInt16 runMode;
        private UInt16 swLevel;

        public UInt16 RunMode
        {
            get { return this.runMode; }
            set { this.runMode = value; }
        }

        public UInt16 SwLevel
        {
            get { return this.swLevel; }
            set { this.swLevel = value; }
        }

        public DeviceStatus()
        {

        }

        public DeviceStatus(UInt16 runMode, UInt16 swLevel)
        {
            this.runMode = runMode;
            this.swLevel = swLevel;
        }
    }
}
