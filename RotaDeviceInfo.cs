using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
	[Serializable()]
	class RotaDeviceInfo
	{
		private UInt16 uSwLevel;                              /* Software Level See eSwLevel for all type*/
		private UInt16 uRunMode;                              /* Run mode See eRunMode for all type*/
		private UInt32 ulManufacturerID;                     /* Manufacturer ID */
		private UInt32 ulDeviceClass;                          /* Rota device class */
		private UInt32[] aulUniqueID = new UInt32[4];        /* Device Unique ID */
		private UInt32 ulHWVersion;                          /* Hardware version */
		private Byte[] acFWVersion = new Byte[20];           /* Firmware version */
		private Byte[] acProjectName = new Byte[100];        /* Project Name*/
		private Byte[] acManufacturer = new Byte[20];        /* Manufacturer Name*/
		private Byte[] acSerialNumber = new Byte[100];       /* Device Serial Number */

		public RotaDeviceInfo() { 
		
		}

		public UInt16 SwLevel
		{
			get { return uSwLevel; }
			set { uSwLevel = value; }
		}

		public UInt16 RunMode
		{
			get { return uRunMode; }
			set { uRunMode = value; }
		}

		public UInt32 ManufacturerID
		{
			get { return ulManufacturerID; }
			set { ulManufacturerID = value; }
		}

		public UInt32 DeviceClass
		{
			get { return ulDeviceClass; }
			set { ulDeviceClass = value; }
		}

		public UInt32[] UniqueID
		{
			get { return aulUniqueID; }
			set { aulUniqueID = value; }
		}

		public UInt32 HWVersion
		{
			get { return ulHWVersion; }
			set { ulHWVersion = value; }
		}

		public byte[] FWVersion
		{
			get { return acFWVersion; }
			set { acFWVersion = value; }
		}

		public byte[] ProjectName
		{
			get { return acProjectName; }
			set { acProjectName = value; }
		}

		public byte[] Manufacturer
		{
			get { return acManufacturer; }
			set { acManufacturer = value; }
		}

		public byte[] SerialNumber
		{
			get { return acSerialNumber; }
			set { acSerialNumber = value; }
		}

		public static String getInfoStringFormat(RotaDeviceInfo deviceInfo)
		{
			String infoStr;
			infoStr = "Manufacturer\t:\t" + Encoding.Default.GetString(deviceInfo.Manufacturer).Replace("\0", string.Empty) + "\n\r";
			infoStr += "Manufacturer ID\t:\t" + deviceInfo.ManufacturerID + "\n\r";
			infoStr += "Device Class ID\t:\t" + ((deviceInfo.DeviceClass == (UInt32)RT_Commands.eDeviceClass.DEVICE_CONTROLLER) ? "CONTROLLER" : "DAQ") + "\n\r";
			infoStr += "Project\t\t:\t" + Encoding.Default.GetString(deviceInfo.ProjectName).Replace("\0", string.Empty) + "\n\r";
			infoStr += "Hardware Version\t:\t" + (deviceInfo.HWVersion / 10) + "." + +(deviceInfo.HWVersion % 10) + "\n\r";
			infoStr += "Firmware Version\t:\t" + Encoding.Default.GetString(deviceInfo.FWVersion).Replace("\0", string.Empty) + "\n\r";
			infoStr += "Serial Number\t:\t" + Encoding.Default.GetString(deviceInfo.SerialNumber).Replace("\0", string.Empty) + "\n\r";
			infoStr += "Software Level\t:\t" + deviceInfo.SwLevel + "\n\r";
			infoStr += "Run Mode\t:\t" + deviceInfo.RunMode + "\n\r";

			return infoStr;
		}

	}
}
