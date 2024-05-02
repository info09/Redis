using IdcCommon.CommonMethods;
using System;
using System.Collections.Generic;
using System.IO;
using static IdcCommon.Exceptions.IdcCustomException;

namespace IdcCommon.IniFiles
{
    [Serializable]
    public abstract class IdcAbstractIniFile
    {

        #region Non-abstract variables

        /// <summary>
        /// iniファイルより読み込んだiniファイルの書式Ver
        /// </summary>
        public string IniFileNameAndVersionFromIniFileRead;

        /// <summary>
        /// iniファイルより読み込んだiniファイルの名前
        /// </summary>
        public string ReadIniFileName;

        /// <summary>
        /// iniファイルより読み込んだiniファイルのVer
        /// </summary>
        public Version ReadIniFileVersion;

        /// <summary>
        /// iniファイルより読み込んだパラメーターの個数
        /// </summary>
        public int ReadIniFileParamsVol;

        /// <summary>
        /// 読み込んだiniファイルにおいて、読み込んだiniファイルの番号（何番目か）と、そのパラメーターListを保持します
        /// </summary>
        public Dictionary<int, List<string>> ReadFileDescriptionAndParams;

        /// <summary>
        /// 参照iniファイルのフルパス
        /// </summary>
        public string ReadIniFileFullPath;

        /// <summary>
        /// iniファイルに書き込むデータ
        /// </summary>
        /// <returns></returns>
        public string WriteData;

        /// <summary>
        /// 書き込み先iniファイルのフルパス
        /// </summary>
        public string WriteIniFileFullPath;

        /// <summary>
        /// none値（読み取り専用）
        /// </summary>
        public static readonly string NoneString = "none";

        /// <summary>
        /// 数値の場合のnone値（読み取り専用）
        /// </summary>
        public static readonly int NoneInteger = 0;

        /// <summary>
        /// List&lt;int&gt; の場合のnone値（読み取り専用）
        /// </summary>
        public static readonly List<int> NoneListInteger = new List<int>();

        /// <summary>
        /// List&lt;string&gt; の場合のnone値（読み取り専用）
        /// </summary>
        public static readonly List<string> NoneListString = new List<string>() { NoneString };

        /// <summary>
        /// List&lt;string[]&gt; の場合のnone値（読み取り専用）
        /// </summary>
        public static readonly List<string[]> NoneListStringArray = new List<string[]> { new string[] { NoneString } };

        #region private values

        protected string mErrorText;

        protected string mErrorTitle;

        #endregion


        #endregion

        #region abstract variables

        /// <summary>
        /// 最新のiniファイルのVersion(property)
        /// </summary>
        public abstract Version IniFileLatestVersion { get; }

        /// <summary>
        /// iniファイルの名前
        /// </summary>
        public abstract string IniFileName { get; }

        #endregion

        #region common methods

        /// <summary>
        /// ファイルを読み込みます。
        /// </summary>
        public void Import()
        {
            try
            {
                this.GetReadFileDescriptionAndParams();
                this.ConvertParams();
                this.SetProperties();
            }
            catch (InvalidIniFileReadException)
            { throw; }
            catch (InvalidDataVolumeException)
            { throw; }
            catch (InvalidVersionException)
            { throw; }
            catch (VersionConvertFailedException)
            { throw; }
            catch (Exception ex)
            { throw new InvalidFileReadException(ex.Message); }
        }

        /// <summary>
        /// パラメーターを変換します。
        /// </summary>
        public void ConvertParams()
        {
            try
            {
                this.ConvertParametersWhenReadFileIsVer1();
                this.ConvertParametersWhenReadFileIsVer2();
                this.ConvertParametersWhenReadFileIsVer3AndNotLatest();
            }
            catch (Exception ex)
            {
                this.mErrorText = CurrentCulture.IsCultureJa
                                ? string.Format("Ver3用にパラメーターを変換しようとしましたが、以下の理由により変換に失敗しました。\r\nエラー原因：{0}", ex.Message)
                                : string.Format("The conversion of parameters for IDC version 3 for the following reasons.\r\nError cause:{0}", ex.Message);
                throw new VersionConvertFailedException();
            }
        }

        /// <summary>
        /// ファイルを読み込み、ReadFileDescriptionAndParams に値を入れ込みます。
        /// </summary>
        public virtual void GetReadFileDescriptionAndParams()
        {
            var readData = FileReadWrite.Read(this.ReadIniFileFullPath, Encode.GetEncoding(CurrentCulture.IsCultureJa ? EncodingEnum.sjis : EncodingEnum.utf8));
            if (readData.Count < 1)
            { throw new InvalidIniFileReadException(); }

            this.IniFileNameAndVersionFromIniFileRead = readData[0];

            this.MakeIniFileVersionFromReadIniFile();

            this.ReadFileDescriptionAndParams = new Dictionary<int, List<string>>();

            var descNo = 0;
            var parameterList = new List<string>();


            for (var i = 1; i < readData.Count; i++)
            {
                if (readData[i].Replace(" ", string.Empty).StartsWith("//****"))
                {
                    if (descNo > 0)
                    {
                        this.ReadFileDescriptionAndParams.Add(descNo, parameterList);
                        parameterList = new List<string>();
                    }
                    descNo++;
                }
                else if (!readData[i].StartsWith("//"))
                {
                    parameterList.Add(readData[i]);

                    if (i == readData.Count - 1)
                    {
                        this.ReadFileDescriptionAndParams.Add(descNo, parameterList);
                    }
                }
                else
                {
                    if (i == readData.Count - 1)
                    {
                        this.ReadFileDescriptionAndParams.Add(descNo, parameterList);
                    }
                }
            }

            for (var i = 1; i <= this.ReadFileDescriptionAndParams.Count; i++)
            {
                if (this.ReadFileDescriptionAndParams[i].Count == 0)
                { this.ReadFileDescriptionAndParams.Remove(i); }
            }

            this.ReadIniFileParamsVol = this.ReadFileDescriptionAndParams.Count;
        }

        /// <summary>
        /// 読み込まれたiniファイルをチェックします。
        /// </summary>
        public void CheckReadFile()
        {
            if (!this.CheckIniFileName())
            {
                this.mErrorText = CurrentCulture.IsCultureJa
                                ? "名称の異なるiniファイルが読み込まれました。対象のiniファイルを確認してください。"
                                : "The invalid name INI file is loaded. Please check the target INI file.";
                throw new InvalidIniFileReadException(this.mErrorText);
            }

            if (!this.CheckItemsVolume())
            {
                this.mErrorText = CurrentCulture.IsCultureJa
                                ? "読み込まれたiniファイルのパラメーター数が異なります。対象のiniファイルを確認してください。"
                                : "The number of parameters of loaded INI file is invalid. Please check the target INI file.";
                throw new InvalidDataVolumeException(this.mErrorText);
            }
        }

        /// <summary>
        /// 各具象クラスで定義された、最新Versionのファイル名文字列を取得。
        /// </summary>
        /// <returns></returns>
        public string GetIniFileNameLatestVersion()
        {
            return string.Format("{0}Ver{1:00}.{2:00}.{3:00}", this.IniFileName, this.IniFileLatestVersion.Major, this.IniFileLatestVersion.Minor, this.IniFileLatestVersion.Build);
        }

        /// <summary>
        /// writeData に格納された内容を書き込みます。
        /// </summary>
        /// <param name="writeData"></param>
        public void WriteFile()
        {
            FileReadWrite.Write(this.WriteData, this.WriteIniFileFullPath, false, Encode.GetEncoding(CurrentCulture.IsCultureJa ? EncodingEnum.sjis : EncodingEnum.utf8));
        }

        /// <summary>
        /// ファイル存在チェック
        /// </summary>
        /// <returns></returns>
        public bool IsExist()
        {
            return File.Exists(this.ReadIniFileFullPath);
        }

        /// <summary>
        /// 項目チェック
        /// </summary>
        public bool CheckItems()
        {
            return this.CheckItemsVolume();
        }

        #endregion

        #region abstract methods

        /// <summary>
        /// 読み込んだファイルのVersionがVer1だった場合、パラメーターをVer3用に改定。
        /// 具象メソッド内でVersionを検証し、その上でパラメーターを改訂するように実装してください。
        /// </summary>
        protected abstract void ConvertParametersWhenReadFileIsVer1();

        /// <summary>
        /// 読み込んだファイルのVersionがVer2だった場合、パラメーターをVer3用に改定。
        /// 具象メソッド内でVersionを検証し、その上でパラメーターを改訂するように実装してください。
        /// </summary>
        protected abstract void ConvertParametersWhenReadFileIsVer2();

        /// <summary>
        /// 読み込んだファイルのVersionがVer3だがMinor, BuildVerが最新でない場合、パラメーターをVer3最新版用に改定。
        /// 具象メソッド内でVersionを検証し、その上でパラメーターを改訂するように実装してください。
        /// </summary>
        protected abstract void ConvertParametersWhenReadFileIsVer3AndNotLatest();

        /// <summary>
        /// 項目数チェックを行い、結果を返送。
        /// </summary>
        protected abstract bool CheckItemsVolume();

        /// <summary>
        /// 具象クラスのプロパティに値をセットします。
        /// </summary>
        public abstract void SetProperties();

        /// <summary>
        /// デフォルト値となる各プロパティを設定します。
        /// </summary>
        public abstract void MakeDefaultValue();

        /// <summary>
        /// iniファイルに書き込むデータを作成・成型します。
        /// 必要に応じて、None値等の挿入も行います。
        /// </summary>
        public abstract void MakeWriteData();

        /// <summary>
        /// 具象クラスの各値のValidateを行います。
        /// 正しくない値の場合、Falseを返します。
        /// </summary>
        public abstract bool Validate();

        #endregion

        #region private methods

        private void MakeIniFileVersionFromReadIniFile()
        {
            try
            {
                this.ReadIniFileName = this.IniFileNameAndVersionFromIniFileRead.Substring(0,this.IniFileNameAndVersionFromIniFileRead.Length - 11);
                var versionArray = this.IniFileNameAndVersionFromIniFileRead.Substring(this.IniFileNameAndVersionFromIniFileRead.Length - 8).Split('.');
                this.ReadIniFileVersion = new Version(int.Parse(versionArray[0]), int.Parse(versionArray[1]), int.Parse(versionArray[2]));
            }
            catch (Exception)
            {
                this.mErrorText = CurrentCulture.IsCultureJa
                                ? "読み込まれたiniファイルのパラメーター数が異なります。対象のiniファイルを確認してください。"
                                : "The version of loaded INI file is invalid. Please check the target INI file.";
                throw new InvalidDataVolumeException(this.mErrorText);
            }
        }

        /// <summary>
        /// iniファイルの名称をチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckIniFileName()
        {
            return this.GetIniFileNameLatestVersion().Contains(this.ReadIniFileName);
        }

        #endregion

    }
}
