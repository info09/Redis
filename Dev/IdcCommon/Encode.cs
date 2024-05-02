using System.Text;

namespace IdcCommon.CommonMethods
{
    /// <summary>
    /// エンコード変換クラス（静的メソッド）
    /// </summary>
    public static class Encode
    {

        #region private values

        private static readonly Encoding mSjis = Encoding.GetEncoding("shift_jis");
        private static readonly Encoding mUtf16LE = Encoding.Unicode;
        private static readonly Encoding mUtf16BE = Encoding.BigEndianUnicode;
        private static readonly Encoding mUtf8 = new UTF8Encoding(false);
        private static readonly Encoding mUtf8BOM = new UTF8Encoding(true);
        private static readonly Encoding mAscii = Encoding.GetEncoding(20127);

        #endregion

        /// <summary>
        /// エンコード文字列 → エンコードクラスを取得します。
        /// </summary>
        /// <param name="encodeName"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(string encodeName)
        {
            switch (encodeName)
            {
                case "sjis":
                    return mSjis;

                case "utf16LE":
                case "unicode":
                    return mUtf16LE;

                case "utf16BE":
                    return mUtf16BE;

                case "utf8":
                default:
                    return mUtf8;

                case "utf8BOM":
                    return mUtf8BOM;

                case "ascii":
                case "ansi":
                    return mAscii;
            }
        }

        /// <summary>
        /// エンコードクラス → エンコード文字列を取得します。
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(EncodingEnum encodingEnum)
        {
            switch (encodingEnum)
            {
                case EncodingEnum.sjis:
                    return mSjis;
                case EncodingEnum.utf16LE:
                    return mUtf16LE;
                case EncodingEnum.utf16BE:
                    return mUtf16BE;
                case EncodingEnum.utf8:
                default:
                    return mUtf8;
                case EncodingEnum.utf8BOM:
                    return mUtf8BOM;
                case EncodingEnum.ascii:
                    return mAscii;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodeName"></param>
        /// <returns></returns>
        public static bool IsExsitingEncodingString(string encodeName)
        {
            switch (encodeName)
            {
                case "sjis":
                case "utf16LE":
                case "unicode":
                case "utf16BE":
                case "utf8":
                case "utf8BOM":
                case "ascii":
                case "ansi":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// エンコードクラス → エンコード文字列を取得します。
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetEncodeString(Encoding encoding)
        {
            if (encoding.Equals(mSjis))
            { return "sjis"; }
            else if (encoding.Equals(mUtf16LE))
            { return "utf16LE"; }
            else if (encoding.Equals(mUtf16BE))
            { return "utf16BE"; }
            else if (encoding.Equals(mUtf8))
            { return "utf8"; }
            else if (encoding.Equals(mUtf8BOM))
            { return "utf8BOM"; }
            else if (encoding.Equals(mAscii))
            { return "ascii"; }
            else
            { return "utf8"; }
        }

        /// <summary>
        /// エンコードクラス → エンコード名称を取得します。
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetEncodeName(Encoding encoding)
        {
            if (encoding.Equals(mSjis))
            { return "SJIS"; }
            if (encoding.Equals(mUtf16LE))
            { return "UTF-16 LE"; }
            else if (encoding.Equals(mUtf16BE))
            { return "UTF-16 BE"; }
            else if (encoding.Equals(mUtf8))
            { return "UTF-8"; }
            else if (encoding.Equals(mUtf8BOM))
            { return CurrentCulture.IsCultureJa ? "UTF-8（BOM付き）" : "UTF-8 (with BOM)"; }
            else if (encoding.Equals(mAscii))
            { return "US-ASCII"; }
            else
            { return "UTF-8"; }
        }

        /// <summary>
        /// エンコード文字列 → エンコード列挙型を取得します。
        /// </summary>
        /// <param name="encodeName"></param>
        /// <returns></returns>
        public static EncodingEnum GetEncodingEnum(string encodeName)
        {
            switch (encodeName)
            {
                case "sjis":
                    return EncodingEnum.sjis;

                case "utf16LE":
                case "unicode":
                    return EncodingEnum.utf16LE;

                case "utf16BE":
                    return EncodingEnum.utf16BE;

                case "utf8":
                default:
                    return EncodingEnum.utf8;

                case "utf8BOM":
                    return EncodingEnum.utf8BOM;

                case "ascii":
                case "ansi":
                    return EncodingEnum.ascii;
            }
        }

        /// <summary>
        /// エンコード列挙型 → エンコード文字列を取得します。
        /// </summary>
        /// <param name="encodingEnum"></param>
        /// <returns></returns>
        public static string GetEncodeString(EncodingEnum encodingEnum)
        {
            switch (encodingEnum)
            {
                case EncodingEnum.sjis:
                    return "sjis";

                case EncodingEnum.utf16LE:
                    return "utf16LE";

                case EncodingEnum.utf16BE:
                    return "utf16BE";

                case EncodingEnum.utf8:
                default:
                    return "utf8";

                case EncodingEnum.utf8BOM:
                    return "utf8BOM";

                case EncodingEnum.ascii:
                    return "ascii";
            }
        }
    }

    /// <summary>
    /// エンコード列挙型（ただし列挙型の値はエンコードのcodepageとは不一致です）
    /// </summary>
    public enum EncodingEnum
    {
        sjis,
        utf16LE,
        utf16BE,
        utf8,
        utf8BOM,
        ascii,
    }
}
