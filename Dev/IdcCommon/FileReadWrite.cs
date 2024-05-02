using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static IdcCommon.Exceptions.IdcCustomException;

namespace IdcCommon.CommonMethods
{
    /// <summary>
    /// ファイルの読み書きを行います。（静的実装）
    /// </summary>
    public static class FileReadWrite
    {
        private static string mErrorText;

        /// <summary>
        /// targetFileFullPathの内容を、List&lt;string&gt;で返送します。（エンコードはデフォルトエンコードで固定）
        /// </summary>
        /// <param name="targetFileFullPath"></param>
        /// <returns></returns>
        public static List<string> ReadByDefaultEncode(string targetFileFullPath)
        {
            var returningValue = new List<string>();

            try
            {
                // iniファイルより、デフォルトエンコードをセット。
                IniFiles.Others.DefaultEncoding.SetEncodingFromIniFile();
                // エンコードを以下の通り代入。
                var defaultEncode = IniFiles.Others.DefaultEncoding.GetEncode();

                using (var streamReader = new StreamReader(targetFileFullPath, defaultEncode))
                {
                    while (!streamReader.EndOfStream)
                    { returningValue.Add(streamReader.ReadLine()); }
                }
            }
            catch (Exception ex)
            {
                mErrorText = CurrentCulture.IsCultureJa
                           ? string.Format("ファイル読み込みに失敗しました。\r\nエラー：{0}", ex.Message)
                           : string.Format("Failed to read the file.\r\nError:{0}", ex.Message);
                throw new CsvFileReadFailedException(mErrorText);
            }

            return returningValue;
        }

        /// <summary>
        /// targetFileFullPathの内容を、List&lt;string&gt;で返送します。（エンコード指定可）
        /// </summary>
        /// <param name="targetFileFullPath"></param>
        /// <returns></returns>
        public static List<string> Read(string targetFileFullPath, Encoding encoding)
        {
            var returningValue = new List<string>();
            if (!File.Exists(targetFileFullPath)) 
            {return returningValue; }

            try
            {
                using (var streamReader = new StreamReader(targetFileFullPath, encoding))
                {
                    while (!streamReader.EndOfStream)
                    { returningValue.Add(streamReader.ReadLine()); }
                }
            }
            catch (Exception ex)
            {
                mErrorText = CurrentCulture.IsCultureJa
                           ? string.Format("ファイル読み込みに失敗しました。\r\nエラー：{0}", ex.Message)
                           : string.Format("Failed to read the file.\r\nError:{0}", ex.Message);
                throw new CsvFileReadFailedException(mErrorText);
            }

            return returningValue;
        }

        /// <summary>
        /// targetFileFullPathの内容を、stringで返送します。（エンコード指定可）
        /// </summary>
        /// <param name="targetFileFullPath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadAndGetByString(string targetFileFullPath, Encoding encoding)
        {
            var returningValue = string.Empty;

            try
            {
                using (var streamReader = new StreamReader(targetFileFullPath, encoding))
                { return streamReader.ReadToEnd(); }
            }
            catch (Exception ex)
            {
                mErrorText = CurrentCulture.IsCultureJa
                           ? string.Format("ファイル読み込みに失敗しました。\r\nエラー：{0}", ex.Message)
                           : string.Format("Failed to read the file.\r\nError:{0}", ex.Message);
                throw new CsvFileReadFailedException(mErrorText);
            }
        }

        /// <summary>
        /// targetContensの内容を、targetFullPathで指定されたファイルに書き込みます。（エンコード指定可）
        /// </summary>
        /// <param name="targetContents">記載文字列</param>
        /// <param name="targetFullPath">記載先ファイル名のフルパス</param>
        /// <param name="allowOverWrite">true：追記をします。</param>
        /// <param name="encoding">エンコード指定</param>
        public static void Write(string targetContents, string targetFullPath, bool allowOverWrite, Encoding encoding)
        {
            using (var streamWriter = new StreamWriter(targetFullPath, allowOverWrite, encoding))
            {
                try
                {
                    streamWriter.WriteLine(targetContents);
                }
                catch (Exception ex)
                {
                    mErrorText = CurrentCulture.IsCultureJa
                               ? string.Format("ファイル書き込みに失敗しました。\r\nエラー：{0}", ex.Message)
                               : string.Format("Failed to write the file.\r\nError:{0}", ex.Message);
                    throw new CsvFileReadFailedException(mErrorText);
                }
            }
        }

        /// <summary>
        /// 指定したパスにディレクトリが存在しない場合
        /// すべてのディレクトリとサブディレクトリを作成します
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 指定したパスにファイルが存在しない場合
        /// ファイルを作成します。
        /// </summary>
        public static FileStream SafeCreateFile(string path)
        {
            if (File.Exists(path)) { return null; }

            return File.Create(path);
        }

        /// <summary>
        /// 対象ファイルがロックされているかを確認します。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string path)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }
    }
}
