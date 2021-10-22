using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    [Serializable()]
    class RT_Command_Header
    {
        private UInt16 command;
        private UInt16 requestID;
        private UInt32 messageLenght;

        public UInt16 Command {
            get { return command; }
            set { this.command = value; }
        }

        public UInt16 RequestID
        {
            get { return this.requestID; }
            set { this.requestID = value; }
        }

        public UInt32 MessgeLength
        {
            get { return messageLenght; }
            set { this.messageLenght = value; }
        }


        public byte[] getHeaderArr(UInt16 command, UInt16 requestID,UInt32 messageLength)
        {
            byte[] hdr = new byte[8];
            int startIndex = 0;
            byte[] temp = BitConverter.GetBytes(command);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, startIndex+temp.Length);
            startIndex += temp.Length;
            temp = BitConverter.GetBytes(requestID);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, temp.Length);
            startIndex += temp.Length;
            temp = BitConverter.GetBytes(messageLength);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, temp.Length);
            startIndex += temp.Length;
            return hdr;
        }

        public byte[] getHeaderArr(RT_Command_Header commandHeader)
        {
            byte[] hdr = new byte[8];
            int startIndex = 0;
            byte[] temp = BitConverter.GetBytes(commandHeader.Command);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, startIndex + temp.Length);
            startIndex += temp.Length;
            temp = BitConverter.GetBytes(commandHeader.RequestID);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, temp.Length);
            startIndex += temp.Length;
            temp = BitConverter.GetBytes(commandHeader.messageLenght);
            System.Buffer.BlockCopy(temp, 0, hdr, startIndex, temp.Length);
            startIndex += temp.Length;
            return hdr;
        }

        public int Size
        {
            get
            {
                int size = 0;
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.command);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.requestID);
                size += System.Runtime.InteropServices.Marshal.SizeOf(this.messageLenght);
                return size;
            }
        }

        public static RT_Command_Header HeaderTypeCasting(byte[] headerArray) {
            RT_Command_Header header = new RT_Command_Header();
            header.Command = BitConverter.ToUInt16(Utils.ByteSlicer(headerArray, 0, 2),0);
            header.RequestID = BitConverter.ToUInt16(Utils.ByteSlicer(headerArray,2, 4), 0);
            header.MessgeLength = BitConverter.ToUInt16(Utils.ByteSlicer(headerArray, 4, 8), 0);
            return header;
        }

    }
}
