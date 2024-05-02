using IdcCommon.CommonMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IdcCommon.NewIdcCommon.ConvertFile
{
    public class AbstractConvertFile
    {

        public enum ConvertFileType
        {
            csv,
            tsv,
            fix,
            xls
        }

        ConvertFileType filetype;

        Microsoft.VisualBasic.FileIO.TextFieldParser textFieldParser;

        ReadExcel excel;

        /// <summary>
        /// ヘッダ行
        /// </summary>
        protected int headerRowIndex = 0;

        /// <summary>
        /// 読取開始行
        /// </summary>
        protected int startRowIndex = 0;

        protected int[] FixLength;

        protected bool IsFixTypeChar = true;

        protected bool IsAdjustColumLength = true;

        public string[] HeaderString;

        /// <summary>
        /// 読取開始列
        /// </summary>
        protected int startColumnIndex = 0;

        /// <summary>
        /// 項目列数
        /// </summary>
        protected int columnLength = 0;

        public List<string[]> fileContents = new List<string[]>();

        protected string filepath = "";

        public string FileFullPath
        {
            get
            {
                return filepath;
            }
        }

        protected string encodeString = "sjis";
        private readonly string defultEncodeString = "sjis";


        protected bool IsSheetTypeNumber;

        protected string targetSheet;

        protected int judgementCol = 0;

        public bool IsExcelSheetsExists()
        {
            if (excel == null) { return true; }

            return excel.IsImportSheetsExists;
        }

        public int ColumnCount { get { return columnLength; } }

        public AbstractConvertFile(string defultencode)
        {
            defultEncodeString = defultencode;
        }

        protected void SetEncodeString(string encodestr)
        {
            if (!Encode.IsExsitingEncodingString(encodestr))
            {
                encodeString = defultEncodeString;
            }
            else
            {
                encodeString = encodestr;
            }
        }


        protected void SetFileType_Csv()
        {
            filetype = ConvertFileType.csv;

            textFieldParser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filepath, Encode.GetEncoding(encodeString))
            {
                //フィールドが文字で区切られているとする            
                TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
                //固定長長さ
                FieldWidths = null,
                //区切り文字
                Delimiters = new string[] { "," },
                //フィールドを"で囲み、改行文字、区切り文字を含めることができるか            
                HasFieldsEnclosedInQuotes = true,
                //フィールドの前後からスペースを削除する            
                TrimWhiteSpace = false
            };
        }

        protected void SetFileType_Tsv()
        {
            filetype = ConvertFileType.tsv;

            textFieldParser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filepath, Encode.GetEncoding(encodeString))
            {
                //フィールドが文字で区切られているとする            
                TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
                //固定長長さ
                FieldWidths = null,
                //区切り文字
                Delimiters = new string[] { "\t" },
                //フィールドを"で囲み、改行文字、区切り文字を含めることができるか            
                HasFieldsEnclosedInQuotes = true,
                //フィールドの前後からスペースを削除する            
                TrimWhiteSpace = false
            };
        }

        protected void SetFileType_Fix()
        {
            filetype = ConvertFileType.fix;

            textFieldParser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filepath, Encode.GetEncoding(encodeString))
            {
                // 解析対象ファイルの指定           
                TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.FixedWidth,
                // 各フィールドにおける固定長の長さ
                FieldWidths = FixLength,
                //フィールドを"で囲み、改行文字、区切り文字を含めることができるか            
                HasFieldsEnclosedInQuotes = false,
                //フィールドの前後からスペースを削除する            
                TrimWhiteSpace = false,
                
            };
        }

        private void SetFileType_Excel()
        {
            filetype = ConvertFileType.xls;
            excel = new ReadExcel(this.filepath, this.IsSheetTypeNumber, this.targetSheet, startRowIndex, startColumnIndex, this.judgementCol, columnLength, headerRowIndex);
        }

        public bool IsExist() { return File.Exists(filepath); }

        public bool IsExistHeaderRow()
        {
            return HeaderString != null;
        }

        public bool IsExistConvertStartRow()
        {
            if (1 < fileContents.Count) { return true; }

            else if (fileContents.Count == 0) { return false; }

            else
            {
                foreach (var content in fileContents[0])
                {
                    if (!string.IsNullOrEmpty(content)) { return true; }
                }
                return false;
            }

        }

        public void SetContents(List<string[]> FileContents)
        {
            fileContents = FileContents;
        }

        public void Create()
        {
            //上書きする
            var streamWriter = new StreamWriter(filepath, false, Encode.GetEncoding(encodeString));

            //TextBox1.Textの内容を書き込む
            streamWriter.Write("");
            //閉じる
            streamWriter.Close();
        }

        /// <summary>
        /// ファイルインポート
        /// </summary>
        public void ImportFile()
        {
            switch (filetype)
            {
                case ConvertFileType.fix:
                    SetFileType_Fix();
                    ReadFix();
                    break;

                case ConvertFileType.xls:
                    SetFileType_Excel();
                    ReadExcel();
                    break;

                case ConvertFileType.tsv:
                    SetFileType_Tsv();
                    ReadFile();
                    break;

                default:
                    SetFileType_Csv();
                    ReadFile();
                    break;
            }
        }


        private void ReadExcel()
        {
            fileContents = excel.Read();
            HeaderString = excel.Header;
        }

        private void ReadFile()
        {
            List<string[]> Records = new List<string[]>();
            string[] fields;
            int columnVolume = 0;
            //ファイル読み込み
            for (int rowindex = 1; !textFieldParser.EndOfData; rowindex++)
            {
                //フィールドを読み込む
                fields = textFieldParser.ReadFields();

                //コメント行
                if (fields[0].Length > 2 && fields[0].Substring(0, 2) == "//") { rowindex--; continue; }

                //ヘッダ行
                if (rowindex == headerRowIndex)
                {
                    //コピーする
                    HeaderString = new string[fields.Length];
                    for (int i = 0; i < HeaderString.Length; i++)
                    {
                        HeaderString[i] = fields[i];
                    }

                    //開始列
                    if (startColumnIndex > 1)
                    {
                        HeaderString = StartColumResize(HeaderString);
                    }

                    //列幅            
                    if (IsAdjustColumLength && HeaderString.Length != columnLength)
                    {
                        HeaderString = ColumnLengthResize(HeaderString);
                    }
                    
                }

                //開始行
                if (rowindex < startRowIndex) { columnVolume = fields.Length; continue; }

                //開始列
                if (startColumnIndex > 1)
                {
                    fields = StartColumResize(fields);
                }

                //列幅            
                if (IsAdjustColumLength && fields.Length != columnLength)
                {
                    fields = ColumnLengthResize(fields);
                }

                //セット
                Records.Add(fields);
            }

            //変数セット
            if (Records.Count == 0 && columnVolume != 0)
            {
                Records.Add(new string[columnVolume]);
            }
            fileContents = Records;
            textFieldParser.Dispose();
        }

        private void ReadFix()
        {
            var records = new List<string[]>();
            var readFile = FileReadWrite.Read(filepath, Encode.GetEncoding(encodeString));
            var columnVolume = 0;

            //FixLength

            for (var i = 0; i < readFile.Count; i++)
            {
                if (2 <= readFile[i].Length && readFile[i].Substring(0, 2) == "//") { continue; }

                var linePadded = readFile[i].PadRight(FixLength.Sum());
                var rowElement = new string[FixLength.Length];

                for (var j = 0; j < FixLength.Length; j++)
                {
                    if (IsFixTypeChar)
                    { rowElement[j] = linePadded.Substring(FixLength.Take(j).Sum(), FixLength[j]); }
                    else
                    { rowElement[j] = this.SubstringByte(linePadded, FixLength.Take(j).Sum(), FixLength[j]); }
                }

                // ヘッダ行
                if (i + 1 == headerRowIndex)
                {
                    HeaderString = new string[FixLength.Length];
                    for (var j = 0; j < HeaderString.Length; j++)
                    { HeaderString[j] = rowElement[j]; }

                    // 開始列
                    if (1 < startColumnIndex)
                    { HeaderString = StartColumResize(HeaderString); }


                    // 列幅
                    if (this.IsAdjustColumLength && HeaderString.Length != columnLength)
                    { HeaderString = ColumnLengthResize(HeaderString); }
                }

                // 開始行
                if (i + 1 < startRowIndex)
                {
                    columnVolume = rowElement.Length;
                    continue;
                }

                // 開始列
                if (1 < startColumnIndex)
                { rowElement = StartColumResize(rowElement); }


                // 列幅
                if (FixLength.Length != columnLength)
                { rowElement = ColumnLengthResize(rowElement); }

                // セット
                records.Add(rowElement);
            }

            if (records.Count == 0 && 0 < columnVolume)
            { records.Add(new string[columnVolume]); }

            fileContents = records;
        }


        private string SubstringByte(string s, int i, int n)
        {
            byte[] b = Encode.GetEncoding(encodeString).GetBytes(s);
            return Encode.GetEncoding(encodeString).GetString(b, i, n);
        }

        private string[] StrByteSplit(string inStr)
        {
            List<string> outArray = new List<string>(); // 分割結果の保存領域
            string outStr = "";                 // 現在処理中の分割後文字列
            Encoding enc = Encode.GetEncoding(encodeString);
            int length = 0;

            // パラメータチェック
            if (inStr == null)
            {
                return outArray.ToArray();
            }

            //--------------------------------------
            // 全ての文字を処理するまで繰り返し
            //--------------------------------------
            string curStr = "";
            int curTotalLength = 0;
            length = textFieldParser.FieldWidths[0];
            for (int offset = 0; offset < inStr.Length; offset++)
            {
                //----------------------------------------------------------
                // 今回処理する文字と、その文字を含めた分割後文字列長を取得
                //----------------------------------------------------------
                curStr = inStr[offset].ToString();
                curTotalLength = enc.GetByteCount(outStr) + enc.GetByteCount(curStr);

                //-------------------------------------
                // この文字が、分割点になるかチェック
                //-------------------------------------
                if (curTotalLength == length)
                {
                    // 処理中の文字を含めると、ちょうどピッタリ
                    outArray.Add(outStr + curStr);
                    if (textFieldParser.FieldWidths.Length <= outArray.Count)
                    {
                        outStr = "";
                        break;
                    }

                    length = textFieldParser.FieldWidths[outArray.Count];
                    outStr = "";
                }
                else if (length < curTotalLength)
                {
                    // 処理中の文字を含めると、あふれる
                    outArray.Add(outStr);
                    if (textFieldParser.FieldWidths.Length <= outArray.Count)
                    {
                        outStr = "";
                        break;
                    }

                    length = textFieldParser.FieldWidths[outArray.Count];
                    outStr = curStr;
                }
                else
                {
                    // 処理中の文字を含めてもまだ余裕あり
                    outStr += curStr;
                }
            }

            // 最後の行の文を追加する
            if (!outStr.Equals(""))
            {
                outArray.Add(outStr);
            }

            // 分割後データを配列に変換して返す
            return outArray.ToArray();
        }


        private string[] ColumnLengthResize(string[] fields)
        {
            int fieldsLength = fields.Length;
            Array.Resize(ref fields, columnLength);

            if (fieldsLength < columnLength)
            {
                for (int i = fieldsLength; i < columnLength; i++)
                {
                    if (string.IsNullOrEmpty(fields[i])) { fields[i] = ""; }
                }

            }

            return fields;
        }

        private string[] StartColumResize(string[] fields)
        {
            Array.Copy(fields, startColumnIndex - 1, fields, 0, fields.Length - startColumnIndex + 1);

            Array.Resize(ref fields, fields.Length - startColumnIndex + 1);
            return fields;
        }

        protected void SetFileTypeIni(string FileTypeString)
        {
            switch (FileTypeString)
            {
                case "csv":
                default:
                    filetype = ConvertFileType.csv;
                    break;

                case "tsv":
                    filetype = ConvertFileType.tsv;
                    break;

                case "fix":
                    filetype = ConvertFileType.fix;
                    break;

                case "xls":
                    filetype = ConvertFileType.xls;
                    break;
            }
        }
    }
}
