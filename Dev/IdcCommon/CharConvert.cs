using IdcCommon.IniFiles.Others;
using System;
using System.Text;
using static IdcCommon.Exceptions.IdcCustomException;

namespace IdcCommon.CommonMethods
{
    public static class CharConvert
    {
        /// <summary>
        /// 入力された文字コードであろうcharCodeStringの値を、指定したエンコード指定に基づいて変換します。
        /// 変換できない場合はInvalidIniFileReadException をスロー。
        /// </summary>
        /// <param name="charCodeString"></param>
        /// <returns></returns>
        public static string ConvertCharCodeToCharacter(string charCodeString, Encoding encoding)
        {
            try
            {
                if (charCodeString.Length % 2 == 1)
                { throw new IdcCharReplaceException(FolderReplace.GetText(IdcCharReplaceErrorEnum.InvalidCharCode), IdcCharReplaceErrorEnum.InvalidCharCode); }

                var bytestr = new string[charCodeString.Length / 2];
                for (var i = 0; i < charCodeString.Length; i = i + 2)
                { bytestr[i / 2] = charCodeString.Substring(i, 2); }

                var data = new byte[bytestr.Length];
                for (var i = 0; i < bytestr.Length; i++)
                { data[i] = Convert.ToByte(bytestr[i], 16); }

                var encodedString = encoding.GetString(data);
                return encodedString;
            }
            catch (Exception ex)
            {
                var errorText = string.Format("{0}\r\n{1}{2}",
                                              FolderReplace.GetText(IdcCharReplaceErrorEnum.InvalidCharCode),
                                              CurrentCulture.IsCultureJa ? "エラー原因：" : "Error cause:",
                                              ex.Message);
                throw new IdcCharReplaceException(errorText, IdcCharReplaceErrorEnum.InvalidCharCode);
            }
        }
    }
}
