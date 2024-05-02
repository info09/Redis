using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdcCommon.NewIdcCommon.ConvertFile
{
    public class ListConvFile : AbstractConvertFile
    {

        public string RECOREINIPATH { get; } = "";

        enum ListParamInd
        {
            Hd,
            Seq,
            target,
            filePath,
            wild,
            fileType,
            sheetType,
            sheet,
            endRow,
            encode,
            startRow,
            matchingCol,
            pairCol,
            unMatch,
            unMatchVal,
            unMatchValType,
            Register,
            log,
        }
        public ListConvFile(string[] param, string IniPath) :base(IniFiles.Others.DefaultEncoding.mEncodingName)
        {
            IniFiles.WildcardSortEnum wildcard;

            if(param[(int)ListParamInd.wild] == "Name")
            {
                wildcard = IniFiles.WildcardSortEnum.Name;
            }
            else
            {
                wildcard = IniFiles.WildcardSortEnum.Date;
            }

            this.filepath =  IniFiles.IniFileMethods.WithoutWildCard(param[(int)ListParamInd.filePath], wildcard);

            this.headerRowIndex = 1;

            this.startRowIndex = 1;

            if(!int.TryParse( param[(int)ListParamInd.startRow],out startRowIndex)) { this.startRowIndex = 1; }
            if (!int.TryParse(param[(int)ListParamInd.matchingCol], out int matchCol)) { matchCol = 1; }
            if (!int.TryParse(param[(int)ListParamInd.pairCol], out int pairCol)) { pairCol = 1; }

            this.startColumnIndex = 1;

            //判定列も含めて一番大きな値を列最大値として読み取る
            

            SetEncodeString(param[(int)ListParamInd.encode]);

            if (!IdcCommon.CommonMethods.Encode.IsExsitingEncodingString(param[(int)ListParamInd.encode]))
            {
                this.encodeString = IdcCommon.IniFiles.Others.DefaultEncoding.mEncodingName;
            }
            else
            {
                this.encodeString = param[(int)ListParamInd.encode];
            }

            SetFileTypeIni(param[(int)ListParamInd.fileType]);

            if (param[(int)ListParamInd.fileType] == "xls")
            {
                this.IsSheetTypeNumber = param[(int)ListParamInd.sheetType] == "Number";
                this.targetSheet = param[(int)ListParamInd.sheet];
                this.judgementCol = int.TryParse(param[(int)ListParamInd.endRow], out int judge) ? judge : 1;
            }

            var listColIndex = new List<int> { matchCol, pairCol, judgementCol };
            this.columnLength = listColIndex.Max();　//() matchCol > pairCol ? matchCol : pairCol;
            this.IsAdjustColumLength = false;

            this.RECOREINIPATH = IniPath;
        }
    }
}
