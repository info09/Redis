using IdcCommon.CommonMethods;
using IdcCommon.IniFiles.Others;
using System;

namespace IdcCommon.Exceptions
{
    [Serializable()]
    /// <summary>
    /// エラーとなり得る内容を列挙
    /// </summary>
    public class IdcCustomException
    {
        /// <summary>
        /// IDCDataConvert用のEXCEPTION
        /// </summary>
        public class ConvertException : Exception
        {
            public int ExceptionId;

            public ConvertException() { }
            public ConvertException(int Id) : base("")
            {
                ExceptionId = Id;
            }

            public ConvertException(string message, int Id) : base(message)
            {
                ExceptionId = Id;
            }

            protected ConvertException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// テキスト置換用のException
        /// </summary>
        public class IdcCharReplaceException : Exception
        {
            public IdcCharReplaceErrorEnum ExceptionEnum;

            private static string mErrorCode;
            private static string mErrorText;

            public IdcCharReplaceException() { }
            public IdcCharReplaceException(IdcCharReplaceErrorEnum exceptionEnum) : base("")
            {
                this.ExceptionEnum = exceptionEnum;
            }

            public IdcCharReplaceException(string message, IdcCharReplaceErrorEnum exceptionEnum) : base(message)
            {
                this.ExceptionEnum = exceptionEnum;
            }

            protected IdcCharReplaceException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }


            public static string GetExceptionMessage(IdcCharReplaceErrorEnum exceptionEnum)
            {
                switch (exceptionEnum)
                {
                    case IdcCharReplaceErrorEnum.NotDirectionTargetFolder:
                        mErrorCode = "01";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "対象フォルダが指定されていません。"
                                   : "The target folder is not specified.";
                        break;

                    case IdcCharReplaceErrorEnum.NotFoundTargetFolder:
                        mErrorCode = "01";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "対象フォルダが見つかりません。"
                                   : "Target folder not found.";
                        break;

                    case IdcCharReplaceErrorEnum.NotDirectionBeforeTarget:
                        mErrorCode = "03";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "変換前指定が指定されていません。"
                                   : "The pre-conversion specification is not set.";
                        break;

                    case IdcCharReplaceErrorEnum.InvalidBeforeTarget:
                        mErrorCode = "03";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "変換前指定が正しくありません。"
                                   : "The pre-conversion specification is incorrect.";
                        break;

                    case IdcCharReplaceErrorEnum.InvalidAfterTarget:
                        mErrorCode = "04";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "変換後指定が正しくありません。"
                                   : "The post-conversion specification is incorrect.";
                        break;

                    case IdcCharReplaceErrorEnum.InvalidExceptTarget:
                        mErrorCode = "05";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "対象外指定が正しくありません。"
                                   : "The non-target specification is incorrect.";
                        break;

                    case IdcCharReplaceErrorEnum.FailedMakingEscapeFolder:
                        mErrorCode = "07";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "退避フォルダを作成できませんでした。"
                                   : "The save folder could not be created.";
                        break;

                    case IdcCharReplaceErrorEnum.FailedMovingBeforeFile:
                        mErrorCode = "07";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "変換前ファイルを移動できませんでした。"
                                   : "The pre-conversion file could not be moved.";
                        break;

                    case IdcCharReplaceErrorEnum.FailedMakingLogFile:
                        mErrorCode = "08";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "ログファイルを作成できませんでした。"
                                   : "The log file could not be created.";
                        break;

                    case IdcCharReplaceErrorEnum.FailedSaveResult:
                        mErrorCode = "10";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "変換結果を保存できませんでした。"
                                   : "The conversion result could not be saved.";
                        break;

                    case IdcCharReplaceErrorEnum.LicenseAuthInvalid:
                        mErrorCode = "9100";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "ライセンス認証がされていないため変換できません。"
                                   : "Could not convert because the license is not be activated.";
                        break;

                    case IdcCharReplaceErrorEnum.TestErrorCode:
                        mErrorCode = "99";
                        mErrorText = CurrentCulture.IsCultureJa
                                   ? "テストエラーコード9999"
                                   : "Test error code 9999.";
                        break;

                    default:
                        return string.Empty;
                }

                return string.IsNullOrEmpty(mErrorText)
                       ? string.Empty
                       : string.Format("{0}:{1}", mErrorCode, mErrorText);
            }
        }

        /// <summary>
        /// 読み込み失敗等は、全てこのクラスで処理を行う。
        /// </summary>
        public class CsvFileReadFailedException : Exception
        {
            public CsvFileReadFailedException() : base() { }

            public CsvFileReadFailedException(string message) : base(message) { }

            protected CsvFileReadFailedException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// CSVへの書き込み失敗時、全てこのクラスで処理を行う。
        /// </summary>
        public class CsvFileWriteFailedException : Exception
        {
            public CsvFileWriteFailedException() : base() { }

            public CsvFileWriteFailedException(string message) : base(message) { }

            protected CsvFileWriteFailedException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// ダブルクォーテーションが文字列に含まれている場合、このクラスで処理を行う。
        /// </summary>
        public class IncludeDoubleQuotationException : Exception
        {
            public IncludeDoubleQuotationException() : base() { }

            public IncludeDoubleQuotationException(string message) : base(message) { }

            protected IncludeDoubleQuotationException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }


        /// <summary>
        /// 文字列先頭にダブルクォーテーションが使用されている場合、このクラスで処理を行う。
        /// </summary>
        public class BeginningDoubleQuotationException : Exception
        {
            public BeginningDoubleQuotationException() : base() { }

            public BeginningDoubleQuotationException(string message) : base(message) { }

            protected BeginningDoubleQuotationException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// エンコードIDが存在しない場合、このクラスで処理を行う。
        /// </summary>
        public class InvalidEncodeException : Exception
        {
            public InvalidEncodeException() : base() { }

            public InvalidEncodeException(string message) : base(message) { }

            protected InvalidEncodeException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// 間違ったiniファイルを読み込んだ場合、このクラスで処理を行う。
        /// </summary>
        public class InvalidIniFileReadException : Exception
        {
            public InvalidIniFileReadException() : base() { }

            public InvalidIniFileReadException(string message) : base(message) { }

            protected InvalidIniFileReadException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }


        /// <summary>
        /// Ver変換に失敗した場合、このクラスで処理を行う。
        /// </summary>
        public class VersionConvertFailedException : Exception
        {
            public VersionConvertFailedException() : base() { }

            public VersionConvertFailedException(string message) : base(message) { }

            protected VersionConvertFailedException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }


        /// <summary>
        /// 間違ったデータの場合、このクラスで処理を行う。
        /// </summary>
        public class InvalidDataValueException : Exception
        {
            public InvalidDataValueException() : base() { }

            public InvalidDataValueException(string message) : base(message) { }

            protected InvalidDataValueException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// データの個数が間違っている場合、このクラスで処理を行う。
        /// </summary>
        public class InvalidDataVolumeException : Exception
        {
            public InvalidDataVolumeException() : base() { }

            public InvalidDataVolumeException(string message) : base(message) { }

            protected InvalidDataVolumeException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// Version表記が間違っている場合、このクラスで処理を行う。
        /// </summary>
        public class InvalidVersionException : Exception
        {
            public InvalidVersionException() : base() { }

            public InvalidVersionException(string message) : base(message) { }

            protected InvalidVersionException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// 実装が間違っている場合、このクラスで処理を行う。
        /// </summary>
        public class InvalidImplementException : Exception
        {
            public InvalidImplementException(string message) : base(message) { }

            protected InvalidImplementException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// 意図しないファイルを読み込んでしまった場合、このクラスで処理を行う。
        /// </summary>
        public class InvalidFileReadException : Exception
        {
            public InvalidFileReadException() : base() { }

            public InvalidFileReadException(string message) : base(message) { }

            protected InvalidFileReadException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        /// <summary>
        /// テキスト置換の起動が不正な場合、このクラスで処理を行う。
        /// </summary>
        public class UnauthorizedStartIdcCharReplaceException : Exception
        {
            public UnauthorizedStartIdcCharReplaceException() : base() { }

            public UnauthorizedStartIdcCharReplaceException(string message) : base(message) { }

            protected UnauthorizedStartIdcCharReplaceException(
             System.Runtime.Serialization.SerializationInfo info,
             System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
    }
}
