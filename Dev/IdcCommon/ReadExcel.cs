using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace IdcCommon.CommonMethods
{
    /// <summary>
    /// エクセルのファイルを読み込みます。（動的実装）
    /// </summary>
    public class ReadExcel
    {
        public bool IsImportSheetsExists = true;

        private readonly List<string[]> returningValue = new List<string[]>();
        private readonly Application excelApplication = new Application();

        /// <summary>
        /// シート番号
        /// </summary>
        private int sheetIndex;

        /// <summary>
        /// ファイルパス
        /// </summary>
        private readonly string path;

        /// <summary>
        /// シート番号か否か（true：シート番号）
        /// </summary>
        private readonly bool sheetTypeIsNumber;

        /// <summary>
        /// 対象シート名
        /// </summary>
        private readonly string targetSheet;

        /// <summary>
        /// 読み取り開始行
        /// </summary>
        private readonly int startRow;

        /// <summary>
        /// 読み取り開始列
        /// </summary>
        private readonly int startColumn;

        /// <summary>
        /// 読み取り判定列
        /// </summary>
        private readonly int judgementColumn;

        /// <summary>
        /// 項目列数
        /// </summary>
        private readonly int importColumnVolume;

        /// <summary>
        /// ヘッダ行
        /// </summary>
        private readonly int headerRowIndex;

        /// <summary>
        /// ヘッダー
        /// </summary>
        public string[] Header { get; private set; }

        public ReadExcel(string path, bool sheetTypeIsNumber, string targetSheet, int startRow, int startColumn, int judgementColumn, int importColumnVolume, int headerRowIndex)
        {
            this.path = path;
            this.sheetTypeIsNumber = sheetTypeIsNumber;
            this.targetSheet = targetSheet;
            this.startRow = startRow;
            this.startColumn = startColumn;
            this.judgementColumn = judgementColumn;
            this.importColumnVolume = importColumnVolume;
            this.headerRowIndex = headerRowIndex;
        }

        public List<string[]> Read()
        {
            // 判定列の列数 ＞ インポート列数 の場合、
            // またはシート番号にも関わらず入力値がシート番号では無い場合は、値なしでreturn
            if (importColumnVolume < judgementColumn || (sheetTypeIsNumber && !int.TryParse(targetSheet, out _)))
            {
                this.IsImportSheetsExists = false;
                return this.returningValue;
            }

            try
            {
                Workbooks workbooks = this.excelApplication.Workbooks;
                try
                {
                    Workbook workbook = workbooks.Open(path);
                    try
                    {
                        Sheets worksheets = workbook.Sheets;
                        try
                        {
                            // シート番号が0以下の場合、シートがないと見做し値なしでreturn
                            sheetIndex = sheetTypeIsNumber ? int.Parse(targetSheet) : this.GetSheetIndex(targetSheet, worksheets);
                            if (sheetIndex < 0)
                            {
                                this.IsImportSheetsExists = false;
                                return this.returningValue;
                            }

                            if (worksheets.Count < sheetIndex)
                            {
                                this.IsImportSheetsExists = false;
                                return this.returningValue;
                            }

                            Worksheet worksheet = worksheets[sheetIndex];

                            try
                            {
                                // 使用範囲を一括で二次元配列にコピー
                                object[,] rangeArray;
                                Range usedRange = worksheet.UsedRange;
                                try
                                { rangeArray = usedRange.Value; }

                                finally { Marshal.ReleaseComObject(usedRange); }

                                // 成形
                                this.Set(rangeArray, this.startRow, this.startColumn, this.judgementColumn, this.importColumnVolume, this.headerRowIndex);
                            }

                            finally { Marshal.ReleaseComObject(worksheet); }
                        }

                        finally { Marshal.ReleaseComObject(worksheets); }
                    }

                    finally
                    {
                        if (workbook != null)
                        { workbook.Close(false); }

                        Marshal.ReleaseComObject(workbook);
                    }
                }

                finally { Marshal.ReleaseComObject(workbooks); }
            }

            finally
            {
                this.excelApplication?.Quit();
                Marshal.ReleaseComObject(this.excelApplication);
            }

            return this.returningValue;
        }

        /// <summary>
        /// シート番号を取得します。
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="sheets"></param>
        /// <returns></returns>
        private int GetSheetIndex(string sheetName, Sheets sheets)
        {
            int i = 0;
            foreach (Worksheet worksheet in sheets)
            {
                if (sheetName == worksheet.Name)
                { return i + 1; }

                i += 1;
            }

            return -1;
        }


        /// <summary>
        /// シートからデータを読み取ります。（パラメーターに関係なく、Excelの値があればそれをそのまま読み取ってListとして返送）
        /// </summary>
        /// <returns></returns>
        public List<string[]> ReadFaithful()
        {
            // 判定列の列数 ＞ インポート列数 の場合、
            // またはシート番号にも関わらず入力値がシート番号では無い場合は、値なしでreturn
            if (sheetTypeIsNumber && !int.TryParse(targetSheet, out _))
            {
                this.IsImportSheetsExists = false;
                return this.returningValue;
            }

            try
            {
                Workbooks workbooks = this.excelApplication.Workbooks;
                try
                {
                    Workbook workbook = workbooks.Open(path);
                    try
                    {
                        Sheets worksheets = workbook.Sheets;
                        try
                        {
                            // シート番号が0以下の場合、シートがないと見做し値なしでreturn
                            sheetIndex = sheetTypeIsNumber ? int.Parse(targetSheet) : this.GetSheetIndex(targetSheet, worksheets);
                            if (sheetIndex < 0)
                            {
                                this.IsImportSheetsExists = false;
                                return this.returningValue;
                            }

                            if (worksheets.Count < sheetIndex)
                            {
                                this.IsImportSheetsExists = false;
                                return this.returningValue;
                            }

                            Worksheet worksheet = worksheets[sheetIndex];

                            try
                            {
                                // 使用範囲を一括で二次元配列にコピー
                                object[,] rangeArray;
                                Range usedRange = worksheet.UsedRange;
                                try
                                { rangeArray = usedRange.Value; }

                                finally { Marshal.ReleaseComObject(usedRange); }

                                var readExcelRowVol = worksheet.UsedRange.Rows.Count;
                                var readExcelColumnVol = worksheet.UsedRange.Columns.Count;

                                // 成形
                                this.Set(readExcelRowVol, readExcelColumnVol, rangeArray, this.startRow, this.startColumn, this.headerRowIndex);
                                return this.returningValue;
                            }

                            finally { Marshal.ReleaseComObject(worksheet); }
                        }
                        finally { Marshal.ReleaseComObject(worksheets); }
                    }
                    finally
                    {
                        workbook?.Close(false);
                        Marshal.ReleaseComObject(workbook);
                    }
                }
                finally { Marshal.ReleaseComObject(workbooks); }
            }
            finally
            {
                this.excelApplication?.Quit();
                Marshal.ReleaseComObject(this.excelApplication);
            }
        }

        /// <summary>
        /// 成形し、returningValueおよびHeaderにセットします。
        /// </summary>
        /// <param name="rangeArray"></param>
        /// <param name="startRow"></param>
        /// <param name="judgementColumn"></param>
        /// <param name="importColumnVolume"></param>
        private void Set(object[,] rangeArray, int startRow, int startColumn, int judgementColumn, int importColumnVolume, int headerRow)
        {
            var arrayLimit = rangeArray.GetLength(1) < importColumnVolume ? rangeArray.GetLength(1) : importColumnVolume;

            //if (rangeArray.GetLength(1) < importColumnVolume)
            //{ return; }

            // Headerを作成
            this.Header = new string[importColumnVolume];
            if (0 < headerRow)
            {
                for (var i = 0; i <= rangeArray.GetLength(0); i++)
                {
                    if (i == headerRow)
                    {
                        for (var j = 0; j < arrayLimit; j++)
                        { this.Header[j] = Convert.ToString(rangeArray[i, j + startColumn]); }

                        break;
                    }
                }
            }

            // Fileを作成
            for (var i = startRow; i <= rangeArray.GetLength(0); i++)
            {
                var stringArray = new string[importColumnVolume];

                if (Convert.ToString(rangeArray[i, judgementColumn]) == string.Empty)
                { break; }

                for (var j = 0; j < arrayLimit; j++)
                { stringArray[j] = Convert.ToString(rangeArray[i, j + startColumn]); }

                for (var k = 0; k < stringArray.Length; k++)
                {
                    if (string.IsNullOrEmpty(stringArray[k]))
                    { stringArray[k] = string.Empty; }
                }

                this.returningValue.Add(stringArray);
            }
        }

        /// <summary>
        /// 成形し、returningValueおよびHeaderにセットします。
        /// </summary>
        /// <param name="rangeArray"></param>
        /// <param name="startRow"></param>
        /// <param name="judgementColumn"></param>
        /// <param name="importColumnVolume"></param>
        private void Set(int readRowVol, int readColumnVol, object[,] rangeArray, int startRow, int startColumn, int headerRow)
        {
            // Headerを作成
            this.Header = new string[readColumnVol];
            if (0 < headerRow)
            {
                for (var i = 0; i <= rangeArray.GetLength(0); i++)
                {
                    if (i == headerRow)
                    {
                        for (var j = 0; j < readColumnVol; j++)
                        { this.Header[j] = Convert.ToString(rangeArray[i, j + startColumn]); }

                        break;
                    }
                }
            }

            // Fileを作成
            for (var i = startRow; i < readRowVol; i++)
            {
                var stringArray = new string[readColumnVol];

                if (Convert.ToString(rangeArray[i, judgementColumn]) == string.Empty)
                { break; }

                for (var j = 0; j < readColumnVol; j++)
                { stringArray[j] = Convert.ToString(rangeArray[i, j + startColumn]); }

                for (var k = 0; k < readColumnVol; k++)
                {
                    if (string.IsNullOrEmpty(stringArray[k]))
                    { stringArray[k] = string.Empty; }
                }

                this.returningValue.Add(stringArray);
            }
        }
    }
}
