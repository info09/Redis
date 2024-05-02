using Microsoft.Win32;
using System.IO;

namespace IdcCommon.CommonMethods
{
    public static class Registry
    {

        private static readonly string[] registryHelpFileNames = CurrentCulture.IsCultureJa
                                                               ? new string[]
                                                               {
                                                                   "",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ0-1：基本設定-ランチャーパスワード設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ0-2：基本設定-デフォルトエンコード設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ0-3：基本設定-メール送信設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ0：基本設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-0-1：ファイル変換設定-バイト数設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-0-2：ファイル変換設定-シート読み取り設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-10：ファイル変換設定-レコード追加（条件付）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-11：ファイル変換設定-レコード追加（条件項目）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-12：ファイル変換設定-アグリゲート変換.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-13：ファイル変換設定-アグリゲート変換（行追加）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-14：ファイル変換設定-アグリゲート変換（列追加）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-15：ファイル変換設定-アグリゲート変換（判定項目）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-16：ファイル変換設定-アグリゲート変換（対象外）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-17：ファイル変換設定-四則演算.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-18：ファイル変換設定-インサート変換.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-19：ファイル変換設定-条件指定除外.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-1：ファイル変換設定-マージ.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-20：ファイル変換設定-行パターン除外.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-2：ファイル変換設定-マージ（条件付）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-3：ファイル変換設定-項目一致条件の設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-4：ファイル変換設定-マトリックス変換設定（ML）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-5：ファイル変換設定-マトリックス変換設定（LM）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-6：ファイル変換設定-位相変換.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-7：ファイル変換設定-ブロック変換（分割）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-8：ファイル変換設定-ブロック変換（結合）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1-9：ファイル変換設定-レコード追加.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ1：ファイル変換設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ20：ランチャーメニュー（管理者モード）.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ2：変換テーブル登録.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-0-1：レコード変換設定-変換項目追加.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-0-2：レコード変換設定-変換追加.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-0-3：レコード変換設定-ファイル一覧.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-0-4：レコード変換設定-ソート指定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-0-5：レコード変換設定-変換編集.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-0-6：レコード変換設定-レジスタ確認.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-10：レコード変換設定-条件付.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-11：レコード変換設定-変換テーブル参照.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-12：レコード変換設定-文字列追加.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-13：レコード変換設定-コマンド.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-14：レコード変換設定-文字形式変換.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-15：レコード変換設定-代入.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-16：レコード変換設定-除去.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-17-1：レコード変換設定-シート読み取り設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-17：レコード変換設定-リストファイル参照.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-18：レコード変換設定-文字列長出力.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-19：レコード変換設定-ランダム文字作成.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-1：レコード変換設定-四則演算.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-2：レコード変換設定-項目結合.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-3：レコード変換設定-囲み文字制御.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-4：レコード変換設定-日付表示方法.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-5：レコード変換設定-文字数制限.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-6：レコード変換設定-バイト数制限.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-7：レコード変換設定-金額表示方法.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-8：レコード変換設定-文字列置換.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3-9：レコード変換設定-自動番号.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ3：レコード変換設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ4-1：実行計画-ファイル変換設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ4-2：実行計画-レコード変換設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ4-3：実行計画-メール送信設定.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ4-4：実行計画-設定チェック.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ4：実行計画.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ5：テキスト置換.pdf",
                                                                   @"\HELP\【ICSデータコンバータ】ヘルプ4-5：実行計画-設定内容出力.pdf",

                                                               }
                                                               : new string[]
                                                               {
                                                                   "",
                                                                   @"\HELP\[ICS Data Converter] Help 0-1 Launcher Password Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 0-2 Basic Settings - Default Encoding Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 0-3 Basic Setings - Email Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 0 - Basic Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-0-1 File Conversion Settings - Byte Count Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-0-2 File Conversion Settings - Reading Sheets Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-10 File Conversion Settings - Records addition (with conditions).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-11 File Conversion Settings - Records addition (Condition items).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-12 File Conversion Settings - Aggregate Conversion.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-13 File Conversion Settings - Aggregate Conversion (Rows Addition).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-14 File Conversion Settings - Aggregate Conversion (Columns Addition).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-15 File Conversion Settings - Aggregate Conversion (Judge Items).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-16 File Conversion Settings - Aggregate Conversion (Exclusions).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-17 File Conversion Settings - Four Arithmetic Operations.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-18 File Convversion Settings - Insert Conversion.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-19 File Conversion Settings - Exclude Certain Conditions.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-1 File Conversion Settings - Merge.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-20 File Conversion Settings - Exclude Patterns.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-2 File Conversion Settings - Merge (with condition).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-3 File Conversion Settings - Item Matching Condition Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-4 File Conversion Settings - Matrix Conversion Settings (ML).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-5 File Conversion Settings - Matrix Conversion Settings (LM).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-6 File Conversion Settings - Phase Conversion.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-7 File Conversion Settings - Block Conversion (Split).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-8 File Conversion Settings - Block Conversion (Combine).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1-9  File Conversion Settings - Records addition.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 1 File Conversion Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 20  Launcher Menu (Administrator Mode).pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 2 Conversion Table Registeration.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-0-1 Record Conversion Settings - Conversion Item Addition.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-0-2 Record Conversion Settings - Conversion Addition.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-0-3 Record Conversion Settings - File List.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-0-4 Record Conversion Settings - Sorting.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-0-5  Record Conversion Settings - Conversion Editing.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-0-6 Record Conversion Settings - Register Confirmation.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-10 Record Conversion Settings - With Conditions.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-11 Record Conversion Settings - Conversion Tables Referenced.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-12 Record Conversion Settings - Text Strings Addition.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-13 Record Conversion Settings - Command.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-14 Record Conversion Settings - Character Format.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-15 Record Conversion Settings - Substitution.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-16 Record Conversion Settings - Removal.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-17-1 Record Conversion Settings - Reading Sheets Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-17 Record Conversion Settings - Refer to List Files.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-18 Record Conversion Settings - Output of Text String Length.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-19 Record Conversion Settings - Generate Random String.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-1 Record Conversion Settings - Four Arithmetic Operations.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-2 Record Conversion Settings - Item Bonding.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-3 Record Conversion Settings - Enclosed Alphanumerics Control.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-4 Record Conversion Settings - Date Format.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-5 Record Conversion Settings - Text Strings Limit.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-6 Record Conversion Settings - Byte Count Limit.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-7 Record Conversion Settings - Accounting Format.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-8 Record Conversion Settings - Text Strings Replacement.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3-9 Record Conversion Settings - Auto-Numbering.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 3 Record Conversion Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 4-1 Execution Plans - File Conversion Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 4-2  Execution Plans - Record Conversion Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 4-3 Execution Plans - Email Sending Settings.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 4-4 Execution Plans - Settings Check.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 4 Execution Plans.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 5 Text Replacement.pdf",
                                                                   @"\HELP\[ICS Data Converter] Help 4-5 Execution Plans - Output of Settings.pdf",
                                                               };

        public static string IniFolder { get; private set; } = "";

        public static string GetRegistryHelp(int index)
        {
            return registryHelpFileNames[index];
        }

        /// <summary>
        /// PTNdirのフルパス
        /// </summary>
        public static string RegistryPattern { get; private set; } = "";

        /// <summary>
        /// LOGdirのフルパス
        /// </summary>
        public static string RegistryLog { get; private set; } = "";

        /// <summary>
        /// CONVdirのフルパス
        /// </summary>
        public static string IcsConvDirectory { get; private set; } = "";

        /// <summary>
        /// PRGdirのフルパス
        /// </summary>
        public static string ProgramDirectory { get; private set; } = "";

        /// <summary>
        /// アドオンのフルパス
        /// </summary>
        public static string AddonPath
        {
            get { return ProgramDirectory + @"\Addon"; }
        }

        /// <summary>
        /// レジストリキー（マシン）
        /// </summary>
        public static readonly RegistryKey RegistryKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);


        public static string MemberPath
        {
            get { return AddonPath + @"\member"; }
        }

        public static void GetRegistry()
        {
            string keyName = @"SOFTWARE\（株）ＩＣＳパートナーズ\IDC";
            string rGetValueName = "";
            var rKey = RegistryKeyLocalMachine.OpenSubKey(keyName);
            rGetValueName = "PTNdir";
            RegistryPattern = (string)rKey.GetValue(rGetValueName);
            rGetValueName = "LOGdir";
            RegistryLog = (string)rKey.GetValue(rGetValueName);
            rGetValueName = "PRGdir";
            ProgramDirectory = (string)rKey.GetValue(rGetValueName);
            rGetValueName = "CONVdir";
            IcsConvDirectory = (string)rKey.GetValue(rGetValueName);

            //INIファイル（color、前回利用フォルダ）
            IniFolder = Path.GetFullPath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\INI");
        }
    }
}
