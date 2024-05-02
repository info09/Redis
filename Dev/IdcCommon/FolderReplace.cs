using IdcCommon.CommonMethods;
using System;
using System.IO;
using System.Text;
using static IdcCommon.Exceptions.IdcCustomException;

namespace IdcCommon.IniFiles.Others
{
    /// <summary>
    /// テキスト置換iniファイルのクラス
    /// </summary>
    public static partial class FolderReplace
    {
        /// <summary>
        /// 01:対象フォルダ指定
        /// </summary>
        public static string TargetFolder { get; set; }

        /// <summary>
        /// 02:使用するエンコード
        /// </summary>
        public static string FileEncode { get; set; }

        /// <summary>
        /// 03:改行文字削除指定
        /// </summary>
        public static DirectionEnum ConvertTargetDirection { get; set; }

        /// <summary>
        /// 04:変換前
        /// </summary>
        public static string BeforeConvertChar { get; set; }

        /// <summary>
        /// 05:変換後
        /// </summary>
        public static string AfterConvertChar { get; set; }

        /// <summary>
        /// 06:変換対象外
        /// </summary>
        public static string ExceptConvertChar { get; set; }

        /// <summary>
        /// 07:処理結果メッセージオプション
        /// </summary>
        public static bool ResultMessageOption { get; set; }

        /// <summary>
        /// 08:変換前ファイル保存オプション
        /// </summary>
        public static bool FileSaveBeforeConvertOption { get; set; }

        /// <summary>
        /// 09:ログファイル作成オプション
        /// </summary>
        public static bool MakeLogFileOption { get; set; }

        /// <summary>
        /// 10:処理結果イベントオプション
        /// </summary>
        public static bool ResultEventOption { get; set; }

        /// <summary>
        /// 11:進捗表示指定（コマンド実行のみ）
        /// </summary>
        public static bool ProgressDisplay { get; set; } = false;

    }

    public static partial class FolderReplace
    {
        private static string mFilePath = Roaming.IdcIniRoamingPath + @"\folder-replace.ini";

        /// <summary>
        /// プロパティに値をセットします。
        /// </summary>
        public static void GetFolderReplace()
        {
            try
            {
                var readData = FileReadWrite.Read(mFilePath, Encode.GetEncoding(CurrentCulture.IsCultureJa ? EncodingEnum.sjis : EncodingEnum.utf8));

                TargetFolder = readData[1];
                FileEncode = readData[4];
                ConvertTargetDirection = readData[7] == "2" ? DirectionEnum.CharEncode : DirectionEnum.Character;
                BeforeConvertChar = readData[10];
                AfterConvertChar = readData[13];
                ExceptConvertChar = readData[16];
                ResultMessageOption = readData[19] == "m";
                FileSaveBeforeConvertOption = readData[22] == "s";
                MakeLogFileOption = readData[25] == "l";
                ResultEventOption = readData[28] == "e";
            }
            catch (Exception)
            {
                TargetFolder = string.Empty;
                DefaultEncoding.SetEncodingFromIniFile();
                FileEncode = Encode.GetEncodeString(DefaultEncoding.GetEncode());
                ConvertTargetDirection = DirectionEnum.Character;
                BeforeConvertChar = string.Empty;
                AfterConvertChar = string.Empty;
                ExceptConvertChar = string.Empty;
                ResultMessageOption = false;
                FileSaveBeforeConvertOption = false;
                MakeLogFileOption = false;
                ResultEventOption = false;
            }
        }

        /// <summary>
        /// プロパティに値をセットします。（コマンド実行の場合）
        /// </summary>
        public static void GetFolderReplaceByCommandExecute()
        {
            TargetFolder = string.Empty;
            DefaultEncoding.SetEncodingFromIniFile();
            FileEncode = Encode.GetEncodeString(DefaultEncoding.GetEncode());
            ConvertTargetDirection = DirectionEnum.Character;
            BeforeConvertChar = string.Empty;
            AfterConvertChar = string.Empty;
            ExceptConvertChar = string.Empty;
            ResultMessageOption = false;
            FileSaveBeforeConvertOption = false;
            MakeLogFileOption = false;
            ResultEventOption = false;
        }

        /// <summary>
        /// folder-replace.iniがあるかをチェックします。
        /// </summary>
        /// <returns></returns>
        public static bool IsExistFolderReplaceIni()
        {
            return File.Exists(mFilePath);
        }

        /// <summary>
        /// プロパティに格納された内容を書き込みます。
        /// </summary>
        public static void SetFolderReplace()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("//  01:対象フォルダ指定");
            stringBuilder.AppendLine(TargetFolder);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  02:ファイルエンコード");
            stringBuilder.AppendLine(FileEncode);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  03:変換対象指定方法");
            stringBuilder.AppendLine(ConvertTargetDirection == DirectionEnum.CharEncode ? "2" : "1");
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  04:変換前");
            stringBuilder.AppendLine(BeforeConvertChar);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  05:変換後");
            stringBuilder.AppendLine(AfterConvertChar);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  06:変換対象外");
            stringBuilder.AppendLine(ExceptConvertChar);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  07:処理結果メッセージオプション");
            stringBuilder.AppendLine(ResultMessageOption ? "m" : string.Empty);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  08:変換前ファイル保存オプション");
            stringBuilder.AppendLine(FileSaveBeforeConvertOption ? "s" : string.Empty);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  09:ログファイル作成オプション");
            stringBuilder.AppendLine(MakeLogFileOption ? "l" : string.Empty);
            stringBuilder.AppendLine("//");
            stringBuilder.AppendLine("//  10:処理結果イベントオプション");
            stringBuilder.AppendLine(ResultEventOption ? "e" : string.Empty);

            FileReadWrite.Write(stringBuilder.ToString(), mFilePath, false, Encode.GetEncoding(CurrentCulture.IsCultureJa ? EncodingEnum.sjis : EncodingEnum.utf8));
        }

        /// <summary>
        /// 値検証を行います。不正な場合、不正内容に応じてIdcCharReplaceException.ExceptionEnumに内容の列挙型をセットした例外をスローします。
        /// 呼び出し元では、IdcCharReplaceException.ExceptionEnumに応じてtry - catchで適宜処理を追記してください。
        /// </summary>
        public static void Validate()
        {
            // 対象フォルダ指定有無
            if (string.IsNullOrEmpty(TargetFolder))
            { throw new IdcCharReplaceException(IdcCharReplaceException.GetExceptionMessage(IdcCharReplaceErrorEnum.NotDirectionTargetFolder), IdcCharReplaceErrorEnum.NotDirectionTargetFolder); }

            // 対象フォルダ有無
            if (!Directory.Exists(TargetFolder))
            { throw new IdcCharReplaceException(IdcCharReplaceException.GetExceptionMessage(IdcCharReplaceErrorEnum.NotFoundTargetFolder), IdcCharReplaceErrorEnum.NotFoundTargetFolder); }

            // 変換前指定有無
            if (string.IsNullOrEmpty(BeforeConvertChar))
            { throw new IdcCharReplaceException(IdcCharReplaceException.GetExceptionMessage(IdcCharReplaceErrorEnum.NotDirectionBeforeTarget), IdcCharReplaceErrorEnum.NotDirectionBeforeTarget); }


            if (ConvertTargetDirection == DirectionEnum.CharEncode)
            {
                // 変換前指定正誤
                try
                { CharConvert.ConvertCharCodeToCharacter(BeforeConvertChar, Encode.GetEncoding(FileEncode)); }
                catch (Exception)
                { throw new IdcCharReplaceException(IdcCharReplaceException.GetExceptionMessage(IdcCharReplaceErrorEnum.InvalidBeforeTarget), IdcCharReplaceErrorEnum.InvalidBeforeTarget); }

                // 変換後指定正誤
                try
                {
                    CharConvert.ConvertCharCodeToCharacter(AfterConvertChar, Encode.GetEncoding(FileEncode));
                }
                catch (Exception)
                { throw new IdcCharReplaceException(IdcCharReplaceException.GetExceptionMessage(IdcCharReplaceErrorEnum.InvalidAfterTarget), IdcCharReplaceErrorEnum.InvalidAfterTarget); }

                // 対象外指定正誤
                try
                {
                    CharConvert.ConvertCharCodeToCharacter(ExceptConvertChar, Encode.GetEncoding(FileEncode));
                }
                catch (Exception)
                { throw new IdcCharReplaceException(IdcCharReplaceException.GetExceptionMessage(IdcCharReplaceErrorEnum.InvalidExceptTarget), IdcCharReplaceErrorEnum.InvalidExceptTarget); }
            }
        }

        public static string GetText(IdcCharReplaceErrorEnum errorEnum)
        {
            string errorNo = string.Empty;
            string errorText = string.Empty;

            switch (errorEnum)
            {
                case IdcCharReplaceErrorEnum.NotDirectionTargetFolder:
                    errorNo = "01";
                    errorText = CurrentCulture.IsCultureJa
                              ? "対象フォルダが指定されていません。"
                              : "The target folder is not specified.";
                    break;

                case IdcCharReplaceErrorEnum.NotFoundTargetFolder:
                    errorNo = "01";
                    errorText = CurrentCulture.IsCultureJa
                              ? "対象フォルダが見つかりません。"
                              : "Target folder not found.";
                    break;

                case IdcCharReplaceErrorEnum.NotDirectionBeforeTarget:
                    errorNo = "03";
                    errorText = CurrentCulture.IsCultureJa
                              ? "変換前指定が指定されていません。"
                              : "The pre-conversion specification is not set.";
                    break;

                case IdcCharReplaceErrorEnum.InvalidBeforeTarget:
                    errorNo = "03";
                    errorText = CurrentCulture.IsCultureJa
                              ? "変換前指定が正しくありません。"
                              : "The pre-conversion specification is incorrect.";
                    break;

                case IdcCharReplaceErrorEnum.InvalidAfterTarget:
                    errorNo = "04";
                    errorText = CurrentCulture.IsCultureJa
                              ? "変換後指定が正しくありません。"
                              : "The post-conversion specification is incorrect.";
                    break;

                case IdcCharReplaceErrorEnum.InvalidExceptTarget:
                    errorNo = "05";
                    errorText = CurrentCulture.IsCultureJa
                              ? "対象外指定が正しくありません。"
                              : "The non-target specification is incorrect.";
                    break;

                case IdcCharReplaceErrorEnum.FailedMakingEscapeFolder:
                    errorNo = "07";
                    errorText = CurrentCulture.IsCultureJa
                              ? "退避フォルダを作成できませんでした。"
                              : "The save folder could not be created.";
                    break;

                case IdcCharReplaceErrorEnum.FailedMovingBeforeFile:
                    errorNo = "07";
                    errorText = CurrentCulture.IsCultureJa
                              ? "変換前ファイルを移動できませんでした。"
                              : "The pre-conversion file could not be moved.";
                    break;

                case IdcCharReplaceErrorEnum.FailedMakingLogFile:
                    errorNo = "08";
                    errorText = CurrentCulture.IsCultureJa
                              ? "ログファイルを作成できませんでした。"
                              : "The log file could not be created.";
                    break;

                case IdcCharReplaceErrorEnum.FailedSaveResult:
                    errorNo = "10";
                    errorText = CurrentCulture.IsCultureJa
                              ? "変換結果を保存できませんでした。"
                              : "The conversion result could not be saved.";
                    break;

                case IdcCharReplaceErrorEnum.InvalidCharCode:
                    errorText = CurrentCulture.IsCultureJa
                              ? "文字コードを変換できません。"
                              : "Could not convert character code.";
                    break;

                case IdcCharReplaceErrorEnum.FileAccessIsForbidden:
                    errorText = CurrentCulture.IsCultureJa
                              ? "ファイルアクセスが拒否されました。"
                              : "File access denied";
                    break;

                case IdcCharReplaceErrorEnum.TestErrorCode:
                default:
                    errorText = CurrentCulture.IsCultureJa
                              ? "その他エラーが発生しました。弊社担当者にお問い合わせください。"
                              : "Unexpected error is occurred.\r\nPlease contact our user site.";
                    break;
            }

            return string.Format("{0}{1}{2}", errorNo, string.IsNullOrEmpty(errorNo) ? string.Empty : (CurrentCulture.IsCultureJa ? "：" : ":"), errorText);
        }
    }

    public enum DirectionEnum
    {
        /// <summary>
        /// 文字
        /// </summary>
        Character = 1,

        /// <summary>
        /// 文字コード
        /// </summary>
        CharEncode = 2,
    }

    public enum IdcCharReplaceErrorEnum
    {
        /// <summary>
        /// 01：対象フォルダが指定されていません。
        /// </summary>
        NotDirectionTargetFolder = 5011,

        /// <summary>
        /// 01：対象フォルダが見つかりません。
        /// </summary>
        NotFoundTargetFolder = 5012,

        /// <summary>
        /// 04：変換前指定が指定されていません。
        /// </summary>
        NotDirectionBeforeTarget = 5031,

        /// <summary>
        /// 04：変換前指定が正しくありません。
        /// </summary>
        InvalidBeforeTarget = 5032,

        /// <summary>
        /// 05：変換後指定が正しくありません。
        /// </summary>
        InvalidAfterTarget = 5041,

        /// <summary>
        /// 06：対象外指定が正しくありません。
        /// </summary>
        InvalidExceptTarget = 5051,

        /// <summary>
        /// 07：退避フォルダを作成できませんでした。
        /// </summary>
        FailedMakingEscapeFolder = 5071,

        /// <summary>
        /// 07：変換前ファイルを移動できませんでした。
        /// </summary>
        FailedMovingBeforeFile = 5072,

        /// <summary>
        /// 08：ログファイルを作成できませんでした。
        /// </summary>
        FailedMakingLogFile = 5081,

        /// <summary>
        /// 10：変換結果を保存できませんでした。
        /// </summary>
        FailedSaveResult = 5101,

        /// <summary>
        /// 文字コードが不正（変換不可能）
        /// </summary>
        InvalidCharCode = 9001,

        /// <summary>
        /// ファイルアクセス拒否
        /// </summary>
        FileAccessIsForbidden = 9002,

        /// <summary>
        /// ライセンス認証がされていない
        /// </summary>
        LicenseAuthInvalid = 9100,

        /// <summary>
        /// テストエラーコード
        /// </summary>
        TestErrorCode = 9999,
    }
}
