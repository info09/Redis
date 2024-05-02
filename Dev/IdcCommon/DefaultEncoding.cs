using IdcCommon.CommonMethods;
using System;
using System.Text;

namespace IdcCommon.IniFiles.Others
{
    /// <summary>
    /// デフォルトエンコードiniファイルのクラス
    /// </summary>
    public static class DefaultEncoding
    {
        #region private values

        public static string mEncodingName;
        private static string mFilePath = Registry.ProgramDirectory + @"\INI\default-encoding.ini";


        #endregion

        /// <summary>
        /// プロパティに値をセットします。
        /// </summary>
        public static void SetEncodingFromIniFile()
        {
            try
            {
                var readData = FileReadWrite.Read(mFilePath, Encode.GetEncoding(CurrentCulture.IsCultureJa ? EncodingEnum.sjis : EncodingEnum.utf8));

                for (var i = 0; i < readData.Count; i++)
                {
                    if (readData[i].StartsWith("//") || string.IsNullOrEmpty(readData[i]))
                    { continue; }
                    else
                    { 
                        mEncodingName = readData[i];
                        break;
                    }
                }

                if (string.IsNullOrEmpty(mEncodingName))
                { mEncodingName = Encode.GetEncodeString(Encoding.Default); }

            }
            catch (Exception)
            {
                mEncodingName = Encode.GetEncodeString(Encoding.Default);
                WriteFile();
            }
        }

        public static void SetEncodingFromIniFile(string filePath)
        {
            mFilePath = filePath;
            SetEncodingFromIniFile();
        }


        public static void SetEncoding(string encodingName)
        {
            mEncodingName = encodingName;
        }

        /// <summary>
        /// エンコードを取得します。
        /// </summary>
        /// <returns></returns>
        public static Encoding GetEncode()
        {
            if (string.IsNullOrEmpty(mEncodingName))
            {
                // プロパティにセットされていない場合、OSのデフォルトエンコードをセット。
                mEncodingName = Encode.GetEncodeString(Encoding.Default);
            }

            return Encode.GetEncoding(mEncodingName);
        }

        /// <summary>
        /// プロパティに格納された内容を書き込みます。
        /// </summary>
        public static void WriteFile()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("// 　・sjis:シフトJIS");
            stringBuilder.AppendLine("// 　・ascii:Us-ascii");
            stringBuilder.AppendLine("// 　・utf16LE:UTF-16LE");
            stringBuilder.AppendLine("// 　・utf16BE:UTF-16BE");
            stringBuilder.AppendLine("// 　・utf8:UTF-8");
            stringBuilder.AppendLine("// 　・utf8BOM:UTF-8BOM");
            stringBuilder.AppendLine(mEncodingName);

            FileReadWrite.Write(stringBuilder.ToString(), mFilePath, false, Encode.GetEncoding(CurrentCulture.IsCultureJa ? EncodingEnum.sjis : EncodingEnum.utf8));
        }

        /// <summary>
        /// プロパティに格納された内容を、指定されたファイルパスに書き込みます。
        /// </summary>
        /// <param name="filePath"></param>
        public static void WriteFile(string filePath)
        {
            mFilePath = filePath;
            WriteFile();
        }
    }
}
