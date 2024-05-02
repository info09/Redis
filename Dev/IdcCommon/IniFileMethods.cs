using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace IdcCommon.IniFiles
{
    /// <summary>
    /// IniFileのエクスポート・インポートで使用するグローバル関数の一覧。
    /// （全て静的実装です）
    /// </summary>
    public static class IniFileMethods
    {
        /// <summary>
        /// 文字列 → int に変換します。
        /// 変換できない場合、0を返します。
        /// </summary>
        /// <returns></returns>
        public static int EncodeStringToInt(string convertString)
        {
            return int.TryParse(convertString, out int i) ? i : 0;
        }

        /// <summary>
        /// 文字列 → List&lt;int&gt;に変換します。
        /// 変換できない場合は空のListを返します。
        /// </summary>
        /// <param name="convertString"></param>
        /// <returns></returns>
        public static List<int> EncodeStringToIntList(string convertString)
        {
            if (convertString == null || convertString == IdcAbstractIniFile.NoneString)
            { return new List<int>(); }

            var targetStringArrays = convertString.Split(',');
            var returningValue = new List<int>();

            foreach (var str in targetStringArrays)
            {
                returningValue.Add(int.TryParse(str, out int j) ? j : 0);
            }

            return returningValue;
        }

        /// <summary>
        /// List&lt;int&gt; → 文字列に変換します。
        /// 変換できない場合はstring.Emptyを返します。
        /// convertStringEmptyCaseWhenZero：trueで0をstring.Emptyに変換。
        /// </summary>
        /// <param name="convertIntArray"></param>
        /// <param name="convertStringEmptyCaseWhenZero"></param>
        /// <returns></returns>
        public static string EncodeIntListToString(List<int> convertIntArray, bool convertStringEmptyCaseWhenZero)
        {
            if (convertIntArray == null || convertIntArray.Count == 0 || (convertIntArray.Count == 1 && convertIntArray[0].ToString() == "0"))
            {
                return string.Empty;
            }

            var returningValue = ((convertIntArray[0] == 0 && convertStringEmptyCaseWhenZero) ? string.Empty : convertIntArray[0].ToString());

            for (var i = 1; i < convertIntArray.Count; i++)
            {
                returningValue += "," + ((convertIntArray[i] == 0 && convertStringEmptyCaseWhenZero) ? string.Empty : convertIntArray[i].ToString());
            }

            return returningValue;
        }

        /// <summary>
        /// 文字列 → List&lt;string&gt;に変換します。
        /// 変換できない場合は空のListを返します。
        /// </summary>
        /// <param name="convertString"></param>
        /// <returns></returns>
        public static List<string> EncodeStringToStringList(string convertString)
        {
            var returningValue = new List<string>();

            var stringList = convertString.Split(',');

            foreach (var str in stringList)
            {
                returningValue.Add(str);
            }

            return returningValue;
        }

        /// <summary>
        /// List&lt;string&gt; → 文字列に変換します。
        /// 変換できない場合はstring.Emptyを返します。
        /// </summary>
        /// <param name="convertStringList"></param>
        /// <returns></returns>
        public static string EncodeStringListToString(List<string> convertStringList)
        {
            if (convertStringList.Count == 0)
            { return string.Empty; }

            var returningValue = string.Format("{0}{1}{2}",
                                               (convertStringList[0] ?? string.Empty).Contains(",") ? "\"" : "",
                                               convertStringList[0] ?? string.Empty,
                                               (convertStringList[0] ?? string.Empty).Contains(",") ? "\"" : "");

            for (var i = 1; i < convertStringList.Count; i++)
            {
                returningValue += string.Format(",{0}{1}{2}",
                                                (convertStringList[i] ?? string.Empty).Contains(",") ? "\"" : "",
                                                convertStringList[i] ?? string.Empty,
                                                (convertStringList[i] ?? string.Empty).Contains(",") ? "\"" : "");
            }

            return returningValue;
        }

        /// <summary>
        /// string[] → 文字列に変換します。
        /// 変換できない場合はstring.Emptyを返します。
        /// </summary>
        /// <returns></returns>
        public static string EncodeStringArrayToString(string[] convertStringArray)
        {
            if (convertStringArray == null || convertStringArray.Length == 0)
            { return string.Empty; }

            var returningValue = string.Format("{0}{1}{2}",
                                               (convertStringArray[0] ?? string.Empty).Contains(",") ? "\"" : "",
                                               convertStringArray[0] ?? string.Empty,
                                               (convertStringArray[0] ?? string.Empty).Contains(",") ? "\"" : "");

            for (var i = 1; i < convertStringArray.Length; i++)
            {
                returningValue += string.Format(",{0}{1}{2}",
                                                (convertStringArray[i] ?? string.Empty).Contains(",") ? "\"" : "",
                                                convertStringArray[i] ?? string.Empty,
                                                (convertStringArray[i] ?? string.Empty).Contains(",") ? "\"" : "");
            }

            return returningValue;
        }

        /// <summary>
        /// 引数が「List&lt;string&gt;」の形に変換できるかをチェック
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static bool IsStringList(string targetString)
        {
            var targetStringArrays = targetString.Split(',');

            foreach (var str in targetStringArrays)
            {
                if (str.GetType() != typeof(string))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 引数が「List&lt;int&gt;」の形に変換できるかをチェック
        /// （nullの場合も含む）
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static bool IsIntegerList(string targetString)
        {
            var targetStringArrays = targetString.Split(',');

            foreach (var str in targetStringArrays)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                if (!int.TryParse(str, out int i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 引数が「List&lt;string[]&gt;」の形に変換できるかをチェック
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static bool IsStringArrayList(string targetString)
        {
            var targetStringListArrays = targetString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var secondTargetString in targetStringListArrays)
            {
                if (!IsStringList(secondTargetString))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 引数が「List&lt;int[]&gt;」の形に変換できるかをチェック
        /// （nullの場合も含む）
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static bool IsIntegerArrayList(string targetString)
        {
            var targetStringListArrays = targetString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (var secondTargetString in targetStringListArrays)
            {
                if (!IsIntegerList(secondTargetString))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// メールアドレスか否かを検証し、結果を返します。
        /// </summary>
        /// <param name="targetString"></param>
        /// <param name="IsMultiple"></param>
        /// <returns></returns>
        public static bool IsValidMailAddress(string targetString, bool IsMultiple)
        {
            if (IsMultiple)
            {
                var addressArray = targetString.Split(';');

                foreach (var address in addressArray)
                {
                    try
                    {
                        var a = new MailAddress(address);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                try
                {
                    var a = new MailAddress(targetString);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// ワイルドカード指定がある場合のパスを返します。
        /// IsNameSort：trueで名前、falseで日付のソートとなります。
        /// </summary>
        /// <returns></returns>
        public static string ReturnPathForWildcard(string path, bool IsNameSort)
        {
            var fileName = Path.GetFileName(path) ?? string.Empty;

            // ワイルドカードが含まれない場合はそのままpathを返送。
            if (!fileName.Contains("?") && !fileName.Contains("*"))
            {
                return path;
            }

            // ワイルドカードが含まれるファイル名を、C#用に書き換え。
            var regexPattern = Regex.Replace(fileName, ".", m =>
            {
                string s = m.Value;
                if (s.Equals("?"))
                {
                    //?は任意の1文字を示す正規表現(.)に変換
                    return ".";
                }
                else if (s.Equals("*"))
                {
                    //*は0文字以上の任意の文字列を示す正規表現(.*)に変換
                    return ".*";
                }
                else
                {
                    //上記以外はエスケープする
                    return Regex.Escape(s);
                }
            });

            GC.Collect();

            var folderName = Path.GetDirectoryName(path) ?? string.Empty;
            var fileList = Directory.GetFiles(folderName);
            var selectedFileList = fileList.Where(x => new Regex(regexPattern).IsMatch(x));

            var filePathAndDateDictionary = new Dictionary<string, DateTime>();
            foreach (var filePath in selectedFileList)
            {
                filePathAndDateDictionary.Add(filePath, File.GetLastWriteTime(filePath));
            }

            if (IsNameSort)
            {
                var sorted = filePathAndDateDictionary.OrderByDescending((x) => x.Key);
                return sorted.FirstOrDefault().Key ?? string.Empty;
            }
            else
            {
                var sorted = filePathAndDateDictionary.OrderByDescending((x) => x.Value);
                return sorted.FirstOrDefault().Key ?? string.Empty;
            }
        }

        /// <summary>
        /// 固定長項目長のパラメーターを返します。この機能はVer.02.01.38より追加されました。
        /// </summary>
        /// <param name="lengthDirection"></param>
        /// <param name="lengthList"></param>
        /// <returns></returns>
        public static string EncodeFixSettingListToString(string lengthDirection, List<int> lengthList)
        {
            if (lengthList == null)
            { return string.Empty; }
            return lengthDirection + "," + EncodeIntListToString(lengthList, true);
        }

        /// <summary>
        /// 先頭文字が"B"または"C"であるかをチェックし、そうであればその文字列を、数字であれば"C"を、いずれでもなければ空文字を返します。
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string GetLengthDirection(string convertString)
        {
            var targetStringArrays = convertString.Split(',');
            if (targetStringArrays[0] == "B" || targetStringArrays[0] == "C")
            { return targetStringArrays[0]; }
            else if (int.TryParse(targetStringArrays[0], out int i))
            { return "C"; }
            else
            { return string.Empty; }
        }

        /// <summary>
        /// List&lt;string[]&gt; → 文字列に変換します。
        /// 変換できない場合はstring.Emptyを返します。
        /// removeConditionString：trueで、"BeginCondition"の1要素のみが入った先頭行及び、
        /// "EndCondition"の1要素のみが入った最終行を文字列には加えないようにします。
        /// </summary>
        /// <param name="convertStringArrayList"></param>
        /// <returns></returns>
        public static string EncodeStringArrayListToString(List<string[]> convertStringArrayList, bool removeConditionString)
        {
            var returningValue = string.Empty;

            for (var i = 0; i < convertStringArrayList.Count; i++)
            {
                if (convertStringArrayList[i][0].Contains(",") && !convertStringArrayList[i][0].Contains("\",\""))
                {
                    convertStringArrayList[i][0] = "\"" + convertStringArrayList[i][0] + "\"";
                }
                returningValue += convertStringArrayList[i][0].ToString();

                for (var j = 1; j < convertStringArrayList[i].Length; j++)
                {
                    if (convertStringArrayList[i][j].Contains(",") && !convertStringArrayList[i][j].Contains("\",\""))
                    {
                        convertStringArrayList[i][j] = "\"" + convertStringArrayList[i][j] + "\"";
                    }

                    returningValue += "," + (string.IsNullOrEmpty(convertStringArrayList[i][j]) ? string.Empty : convertStringArrayList[i][j]);
                }

                if (i < convertStringArrayList.Count - 1)
                {
                    returningValue += Environment.NewLine;
                }
            }

            if (returningValue.IndexOf("BeginCondition\r\n") == 0)
            {
                returningValue = returningValue.Substring("BeginCondition\r\n".Length, returningValue.Length - "BeginCondition\r\n".Length);
            }

            if (returningValue.LastIndexOf("\r\nEndCondition") > 0)
            {
                returningValue = returningValue.Substring(0, returningValue.LastIndexOf("\r\nEndCondition"));
            }

            if (!removeConditionString)
            {
                if (string.IsNullOrEmpty(returningValue))
                {
                    returningValue = "BeginCondition\r\nEndCondition";
                }
                else
                {
                    returningValue = "BeginCondition\r\n" + returningValue + "\r\nEndCondition";
                }

            }

            return returningValue;
        }

        /// <summary>
        /// List&lt;string&gt; → List&lt;string[]&gt;に変換します。
        /// 変換前の各列において、「,」カンマで当該列の各行要素として分割します。
        /// addEndCondition：trueで、先頭に"BeginCondition"、末尾に"EndCondition"の1要素が入ったstring[]を追加します。
        /// "none"のみの場合、その1要素のみが入ったList&lt;string[]&gt;を返します。（"BeginCondition"、"EndCondition"は付けません）
        /// </summary>
        /// <param name="convertList"></param>
        /// <param name="addConditionString"></param>
        /// <returns></returns>
        public static List<string[]> EncodeStringListToStringArrayList(List<string> convertList, bool addConditionString)
        {
            var returningValue = new List<string[]>();

            if (convertList.Count == 3 && convertList[1] == "none")
            {
                return new List<string[]> { new string[] { "none" } };
            }

            // BeginCondition を追加
            if (addConditionString)
            { returningValue.Add(new string[] { "BeginCondition" }); }

            // 各要素を追加
            foreach (var targetString in convertList)
            {
                if (targetString != "BeginCondition" && targetString != "EndCondition")
                {
                    var stringArray = targetString.Split(',');
                    returningValue.Add(stringArray);
                }
            }

            // EndCondition を追加
            if (addConditionString)
            { returningValue.Add(new string[] { "EndCondition" }); }

            return returningValue;
        }

        /// <summary>
        /// 文字列 → List&lt;string[]&gt;に変換します。
        /// 変換前文字列において「\r\n」で列、「,」カンマで当該列の各行要素として分割します。
        /// addEndCondition：trueで、先頭に"BeginCondition"、末尾に"EndCondition"の1要素が入ったstring[]を追加します。
        /// "none"のみの場合、その1要素のみが入ったList&lt;string[]&gt;を返します。
        /// </summary>
        /// <param name="convertString"></param>
        /// <returns></returns>
        public static List<string[]> EncodeStringToStringArrayList(string convertString, bool addConditionString)
        {
            var returningValue = new List<string[]>();

            if (convertString == "none")
            {
                return new List<string[]> { new string[] { "none" } };
            }

            if (string.IsNullOrEmpty(convertString))
            {
                return new List<string[]>();
            }

            var targetStringArrays = convertString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            // BeginCondition を追加
            if (addConditionString)
            {
                returningValue.Add(new string[] { "BeginCondition" });
            }

            // targetStringArraysの各要素 を追加
            foreach (var beforeStringArray in targetStringArrays)
            {
                var stringArray = beforeStringArray.Split(',');
                returningValue.Add(stringArray);
            }

            // EndCondition を追加
            if (addConditionString)
            {
                returningValue.Add(new string[] { "EndCondition" });
            }

            return returningValue;
        }

        /// <summary>
        /// ワイルドカードが指定されている場合、そのワイルドカードを反映したパスを返します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="propertiesEnum"></param>
        /// <returns></returns>
        public static string WithoutWildCard(string filePath, WildcardSortEnum propertiesEnum)
        {
            return ReturnPathForWildcard(filePath, propertiesEnum == WildcardSortEnum.Name);
        }

        #region private method

        /// <summary>
        /// 引数が「string[]」の形に変換できるかをチェック
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        private static bool IsStringArray(string targetString)
        {
            var targetStringArrays = targetString.Split(',');
            return targetStringArrays.GetType() == typeof(string[]);
        }

        /// <summary>
        /// 引数が「int[]」の形に変換できるかをチェック
        /// (nullの場合も含む)
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        private static bool IsIntegerArray(string targetString)
        {
            var targetStringArrays = targetString.Split(',');

            foreach (var str in targetStringArrays)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                if (!int.TryParse(str, out int i))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

    }

    public enum WildcardSortEnum
    {
        Date,
        Name,
        none,
    }
}
