using System;
using System.Collections.Generic;
using static IdcRecordConvert.IdcConvCls.CnvMethod;

namespace NewIDC.Projects
{
    [Serializable]
    public class MoneyConversion : ConversionBase
	{
		private ConvMoney convMoney;
        public int targetColumn {  get; set; }
        public string[] param {  get; set; }
        public MoneyConversion()
        {
        }
        public MoneyConversion(string[] param, int targetColumn)
        {
            this.targetColumn = targetColumn;
            this.param = param;
        }
        public override List<string[]> Convert() {
            convMoney = new ConvMoney(param);
            convertResult = new List<string[]>();
#if !DEBUG
            List<string[]> inputContent = new List<string[]>();
            inputContent.Add(new string[] { "11", "22", "Åè200", "Åè500" });
            inputContent.Add(new string[] { "21", "22", "Åè2200", "Åè2500" });
#else
            List<string[]> inputContent = preConversion.Convert();
            HeaderString = preConversion.GetHeader();
#endif
            foreach (string[] row in inputContent) {
                string convertedValue = convMoney.Conv(row, "");
                row[targetColumn] = convertedValue;
                convertResult.Add(row);
            }
            return convertResult;
        }
    }

}

