using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Firmware_Update
{
    class RTFile{
        private RT_Commands.eHWUnit hwType;
        private RT_OperationCommand.eOperationClass fileType;
        private String filePath;

        public RTFile(RT_Commands.eHWUnit hwType, RT_OperationCommand.eOperationClass fileType, String filePath) {
            this.hwType = hwType;
            this.fileType = fileType;
            this.filePath = filePath;
        }

        public RT_Commands.eHWUnit HwType
        {
            set { this.hwType = value; }
            get { return this.hwType; }
        }


        public RT_OperationCommand.eOperationClass FileType
        {
            set { this.fileType = value; }
            get { return this.fileType; }
        }


        public String FilePath
        {
            set { this.filePath = value; }
            get { return this.filePath; }
        }

    }
    class FileOperations
    {
        public static List<RTFile> fileList = new List<RTFile>();

        public static void addFile(RTFile file)
        {
            int index = searchFile(file);
            if (index == 0)
            {
                fileList.Add(file);
            }
            else {
                fileList[index] = file;
            }
        }


        public static int searchFile(RTFile file) {
            for (int i=0;i<fileList.Count;i++) {
                if (file.FileType == fileList[i].FileType && file.HwType == fileList[i].HwType) {
                    return (i+1);
                }
            }
            return 0;
        }
    }
}
