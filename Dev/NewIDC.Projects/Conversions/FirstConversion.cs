using IdcCommon.CommonMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewIDC.Projects {
    public class FirstConversion : ConversionBase {
        public string SourceFilePath { get; set; }
        public FirstConversion(string sourceFilePath)
        {
            SourceFilePath = sourceFilePath;
        }
        public override List<string[]> Convert() {
            ReadExcel excelReader = new ReadExcel(SourceFilePath, true, "1", 2, 1, 3, 10, 1);//dummy
            var convertedContent = excelReader.Read();
            HeaderString = excelReader.Header;
            return convertedContent;
        }
    }
}
