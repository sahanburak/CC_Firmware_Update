using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    class Utils
    {
        public static byte[] ByteSlicer(byte[] rawArray,int startIndex, int endIndex) {
            byte[] temp = new byte[endIndex-startIndex];
            for (int i = startIndex; i < endIndex; i++) {
                temp[i - startIndex] = rawArray[i];
            }
            return temp;
        }
    }
}
