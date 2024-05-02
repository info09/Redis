//using GrapeCity.Win.Buttons;
//using FarPoint.Win.Spread;
using IdcRecordConvert.classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using IdcCommon.CommonMethods;

namespace IdcRecordConvert {
    public static class IEnumerableExtensions {
        /// <summary>
        /// 最小値を持つ要素を返します
        /// </summary>
        public static TSource FindMin<TSource, TResult>(
            this IEnumerable<TSource> self,
            Func<TSource, TResult> selector) {
            return self.First(c => selector(c).Equals(self.Min(selector)));
        }

        /// <summary>
        /// 最大値を持つ要素を返します
        /// </summary>
        public static TSource FindMax<TSource, TResult>(
            this IEnumerable<TSource> self,
            Func<TSource, TResult> selector) {
            return self.First(c => selector(c).Equals(self.Max(selector)));
        }
    }

    public enum ConvType {
        hold,

        /// <summary>
        /// 四則演算
        /// </summary>
        Calc,

        /// <summary>
        /// 項目結合
        /// </summary>
        Bond,

        /// <summary>
        /// 囲み文字制御
        /// </summary>
        Kakomi,

        /// <summary>
        /// 日付表示方法
        /// </summary>
        Date,

        /// <summary>
        /// 文字数制限
        /// </summary>
        Ccut,

        /// <summary>
        /// バイト数制限
        /// </summary>
        Bcut,

        /// <summary>
        /// 金額表示方法
        /// </summary>
        Money,

        /// <summary>
        /// 文字列置換
        /// </summary>
        Cchg,

        /// <summary>
        /// 自動番号
        /// </summary>
        Seq,

        /// <summary>
        /// 条件付き
        /// </summary>
        If,

        /// <summary>
        /// テーブル参照
        /// </summary>
        Pat,

        /// <summary>
        /// 文字列追加
        /// </summary>
        Add,

        /// <summary>
        /// コマンド
        /// </summary>
        Cmd,

        /// <summary>
        /// 文字形式変換
        /// </summary>
        Fmt,

        /// <summary>
        /// 代入
        /// </summary>
        Set,

        /// <summary>
        /// 除去
        /// </summary>
        Trim,

        /// <summary>
        /// リストファイル参照
        /// </summary>
        List,

        /// <summary>
        /// 文字列長出力
        /// </summary>
        Len,

        /// <summary>
        /// ランダム文字作成
        /// </summary>
        RNG,
    }

    public enum RecoInd {
        Header,
        ConvTimes,
        ItemNum,
        MotoNum,
        ItemNm,
        ConvType,
        ConvSeq,
    }

    /// ボタンのテキストをカスタマイズできるメッセージボックスです。
    /// 

    public class ButtonTextCustomizableMessageBox {
        private IntPtr hHook = IntPtr.Zero;

        /// 

        /// ボタンに表示するテキストを指定します。
        /// 

        public CustomButtonText ButtonText { get; set; }

        /// 

        /// コンストラクタ。
        /// 

        public ButtonTextCustomizableMessageBox() {
            this.ButtonText = new CustomButtonText();
        }

        /// 

        /// ダイアログボックスを表示します。
        /// 

        //public DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icons)
        //{
        //    try
        //    {
        //        BeginHook();
        //        return MessageBox.Show(text, caption, buttons, icons);
        //    }
        //    finally
        //    {
        //        EndHook();
        //    }
        //}

        /// 

        /// フックを開始します。
        /// 

        void BeginHook() {
            EndHook();
            this.hHook = SetWindowsHookEx(WH_CBT, new HOOKPROC(this.HookProc), IntPtr.Zero, GetCurrentThreadId());
        }

        IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode == HCBT_ACTIVATE) {
                if (this.ButtonText.Abort != null) SetDlgItemText(wParam, ID_BUT_ABORT, this.ButtonText.Abort);
                if (this.ButtonText.Cancel != null) SetDlgItemText(wParam, ID_BUT_CANCEL, this.ButtonText.Cancel);
                if (this.ButtonText.Ignore != null) SetDlgItemText(wParam, ID_BUT_IGNORE, this.ButtonText.Ignore);
                if (this.ButtonText.No != null) SetDlgItemText(wParam, ID_BUT_NO, this.ButtonText.No);
                if (this.ButtonText.OK != null) SetDlgItemText(wParam, ID_BUT_OK, this.ButtonText.OK);
                if (this.ButtonText.Retry != null) SetDlgItemText(wParam, ID_BUT_RETRY, this.ButtonText.Retry);
                if (this.ButtonText.Yes != null) SetDlgItemText(wParam, ID_BUT_YES, this.ButtonText.Yes);

                EndHook();
            }

            return CallNextHookEx(this.hHook, nCode, wParam, lParam);
        }

        /// 

        /// フックを終了します。何回呼んでもOKです。
        /// 

        void EndHook() {
            if (this.hHook != IntPtr.Zero) {
                UnhookWindowsHookEx(this.hHook);
                this.hHook = IntPtr.Zero;
            }
        }

        #region メッセージのテキストのクラス定義

        public class CustomButtonText {
            public string OK { get; set; }
            public string Cancel { get; set; }
            public string Abort { get; set; }
            public string Retry { get; set; }
            public string Ignore { get; set; }
            public string Yes { get; set; }
            public string No { get; set; }
        }

        #endregion

        #region Win32API

        const int WH_CBT = 5;
        const int HCBT_ACTIVATE = 5;

        const int ID_BUT_OK = 1;
        const int ID_BUT_CANCEL = 2;
        const int ID_BUT_ABORT = 3;
        const int ID_BUT_RETRY = 4;
        const int ID_BUT_IGNORE = 5;
        const int ID_BUT_YES = 6;
        const int ID_BUT_NO = 7;

        private delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetDlgItemText(IntPtr hWnd, int nIDDlgItem, string lpString);

        #endregion
    }

    [Serializable]
    public class IdcConvCls {
        public bool IS = true;
        public static bool IsIDCprocess { get; private set; } = false;

        public static void SetIDCprocess() {
            IsIDCprocess = System.Diagnostics.Process.GetCurrentProcess().ProcessName.Substring(0, 3) == "IDC";
        }


        /// <summary>
        /// レコード変換に必要な要素を格納
        /// </summary>
        [Serializable]
        public class CnvMethod {
            /// <summary>
            /// レコード変換の各要素を格納
            /// </summary>
            [Serializable]
            public class Reco {
                /// <summary>
                /// 変換の回数（第nMoto変換か）
                /// </summary>
                public int nMoto = 0;

                /// <summary>
                /// 列名称
                /// </summary>
                public string sName = "";

                /// <summary>
                /// 変換種別（hold, Fmtなど）
                /// </summary>
                public string sType = "";

                /// <summary>
                /// SEQ属性
                /// </summary>
                public int nTypeSeq = 0;

                /// <summary>
                /// 変換後サンプル値
                /// </summary>
                public string sResult = "";

                /// <summary>
                /// 変換に必要なパラメーター
                /// </summary>
                public string[] sParam = new string[] { };

                /// <summary>
                /// 変換に必要なパラメーター（IfTrue/IfFalseでのみ使用するサブパラメーター）
                /// </summary>
                public string[] sParamSub1 = new string[] { };

                /// <summary>
                /// 変換に必要なパラメーター（IfTrue/IfFalseでのみ使用するサブパラメーター）
                /// </summary>
                public string[] sParamSub2 = new string[] { };

                public Reco() { }

                public Reco(string[] sAry) {
                    nMoto = StaticFunctionClass.RetstringToInt(sAry[(int)RecoInd.MotoNum], 0);
                    sName = sAry[(int)RecoInd.ItemNm];
                    sType = sAry[(int)RecoInd.ConvType];
                    nTypeSeq = StaticFunctionClass.RetstringToInt(sAry[(int)RecoInd.ConvSeq], 0);
                }
            }

            /// <summary>
            /// CnvMethodのインターフェース
            /// </summary>
            public interface IbaseConvMethod {
                bool ChkPrmCnt();
                string[] ReturnParam();

                string Conv(string[] sMoto, string def);

            }

            /// <summary>
            /// 変換設定の抽象クラス
            /// </summary>
            public abstract class baseConv : IbaseConvMethod {
                public Dictionary<int, string> mRegister;
                public string[] mMotoDataArray;


                public string[] pram = new string[] { };

                public int parameterCnt = 0;

                public string[] RetParm() { return pram; }

                public abstract string Conv(string[] sMoto, string def);

                public void SetParm(int ind, string val) { pram[ind] = val; }

                bool IbaseConvMethod.ChkPrmCnt() {
                    return pram.Length == parameterCnt;
                }

                string[] IbaseConvMethod.ReturnParam() {
                    return RetParm();
                }

                string IbaseConvMethod.Conv(string[] sMoto, string def) {
                    return Conv(sMoto, def);
                }

                protected bool ParseControlTargetToString(string ControlTargetParam, out string targetString) {

                    int index = -1;
                    try {
                        if (ControlTargetParam[0] == 'T') {
                            index = 0;// GetTargetInd(ControlTargetParam, mMotoDataArray);
                            if (index > 0) {
                                targetString = mMotoDataArray[index - 1];
                                return true;
                            }
                        } else if (ControlTargetParam[0] == 'R') {
                            index = RegisterParamToIndex(ControlTargetParam);
                            if (index >= 0) {
                                targetString = mRegister[index];
                                return true;
                            }
                        }

                        targetString = null;
                        return false;

                    } catch (Exception) {
                        targetString = null;
                        return false;
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                /// <returns>0未満はレジスタなし、１以上は使用するINDEX</returns>
                protected int RegisterParamToIndex(string regParam) {
                    if (string.IsNullOrEmpty(regParam)) { return -1; }

                    if (regParam[0] != 'R') { return -1; }

                    if (!int.TryParse(regParam.Replace("R", ""), out int rIndex)) { return -1; }
                    if (rIndex < 0 || rIndex > 9) { return -1; }
                    return rIndex;
                }

                protected bool IsTR(string Param) {
                    return Param == "TR";
                }

                //protected string TRGetting(string getparam)
                //{
                //    if (getparam[0] == 'T')
                //    {

                //    }
                //    else if (getparam[0] == 'R')
                //    {

                //    }
                //    else
                //    {
                //        return null;
                //    }

                //}

                //protected abstract class TargetGatting
                //{
                //    public TargetGatting()
                //    {
                //    }

                //    public abstract bool IsSuccessGetting();


                //    protected int Index;
                //    protected string Target;
                //    internal string TargetValue;

                //    protected abstract int GetTargetIndex();
                //    protected abstract string GetTargetString();
                //}

                //protected class TableGet : TargetGatting
                //{
                //    protected override int GetTargetIndex()
                //    {
                //        throw new NotImplementedException();
                //    }

                //    protected override string GetTargetString()
                //    {
                //        throw new NotImplementedException();
                //    }
                //}

                //protected class RegistryGet : TargetGatting
                //{
                //    protected override int GetTargetIndex()
                //    {
                //        throw new NotImplementedException();
                //    }

                //    protected override string GetTargetString()
                //    {
                //        throw new NotImplementedException();
                //    }
                //}


            }

            /// <summary>
            /// 四則演算の結果設定を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            //public class ConvCalc : baseConv {
            //    //private string[] _prm = new string[] { };
            //    private const int _prmCnt = 6;

            //    public enum prmInd {
            //        Hd,
            //        Seq,
            //        Calc,
            //        Hasu,
            //        syosu,
            //        Register
            //    }

            //    private void init() {
            //        base.parameterCnt = _prmCnt;
            //        base.pram[(int)prmInd.Hd] = ConvType.Calc.ToString();
            //    }

            //    public ConvCalc() {
            //        base.pram = new string[_prmCnt];
            //        init();
            //    }

            //    public ConvCalc(string[] prm) {
            //        base.pram = prm;
            //        init();
            //    }

            //    private double RoundInteger(int value, int decimals) {
            //        // 小数部桁数の10の累乗を取得
            //        double pow = Math.Pow(10, decimals);
            //        return Math.Round(value * pow, MidpointRounding.AwayFromZero) / pow;
            //    }

            //    private double TruncateInteger(double value, int decimals) {
            //        // 小数部桁数の10の累乗を取得
            //        double pow = Math.Pow(10, decimals);
            //        return Math.Truncate(value * pow) / pow;
            //    }

            //    private decimal TruncateInteger(decimal value, int decimals) {
            //        // 小数部桁数の10の累乗を取得
            //        decimal pow = (decimal)Math.Pow(10, decimals);
            //        return Math.Truncate(value * pow) / pow;
            //    }
            //    private decimal TruncateInteger(int value, int decimals) {
            //        // 小数部桁数の10の累乗を取得
            //        decimal pow = (decimal)Math.Pow(10, decimals);
            //        return Math.Truncate(value * pow) / pow;
            //    }

            //    private double CellingInteger(double value, int decimals) {
            //        // 小数部桁数の10の累乗を取得
            //        double pow = Math.Pow(10, decimals);
            //        return Math.Ceiling(value * pow) / pow;
            //    }

            //    private decimal CeilingInteger(decimal value, int decimals) {
            //        // 小数部桁数の10の累乗を取得
            //        decimal pow = (decimal)Math.Pow(10, decimals);
            //        return Math.Ceiling(value * pow) / pow;
            //    }
            //    private decimal CeilingInteger(int value, int decimals) {
            //        // 小数部桁数の10の累乗を取得
            //        decimal pow = (decimal)Math.Pow(10, decimals);
            //        return Math.Ceiling(value * pow) / pow;
            //    }

            //    public override string Conv(string[] sMoto, string def) {
            //        if (string.IsNullOrEmpty(base.pram[(int)prmInd.Calc])) { return def; }
            //        string sRet = "";

            //        int nTargetIndex = 0;
            //        int nOut = 0;
            //        //置換
            //        string sPrm = base.pram[(int)prmInd.Calc];
            //        string sCalc = base.pram[(int)prmInd.Calc];
            //        string sResult = base.pram[(int)prmInd.Calc];



            //        sCalc = sCalc.Replace(" ", "");
            //        //Tの文字列をすべて数値に変換する。
            //        for (int i = 0; i < sCalc.Length; i++) {
            //            string s = sCalc[i].ToString();

            //            if (int.TryParse(s, out nOut)) { continue; }

            //            if (s == "T") {

            //                int ncInd = 0;
            //                while (int.TryParse(sCalc[i + 1].ToString(), out ncInd)) {
            //                    s += sCalc[i + 1].ToString();
            //                    ++i;
            //                    if (sCalc.Length == i + 1) { break; }
            //                }


            //                //ターゲット
            //                //パラメータチェック
            //                nTargetIndex = 0;// GetTargetInd(s);

            //                //対象範囲外
            //                if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

            //                //対象
            //                var re = new Regex(s);
            //                if (double.TryParse(sMoto[nTargetIndex - 1], out double d)) {
            //                    sResult = re.Replace(sResult, d.ToString(), 1);

            //                } else {
            //                    return def;
            //                }

            //                continue;

            //            } else if (s == "R") {

            //                int ncInd = 0;
            //                while (int.TryParse(sCalc[i + 1].ToString(), out ncInd)) {
            //                    s += sCalc[i + 1].ToString();
            //                    ++i;
            //                    if (sCalc.Length == i + 1) { break; }
            //                }


            //                //ターゲット


            //                //パラメータチェック
            //                nTargetIndex = base.RegisterParamToIndex(s);

            //                //レジストリチェック
            //                if (nTargetIndex < 0) {
            //                    return def;
            //                }

            //                //対象
            //                var re = new Regex(s);
            //                if (double.TryParse(base.mRegister[nTargetIndex], out double d)) {
            //                    sResult = re.Replace(sResult, d.ToString(), 1);

            //                } else {
            //                    return def;
            //                }

            //                continue;

            //            }

            //            if (s == ")") { continue; }

            //            if (s == "+" || s == "-" || s == "*" || s == "/" || s == "%" || s == "(" || s == ")" || s == ".") {
            //                if (i == sCalc.Length - 1 && s != ")") { return def; } else if (i == sCalc.Length - 1) { continue; }

            //                string sSecond = sCalc[i + 1].ToString();
            //                if (sSecond == "+" || sSecond == "-" || sSecond == "*" || sSecond == "/" || sSecond == "%" || sSecond == ".") {
            //                    return def;
            //                } else { continue; }

            //            } else {
            //                return def;
            //            }


            //        }

            //        System.Data.DataTable dt = new System.Data.DataTable();

            //        object result = null;
            //        try {
            //            result = dt.Compute(sResult, "");
            //        } catch (System.Data.SyntaxErrorException) {
            //            return def;
            //        } catch (System.Data.EvaluateException) {
            //            return "0";
            //        } catch (DivideByZeroException) {
            //            //少数を含む数値の場合
            //            result = "∞";
            //        } catch (OverflowException) {
            //            sResult = Regex.Replace(
            //                      sResult,
            //                      @"\d+(\.\d+)?",
            //                      m => {
            //                          var x = m.ToString();
            //                          return x.Contains(".") ? x : string.Format("{0}.0", x);
            //                      }
            //                      );

            //            var kariResult = dt.Compute(sResult, "");

            //            if (kariResult.ToString().EndsWith(".0")) { result = decimal.Parse(kariResult.ToString().Replace(".0", string.Empty)); } else { result = kariResult; }
            //        } finally {
            //            if (result.ToString() == "∞") { //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException() { ExceptionId = 3411 }; }
            //            }

            //            //計算結果を文字列へ
            //            sRet = result.ToString();

            //            if (sRet.Contains("E")) {
            //                result = Convert.ToDecimal(result);
            //                sRet = result.ToString();
            //            }

            //            int nIndex = result.ToString().IndexOf(".");
            //            //#1077 小数点桁数が数字以外の場合は処理しない。
            //            if ((int.TryParse(base.pram[(int)prmInd.Hasu], out int roudingLen)) == false) { return def; } //roudingLenはこの行ではダミー。
            //                                                                                                          //#978 小数点桁数が数字以外の場合は処理しない。
            //            if ((int.TryParse(base.pram[(int)prmInd.syosu], out roudingLen)) == false) { return def; }

            //            if (base.pram[(int)prmInd.Hasu] == "0") {
            //                //return sRet;////
            //            } else if (base.pram[(int)prmInd.Hasu] == "3") {
            //                //四捨五入
            //                if (roudingLen == 0) {
            //                    if (result is double) {
            //                        //整数
            //                        sRet = Math.Round((double)result, MidpointRounding.AwayFromZero).ToString();
            //                    } else if (result is decimal) {
            //                        //整数
            //                        sRet = Math.Round((decimal)result, MidpointRounding.AwayFromZero).ToString();
            //                    }
            //                } else if (roudingLen > 0) {
            //                    //指定小数桁数
            //                    if (result is double) {
            //                        //整数
            //                        sRet = Math.Round((double)result, roudingLen).ToString();
            //                    } else if (result is decimal) {
            //                        //整数
            //                        sRet = Math.Round((decimal)result, roudingLen).ToString();
            //                    }
            //                } else {
            //                    //指定整数桁数    
            //                    //まず整数にする。
            //                    int Seisu = 0;

            //                    if (result is double) {
            //                        //整数
            //                        Seisu = int.Parse(Math.Round((double)result, MidpointRounding.AwayFromZero).ToString());
            //                    } else if (result is decimal) {
            //                        //整数
            //                        Seisu = int.Parse(Math.Round((decimal)result, MidpointRounding.AwayFromZero).ToString());
            //                    } else if (result is int) {
            //                        //整数
            //                        Seisu = (int)result;
            //                    }

            //                    sRet = RoundInteger(Seisu, roudingLen).ToString();
            //                }
            //            } else if (base.pram[(int)prmInd.Hasu] == "1") {
            //                //切り捨て
            //                if (roudingLen == 0) {
            //                    if (result is double) {
            //                        //整数
            //                        sRet = Math.Truncate((double)result).ToString();
            //                    } else if (result is decimal) {
            //                        //整数
            //                        sRet = Math.Truncate((decimal)result).ToString();
            //                    }
            //                } else if (roudingLen > 0) {
            //                    //指定小数桁数
            //                    if (result is double) {
            //                        //整数
            //                        sRet = TruncateInteger((double)result, roudingLen).ToString();
            //                    } else if (result is decimal) {
            //                        //整数                                
            //                        sRet = TruncateInteger((decimal)result, roudingLen).ToString();
            //                    }
            //                } else {
            //                    //指定整数桁数    
            //                    //まず整数にする。
            //                    int Seisu = 0;

            //                    if (result is double) {
            //                        //整数
            //                        Seisu = int.Parse(Math.Truncate((double)result).ToString());
            //                    } else if (result is decimal) {
            //                        //整数
            //                        Seisu = int.Parse(Math.Truncate((decimal)result).ToString());
            //                    } else if (result is int) {
            //                        //整数
            //                        Seisu = (int)result;
            //                    }

            //                    sRet = TruncateInteger(Seisu, roudingLen).ToString();
            //                }
            //            } else if (base.pram[(int)prmInd.Hasu] == "2") {
            //                //切り捨て
            //                if (roudingLen == 0) {
            //                    if (result is double) {
            //                        //整数
            //                        sRet = Math.Ceiling((double)result).ToString();
            //                    } else if (result is decimal) {
            //                        //整数
            //                        sRet = Math.Ceiling((decimal)result).ToString();
            //                    }
            //                } else if (roudingLen > 0) {
            //                    //指定小数桁数
            //                    if (result is double) {
            //                        //整数
            //                        sRet = CellingInteger((double)result, roudingLen).ToString();
            //                    } else if (result is decimal) {
            //                        //整数                                
            //                        sRet = CeilingInteger((decimal)result, roudingLen).ToString();
            //                    }
            //                } else {
            //                    //指定整数桁数    
            //                    //まず整数にする。
            //                    int Seisu = 0;

            //                    if (result is double) {
            //                        //整数
            //                        Seisu = int.Parse(Math.Ceiling((double)result).ToString());
            //                    } else if (result is decimal) {
            //                        //整数
            //                        Seisu = int.Parse(Math.Ceiling((decimal)result).ToString());
            //                    } else if (result is int) {
            //                        //整数
            //                        Seisu = (int)result;
            //                    }

            //                    sRet = CeilingInteger(Seisu, roudingLen).ToString();
            //                }
            //            }

            //            //レジスタセット
            //            var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

            //            if (rIndex >= 0) {
            //                mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
            //            }

            //            return sRet;
            //        }
            //    }
            //}
            /// <summary>
            /// 項目結合を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            public class ConvBond : baseConv {
                public enum prmInd {
                    Hd,
                    Seq,
                    bond,
                    Register
                }
                private string[] _prm = new string[] { };
                private const int _prmCnt = 4;
                public ConvBond(string[] prm) {
                    base.pram = prm;
                    init();
                }
                public ConvBond() {
                    base.pram = new string[_prmCnt];
                    init();
                }
                private void init() {
                    base.parameterCnt = _prmCnt;
                    base.pram[(int)prmInd.Hd] = ConvType.Bond.ToString();
                }

                public override string Conv(string[] sMoto, string def) {
                    string sRet = null;
                    try {
                        string sBond = base.pram[(int)prmInd.bond];
                        List<string> lstPulus = new List<string>();
                        List<string> lstChar = new List<string>();
                        List<string> lstItem = new List<string>();
                        base.mMotoDataArray = sMoto;
                        string str = "";

                        if (sBond.Length <= 0) { return def; }

                        for (int i = 0; i < sBond.Length; i++) {
                            //1文字取得
                            char c = sBond[i];

                            if (char.IsWhiteSpace(c)) { continue; }

                            //Itemかどうか。
                            if (c == 'T' || c == 'R') {
                                char citem = sBond[i + 1];
                                string sItmInd = "";
                                //数値が終わるまでindexを進める                            
                                while (char.IsNumber(citem)) {
                                    //項目数値のstringにセット
                                    sItmInd += sBond[i + 1];

                                    ++i;

                                    //次文字の取得
                                    if (i + 1 < sBond.Length) { citem = sBond[i + 1]; } else { break; }

                                }

                                //T項目の後に数値が無い場合は、デフォルト値へ
                                if (string.IsNullOrEmpty(sItmInd)) { break; }

                                if (c == 'T') {
                                    lstItem.Add(sItmInd);
                                    int index = int.Parse(sItmInd);
                                    //0以上かつ前変換データ以上にあるかどうか。
                                    if (sMoto.Length >= index && index > 0) {
                                        str += sMoto[index - 1];
                                    }
                                } else {
                                    lstItem.Add(sItmInd);
                                    var strR = "";
                                    if (base.ParseControlTargetToString("R" + sItmInd, out strR)) {
                                        str += strR;
                                    }

                                }


                                continue;
                            }

                            //＋かどうか
                            if (c == '+') {
                                lstPulus.Add(c.ToString());
                                continue;
                            }

                            //'かどうか
                            if (c == '\'') {
                                //str += sItmInd;

                                string sCr = "";
                                char cNow = sBond[i + 1];
                                //数値が終わるまでindexを進める                            
                                while (cNow != '\'') {
                                    //項目数値のstringにセット
                                    sCr += sBond[i + 1];

                                    ++i;

                                    //次文字の取得
                                    if (i + 1 < sBond.Length) { cNow = sBond[i + 1]; } else { break; }

                                }

                                //終了した場合
                                if (cNow == '\'') {
                                    ++i;
                                    lstChar.Add(sCr);
                                    str += sCr;

                                }

                                continue;
                            }

                            //"かどうか
                            if (c == '\"') {
                                string sCr = "";
                                char cNow = sBond[i + 1];
                                //数値が終わるまでindexを進める                            
                                while (cNow != '\"') {
                                    //項目数値のstringにセット
                                    sCr += sBond[i + 1];

                                    ++i;

                                    //次文字の取得
                                    if (i + 1 < sBond.Length) { cNow = sBond[i + 1]; } else { break; }

                                }

                                //終了した場合
                                if (cNow == '\"') {
                                    ++i;
                                    lstChar.Add(sCr);
                                    str += sCr;

                                }

                                continue;
                            }


                        }

                        //＋の個数確認
                        if (lstPulus.Count + 1 == lstItem.Count + lstChar.Count && lstPulus.Count < 10) {

                            sRet = str;

                        }

                    } finally {

                        if (sRet == null) { sRet = def; }
                    }

                    //レジスタセット
                    var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                    if (rIndex >= 0) {
                        mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                    }


                    return sRet;


                }


            }

            /// <summary>
            /// 囲み文字の制御を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            //public class ConvKakomi : baseConv {
            //    public enum prmInd {
            //        Hd,
            //        Seq,
            //        target,
            //        kakomi,
            //        kakunin,
            //        operate
            //    }
            //    private string[] _prm = new string[] { };
            //    private const int _prmCnt = 6;

            //    public ConvKakomi(string[] prm) {
            //        base.pram = prm;
            //        init();
            //    }
            //    public ConvKakomi() {
            //        base.pram = new string[_prmCnt];
            //        init();
            //    }
            //    private void init() {
            //        base.parameterCnt = _prmCnt;
            //        base.pram[(int)prmInd.Hd] = ConvType.Kakomi.ToString();
            //    }


            //    private string sTarget = "";
            //    private string sKakomiChar = "";

            //    public override string Conv(string[] sMoto, string def) {
            //        string sRet = null;
            //        int nTargetIndex = 0;

            //        //対象
            //        //パラメータチェック
            //        nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target]);

            //        //対象範囲外
            //        if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

            //        //対象
            //        sTarget = sMoto[nTargetIndex - 1];

            //        //オプションパラメータチェック
            //        if (!chkOptParam()) { return sTarget; }

            //        //囲み文字チェック
            //        sKakomiChar = RetkakomiChar();

            //        //囲み文字変換
            //        sRet = KakomiConv();

            //        return sRet;

            //    }

            //    private string KakomiConv() {
            //        string sRet = sTarget;

            //        //囲み確認の場合
            //        if (IsKakomiChk()) {
            //            //対で存在するか
            //            if (!string.IsNullOrEmpty(sTarget) && RetKakomiExist()) {
            //                if (!IsAdd()) {
            //                    //削除                                
            //                    sRet = (sRet.Remove(sRet.Length - 1, 1)).Remove(0, 1);

            //                }

            //            } else {
            //                if (IsAdd()) {
            //                    if (string.IsNullOrEmpty(sTarget) || !IsTopExist()) {
            //                        //追加
            //                        sRet = sKakomiChar + sRet;
            //                    }


            //                    if (string.IsNullOrEmpty(sTarget) || !IsLastExist()) {
            //                        //追加
            //                        sRet = sRet + sKakomiChar;

            //                    }
            //                }
            //            }

            //            //sRet = sTarget;
            //        } else {
            //            if (IsAdd()) {

            //                sRet = sKakomiChar + sRet + sKakomiChar;

            //            } else {
            //                if (!string.IsNullOrEmpty(sTarget) && IsTopExist()) {
            //                    //削除
            //                    sRet = sRet.Remove(0, 1);
            //                }


            //                if (!string.IsNullOrEmpty(sTarget) && IsLastExist()) {
            //                    //削除
            //                    sRet = sRet.Remove(sRet.Length - 1, 1);

            //                }

            //            }

            //        }

            //        return sRet;
            //    }

            //    private bool chkOptParam() {
            //        return
            //            (base.pram[(int)prmInd.kakomi] == "1" || base.pram[(int)prmInd.kakomi] == "2") &&
            //            (base.pram[(int)prmInd.kakunin] == "1" || base.pram[(int)prmInd.kakunin] == "2") &&
            //            (base.pram[(int)prmInd.operate] == "1" || base.pram[(int)prmInd.operate] == "2");

            //    }

            //    private bool IsKakomiChk() {
            //        return base.pram[(int)prmInd.kakunin] == "2";
            //    }


            //    private string RetkakomiChar() {
            //        return base.pram[(int)prmInd.kakomi] == "1" ? "'" : @"""";
            //    }

            //    private bool RetKakomiExist() {
            //        return IsTopExist() && IsLastExist();
            //    }

            //    private bool IsTopExist() {
            //        return sTarget.Substring(0, 1) == sKakomiChar;
            //    }

            //    private bool IsLastExist() {
            //        return sTarget.Substring(sTarget.Length - 1, 1) == sKakomiChar;
            //    }

            //    private bool IsAdd() {
            //        return base.pram[(int)prmInd.operate] == "1";
            //    }
            //}

            /// <summary>
            /// 日付表示方法の設定を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            public class ConvDate : baseConv {

                private const int _prmCnt = 25;
                public enum prmInd {
                    Hd,
                    Seq,
                    target,
                    preOrder_Y,
                    preKugi_Y,
                    preUnit_Y,
                    prekoyomi_Y,
                    preOrder_M,
                    preKugi_M,
                    preUnit_M,
                    preDsp_M,
                    preOrder_D,
                    preKugi_D,
                    preUnit_D,
                    afterOrder_Y,
                    afterKugi_Y,
                    afterLen_Y,
                    afterkoyomi_Y,
                    afterOrder_M,
                    afterKugi_M,
                    afterZero_M,
                    afterDsp_M,
                    afterOrder_D,
                    afterKugi_D,
                    afterZero_D
                }

                public ConvDate(string[] sRet) {
                    base.pram = sRet;
                    init();
                }

                public ConvDate() {
                    base.pram = new string[_prmCnt];
                    init();
                }
                private void init() {
                    base.parameterCnt = _prmCnt;
                    base.pram[(int)prmInd.Hd] = ConvType.Date.ToString();
                }


                public override string Conv(string[] sMoto, string def) {
                    JapaneseCalendar jc = new JapaneseCalendar();
                    string sRet = null;
                    int nTargetIndex = 0;
                    string sTarget = "";
                    string sSplit = "";
                    int nPyear = 0;
                    int nMonth = 0;
                    int nDay = 0;
                    Dictionary<int, string> DicMonEnglish = new Dictionary<int, string>()
                    {
                        { 1,"Jan"},{2,"Feb"},{3,"Mar"},{4,"Apr"},
                        { 5,"May"},{6,"Jun"},{7,"Jul"},{8,"Aug"},
                        { 9,"Sep"},{10,"Oct"},{11,"Nov"},{12,"Dec"},
                    };

                    //対象
                    //パラメータチェック
                    nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target]);

                    //対象範囲外
                    if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

                    //対象
                    sTarget = sMoto[nTargetIndex - 1];

#if DEBUG
                    //sTarget = "令和3/05/01";
#endif



                    if (string.IsNullOrEmpty(sTarget) || sTarget.Length < 5) { return sTarget; }

                    //オプションパラメータチェック
                    if (!chkOptParam()) { return sTarget; }

                    //区切り単位オプションパラメータチェック
                    if (!ChkUnitParam()) { return sTarget; }

                    //前　年月日取得
                    //１番目の項目取得
                    sSplit = sTarget;
                    string year = "";
                    for (int i = 1; i <= 3; i++) {

                        if (base.pram[(int)prmInd.preOrder_Y] == i.ToString()) {

                            //桁数
                            if (base.pram[(int)prmInd.preKugi_Y] == "1") {

                                int nLen = int.Parse(base.pram[(int)prmInd.preUnit_Y]);
                                if (nLen > sSplit.Length) { nLen = sSplit.Length; }

                                year = sSplit.Substring(0, nLen);

                                //和暦はあとで判断
                                if (base.pram[(int)prmInd.prekoyomi_Y] == "0") {
                                    if (!int.TryParse(year, out nPyear)) { return sTarget; }
                                }


                                sSplit = sSplit.Remove(0, nLen);


                            }
                            //文字
                            else {
                                int nLen = sSplit.IndexOf(base.pram[(int)prmInd.preUnit_Y]);

                                //存在しない
                                if (nLen < 0) { return sTarget; }
                                //存在する
                                else {
                                    year = sSplit.Substring(0, nLen);
                                    if (base.pram[(int)prmInd.prekoyomi_Y] == "0") {
                                        if (!int.TryParse(sSplit.Substring(0, nLen), out nPyear)) { return sTarget; }
                                    }
                                    sSplit = sSplit.Remove(0, nLen + base.pram[(int)prmInd.preUnit_Y].Length);
                                }
                            }

                        }

                        if (base.pram[(int)prmInd.preOrder_M] == i.ToString()) {
                            //英語判断
                            if (base.pram[(int)prmInd.preDsp_M] == "1") {
                                if (base.pram[(int)prmInd.preKugi_M] != "1") {
                                    //区切り文字存在チェック
                                    int nLen = sSplit.IndexOf(base.pram[(int)prmInd.preUnit_M]);

                                    //存在しない
                                    if (nLen < 0) { return sTarget; }
                                    //存在する
                                    else {
                                        //区切り文字までの長さが3以外リターン
                                        if (nLen != 3) { return sTarget; }

                                        //3文字が英語Dicに存在するか                                        
                                        if (!DicMonEnglish.ContainsValue(sSplit.Substring(0, 3))) { return sTarget; } else {
                                            //変換
                                            nMonth = DicMonEnglish.First(x => x.Value == sSplit.Substring(0, 3)).Key;
                                        }
                                        //削除
                                        sSplit = sSplit.Remove(0, nLen + base.pram[(int)prmInd.preUnit_M].Length);
                                    }
                                } else {
                                    if (DicMonEnglish.ContainsValue(sSplit.Substring(0, 3))) {
                                        nMonth = DicMonEnglish.First(x => x.Value == sSplit.Substring(0, 3)).Key;
                                        sSplit = sSplit.Remove(0, 3);
                                    } else { return sTarget; }
                                }



                            } else {
                                //桁数
                                if (base.pram[(int)prmInd.preKugi_M] == "1") {

                                    int nLen = int.Parse(base.pram[(int)prmInd.preUnit_M]);
                                    if (nLen > sSplit.Length) { nLen = sSplit.Length; }

                                    if (!int.TryParse(sSplit.Substring(0, nLen), out nMonth)) { return sTarget; }

                                    sSplit = sSplit.Remove(0, nLen);


                                }
                                //文字
                                else {
                                    //区切り文字存在チェック
                                    int nLen = sSplit.IndexOf(base.pram[(int)prmInd.preUnit_M]);

                                    //存在しない
                                    if (nLen < 0) { return sTarget; }
                                    //存在する
                                    else {
                                        if (!int.TryParse(sSplit.Substring(0, nLen), out nMonth)) { return sTarget; }
                                        sSplit = sSplit.Remove(0, nLen + base.pram[(int)prmInd.preUnit_M].Length);
                                    }

                                }
                            }
                        }

                        if (base.pram[(int)prmInd.preOrder_D] == i.ToString()) {
                            //区切り判断

                            //桁数
                            if (base.pram[(int)prmInd.preKugi_D] == "1") {

                                int nLen = int.Parse(base.pram[(int)prmInd.preUnit_D]);
                                if (nLen > sSplit.Length) { nLen = sSplit.Length; }
                                if (!int.TryParse(sSplit.Substring(0, nLen), out nDay)) { return sTarget; }

                                sSplit = sSplit.Remove(0, nLen);

                            }
                            //文字
                            else {
                                int nLen = sSplit.IndexOf(base.pram[(int)prmInd.preUnit_D]);

                                if (nLen < 0) { return sTarget; } else {
                                    if (!int.TryParse(sSplit.Substring(0, nLen), out nDay)) { return sTarget; }
                                    sSplit = sSplit.Remove(0, nLen + base.pram[(int)prmInd.preUnit_D].Length);
                                }

                            }
                        }

                    }

                    //和暦変換
                    if (base.pram[(int)prmInd.prekoyomi_Y] != "0") {
                        GenGoCls gg = new GenGoCls(base.pram, year, nMonth, nDay);
                        nPyear = gg.WarekiToSeireki();
                        if (nPyear == 0) { return sTarget; }
                    }

                    if (!DateTime.TryParse(nPyear.ToString() + "/" + nMonth.ToString() + "/" + nDay.ToString(), out DateTime daytim)) {
                        return sTarget;
                    }

                    //年str作成
                    //西暦和暦変換
                    daytim = new DateTime(nPyear, nMonth, nDay);


                    //和暦変換
                    if (base.pram[(int)prmInd.afterkoyomi_Y] == "1") {
                        nPyear = jc.GetYear(daytim);
                    }

                    //年の桁数制御
                    string sY = "";

                    if (int.Parse(base.pram[(int)prmInd.afterLen_Y]) < nPyear.ToString().Length) {
                        sY = nPyear.ToString().Substring(nPyear.ToString().Length - int.Parse(base.pram[(int)prmInd.afterLen_Y]));
                    } else if (int.Parse(base.pram[(int)prmInd.afterLen_Y]) > nPyear.ToString().Length) {
                        sY = nPyear.ToString().PadLeft(int.Parse(base.pram[(int)prmInd.afterLen_Y]), '0');
                    } else {
                        sY = nPyear.ToString();
                    }

                    sY = base.pram[(int)prmInd.afterKugi_Y] == "0" ? sY : sY + base.pram[(int)prmInd.afterKugi_Y];


                    //月
                    string sM = "";
                    if (base.pram[(int)prmInd.afterDsp_M] == "0") {
                        sM = nMonth.ToString().PadLeft(int.Parse(base.pram[(int)prmInd.afterZero_M]), '0');

                    } else if (base.pram[(int)prmInd.afterDsp_M] == "1") {
                        sM = DicMonEnglish[nMonth];
                    }
                    sM = base.pram[(int)prmInd.afterKugi_M] == "0" ? sM : sM + base.pram[(int)prmInd.afterKugi_M];

                    //日
                    string sD = "";
                    sD = nDay.ToString().PadLeft(int.Parse(base.pram[(int)prmInd.afterZero_D]), '0');
                    sD = base.pram[(int)prmInd.afterKugi_D] == "0" ? sD : sD + base.pram[(int)prmInd.afterKugi_D];


                    for (int i = 1; i < 4; i++) {
                        if (base.pram[(int)prmInd.afterOrder_Y] == i.ToString()) {
                            sRet += sY;
                        }

                        if (base.pram[(int)prmInd.afterOrder_M] == i.ToString()) {
                            sRet += sM;
                        }

                        if (base.pram[(int)prmInd.afterOrder_D] == i.ToString()) {
                            sRet += sD;
                        }

                    }

                    return sRet;

                }

                private class GenGoCls {
                    //元号設定
                    int GenGoCondition = 0;
                    string monAndDay = "";
                    string year = "";

                    public GenGoCls(string[] prm, string year, int mon, int day) {
                        GenGoCondition = int.Parse(prm[(int)prmInd.prekoyomi_Y]);
                        monAndDay = "年" + mon.ToString() + "月" + day.ToString() + "日";
                        this.year = year;
                    }

                    public int WarekiToSeireki() {

                        //日付構造体
                        CultureInfo culture = new CultureInfo("ja-JP", true);
                        culture.DateTimeFormat.Calendar = new JapaneseCalendar();

                        try {
                            DateTime result;
                            if (IsHandan()) {

                                this.year = ChgEnglishIniToGG(this.year);

                                result = DateTime.ParseExact(this.year + monAndDay, "ggyy年M月d日", culture);
                                return result.Year;
                            } else {
                                //平成
                                if (IsHeisei()) {
                                    this.year = "平成" + this.year;
                                } else if (IsReiwa()) {
                                    this.year = "令和" + this.year;
                                }
                                  //昭和
                                  else if (IsSyowa()) {
                                    this.year = "昭和" + this.year;
                                }
                                  //大正
                                  else if (IsTaisyo()) {
                                    this.year = "大正" + this.year;
                                }
                                  //明治
                                  else if (IsMeige()) {
                                    this.year = "明治" + this.year;
                                }

                                result = DateTime.ParseExact(this.year + monAndDay, "ggyy年M月d日", culture);
                                return result.Year;

                            }

                        } catch (FormatException) {
                            return 0;

                        }

                    }

                    private string ChgEnglishIniToGG(string year) {

                        string c = year[0].ToString();
                        c = Microsoft.VisualBasic.Strings.StrConv(c, Microsoft.VisualBasic.VbStrConv.Narrow, 0x411);

                        if (c == "R") { year = year.Replace("R", "令和"); } else if (c == "H") { year = year.Replace("H", "平成"); } else if (c == "S") { year = year.Replace("S", "昭和"); } else if (c == "T") { year = year.Replace("T", "大正"); } else if (c == "M") { year = year.Replace("M", "明治"); }

                        return year;
                    }

                    private bool IsHandan() {
                        return IsGenGo(9);
                    }

                    private bool IsMeige() {
                        return IsGenGo(1);
                    }

                    private bool IsTaisyo() {
                        return IsGenGo(2);
                    }

                    private bool IsSyowa() {
                        return IsGenGo(3);
                    }

                    private bool IsHeisei() {
                        return IsGenGo(4);
                    }

                    private bool IsReiwa() {
                        return IsGenGo(5);
                    }


                    private bool IsGenGo(int ggIndex) {
                        return this.GenGoCondition == ggIndex;
                    }
                }

                private bool ChkUnitParam() {
                    return
                    (((base.pram[(int)prmInd.preKugi_Y] == "1" && int.TryParse(base.pram[(int)prmInd.preUnit_Y], out int n))) || ((base.pram[(int)prmInd.preKugi_Y] == "2" && !int.TryParse(base.pram[(int)prmInd.preUnit_Y], out n)))) &&
                    (((base.pram[(int)prmInd.preKugi_M] == "1" && int.TryParse(base.pram[(int)prmInd.preUnit_M], out n))) || ((base.pram[(int)prmInd.preKugi_M] == "2" && !int.TryParse(base.pram[(int)prmInd.preUnit_M], out n)))) &&
                    (((base.pram[(int)prmInd.preKugi_D] == "1" && int.TryParse(base.pram[(int)prmInd.preUnit_D], out n))) || ((base.pram[(int)prmInd.preKugi_D] == "2" && !int.TryParse(base.pram[(int)prmInd.preUnit_D], out n))));
                }

                private bool chkOptParam() {
                    return (base.pram[(int)prmInd.preOrder_Y] == "1" || base.pram[(int)prmInd.preOrder_Y] == "2" || base.pram[(int)prmInd.preOrder_Y] == "3") &&
                        (base.pram[(int)prmInd.preOrder_M] == "1" || base.pram[(int)prmInd.preOrder_M] == "2" || base.pram[(int)prmInd.preOrder_M] == "3") &&
                        (base.pram[(int)prmInd.preOrder_D] == "1" || base.pram[(int)prmInd.preOrder_D] == "2" || base.pram[(int)prmInd.preOrder_D] == "3") &&
                        (base.pram[(int)prmInd.preKugi_Y] == "1" || base.pram[(int)prmInd.preKugi_Y] == "2") &&
                        (base.pram[(int)prmInd.preKugi_M] == "1" || base.pram[(int)prmInd.preKugi_M] == "2") &&
                        (base.pram[(int)prmInd.preKugi_D] == "1" || base.pram[(int)prmInd.preKugi_D] == "2") &&
                        (base.pram[(int)prmInd.prekoyomi_Y] == "1" || base.pram[(int)prmInd.prekoyomi_Y] == "0" || base.pram[(int)prmInd.prekoyomi_Y] == "2"
                        || base.pram[(int)prmInd.prekoyomi_Y] == "3" || base.pram[(int)prmInd.prekoyomi_Y] == "4" || base.pram[(int)prmInd.prekoyomi_Y] == "5" || base.pram[(int)prmInd.prekoyomi_Y] == "9") &&
                        (base.pram[(int)prmInd.preDsp_M] == "1" || base.pram[(int)prmInd.preDsp_M] == "0") &&
                        (base.pram[(int)prmInd.afterOrder_Y] == "1" || base.pram[(int)prmInd.afterOrder_Y] == "2" || base.pram[(int)prmInd.afterOrder_Y] == "3") &&
                        (base.pram[(int)prmInd.afterOrder_M] == "1" || base.pram[(int)prmInd.afterOrder_M] == "2" || base.pram[(int)prmInd.afterOrder_M] == "3") &&
                        (base.pram[(int)prmInd.afterOrder_D] == "1" || base.pram[(int)prmInd.afterOrder_D] == "2" || base.pram[(int)prmInd.afterOrder_D] == "3") &&
                        (base.pram[(int)prmInd.afterkoyomi_Y] == "0" || base.pram[(int)prmInd.afterkoyomi_Y] == "1") &&
                        (base.pram[(int)prmInd.afterDsp_M] == "0" || base.pram[(int)prmInd.afterDsp_M] == "1") &&
                        (base.pram[(int)prmInd.afterZero_M] == "1" || base.pram[(int)prmInd.afterZero_M] == "2") &&
                        (base.pram[(int)prmInd.afterZero_D] == "1" || base.pram[(int)prmInd.afterZero_D] == "2") &&
                        (base.pram[(int)prmInd.afterLen_Y] == "1" || base.pram[(int)prmInd.afterLen_Y] == "2" || base.pram[(int)prmInd.afterLen_Y] == "3" || base.pram[(int)prmInd.afterLen_Y] == "4");
                }
            }

            /// <summary>
            /// 文字数制限を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            public class ConvCcut : baseConv {
                /// <summary>
                /// TRUE:文字数　FLSE:文字列
                /// </summary>
                bool mIsNum = false;
                /// <summary>
                /// TRUE:先頭から　FLSE:後ろ
                /// </summary>
                bool mIsFront = false;
                /// <summary>
                /// TRUE:制御まで　FLSE:制御直前まで
                /// </summary>
                bool mIsContainsLimitVal = false;
                /// <summary>
                /// TRUE:削除　FLSE:残す
                /// </summary>
                bool mIsRemove = false;
                int mCutStartIndex = 0;
                string mTarget = "";
                string mValue = "";
                int mLengthOfCutValue = 0;

                public enum prmInd {
                    Hd,
                    Seq,
                    target,
                    start,
                    limitMeth,
                    limitVal,
                    limitValType,
                    remove,
                    Register
                }
                private const int _prmCnt = 9;

                public ConvCcut(string[] sRet) {
                    base.pram = sRet;
                    init();
                }

                public ConvCcut() {
                    base.pram = new string[_prmCnt];
                    init();
                }
                private void init() {
                    base.parameterCnt = _prmCnt;
                    base.pram[(int)prmInd.Hd] = ConvType.Ccut.ToString();
                }

                public override string Conv(string[] sMoto, string def) {
                    mMotoDataArray = sMoto;

                    //対象

                    //対象(テーブルレジスタ対応)
                    if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out mTarget)) {
                        return def;
                    }


                    ////パラメータチェック
                    //int nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target]);

                    ////対象範囲外
                    //if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

                    ////対象
                    //mTarget = sMoto[nTargetIndex - 1];

                    if (mTarget == null) { return mTarget; }

                    //オプションパラメータチェック
                    if (!chkOptParam()) { return mTarget; }

                    //オプションセット（組み合わせで作業が決まる。）
                    SetOptionProcess();

                    //制御値チェック(T,Rにも対応)
                    if (!IsLimitValueOk(sMoto)) { return mTarget; }

                    //制御値のセット（文字数or文字列）
                    SetLimitValue();

                    //開始位置セット
                    SetCutStartIndex();

                    //開始位置チェック
                    if (!IsStartPositionOk()) { return mTarget; }

                    //開始位置＝対象チェック
                    if (mCutStartIndex == mTarget.Length) {
                        if (mIsRemove) { return ""; } else { return mTarget; }
                    }

                    //実行
                    var cutStr = Ccut();

                    //レジスタセット
                    var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                    if (rIndex >= 0) {
                        mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = cutStr;
                    }


                    return cutStr;

                }

                private bool IsLimitValueOk(string[] sMoto) {
                    if (mIsNum) {
                        var limitval = base.pram[(int)prmInd.limitVal];

                        if (IsTR(base.pram[(int)prmInd.limitValType])) {
                            if (!ParseControlTargetToString(base.pram[(int)prmInd.limitVal], out limitval)) {
                                limitval = base.pram[(int)prmInd.limitVal];
                            }

                        }

                        return int.TryParse(limitval, out mLengthOfCutValue);
                    }
                    return true;
                }

                private void SetLimitValue() {
                    if (!mIsNum) {


                        if (IsTR(base.pram[(int)prmInd.limitValType])) {
                            if (!ParseControlTargetToString(base.pram[(int)prmInd.limitVal], out mValue)) {
                                mValue = base.pram[(int)prmInd.limitVal];
                            }

                        } else {
                            mValue = base.pram[(int)prmInd.limitVal];
                        }

                        mLengthOfCutValue = mValue.Length;
                    }
                }

                private void SetCutStartIndex() {
                    //文字数
                    if (mIsNum) {
                        ////開始位置
                        mCutStartIndex = SetCutStartIndByNumType();
                    } else {
                        //開始位置
                        mCutStartIndex = SetCutStartIndByValueType();
                    }
                }

                private int SetCutStartIndByValueType() {
                    int index = 0;


                    if (mIsFront) {
                        index = mTarget.IndexOf(mValue);

                        if (!mIsContainsLimitVal) { index = index + mLengthOfCutValue; }

                    } else {
                        index = mTarget.LastIndexOf(mValue);

                        if (mIsContainsLimitVal) { index = index + mLengthOfCutValue; }
                    }

                    return index;
                }

                private bool IsStartPositionOk() {
                    //文字数
                    if (mIsNum) {
                        //開始位置チェック
                        if (mLengthOfCutValue < 0 || mLengthOfCutValue > mTarget.Length) { return false; }
                    } else {

                        //開始位置チェック(変換値ではなく、パラメータ使用していたので修正)
                        if (mTarget.IndexOf(mValue) < 0) { return false; }

                    }

                    return true;
                }

                private void SetOptionProcess() {

                    switch (int.Parse(base.pram[(int)prmInd.start] + base.pram[(int)prmInd.limitMeth] + base.pram[(int)prmInd.remove])) {
                        //先頭から（文字数）削除する
                        case 111:
                            mIsFront = true;
                            mIsNum = true;
                            mIsRemove = true;
                            mIsContainsLimitVal = false;
                            break;
                        //先頭から（指定文字まで）削除する
                        case 121:
                            mIsFront = true;
                            mIsNum = false;
                            mIsRemove = true;
                            mIsContainsLimitVal = false;
                            break;
                        //先頭から（指定文字直前まで）削除する
                        case 222:
                            mIsFront = true;
                            mIsNum = false;
                            mIsRemove = true;
                            mIsContainsLimitVal = true;
                            break;
                        //先頭から（文字数）残す
                        case 112:
                            mIsFront = true;
                            mIsNum = true;
                            mIsRemove = false;
                            mIsContainsLimitVal = false;
                            break;
                        //先頭から（指定文字まで）残す
                        case 122:
                            mIsFront = true;
                            mIsNum = false;
                            mIsRemove = false;
                            mIsContainsLimitVal = false;
                            break;
                        //先頭から（指定文字直前まで）残す
                        case 221:
                            mIsFront = true;
                            mIsNum = false;
                            mIsRemove = false;
                            mIsContainsLimitVal = true;
                            break;
                        //後ろから（文字数）削除する
                        case 211:
                            mIsFront = false;
                            mIsNum = true;
                            mIsRemove = true;
                            mIsContainsLimitVal = false;
                            break;
                        //後ろから（指定文字まで）削除する
                        case 321:
                            mIsFront = false;
                            mIsNum = false;
                            mIsRemove = true;
                            mIsContainsLimitVal = false;
                            break;
                        //後ろから（指定文字直前まで）削除する
                        case 421:
                            mIsFront = false;
                            mIsNum = false;
                            mIsRemove = true;
                            mIsContainsLimitVal = true;
                            break;
                        //後ろから（文字数）残す
                        case 212:
                            mIsFront = false;
                            mIsNum = true;
                            mIsRemove = false;
                            mIsContainsLimitVal = false;
                            break;
                        //後ろから（指定文字まで）残す
                        case 322:
                            mIsFront = false;
                            mIsNum = false;
                            mIsRemove = false;
                            mIsContainsLimitVal = false;
                            break;
                        //後ろから（指定文字直前まで）残す
                        case 422:
                            mIsFront = false;
                            mIsNum = false;
                            mIsRemove = false;
                            mIsContainsLimitVal = true;
                            break;
                        default:
                            break;
                    }

                }

                private string Ccut() {

                    string sRet = "";

                    //前残し→前
                    if (!mIsRemove && mIsFront || mIsRemove && !mIsFront) {
                        return mTarget.Remove(mCutStartIndex);
                    }
                    //前削除→後
                    else if (mIsRemove && mIsFront || !mIsRemove && !mIsFront) {
                        return mTarget.Remove(0, mCutStartIndex);
                    }


                    return sRet;
                }

                private int SetCutStartIndByNumType() {
                    int nSt = mLengthOfCutValue;

                    if (!mIsFront) {
                        nSt = mTarget.Length - mLengthOfCutValue;
                    }

                    return nSt;
                }



                private bool IsStart() {
                    return base.pram[(int)prmInd.start] == "1";
                }


                private bool chkOptParam() {
                    return
                       (base.pram[(int)prmInd.start] == "1" || base.pram[(int)prmInd.start] == "2" || base.pram[(int)prmInd.start] == "3" || base.pram[(int)prmInd.start] == "4") &&
                       (base.pram[(int)prmInd.limitMeth] == "1" || base.pram[(int)prmInd.limitMeth] == "2") &&
                       (base.pram[(int)prmInd.remove] == "1" || base.pram[(int)prmInd.remove] == "2");

                }



            }

            /// <summary>
            /// バイト数制限を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            public class ConvBcut : baseConv {
                public enum prmInd {
                    Hd,
                    Seq,
                    target,
                    start,
                    bytenum,
                    remove,
                    Register
                }
                private const int _prmCnt = 7;

                public ConvBcut(string[] sRet) {
                    base.pram = sRet;
                    init();
                }

                public ConvBcut() {
                    base.pram = new string[_prmCnt];
                    init();
                }
                private void init() {
                    base.parameterCnt = _prmCnt;
                    base.pram[(int)prmInd.Hd] = ConvType.Bcut.ToString();
                }

                private Encoding e;//= Encode.GetEncoding(CurrentCulture.IsCultureJa ? EncodingEnum.sjis : EncodingEnum.utf8);

                public override string Conv(string[] sMoto, string def) {
                    string sRet = null;
                    //int nTargetIndex = 0;
                    base.mMotoDataArray = sMoto;

                    string sTarget;
                    //対象(テーブルレジスタ対応)
                    if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out sTarget)) {
                        return def;
                    }

                    ////パラメータチェック
                    //nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target]);

                    ////対象範囲外
                    //if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

                    ////対象
                    //sTarget = sMoto[nTargetIndex - 1];

                    //オプションパラメータチェック
                    if (!chkOptParam()) { return sTarget; }


                    //開始位置(バイト数)
                    string startIndexString;
                    if (!base.ParseControlTargetToString(base.pram[(int)prmInd.bytenum], out startIndexString)) {
                        //数値、または、テーブルレジスタ参照できない（範囲外）そのままの値にする。
                        startIndexString = base.pram[(int)prmInd.bytenum];
                    }


                    //数値に変換できない場合は、制御対象を返送
                    if (!int.TryParse(startIndexString, out int nSt)) { return sTarget; }


                    //開始位置チェック
                    //if (nSt < 0 || nSt > e.GetByteCount(sTarget.ToArray())) { return sTarget; }
                    if (nSt < 0) { return sTarget; }

                    if (string.IsNullOrEmpty(sTarget)) { return sTarget; }

                    //#945 （追記分の対応）指定バイト数を超えている場合の処理
                    //if (nSt > e.GetByteCount(sTarget.ToArray())) { nSt = e.GetByteCount(sTarget.ToArray()); }

                    //開始位置（ﾊﾞｲﾄ数）
                    nSt = SetCutStartInd(nSt, sTarget);

                    //ﾊﾞｲﾄ数 から　文字数へ
                    nSt = ConvertCharFromByte(nSt, sTarget);

                    //文字数チェック
                    if (nSt == sTarget.Length) {
                        if (IsRemove()) { return ""; } else { return sTarget; }
                    }

                    //実行
                    sRet = Bcut(nSt, sTarget);

                    //レジスタセット
                    var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                    if (rIndex >= 0) {
                        mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                    }

                    return sRet;

                }

                private bool chkOptParam() {
                    return
                       (base.pram[(int)prmInd.start] == "1" || base.pram[(int)prmInd.start] == "2") &&
                       (base.pram[(int)prmInd.remove] == "1" || base.pram[(int)prmInd.remove] == "2");

                }

                private int SetCutStartInd(int nSt, string sTarget) {
                    if (!IsStart()) {
                        nSt = e.GetByteCount(sTarget.ToArray()) - nSt;
                    }

                    return nSt;
                }
                private bool IsStart() {
                    return base.pram[(int)prmInd.start] == "1";
                }

                private string Bcut(int nSt, string sTarget) {
                    string sRet;
                    string sMae = sTarget;      //#945　（例外処理）指定したバイト数と同じバイト数だった場合
                    if (nSt != sTarget.Length)  //#945  指定したバイト数と異なるバイト数だった場合
                    {
                        sMae = sTarget.Remove(nSt);
                    }
                    string sUsiro = sTarget.Remove(0, nSt);

                    //前残し→前
                    if (!IsRemove() && IsStart()) { sRet = sMae; }
                    //前削除→後
                    else if (IsRemove() && IsStart()) { sRet = sUsiro; }
                    //後残し→後
                    else if (!IsRemove() && !IsStart()) { sRet = sUsiro; }
                    //後削除→前
                    else { sRet = sMae; }

                    return sRet;
                }
                private bool IsRemove() {
                    return base.pram[(int)prmInd.remove] == "1";
                }

                private int ConvertCharFromByte(int bite, string str) {

                    int nByteConut = 0;
                    char[] chars = str.ToCharArray();


                    for (int i = 0; i < str.Length; i++) {
                        nByteConut += e.GetByteCount(chars, i, 1);

                        if (nByteConut == bite) {
                            return i + 1;
                        }

                        if (nByteConut > bite) {
                            return IsRemove() ? i : i;

                        }

                    }

                    return 0;
                }
            }

            /// <summary>
            /// 金額表示方法の設定を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            public class ConvMoney : baseConv {
                public enum prmInd {
                    Hd,
                    Seq,
                    target,
                    preSblprocess,
                    preSbl,
                    afterSblprocess,
                    afterSbl,
                    comma,
                    syosu,
                    syosulen
                }

                private const int _prmCnt = 10;

                public ConvMoney(string[] sRet) {
                    base.pram = sRet;
                    init();
                }

                public ConvMoney() {
                    base.pram = new string[_prmCnt];
                    init();
                }
                private void init() {
                    base.parameterCnt = _prmCnt;
                    base.pram[(int)prmInd.Hd] = ConvType.Money.ToString();
                }
                public override string Conv(string[] sMoto, string def) {
                    string sRet = null;
                    int nTargetIndex = 0;
                    string PreSimbol = "";
                    string Integer = "";
                    string Decimal = "";
                    string AftSimbol = "";

                    //対象
                    //パラメータチェック
                    nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target]);

                    //対象範囲外
                    if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

                    //対象
                    string sTarget = sMoto[nTargetIndex - 1];

                    //パラメータ☑
                    if (!ChkParameter()) { return sTarget; }


                    //ターゲットを分解して、前記号、金額部分、後ろ記号に分ける
#if DEBUG
                    //sTarget = "doll100,000.01円";
#endif

                    string buff = sTarget;
                    //前記号（数字以外の部分）                    
                    PreSimbol = GetPreSimbol(ref buff);
                    PreSimbol = PreSimbolProcess(PreSimbol);

                    //整数部
                    Integer = GetTargetInteger(ref buff);
                    if (Integer == "") { return sTarget; }

                    //コンマ
                    if (IsCommaAdd()) {
                        //フォーマット
                        Integer = string.Format("{0:#,0}", long.Parse(Integer));
                    }

                    //小数部
                    Decimal = GetTargetDecimal(ref buff);
                    if (!int.TryParse(base.pram[(int)prmInd.syosulen], out int syo)) { syo = 0; }

                    if (IsSyosuu() && Decimal == "") {

                        string addSyo = ".";
                        for (int i = 0; i < syo; i++) {
                            addSyo += "0";
                        }
                        Decimal = addSyo;
                    } else if (IsSyosuu() && Decimal != "") {

                        int Len = Decimal.Length;
                        if (syo == Len) { Decimal = "." + Decimal; } else if (syo > Len) {
                            for (int i = 0; i < syo - Len; i++) {
                                Decimal += "0";
                            }
                            Decimal = "." + Decimal;
                        } else {
                            Decimal = "." + Decimal.Remove(syo);
                        }



                    } else {
                        Decimal = "";
                    }

                    //後記号
                    AftSimbol = buff;
                    AftSimbol = AfterSimbolProcess(AftSimbol);

                    sRet = PreSimbol + Integer + Decimal + AftSimbol;

                    //ダブルクオーテーション
                    //if (IsCommaAdd()) { sRet = "\"" + sRet + "\""; }

                    return sRet;
                }

                private string GetTargetDecimal(ref string buff) {
                    string deci = "";
                    if (buff == "") { return ""; }
                    //.の後ろが数字だった場合のみ判定する。
                    if (buff[0] == '.' && buff.Length >= 2 && int.TryParse(buff[1].ToString(), out int n)) {

                        for (int i = 1; i < buff.Length; i++) {
                            if (int.TryParse(buff[i].ToString(), out n)) {
                                deci += buff[i].ToString();
                            } else {
                                buff = buff.Remove(0, i);
                                break;
                            }
                        }

                        buff = "";

                    } else {
                        deci = "";
                    }

                    return deci;
                }

                private static string GetTargetInteger(ref string buff) {
                    string integer = "";
                    if (buff == "") { return ""; }
                    if (int.TryParse(buff[0].ToString(), out int n)) {

                        for (int i = 0; i < buff.Length; i++) {
                            if (int.TryParse(buff[i].ToString(), out n)) {
                                integer += buff[i].ToString();
                            } else {
                                if (buff[i] == ',') { continue; } else {
                                    buff = buff.Remove(0, i);
                                    return integer;
                                }
                            }
                        }

                        buff = "";


                    } else {
                        integer = "";
                    }

                    return integer;
                }

                private string GetPreSimbol(ref string buff) {
                    string pre = "";
                    if (buff == "") { return ""; }
                    if (int.TryParse(buff[0].ToString(), out int nPre)) {
                        pre = "";
                    } else {
                        for (int i = 0; i < buff.Length; i++) {
                            if (int.TryParse(buff[i].ToString(), out nPre)) {
                                //削除
                                buff = buff.Remove(0, i);
                                break;
                            } else {
                                pre += buff[i].ToString();
                            }

                        }

                    }

                    return pre;
                }

                private bool IsSyosuu() {
                    bool IsketaOk = int.TryParse(base.pram[(int)prmInd.syosulen], out int n);
                    return (base.pram[(int)prmInd.syosu] == "1" && IsketaOk);
                }

                private string AfterSimbolProcess(string def) {
                    int nAftSm = ChkAftSimbol();
                    string sAftSm = base.pram[(int)prmInd.afterSbl];
                    if (nAftSm == 1) {
                        return sAftSm;

                    } else if (nAftSm == 2) {
                        //削除
                        if (sAftSm == def) { return ""; } else { return def; }


                    } else {
                        return def;
                    }

                }

                private string PreSimbolProcess(string def) {
                    int nPreSm = ChkPreSimbol();
                    string sPreSm = base.pram[(int)prmInd.preSbl];

                    if (nPreSm == 1) {
                        return sPreSm;
                    } else if (nPreSm == 2) {
                        //完全一致していれば入れ替え
                        if (sPreSm == def) { return ""; } else {
                            return def;
                        }

                    } else {
                        return def;
                    }



                }

                private bool IsCommaAdd() {
                    return (base.pram[(int)prmInd.comma] == "1");
                }


                private bool IsAfterSmExsist(string sTarget) {
                    int nSt = sTarget.Length - base.pram[(int)prmInd.afterSbl].Length;
                    if (nSt < 0) { return false; }
                    return sTarget.Substring(nSt, base.pram[(int)prmInd.afterSbl].Length) == base.pram[(int)prmInd.afterSbl];
                }

                private bool ChkParameter() {
                    return (base.pram[(int)prmInd.comma] == "1" || base.pram[(int)prmInd.comma] == "2") &&
                        (base.pram[(int)prmInd.syosu] == "1" || base.pram[(int)prmInd.syosu] == "2");
                }

                private int ChkPreSimbol() {
                    int nPreSm;
                    nPreSm = ChkParam((int)prmInd.preSblprocess);

                    return nPreSm;
                }

                private int ChkAftSimbol() {
                    int nASm;
                    nASm = ChkParam((int)prmInd.afterSblprocess);

                    return nASm;
                }

                private int ChkParam(int index) {
                    int nPreSm;
                    string sPreSm = base.pram[index];
                    if (sPreSm == "0" || sPreSm == "1" || sPreSm == "2") { nPreSm = int.Parse(sPreSm); } else { nPreSm = 0; }

                    return nPreSm;
                }
            }

            /// <summary>
            /// 文字列置換を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            public class ConvCchg : baseConv {
                public enum prmInd {
                    Hd,
                    Seq,
                    target,
                    chgTarget,
                    chgTargetType,
                    chgContents,
                    chgContentsType,
                    exceptContents,
                    exceptContentsType,
                    Register
                }


                private const int _prmCnt = 10;




                public ConvCchg(string[] sRet) {
                    base.pram = sRet;
                    init();
                }

                public ConvCchg() {
                    base.pram = new string[_prmCnt];
                    init();


                }
                private void init() {
                    base.parameterCnt = _prmCnt;
                    base.pram[(int)prmInd.Hd] = ConvType.Cchg.ToString();
                }

                public override string Conv(string[] sMoto, string def) {
                    string sRet = null;
                    base.mMotoDataArray = sMoto;


                    //対象
                    //パラメータチェック
                    //nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target], sMoto);

                    ////対象範囲外
                    //if (nTargetIndex < 0) { return def; }

                    ////対象
                    string sTarget;

                    //対象もレジスタ対応する。
                    if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out sTarget)) {
                        return def;
                    }

                    if (string.IsNullOrEmpty(sTarget)) { return def; }


                    //置換ターゲット（置き換え元）
                    string chgtarget = pram[(int)prmInd.chgTarget];

                    if (base.IsTR(pram[(int)prmInd.chgTargetType])) {
                        if (!base.ParseControlTargetToString(pram[(int)prmInd.chgTarget], out chgtarget)) { return def; }
                    }

                    //置換文字列が無い場合
                    if (string.IsNullOrEmpty(chgtarget)) {
                        return base.pram[(int)prmInd.chgContents];
                    }

                    //除外ターゲット
                    string excepttarget = pram[(int)prmInd.exceptContents];

                    if (base.IsTR(pram[(int)prmInd.exceptContentsType])) {
                        if (!base.ParseControlTargetToString(pram[(int)prmInd.exceptContents], out excepttarget)) { return def; }
                    }

                    //置換内容
                    string chgContents = pram[(int)prmInd.chgContents];

                    if (base.IsTR(pram[(int)prmInd.chgContentsType])) {
                        if (!base.ParseControlTargetToString(pram[(int)prmInd.chgContents], out chgContents)) { return def; }
                    }

                    //対象外を置き換える
                    if (!string.IsNullOrEmpty(excepttarget)) {
                        sTarget = sTarget.Replace(excepttarget, "\r\n");
                    }


                    //置換
                    sRet = sTarget.Replace(chgtarget, chgContents);

                    //対象外戻す
                    if (!string.IsNullOrEmpty(excepttarget)) {
                        sRet = sRet.Replace("\r\n", excepttarget);
                    }

                    //レジスタセット
                    var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                    if (rIndex >= 0) {
                        mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                    }

                    return sRet;


                }


            }

            /// <summary>
            /// 自動番号付番を行うにあたり、そのための変数を格納等する変換クラス
            /// </summary>
            public class ConvSeq : baseConv {
                public enum prmInd {
                    Hd,
                    Seq,
                    start,
                    step,
                    Register
                }

                private const int _prmCnt = 5;
                public int sampleRowIndex = 0;
                private string StartIndexString;
                public long nNowSeqNum = 0;
                private int stepNum = 0;
                private bool IsStepparamNumber = false;
                public string SEQRESULT { get; private set; } = "";

                public ConvSeq(string[] sRet) {
                    base.pram = sRet;
                    init();

                    IsStepparamNumber = int.TryParse(base.pram[(int)prmInd.step], out stepNum);
                    if (!IsStepparamNumber) { StartIndexString = null; return; }


                    if (!long.TryParse(base.pram[(int)prmInd.start], out long nSt)) {
                        //try {
                        //    if (!IsStartNumFromTxt(out nSt)) {
                        //        StartIndexString = null;
                        //    } else {
                        //        StartIndexString = nSt.ToString();
                        //    }
                        //}
                        //catch (IdcCommon.Exceptions.IdcCustomException.ConvertException)
                        //{
                        //    if (IsIDCprocess)
                        //    {
                        //        StartIndexString = "1";
                        //    }
                        //    else
                        //    {
                        //        throw;
                        //    }

                        //}
                    } else {
                        StartIndexString = nSt.ToString();
                        nSt = nSt - stepNum;
                    }

                    nNowSeqNum = nSt;

                }

                public ConvSeq(int ind, string[] sRet) {
                    base.pram = sRet;
                    sampleRowIndex = ind;
                    init();
                }

                public ConvSeq(int ind) {
                    base.pram = new string[_prmCnt];
                    sampleRowIndex = ind;
                    init();
                }

                private void init() {
                    base.parameterCnt = _prmCnt;
                    base.pram[(int)prmInd.Hd] = ConvType.Seq.ToString();
                }

                public override string Conv(string[] sMoto, string def) {
                    // パラメータチェック
                    IsStepparamNumber = int.TryParse(base.pram[(int)prmInd.step], out stepNum);
                    if (!IsStepparamNumber) { return def; }

                    // プロセス名で判断する。
                    int rIndex;
                    if (IsIDCprocess) {
                        if (!long.TryParse(base.pram[(int)prmInd.start], out long nSt)) {
                            //try {
                            //    if (!IsStartNumFromTxt(out nSt)) {
                            //        return def;
                            //    }

                            //    //ファイルからの場合は開始番号は、読み取り値+ステップ数
                            //    nSt = nSt + stepNum;

                            //}
                            //catch (IdcCommon.Exceptions.IdcCustomException.ConvertException)
                            //{
                            //    throw;
                            //}
                        }

                        // レジスタセット
                        rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);
                        var sRetIdc = (nSt + (sampleRowIndex - 1) * stepNum).ToString();
                        if (rIndex >= 0) {
                            mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRetIdc;
                        }

                        return sRetIdc;
                    }

                    // 初期値がテキストより取れていない場合

                    if (string.IsNullOrEmpty(StartIndexString)) { return def; }

                    nNowSeqNum = nNowSeqNum + stepNum;

                    var sRet = nNowSeqNum.ToString();

                    // レジスタセット
                    rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                    if (rIndex >= 0) {
                        mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                    }

                    SEQRESULT = sRet;

                    return sRet;
                }

                private bool IsStartNumFromTxt(out long startNum) {
                    var fileNameHead = CurrentCulture.IsCultureJa ? "[ファイル名]\r\n" : "[File name]\r\n";

                    startNum = 0;
                    return false;
                    //if (!File.Exists(base.pram[(int)prmInd.start])) { //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException(fileNameHead + base.pram[(int)prmInd.start], 3091); }

                    //    try {
                    //        ////読込
                    //        //using (StreamReader re = new StreamReader(base.pram[(int)prmInd.start], IdcCommon.IniFiles.Others.DefaultEncoding.GetEncode()))
                    //        //{
                    //        //    string st = re.ReadLine();

                    //        //    if (string.IsNullOrEmpty(st))
                    //        //    { throw new IdcCommon.Exceptions.IdcCustomException.ConvertException(fileNameHead + base.pram[(int)prmInd.start], 3093); }

                    //        //    return long.TryParse(st, out startNum);
                    //        //}
                    //    } catch (IOException) {
                    //        //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException(fileNameHead + base.pram[(int)prmInd.start], 3092);
                    //    } catch (UnauthorizedAccessException) {
                    //        //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException(fileNameHead + base.pram[(int)prmInd.start], 3092);
                    //    }
                    //}
                }
            }
                /// <summary>
                /// 条件付指定を行うにあたり、そのための変数を格納等する変換クラス
                /// </summary>
                //public class ConvIf : baseConv {

                //    private const int _prmCnt = 21;
                //    private string mdef;
                //    public string[] TrueParam = new string[] { };
                //    public string[] FalseParam = new string[] { };

                //    public enum prmInd {
                //        Hd,
                //        Seq,
                //        ifTarget1,
                //        ifvalue1,
                //        ifvalueType1,
                //        ifcond1,
                //        logic1,
                //        ifTarget2,
                //        ifvalue2,
                //        ifvalueType2,
                //        ifcond2,
                //        logic2,
                //        ifTarget3,
                //        ifvalue3,
                //        ifvalueType3,
                //        ifcond3,
                //        iftrue,
                //        iftrueType,
                //        iffalse,
                //        iffalseType,
                //        Register
                //    }

                //    public ConvIf(string[] sRet, string[] sub1, string[] sub2) {
                //        TrueParam = sub1;
                //        FalseParam = sub2;
                //        base.pram = sRet;
                //        init();
                //    }

                //    public ConvIf(string[] sRet) {
                //        TrueParam = new string[] { };
                //        FalseParam = new string[] { };
                //        base.pram = sRet;
                //        init();
                //    }

                //    public ConvIf() {
                //        base.pram = new string[_prmCnt];
                //        init();
                //    }
                //    private void init() {
                //        base.parameterCnt = _prmCnt;
                //        base.pram[(int)prmInd.Hd] = ConvType.If.ToString();
                //    }
                //    public override string Conv(string[] sMoto, string def) {
                //        string sRet = null;
                //        mMotoDataArray = sMoto;
                //        mdef = def;
                //        bool[] bIfresult = new bool[3];
                //        //各条件のTrueＯｒFalseの判断
                //        #region 
                //        for (int i = 0; i < bIfresult.Length; i++) {
                //            if (i == 1 && !IsUseLogic((int)prmInd.logic1)) { break; }
                //            if (i == 2 && !(IsUseLogic((int)prmInd.logic1) && IsUseLogic((int)prmInd.logic2))) { break; }


                //            int nCondIndex = 0;

                //            string sValue = "";

                //            //条件のParameterIndexを取得
                //            int nInd = RetTagetIndex(i);
                //            if (nInd < 0) { return def; }

                //            //対象
                //            if (!ParseControlTargetToString(base.pram[nInd], out string sTarget)) { return def; }


                //            //比較対象
                //            //比較対象が項目か実地か
                //            int nCInd = RetCompareTagetIndex(i);
                //            int nTypeIdex = GetValueTypeIndex(i);

                //            //比較対象が項目の場合はインデックス範囲内チェック
                //            if (IsTR(base.pram[nTypeIdex])) {
                //                if (!ParseControlTargetToString(base.pram[nCInd], out sValue)) {
                //                    //比較対象は実値とする。
                //                    sValue = base.pram[nCInd];
                //                }
                //            } else {
                //                //比較対象は実値とする。
                //                sValue = base.pram[nCInd];
                //            }



                //            //パラメータチェっく
                //            nCondIndex = GetConditonIndex(i);
                //            if (!CheckParameter(nCondIndex)) { return def; }

                //            //比較
                //            bool bResult = false;
                //            bResult = GetCompareResult(nCondIndex, sTarget, sValue);

                //            bIfresult[i] = bResult;

                //        }
                //        #endregion

                //        //条件論理演算はいくつあるか。
                //        bool bFinally = false;

                //        bFinally = bIfresult[0];

                //        //条件2を使用するか。                    
                //        if (IsUseLogic((int)prmInd.logic1)) {
                //            bFinally = RetCondtionBool(bIfresult[1], bFinally, int.Parse(base.pram[(int)prmInd.logic1]));

                //        }

                //        //条件3を使用するか
                //        if (IsUseLogic((int)prmInd.logic1) && IsUseLogic((int)prmInd.logic2)) {
                //            bFinally = RetCondtionBool(bIfresult[2], bFinally, int.Parse(base.pram[(int)prmInd.logic2]));
                //        }


                //        //IFELSE項目取得
                //        //取得に失敗（対象が範囲外の場合）にはデフォルトで返送する。
                //        try {
                //            sRet = GetRetIfVal(sMoto, bFinally);
                //            //レジスタセット
                //            var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);
                //            if (rIndex >= 0) {
                //                mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                //            }
                //        } catch (Exception) {

                //            sRet = def;
                //        }


                //        return sRet;

                //    }

                //    private int GetValueTypeIndex(int Cindex) {
                //        if (Cindex == 0) {
                //            return (int)prmInd.ifvalueType1;
                //        } else if (Cindex == 1) {
                //            return (int)prmInd.ifvalueType2;
                //        } else if (Cindex == 2) {
                //            return (int)prmInd.ifvalueType3;
                //        } else {
                //            return -1;
                //        }
                //    }

                //    private string GetRetIfVal(string[] sMoto, bool bFinally) {
                //        string sRet;

                //        if (bFinally) {
                //            sRet = GetItemOrValue(sMoto, (int)prmInd.iftrue, (int)prmInd.iftrueType);
                //        } else {
                //            sRet = GetItemOrValue(sMoto, (int)prmInd.iffalse, (int)prmInd.iffalseType);
                //        }

                //        return sRet;
                //    }

                //    private bool RetCondtionBool(bool bIfresult, bool bFinally, int n) {

                //        if (n == 1) {
                //            bFinally = bFinally && bIfresult;
                //        } else if (n == 2) {
                //            bFinally = bFinally || bIfresult;
                //        }

                //        return bFinally;
                //    }

                //    private bool IsUseLogic(int ind) {

                //        return base.pram[ind] == "1" || base.pram[ind] == "2";

                //    }


                //    private bool GetCompareResult(int nCondIndex, string sTarget, string sValue) {
                //        bool bRet = false;

                //        if (base.pram[nCondIndex] == "1") {
                //            bRet = sTarget == sValue;
                //        } else if (base.pram[nCondIndex] == "2") {
                //            bRet = sTarget != sValue;
                //        } else if (base.pram[nCondIndex] == "3") {
                //            bRet = sTarget.IndexOf(sValue) >= 0;
                //        } else if (base.pram[nCondIndex] == "4") {
                //            bRet = sTarget.IndexOf(sValue) < 0;
                //        } else if (base.pram[nCondIndex] == "5" || base.pram[nCondIndex] == "6") {
                //            if (decimal.TryParse(sTarget, out decimal nT) && decimal.TryParse(sValue, out decimal nV)) {
                //                if (base.pram[nCondIndex] == "5") {
                //                    bRet = nT >= nV;
                //                } else {
                //                    bRet = nT <= nV;
                //                }

                //            } else {
                //                bRet = false;
                //            }
                //        }

                //        return bRet;
                //    }

                //    private int GetConditonIndex(int ind) {
                //        if (ind == 0) {
                //            return (int)prmInd.ifcond1;
                //        } else if (ind == 1) {
                //            return (int)prmInd.ifcond2;
                //        } else if (ind == 2) {
                //            return (int)prmInd.ifcond3;
                //        } else {
                //            return -1;
                //        }
                //    }

                //    private bool CheckParameter(int Index) {
                //        if (Index < 0) { return false; }
                //        return base.pram[Index] == "1" ||
                //            base.pram[Index] == "2" ||
                //            base.pram[Index] == "3" ||
                //            base.pram[Index] == "4" ||
                //            base.pram[Index] == "5" ||
                //            base.pram[Index] == "6";
                //    }

                //    private string GetItemOrValue(string[] sMoto, int nCInd, int nCTypeInd) {
                //        string sValue;
                //        if (IsValueConvertReco(nCInd, nCTypeInd)) {
                //            //パラメータ存在チェック
                //            if (!IsExistSubParam(nCInd)) { return mdef; }

                //            //Seq,Ifチェック
                //            if (!IsPermitConv(nCInd)) { return mdef; }

                //            //パラメータ正当性チェック
                //            //これは無しにする。→変換自体は別で行われるため

                //            var reco = new Reco();


                //            //デフォルト値セット
                //            for (int i = 0; i < sMoto.Length; i++) {
                //                if (mdef == sMoto[i]) { reco.nMoto = i + 1; }
                //            }

                //            reco.sParam = GetSubParam(nCInd);
                //            reco.sType = reco.sParam[0];

                //            try {
                //                sValue = ConvertItem(sMoto, reco);
                //            } catch (Exception) {
                //                sValue = mdef;
                //            }
                //        } else {

                //            if (IsTR(base.pram[nCTypeInd])) {
                //                if (!ParseControlTargetToString(base.pram[nCInd], out sValue)) {
                //                    //実値
                //                    sValue = base.pram[nCInd];
                //                }
                //            } else {
                //                sValue = base.pram[nCInd];
                //            }
                //        }

                //        return sValue;
                //    }

                //    private string[] GetSubParam(int nCInd) {
                //        string[] Target;
                //        string[] param;
                //        if (nCInd == (int)prmInd.iftrue) {
                //            Target = TrueParam;
                //        } else {
                //            Target = FalseParam;
                //        }

                //        param = new string[Target.Length - 2];

                //        for (int i = 2; i < Target.Length; i++) {
                //            param[i - 2] = Target[i];
                //        }

                //        return param;
                //    }

                //    private bool IsExistSubParam(int nCInd) {
                //        if (nCInd == (int)prmInd.iftrue) { return this.TrueParam != null && this.TrueParam.Length > 2; }

                //        if (nCInd == (int)prmInd.iffalse) { return this.FalseParam != null && this.FalseParam.Length > 2; }

                //        return false;
                //    }

                //    private bool IsPermitConv(int nCInd) {
                //        string header = "";
                //        if (nCInd == (int)prmInd.iftrue) { header = this.TrueParam[2]; } else if (nCInd == (int)prmInd.iffalse) { header = this.FalseParam[2]; }

                //        if (header == IdcRecordConvert.ConvType.If.ToString() || header == IdcRecordConvert.ConvType.Seq.ToString()) { return false; }

                //        foreach (ConvType Value in Enum.GetValues(typeof(ConvType))) {
                //            if (header == Enum.GetName(typeof(ConvType), Value)) { return true; }
                //        }

                //        return false;

                //    }

                //    private bool IsValueConvertReco(int Index, int TypeIndex) {
                //        if ((base.pram[Index] == "IfFalse" && base.pram[TypeIndex] == "Reco") ||
                //            (base.pram[Index] == "IfTrue" && base.pram[TypeIndex] == "Reco")) {
                //            return true;
                //        }

                //        return false;

                //    }

                //    private int RetCompareTagetIndex(int Cindex) {
                //        if (Cindex == 0) {
                //            return (int)prmInd.ifvalue1;
                //        } else if (Cindex == 1) {
                //            return (int)prmInd.ifvalue2;
                //        } else if (Cindex == 2) {
                //            return (int)prmInd.ifvalue3;
                //        } else {
                //            return -1;
                //        }
                //    }

                //    private int RetTagetIndex(int IfIndex) {
                //        if (IfIndex == 0) {
                //            return (int)prmInd.ifTarget1;
                //        } else if (IfIndex == 1) {
                //            return (int)prmInd.ifTarget2;
                //        } else if (IfIndex == 2) {
                //            return (int)prmInd.ifTarget3;
                //        } else {
                //            return -1;
                //        }
                //    }

                //    public void SetTrueConvParam(string[] param) {
                //        if (param == null || param.Length == 0) { return; }

                //        TrueParam = param;
                //    }

                //    public void SetFalseConvParam(string[] param) {
                //        if (param == null || param.Length == 0) { return; }

                //        FalseParam = param;
                //    }

                //}

                /// <summary>
                /// 変換テーブル参照を行うにあたり、そのための変数を格納等する変換クラス
                /// </summary>
                public class ConvPat : baseConv {
                    //public bool IsLogNotMake = false;

                    public enum prmInd {
                        Hd,
                        Seq,
                        target,
                        pat,
                        unmatch,
                        fixvalue,
                        fixvalueType,
                        log
                    }
                    private const int _prmCnt = 8;
                    public string sPtn = "";
                    public ConvPat(string[] sRet) {
                        base.pram = sRet;
                        init();
                    }

                    public ConvPat() {
                        base.pram = new string[_prmCnt];
                        init();
                    }
                    private void init() {
                        base.parameterCnt = _prmCnt;
                        base.pram[(int)prmInd.Hd] = ConvType.Pat.ToString();
                    }

                    private bool ParseUnmatchValue(string target, out string parsetarget) {
                        var sRet = target;
                        parsetarget = target;
                        if (base.IsTR(base.pram[(int)prmInd.fixvalueType])) {
                            int nFixItemNo = 0;// GetTargetInd(base.pram[(int)prmInd.fixvalue]);
                            if (nFixItemNo < 0) {
                                sRet = base.pram[(int)prmInd.fixvalue];
                            } else {
                                if (mMotoDataArray.Length < nFixItemNo) { return false; } else {
                                    sRet = mMotoDataArray[nFixItemNo - 1];
                                }
                            }
                        } else {
                            sRet = base.pram[(int)prmInd.fixvalue];
                        }
                        parsetarget = sRet;
                        return true;
                    }

                    public override string Conv(string[] sMoto, string sDef) {
                        //プロセス名で判断する。
                        //IsLogNotMake = System.Diagnostics.Process.GetCurrentProcess().ProcessName.Substring(0, 3) == "IDC";
                        bool IsMatch = false;
                        mMotoDataArray = sMoto;
                        string sRet = "変換";
                        string sTar = "";
                        int n = 0;
                        //n = GetTargetInd(base.pram[(int)prmInd.target]);
                        //ターゲット
                        if (n < 0) {
                            return sDef;
                        }

                        //
                        if (n > sMoto.Length) {
                            return sDef;
                        }


                        sTar = sMoto[n - 1];
                        sRet = sMoto[n - 1];

                        IdcCommon.NewIdcCommon.ConvertFile.ListConvFile listFile;
                        //SeqNoをも
                        int patNo = int.Parse(base.pram[(int)prmInd.Seq]);
                        bool IsIDCDisp = IsIDCprocess;
                        if (!IsIDCDisp) {
                            if (dicPat.ContainsKey(patNo)) {
                                listFile = dicPat[patNo];

                                if (listFile.fileContents == null) { return sDef; }
                            } else {
                                return sDef;
                            }
                        } else {
                            //CnvMethod.ptnFileList = IdcCommon.NewIdcCommon.CommonMethods.FileUtilities.GetPatternList();
                            //var PatNo = int.Parse(base.pram[(int)CnvMethod.ConvPat.prmInd.pat]);

                            try {
                                //listFile = new IdcCommon.NewIdcCommon.ConvertFile.ListConvFile(new string[] { "pat", base.pram[(int)CnvMethod.ConvList.prmInd.Seq], base.pram[(int)CnvMethod.ConvPat.prmInd.target], CnvMethod.ptnFileList.Find(lst => PatNo == int.Parse(Path.GetFileName(lst).Substring(0, 2))), "Date", "csv", "", "", "", "sjis", "1", "1", "2", base.pram[(int)CnvMethod.ConvPat.prmInd.unmatch], base.pram[(int)CnvMethod.ConvPat.prmInd.fixvalue], base.pram[(int)CnvMethod.ConvPat.prmInd.fixvalueType], "", "0" }, "画面から実行");
                                //listFile.ImportFile();
                            } catch (Exception) {

                                return sDef;
                            }
                        }

                        //int matchCol = 1;
                        //int pairCol = 2;
                        //foreach (string[] AryStr in listFile.fileContents) {
                        //    if (AryStr.Length < matchCol || AryStr.Length < pairCol) {
                        //        //項目数が足りない場合のエラー
                        //        throw new IdcCommon.Exceptions.IdcCustomException.ConvertException(listFile.FileFullPath, 3113);
                        //    }
                        //    if (sTar == AryStr[matchCol - 1]) { sRet = AryStr[pairCol - 1]; IsMatch = true; break; }
                        //}

                        //アンマッチ時の処理                        
                        if (!IsMatch) {
                            if (base.pram[(int)prmInd.unmatch] == "1") {
                                if (base.IsTR(base.pram[(int)prmInd.fixvalueType])) {
                                    if (!base.ParseControlTargetToString(base.pram[(int)prmInd.fixvalue], out sRet)) { return sDef; }

                                } else {
                                    sRet = base.pram[(int)prmInd.fixvalue];
                                }
                            }

                            if (!IsIDCDisp && (base.pram[(int)prmInd.log] == "1")) {
                                //変換ファイル参照アンマッチの場合のログファイル作成 (タイムスタンプは２４時間表記)                               
                                //var logfilename = String.Format("{0}\\unmatch[{1}][{2}].log", Registry.RegistryLog, Path.GetFileNameWithoutExtension(listFile.FileFullPath), DateTime.Now.ToString("yyyyMMdd-HHmmss"));
                                //var log = new IdcCommon.NewIdcCommon.CommonMethods.FileCreate.UnmatchLog(logfilename);
                                //var logConttents = String.Format("アンマッチが発生しました。\r\nレコード変換設定：{0} \r\n参照ファイル：{1}", listFile.RECOREINIPATH, listFile.FileFullPath);
                                //log.SetLogContents(logConttents);

                                try {
                                    //log.Create();
                                } catch (Exception) {
                                    //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException("", 3114);
                                }

                            }

                            if ((!IsIDCDisp && base.pram[(int)prmInd.log] == "2")) {
                                //変換ファイル参照アンマッチの場合のエラーのイベントログ     
                                //IdcEventlog.ptnUnMatchEventlogMake(listFile.FileFullPath);
                            }
                        }

                        return sRet;

                    }

                }

                /// <summary>
                /// 文字列追加を行うにあたり、そのための変数を格納等する変換クラス
                /// </summary>
                public class ConvAdd : baseConv {
                    public enum prmInd {
                        Hd,
                        Seq,
                        target,
                        pos,
                        add,
                        addType,
                        opt,
                        Register
                    }
                    private const int _prmCnt = 8;

                    public ConvAdd(string[] sRet) {
                        base.pram = sRet;
                        init();
                    }

                    public ConvAdd() {
                        base.pram = new string[_prmCnt];
                        init();
                    }
                    private void init() {
                        base.parameterCnt = _prmCnt;
                        base.pram[(int)prmInd.Hd] = ConvType.Add.ToString();
                    }

                    public override string Conv(string[] sMoto, string def) {
                        string sRet = null;
                        int nTargetLength = 0;
                        base.mMotoDataArray = sMoto;

                        //対象
                        //パラメータチェック
                        //nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target]);
                        //nAddTargetIndex = GetTargetInd(base.pram[(int)prmInd.add]);
                        ////対象範囲外
                        //if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

                        //対象
                        if (!base.ParseControlTargetToString(pram[(int)prmInd.target], out string sTarget)) { return def; }

                        if (sTarget == null) { return def; }

                        nTargetLength = sTarget.Length;


                        string sAddTarget;
                        if (base.IsTR(pram[(int)prmInd.addType])) {
                            if (!base.ParseControlTargetToString(pram[(int)prmInd.add], out sAddTarget)) {
                                sAddTarget = base.pram[(int)prmInd.add];
                            }
                        } else {
                            //追加対象は範囲外の場合文字列として追加する。
                            sAddTarget = base.pram[(int)prmInd.add];
                        }
                        //= (nAddTargetIndex < 0 || nAddTargetIndex - 1 >= sMoto.Length) ? base.pram[(int)prmInd.add] : sMoto[nAddTargetIndex - 1];


                        if (!int.TryParse(base.pram[(int)prmInd.opt], out int nOpt)) { nOpt = 0; }

                        //オプションパラメータチェック
                        if (!chkOptParam()) { return sTarget; }

                        //
                        if (nOpt == 0) {
                            sRet = Add(sTarget, sAddTarget);
                        } else {


                            //連続オプションが0以上、かつ、制御対象文字列以下の場合はそのまま返す
                            if (nTargetLength >= nOpt) { return sTarget; }

                            sRet = sTarget;
                            while (sRet.Length < nOpt) {
                                sTarget = Add(sTarget, sAddTarget);
                                sRet = sTarget;
                            }

                            //文字数調整
                            if (IsPosPre()) {
                                StringBuilder sb = new StringBuilder(sRet);
                                sRet = sb.Remove(nOpt - nTargetLength, sRet.Length - nOpt).ToString();
                            } else {
                                sRet = sRet.Substring(0, nOpt);
                            }
                        }

                        //レジスタセット
                        var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                        if (rIndex >= 0) {
                            mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                        }

                        return sRet;
                    }

                    private string Add(string sTarget, string sAddTarget) {
                        return IsPosPre() ? sAddTarget + sTarget : sTarget + sAddTarget;
                    }

                    private bool chkOptParam() {



                        return
                           (base.pram[(int)prmInd.pos] == "1" || base.pram[(int)prmInd.pos] == "2");
                    }

                    private bool IsPosPre() {
                        return base.pram[(int)prmInd.pos] == "1";
                    }

                }

                /// <summary>
                /// コマンド実行結果を設定するにあたり、そのための変数を格納等する変換クラス
                /// </summary>
                public class ConvCmd : baseConv {
                    public enum prmInd {
                        Hd,
                        Seq,
                        cmd
                    }
                    private const int _prmCnt = 3;

                    public ConvCmd(string[] sRet) {
                        base.pram = sRet;
                        init();
                    }

                    public ConvCmd() {
                        base.pram = new string[_prmCnt];
                        init();
                    }
                    private void init() {
                        base.parameterCnt = _prmCnt;
                        base.pram[(int)prmInd.Hd] = ConvType.Cmd.ToString();
                    }

                    static string sRetOkEvent;
                    static string sRetErrEvent = "";
                    public override string Conv(string[] sMoto, string def) {
                        //設定画面からはコマンドを実行せずに、コマンドの文字列のみ返す。
                        if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.Substring(0, 3) == "IDC") {
                            return base.pram[(int)prmInd.cmd];
                        }

                        string sRet = "";
                        sRetOkEvent = "";
                        sRetErrEvent = "";
                        //Processオブジェクトを作成
                        System.Diagnostics.Process p = new System.Diagnostics.Process();

                        //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
                        p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
                        //出力を読み取れるようにする
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.RedirectStandardError = true;
                        //ウィンドウを表示しないようにする
                        p.StartInfo.CreateNoWindow = true;
                        //コマンドラインを指定（"/c"は実行後閉じるために必要）
                        //p.StartInfo.Arguments = "\"" + base.pram[(int)prmInd.cmd] + "\"";
                        p.StartInfo.Arguments = "/c " + base.pram[(int)prmInd.cmd];
                        p.OutputDataReceived += p_OutputDataReceived;
                        p.ErrorDataReceived += p_ErrorDataReceived;

                        //起動
                        if (p.Start()) {

                            //出力を読み取る
                            //非同期で出力とエラーの読み取りを開始
                            p.BeginOutputReadLine();
                            p.BeginErrorReadLine();

                        }
                        //プロセス終了まで待機する
                        //WaitForExitはReadToEndの後である必要がある(１０待つ)
                        //(親プロセス、子プロセスでブロック防止のため)
                        p.WaitForExit(10000);
                        p.Close();
                        p.Dispose();

                        sRet = string.IsNullOrEmpty(sRetOkEvent) ? sRetErrEvent : sRetOkEvent;

                        return sRet;

                    }
                    static void p_OutputDataReceived(object sender,
            System.Diagnostics.DataReceivedEventArgs e) {

                        //出力された文字列を表示する
                        sRetOkEvent += e.Data;
                    }

                    static void p_ErrorDataReceived(object sender,
            System.Diagnostics.DataReceivedEventArgs e) {
                        //エラー出力された文字列を表示する
                        sRetErrEvent += e.Data;
                    }

                }

                /// <summary>
                /// 文字形式変換を行うにあたり、そのための変数を格納等する変換クラス
                /// </summary>
                public class ConvFmt : baseConv {
                    public enum prmInd {
                        Hd,
                        Seq,
                        target,
                        fmt,
                        Register,
                    }
                    private const int _prmCnt = 5;

                    public ConvFmt(string[] sRet) {
                        base.pram = sRet;
                        init();
                    }

                    public ConvFmt() {
                        base.pram = new string[_prmCnt];
                        init();
                    }
                    private void init() {
                        base.parameterCnt = _prmCnt;
                        base.pram[(int)prmInd.Hd] = ConvType.Fmt.ToString();
                    }

                    public override string Conv(string[] sMoto, string def) {
                        string sRet = null;

                        mMotoDataArray = sMoto;
                        //対象
                        string sTarget;
                        //対象(テーブルレジスタ対応)
                        if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out sTarget)) {
                            return def;
                        }

                        //パラメータチェック
                        //nTargetIndex = GetTargetInd(base.pram[(int)prmInd.target]);

                        ////対象範囲外
                        //if (nTargetIndex < 0 || nTargetIndex - 1 >= sMoto.Length) { return def; }

                        ////対象
                        //string sTarget = sMoto[nTargetIndex - 1];

                        //パラメータチェック
                        if (!int.TryParse(base.pram[(int)prmInd.fmt], out int n)) { return sTarget; }


                        //半角→全角
                        if (n == 1) {
                            sRet = Microsoft.VisualBasic.Strings.StrConv(sTarget, Microsoft.VisualBasic.VbStrConv.Wide);
                        }

                        //全角→半角
                        else if (n == 2) {
                            sRet = Microsoft.VisualBasic.Strings.StrConv(sTarget, Microsoft.VisualBasic.VbStrConv.Narrow);
                        }
                        //大文字→小文字
                        else if (n == 3) {
                            sRet = sTarget.ToLower();
                        }
                        //小文字→大文字
                        else if (n == 4) {
                            sRet = sTarget.ToUpper();
                        } else {
                            sRet = sTarget;
                        }

                        //レジスタセット
                        var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                        if (rIndex >= 0) {
                            mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                        }

                        return sRet;
                    }

                }

                /// <summary>
                /// 値セットを行うにあたり、そのための変数を格納等する変換クラス
                /// </summary>
                public class ConvSet : baseConv {
                    public enum prmInd {
                        Hd,
                        Seq,
                        target,
                        targetType,
                        Register,
                    }
                    private const int _prmCnt = 5;

                    public ConvSet(string[] sRet) {
                        base.pram = sRet;
                        init();
                    }

                    public ConvSet() {
                        base.pram = new string[_prmCnt];
                        init();
                    }
                    private void init() {
                        base.parameterCnt = _prmCnt;
                        base.pram[(int)prmInd.Hd] = ConvType.Set.ToString();
                    }


                    public override string Conv(string[] sMoto, string def) {
                        mMotoDataArray = sMoto;

                        string sRet;
                        //代入値を持ってくる
                        if (base.IsTR(base.pram[(int)prmInd.targetType])) {
                            if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out sRet)) { return def; }
                        } else {
                            sRet = base.pram[(int)prmInd.target];
                        }

                        //レジスタセット
                        var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                        if (rIndex >= 0) {
                            mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                        }

                        return sRet;
                    }
                }

                /// <summary>
                /// 値除去の変換を行うにあたり、そのための変数を格納等するクラス
                /// </summary>
                public class ConvTrim : baseConv {
                    public enum prmInd {
                        Hd,
                        Seq,
                        target,
                        RemovalValue,
                        RemovalType,
                        Register,
                    }
                    private const int _prmCnt = 6;

                    public ConvTrim(string[] sRet) {
                        base.pram = sRet;
                        init();
                    }

                    public ConvTrim() {
                        base.pram = new string[_prmCnt];
                        init();
                    }
                    private void init() {
                        base.parameterCnt = _prmCnt;
                        base.pram[(int)prmInd.Hd] = ConvType.Trim.ToString();
                    }


                    public override string Conv(string[] sMoto, string def) {
                        mMotoDataArray = sMoto;

                        string sRet;
                        //対象を持ってくる
                        if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out string sTarget)) { return def; }

                        //除去値がない場合
                        if (string.IsNullOrEmpty(base.pram[(int)prmInd.RemovalValue])) { return def; }

                        //除去値                    
                        if (base.pram[(int)prmInd.RemovalType] == "1") {
                            sRet = sTarget.TrimStart(base.pram[(int)prmInd.RemovalValue][0]);
                        } else if (base.pram[(int)prmInd.RemovalType] == "2") {
                            sRet = sTarget.TrimEnd(base.pram[(int)prmInd.RemovalValue][0]);
                        } else {
                            //デフォルトは全体
                            sRet = sTarget.Replace(base.pram[(int)prmInd.RemovalValue][0].ToString(), "");
                        }



                        //レジスタセット
                        var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                        if (rIndex >= 0) {
                            mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                        }

                        return sRet;
                    }



                }

                /// <summary>
                /// ファイル参照を行うにあたり、そのための変数を格納等する変換クラス
                /// </summary>
                public class ConvList : baseConv {
                    public enum prmInd {
                        Hd,
                        Seq,
                        target,
                        filePath,
                        wild,
                        fileType,
                        sheetType,
                        sheet,
                        endRow,
                        encode,
                        startRow,
                        matchingCol,
                        pairCol,
                        unMatch,
                        unMatchVal,
                        unMatchValType,
                        Register,
                        log,
                    }
                    private const int _prmCnt = 18;

                    public ConvList(string[] sRet) {
                        base.pram = sRet;
                        init();
                    }

                    public ConvList() {
                        base.pram = new string[_prmCnt];
                        init();
                    }
                    private void init() {
                        base.parameterCnt = _prmCnt;
                        base.pram[(int)prmInd.Hd] = ConvType.List.ToString();
                    }


                    public override string Conv(string[] sMoto, string def) {
                        mMotoDataArray = sMoto;

                        string sRet = "";
                        return sRet;
                        ////代入値を持ってくる
                        //if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out string sTarget)) { return def; }
                        ////ファイルを読み込む
                        ////一致行と変換列
                        //if (!int.TryParse(base.pram[(int)prmInd.matchingCol], out int matchCol) || matchCol < 1) { matchCol = 1; }

                        //if (!int.TryParse(base.pram[(int)prmInd.pairCol], out int pairCol) || pairCol < 1) { pairCol = 1; }

                        //bool IsMatch = false;

                        //sRet = sTarget;

                        //IdcCommon.NewIdcCommon.ConvertFile.ListConvFile listFile;
                        ////SeqNoをも
                        //int patNo = int.Parse(base.pram[(int)prmInd.Seq]);
                        //bool IsIDCDisp = IsIDCprocess;
                        //if (!IsIDCDisp) {
                        //    if (dicList.ContainsKey(patNo)) {
                        //        listFile = dicList[patNo];

                        //        if (listFile.fileContents == null) { return def; }
                        //    } else {
                        //        return def;
                        //    }
                        //} else {

                        //    listFile = new IdcCommon.NewIdcCommon.ConvertFile.ListConvFile(base.pram, base.pram[(int)prmInd.filePath]);

                        //    try {
                        //        listFile.ImportFile();
                        //    } catch (Exception) {

                        //        return def;
                        //    }
                        //}




                        //foreach (string[] AryStr in listFile.fileContents) {
                        //    if (AryStr.Length < matchCol) { //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException("", 3175); }

                        //        if (AryStr.Length < pairCol) { //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException("", 3176); }

                        //            if (sTarget == AryStr[matchCol - 1]) { sRet = AryStr[pairCol - 1]; IsMatch = true; break; }
                        //        }

                        //        //アンマッチ時の処理                        
                        //        if (!IsMatch) {
                        //            if (base.pram[(int)prmInd.unMatch] == "1") {
                        //                if (base.IsTR(base.pram[(int)prmInd.unMatchValType])) {
                        //                    if (!base.ParseControlTargetToString(base.pram[(int)prmInd.unMatchVal], out sRet)) { return def; }

                        //                } else {
                        //                    sRet = base.pram[(int)prmInd.unMatchVal];
                        //                }
                        //            }

                        //            if (!IsIDCDisp && (base.pram[(int)prmInd.log] == "1" || base.pram[(int)prmInd.log] == "3")) {
                        //                //変換ファイル参照アンマッチの場合のログファイル作成 (タイムスタンプは２４時間表記)                               
                        //                //var logfilename = String.Format("{0}\\unmatch[{1}][{2}].log", Registry.RegistryLog, Path.GetFileNameWithoutExtension(listFile.FileFullPath), DateTime.Now.ToString("yyyyMMdd-HHmmss"));
                        //                //var log = new IdcCommon.NewIdcCommon.CommonMethods.FileCreate.UnmatchLog(logfilename);
                        //                //var logConttents = String.Format("アンマッチが発生しました。\r\nレコード変換設定：{0} \r\n参照ファイル：{1}", listFile.RECOREINIPATH, listFile.FileFullPath);
                        //                //log.SetLogContents(logConttents);
                        //                try {
                        //                    //log.Create();
                        //                } catch (Exception) {

                        //                    //throw new IdcCommon.Exceptions.IdcCustomException.ConvertException("", 3178);
                        //                }

                        //            }

                        //            if ((!IsIDCDisp && base.pram[(int)prmInd.log] == "2" || base.pram[(int)prmInd.log] == "3")) {
                        //                //変換ファイル参照アンマッチの場合のエラーのイベントログ     
                        //                //IdcEventlog.ListUnMatchEventlogMake(listFile.FileFullPath);
                        //            }
                        //        }

                        //        //レジスタセット
                        //        var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                        //        if (rIndex >= 0) {
                        //            mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                        //        }

                        //        return sRet;


                        //    }
                        //}
                    }
                }
                    /// <summary>
                    /// 文字列数・バイト数取得にあたり、そのための変数を格納等する変換クラス
                    /// </summary>
                    public class ConvLen : baseConv {
                        public enum prmInd {
                            Hd,
                            Seq,
                            target,
                            type,
                            search,
                            searchType,
                            Register,
                        }

                        private const int _prmCnt = 7;

                        public ConvLen(string[] sRet) {
                            base.pram = sRet;
                            init();
                        }

                        public ConvLen() {
                            base.pram = new string[_prmCnt];
                            init();
                        }
                        private void init() {
                            base.parameterCnt = _prmCnt;
                            base.pram[(int)prmInd.Hd] = ConvType.Len.ToString();
                        }


                        public override string Conv(string[] sMoto, string def) {
                            mMotoDataArray = sMoto;

                            string sRet;
                            string sSearch;
                            string sCountTarget;

                            //対象値
                            if (!base.ParseControlTargetToString(base.pram[(int)prmInd.target], out string sTarget)) { return def; }


                            //検索値
                            if (string.IsNullOrEmpty(base.pram[(int)prmInd.search])) {
                                sSearch = "";
                            } else {
                                if (base.IsTR(base.pram[(int)prmInd.searchType])) {
                                    if (!base.ParseControlTargetToString(base.pram[(int)prmInd.search], out sSearch)) { return "0"; }
                                } else {
                                    sSearch = base.pram[(int)prmInd.search];
                                }
                            }


                            int index;

                            if (string.IsNullOrEmpty(sSearch)) {
                                index = sTarget.Length;
                            } else {
                                index = sTarget.IndexOf(sSearch);
                            }



                            if (index < 0) {
                                sCountTarget = "";
                            } else {
                                sCountTarget = String.Format("{0}{1}", sTarget.Substring(0, index), (sSearch.Length > 0 ? sSearch.Substring(0, 1) : ""));
                            }


                            //文字数
                            if (base.pram[(int)prmInd.type] != "2") {
                                //
                                sRet = sCountTarget.Length.ToString();
                            } else {
                                sRet = Encoding.Default.GetByteCount(sCountTarget).ToString();
                            }




                            //レジスタセット
                            var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                            if (rIndex >= 0) {
                                mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                            }

                            return sRet;
                        }
                    }

                    /// <summary>
                    /// ランダム文字列作成にあたり、そのための変数を格納等する変換クラス
                    /// </summary>
                    public class ConvRNG : baseConv {
                        public enum prmInd {
                            Hd,
                            Seq,
                            charNum,
                            upper,
                            lower,
                            figure,
                            symbol,
                            mandatory,
                            Register,
                        }
                        private const int _prmCnt = 9;

                        public ConvRNG(string[] sRet) {
                            base.pram = sRet;
                            init();
                        }

                        public ConvRNG() {
                            base.pram = new string[_prmCnt];
                            init();
                        }
                        private void init() {
                            base.parameterCnt = _prmCnt;
                            base.pram[(int)prmInd.Hd] = ConvType.RNG.ToString();
                        }


                        public override string Conv(string[] sMoto, string def) {
                            mMotoDataArray = sMoto;

                            //文字数
                            if (!int.TryParse(base.pram[(int)prmInd.charNum], out int Number)) { return def; }

                            if (Number < 4 || Number > 127) { return def; }
                            char[] list = new char[Number];

                            //
                            StringBuilder sb = new StringBuilder();
                            int startIndex = 0;
                            if (IsUseUpper()) {
                                sb.Append(STR_GROUP_ALPHABET_UPPER);
                                if (IsMandatory()) {
                                    list[startIndex] = STR_GROUP_ALPHABET_UPPER[random.Next(0, STR_GROUP_ALPHABET_UPPER.Length)];
                                    ++startIndex;
                                }

                            }

                            if (IsUseLower()) {
                                sb.Append(STR_GROUP_ALPHABET_LOWER);
                                if (IsMandatory()) {
                                    list[startIndex] = STR_GROUP_ALPHABET_LOWER[random.Next(0, STR_GROUP_ALPHABET_LOWER.Length)];
                                    ++startIndex;
                                }
                            }

                            if (IsUseNumber()) {
                                sb.Append(STR_GROUP_NUMBER);
                                if (IsMandatory()) {
                                    list[startIndex] = STR_GROUP_NUMBER[random.Next(0, STR_GROUP_NUMBER.Length)];
                                    ++startIndex;
                                }
                            }

                            if (IsUseSymbol()) {
                                sb.Append(STR_GROUP_SYMBOL);
                                if (IsMandatory()) {
                                    list[startIndex] = STR_GROUP_SYMBOL[random.Next(0, STR_GROUP_SYMBOL.Length)];
                                    ++startIndex;
                                }
                            }


                            string RamdamTargetValue = sb.ToString();

                            if (string.IsNullOrEmpty(RamdamTargetValue)) { return def; }

                            for (int i = startIndex; i < Number; i++) {
                                list[i] = RamdamTargetValue[random.Next(0, RamdamTargetValue.Length)];
                            }

                            string sRet = (new StringBuilder()).Append(list).ToString();

                            //レジスタセット
                            var rIndex = base.RegisterParamToIndex(base.pram[(int)prmInd.Register]);

                            if (rIndex >= 0) {
                                mRegister[base.RegisterParamToIndex(base.pram[(int)prmInd.Register])] = sRet;
                            }

                            return sRet;
                        }

                        private bool IsUseSymbol() {
                            return base.pram[(int)prmInd.symbol] == "1";
                        }

                        private bool IsUseNumber() {
                            return base.pram[(int)prmInd.figure] == "1";
                        }

                        private bool IsUseLower() {
                            return base.pram[(int)prmInd.lower] == "1";
                        }

                        private bool IsUseUpper() {
                            return base.pram[(int)prmInd.upper] == "1";
                        }

                        private bool IsMandatory() {
                            return base.pram[(int)prmInd.mandatory] == "1";
                        }

                    }


                    private static int GetTargetInd(string sTarget) {
                        return GetTargetInd(sTarget, new string[] { });
                    }

                    private static int GetTargetInd(string sTarget, string[] sMoto) {
                        int n = 0;
                        if (sTarget == null || sTarget.Length <= 0) { return -1; }
                        if (sTarget.Substring(0, 1) == "T") {
                            if (!int.TryParse(sTarget.Substring(1, sTarget.Length - 1), out n)) {
                                return -1;
                            }


                            if (sMoto.Length != 0 && n - 1 > sMoto.Length) { return -1; } else {
                                return n;
                            }

                        }

                        return -1;


                    }

                    public static Dictionary<int, ConvSeq> dicSeq = new Dictionary<int, ConvSeq>();


                    public static Dictionary<int, IdcCommon.NewIdcCommon.ConvertFile.ListConvFile> dicList = new Dictionary<int, IdcCommon.NewIdcCommon.ConvertFile.ListConvFile>();
                    public static Dictionary<int, IdcCommon.NewIdcCommon.ConvertFile.ListConvFile> dicPat = new Dictionary<int, IdcCommon.NewIdcCommon.ConvertFile.ListConvFile>();
                    public static List<string> ptnFileList = new List<string>();
                    internal static string STR_GROUP_NUMBER = "0123456789";
                    internal static string STR_GROUP_ALPHABET_UPPER = "ABCDEFGHIJKLNMOPQRSTUVWXYZ";
                    internal static string STR_GROUP_ALPHABET_LOWER = "abcdefghijklnmopqrstuvwxyz";
                    internal static string STR_GROUP_SYMBOL = "#$-=?@[]_!()";
                    internal static Random random = new Random();

                    public static Dictionary<int, string> dicRegister = new Dictionary<int, string>
                    {
                {0,"" },
                {1,"" },
                {2,"" },
                {3,"" },
                {4,"" },
                {5,"" },
                {6,"" },
                {7,"" },
                {8,"" },
                {9,"" },
            };

                    public static string ConvertItem(string[] sAryPre, Reco reco) {
                        string sNow = string.Empty;
                        string sdef = reco.nMoto == 0 || reco.nMoto > sAryPre.Length ? "" : sAryPre[reco.nMoto - 1];
                        if (reco.sType == ConvType.Pat.ToString()) {
                            ConvPat c = new ConvPat(reco.sParam) {
                                sPtn = reco.sParam[3],
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);

                        } else if (reco.sType == ConvType.hold.ToString()) {
                            if (reco.nMoto == 0) { sNow = ""; } else {
                                sNow = sAryPre[reco.nMoto - 1];
                            }

                        } else if (reco.sType == ConvType.Bond.ToString()) {
                            ConvBond c = new ConvBond(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Kakomi.ToString()) {
                            //ConvKakomi c = new ConvKakomi(reco.sParam);

                            //sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Ccut.ToString()) {
                            ConvCcut c = new ConvCcut(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Bcut.ToString()) {
                            ConvBcut c = new ConvBcut(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Add.ToString()) {
                            ConvAdd c = new ConvAdd(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Fmt.ToString()) {
                            ConvFmt c = new ConvFmt(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Cchg.ToString()) {
                            ConvCchg c = new ConvCchg(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Money.ToString()) {
                            ConvMoney c = new ConvMoney(reco.sParam);

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Cmd.ToString()) {
                            ConvCmd c = new ConvCmd(reco.sParam);

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.If.ToString()) {
                            //ConvIf c = new ConvIf(reco.sParam) {
                            //    mRegister = dicRegister
                            //};
                            //c.SetTrueConvParam(reco.sParamSub1);
                            //c.SetFalseConvParam(reco.sParamSub2);
                            //sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Date.ToString()) {
                            ConvDate c = new ConvDate(reco.sParam);

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Calc.ToString()) {
                            //ConvCalc c = new ConvCalc(reco.sParam) {
                            //    mRegister = dicRegister
                            //};

                            //sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Seq.ToString()) {
                            ConvSeq c = dicSeq[int.Parse(reco.sParam[1])];

                            c.mRegister = dicRegister;
                            sNow = c.Conv(sAryPre, sdef);
                            dicSeq[int.Parse(reco.sParam[1])] = c;
                        } else if (reco.sType == ConvType.Trim.ToString()) {
                            var c = new ConvTrim(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Set.ToString()) {
                            var c = new ConvSet(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.List.ToString()) {
                            var c = new ConvList(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.Len.ToString()) {
                            var c = new ConvLen(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else if (reco.sType == ConvType.RNG.ToString()) {
                            ConvRNG c = new ConvRNG(reco.sParam) {
                                mRegister = dicRegister
                            };

                            sNow = c.Conv(sAryPre, sdef);
                        } else {
                            // 2017/02/27修正 #567
                            //sNow = sAryPre[j];
                            sNow = reco.nMoto == 0 ? "" : sAryPre[reco.nMoto - 1];
                        }

                        return sNow;
                    }
                }

                /// <summary>
                /// 文字変換ユーティリティクラスです
                /// </summary>
                public static class ConvertUtil {
                    public static string intListToCSVstring(int[] prmIntAry) {
                        string sRet = "";

                        foreach (int n in prmIntAry) {
                            sRet += n.ToString() + ",";
                        }

                        //2017/02/28 最後のカンマは削除
                        sRet = sRet.TrimEnd(',');

                        return sRet;

                    }

                    private static string stringAryToAnySplitstring(string[] prm, string split) {
                        var returningValue = string.Empty;
                        for (var i = 0; i < prm.Length; i++) {
                            if (string.IsNullOrEmpty(prm[i])) {
                                prm[i] = string.Empty;
                            }

                            switch (prm[i].Contains(split)) {
                                case true:
                                    prm[i] = (prm[i].StartsWith("\"") ? string.Empty : "\"")
                                           + prm[i]
                                           + (prm[i].EndsWith("\"") ? string.Empty : "\"");
                                    break;

                                default:
                                    break;
                            }

                            returningValue += prm[i];
                            returningValue += (i == prm.Length - 1) ? string.Empty : split;
                        }

                        return returningValue;
                    }

                    public static string stringAryToCSVstring(string[] prmStrig) {
                        string sRet = "";
                        //sRet += string.Join(",", prmStrig);
                        sRet += stringAryToAnySplitstring(prmStrig, ",");
                        return sRet;
                    }

                    public static string stringAryToTSVsting(string[] prmStrig) {
                        string sRet = "";
                        //sRet += string.Join(",", prmStrig);
                        sRet += stringAryToAnySplitstring(prmStrig, "   ");
                        return sRet;
                    }

                    public static string stringAryToFixsting(string[] prmStrig) {
                        string sRet = "";
                        //sRet += string.Join(",", prmStrig);
                        sRet += stringAryToAnySplitstring(prmStrig, "");
                        return sRet;
                    }



                    public static string ArystringListToAnystring(List<string[]> lst, INIenum.enumFileType fileType, INIenum.enumRecordEnd rEnd, INIenum.enumFileEnd fEnd) {
                        bool bRet = false;
                        StringBuilder sBuld = new StringBuilder();
                        foreach (string[] str in lst) {
                            if (bRet) { sBuld.Append(INIenum.eRecordEndCls.RetChar(rEnd)); }
                            sBuld.Append(stringAryToAnySplitstring(str, INIenum.eFileTypeCls.RetChar(fileType)));
                            bRet = true;
                        }
                        return sBuld.ToString() + INIenum.eRecordEndCls.RetChar(rEnd) + INIenum.eFileEndCls.RetChar(fEnd);

                    }

                    /// <summary>
                    /// List&lt;string[]&gt; を、CSV形式に変換します。
                    /// </summary>
                    /// <param name="ls"></param>
                    /// <returns></returns>
                    public static string ArystringListToCSVstring(List<string[]> ls) {
                        bool bRet = false;
                        StringBuilder sBuld = new StringBuilder();
                        foreach (string[] str in ls) {
                            if (bRet) { sBuld.Append("\r\n"); }
                            sBuld.Append(stringAryToCSVstring(str));
                            bRet = true;
                        }
                        return sBuld.ToString();
                    }

                }

                /// <summary>
                /// 静的実装の関数クラスです。
                /// </summary>
                public static class StaticFunctionClass {
                    /// <summary>
                    /// Pen及びSizeで指定された形に、イメージを描画します。
                    /// </summary>
                    /// <param name="p"></param>
                    /// <param name="s"></param>
                    /// <returns></returns>
                    //public static Bitmap RetImage(Pen p, Size s)
                    //{
                    //    Bitmap canvas;
                    //    Graphics g;
                    //    //描画先とするImageオブジェクトを作成する
                    //    canvas = new Bitmap(s.Width, s.Height);
                    //    //ImageオブジェクトのGraphicsオブジェクトを作成する
                    //    g = Graphics.FromImage(canvas);

                    //    g.DrawLine(p, 0, 6, canvas.Width, 6);

                    //    g.Dispose();

                    //    return canvas;
                    //}



                    /// <summary>
                    /// Spreadに関するユーティリティクラスです
                    /// </summary>
                    public static class SpreadUtils {
                        //public static void SetColWhdJustSize(object sender)
                        //{
                        //    FpSpread fp = (FpSpread)sender;
                        //    for (int i = 0; i < fp.ActiveSheet.Columns.Count; i++)
                        //    {
                        //        fp.ActiveSheet.Columns[i].Width = fp.ActiveSheet.Columns[i].GetPreferredWidth();
                        //    }
                        //}

                        //public static void SetColHeaderEnterForeColor(FpSpread fp, int col)
                        //{
                        //    fp.ActiveSheet.ColumnHeader.Columns[col].ForeColor = Color.Black;
                        //}

                        //public static void SetColHeaderLeaveForeColor(FpSpread fp, int col)
                        //{
                        //    fp.ActiveSheet.ColumnHeader.Columns[col].ForeColor = Color.White;
                        //}

                        //public static void SetRowHeaderEnterForeColor(FpSpread fp, int row)
                        //{
                        //    fp.ActiveSheet.RowHeader.Rows[row].ForeColor = Color.Black;
                        //}

                        //public static void SetRowHeaderLeaveForeColor(FpSpread fp, int row)
                        //{
                        //    fp.ActiveSheet.RowHeader.Rows[row].ForeColor = Color.White;
                        //}


                        //public static void SetColsVisibleFalse(FpSpread fp)
                        //{
                        //    for (int i = 0; i < fp.ActiveSheet.Columns.Count; i++)
                        //    {
                        //        fp.ActiveSheet.Columns[i].Visible = false;
                        //    }
                        //}
                    }

                    /// <summary>
                    /// string → int型に変換します。
                    /// 変換できない場合、0を返します。
                    /// </summary>
                    /// <param name="str"></param>
                    /// <param name="def"></param>
                    /// <returns></returns>
                    public static int RetstringToInt(string str, int def) {
                        if (!int.TryParse(str, out int nRet)) { return def; }
                        return nRet;
                    }

                    /// <summary>
                    /// string[] → stringに変換します。
                    /// </summary>
                    /// <param name="sAry"></param>
                    /// <returns></returns>
                    public static int[] RetStringAryToIntAry(string[] sAry) {
                        int[] nAry = new int[sAry.Length];

                        for (int i = 0; i < sAry.Length; i++) {
                            nAry[i] = RetstringToInt(sAry[i], 1);
                        }

                        return nAry;
                    }

                    /// <summary>
                    /// svに指定されたSheetVIewに対し、最下段に行を1行追加します。
                    /// </summary>
                    /// <param name="sv"></param>
                    //public static void AddRow(SheetView sv)
                    //{
                    //    if (sv.ActiveRowIndex + 1 == sv.Rows.Count) { sv.AddRows(sv.Rows.Count, 1); }
                    //}

                    //public static void SetFileToRadiobtn(string Ini, Panel Pnl, GcRadioButton defrb)
                    //{
                    //    foreach (GcRadioButton rb in Pnl.Controls.Cast<GcRadioButton>().ToList().Where(rb => rb is GcRadioButton))
                    //    {
                    //        rb.Checked = (rb.Tag.ToString() == Ini);
                    //    }
                    //    if (!Pnl.Controls.Cast<GcRadioButton>().ToList().Exists(rb => rb.Checked)) { defrb.Checked = true; }
                    //}


                }

                /// <summary>
                /// 許容する最大変換回数
                /// </summary>
                public const int MAXTIMES = 100;

                /// <summary>
                /// UI上の変換名称をリストにしています。
                /// </summary>
                public static string[] ConvTypeTitle = CurrentCulture.IsCultureJa
                                                     ? new string[] {
                                                 "",
                                                 "A:四則演算",
                                                 "B:項目結合",
                                                 "C:囲み文字制御",
                                                 "D:日付表示方法",
                                                 "E:文字数制限",
                                                 "F:バイト数制限",
                                                 "G:金額表示方法",
                                                 "H:文字列置換",
                                                 "I:自動番号",
                                                 "J:条件付",
                                                 "K:変換テーブル参照",
                                                 "L:文字列追加",
                                                 "M:コマンド",
                                                 "N:文字形式変換",
                                                 "O:代入",
                                                 "P:除去",
                                                 "Q:リストファイル参照",
                                                 "R:文字列長出力",
                                                 "S:ランダム文字作成",
                                                     }
                                                     : new string[] {
                                                 "",
                                                 "A:Four arithmetic operations",
                                                 "B:Combine items",
                                                 "C:Enclosing character control",
                                                 "D:Date display method",
                                                 "E:Character limit",
                                                 "F:Limit byte count",
                                                 "G:How to display the amount",
                                                 "H:String replacement",
                                                 "I:Automatic number",
                                                 "J:With conditions",
                                                 "K:Refer to conversion table",
                                                 "L:Add string",
                                                 "M:Command",
                                                 "N:Character format conversion",
                                                 "O:Assignments",
                                                 "P:Removal",
                                                 "Q:Refer to the list file",
                                                 "R:String length output",
                                                 "S:Create random character",
                                                     };

                /// <summary>
                /// UI上の変換名称をリストにしています。
                /// </summary>
                public static string[] ConvTypeTitleForIfDlg = CurrentCulture.IsCultureJa
                                                             ? new string[] {
                                                         "値指定",
                                                         "テーブル/レジスタ指定",
                                                         "A:四則演算",
                                                         "B:項目結合",
                                                         "C:囲み文字制御",
                                                         "D:日付表示方法",
                                                         "E:文字数制限",
                                                         "F:バイト数制限",
                                                         "G:金額表示方法",
                                                         "H:文字列置換",
                                                         "K:変換テーブル参照",
                                                         "L:文字列追加",
                                                         "M:コマンド",
                                                         "N:文字形式変換",
                                                         "O:代入",
                                                         "P:除去",
                                                         "Q:リストファイル参照",
                                                         "R:文字列長出力",
                                                         "S:ランダム文字作成",
                                                             }
                                                             : new string[] {
                                                         "Specify value",
                                                         "Table / register specification",
                                                         "A:Four arithmetic operations",
                                                         "B:Combine items",
                                                         "C:Enclosing character control",
                                                         "D:Date display method",
                                                         "E:Character limit",
                                                         "F:Limit byte count",
                                                         "G:How to display the amount",
                                                         "H:String replacement",
                                                         "K:Refer to conversion table",
                                                         "L:Add string",
                                                         "M:Command",
                                                         "N:Character format conversion",
                                                         "O:Assignments",
                                                         "P:Removal",
                                                         "Q:Refer to the list file",
                                                         "R:String length output",
                                                         "S:Create random character",
                                                             };


                /// <summary>
                /// 内部的な変換名（type） → UI上の変換名称 に変換します。
                /// </summary>
                /// <param name="type"></param>
                /// <returns></returns>
                public static string GetTypeName(string type) {
                    switch (type) {
                        case "hold":
                        default:
                            return string.Empty;

                        case "Calc":
                            return CurrentCulture.IsCultureJa
                                   ? "A:四則演算"
                                   : "A:Four arithmetic operations";

                        case "Bond":
                            return CurrentCulture.IsCultureJa
                                   ? "B:項目結合"
                                   : "B:Combine items";

                        case "Kakomi":
                            return CurrentCulture.IsCultureJa
                                   ? "C:囲み文字制御"
                                   : "C:Enclosing character control";

                        case "Date":
                            return CurrentCulture.IsCultureJa
                                   ? "D:日付表示方法"
                                   : "D:Date display method";

                        case "Ccut":
                            return CurrentCulture.IsCultureJa
                                   ? "E:文字数制限"
                                   : "E:Character limit";

                        case "Bcut":
                            return CurrentCulture.IsCultureJa
                                   ? "F:バイト数制限"
                                   : "F:Limit byte count";

                        case "Money":
                            return CurrentCulture.IsCultureJa
                                   ? "G:金額表示方法"
                                   : "G:How to display the amount";

                        case "Cchg":
                            return CurrentCulture.IsCultureJa
                                   ? "H:文字列置換"
                                   : "H:String replacement";

                        case "Seq":
                            return CurrentCulture.IsCultureJa
                                   ? "I:自動番号"
                                   : "I:Automatic number";

                        case "If":
                            return CurrentCulture.IsCultureJa
                                   ? "J:条件付"
                                   : "J:With conditions";

                        case "Pat":
                            return CurrentCulture.IsCultureJa
                                   ? "K:変換テーブル参照"
                                   : "K:Refer to conversion table";

                        case "Add":
                            return CurrentCulture.IsCultureJa
                                   ? "L:文字列追加"
                                   : "L:Add string";

                        case "Cmd":
                            return CurrentCulture.IsCultureJa
                                   ? "M:コマンド"
                                   : "M:Command";

                        case "Fmt":
                            return CurrentCulture.IsCultureJa
                                   ? "N:文字形式変換"
                                   : "N:Character format conversion";

                        case "Set":
                            return CurrentCulture.IsCultureJa
                                   ? "O:代入"
                                   : "O:Assignments";

                        case "Trim":
                            return CurrentCulture.IsCultureJa
                                   ? "P:除去"
                                   : "P:Removal";

                        case "List":
                            return CurrentCulture.IsCultureJa
                                   ? "Q:リストファイル参照"
                                   : "Q:Refer to the list file";

                        case "Len":
                            return CurrentCulture.IsCultureJa
                                   ? "R:文字列長出力"
                                   : "R:String length output";

                        case "RNG":
                            return CurrentCulture.IsCultureJa
                                   ? "S:ランダム文字作成"
                                   : "S:Create random character";
                    }
                }

                /// <summary>
                /// UI上の変換名称（typeName） → 内部的な変換名 に変換します。
                /// </summary>
                /// <param name="typeName"></param>
                /// <returns></returns>
                public static string GetType(string typeName) {
                    switch (typeName) {
                        default:
                            return "hold";

                        case "A:四則演算":
                        case "A:Four arithmetic operations":
                            return "Calc";

                        case "B:項目結合":
                        case "B:Combine items":
                            return "Bond";

                        case "C:囲み文字制御":
                        case "C:Enclosing character control":
                            return "Kakomi";

                        case "D:日付表示方法":
                        case "D:Date display method":
                            return "Date";

                        case "E:文字数制限":
                        case "E:Character limit":
                            return "Ccut";

                        case "F:バイト数制限":
                        case "F:Limit byte count":
                            return "Bcut";

                        case "G:金額表示方法":
                        case "G:How to display the amount":
                            return "Money";

                        case "H:文字列置換":
                        case "H:String replacement":
                            return "Cchg";

                        case "I:自動番号":
                        case "I:Automatic number":
                            return "Seq";

                        case "J:条件付":
                        case "J:With conditions":
                            return "If";

                        case "K:変換テーブル参照":
                        case "K:Refer to conversion table":
                            return "Pat";

                        case "L:文字列追加":
                        case "L:Add string":
                            return "Add";

                        case "M:コマンド":
                        case "M:Command":
                            return "Cmd";

                        case "N:文字形式変換":
                        case "N:Character format conversion":
                            return "Fmt";

                        case "O:代入":
                        case "O:Assignments":
                            return "Set";

                        case "P:除去":
                        case "P:Removal":
                            return "Trim";

                        case "Q:リストファイル参照":
                        case "Q:Refer to the list file":
                            return "List";

                        case "R:文字列長出力":
                        case "R:String length output":
                            return "Len";

                        case "S:ランダム文字作成":
                        case "S:Create random character":
                            return "RNG";
                    }
                }

                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
                public static string[] ConvTypeVal() {
                    string[] sAry = Enum.GetNames(typeof(ConvType));
                    sAry[0] = "";
                    return sAry;
                }

                /// <summary>
                /// 変換名称（name） → 変換種別（ConvType）に変換します。
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public static ConvType RetConvTypeFromName(string name) {
                    string str = name;
                    try {
                        ConvType ct = (ConvType)Enum.Parse(typeof(ConvType), str);

                        return ct;
                    } catch (ArgumentException) {
                        return ConvType.hold;
                    }
                }


                public static int RetRegisterIntegerFromConvType(ConvType convType) {
                    switch (convType) {
                        default:
                        case ConvType.hold:
                            return 0;

                        case ConvType.Calc:
                        case ConvType.Trim:
                            return 5;

                        case ConvType.Bond:
                            return 3;

                        case ConvType.Ccut:
                        case ConvType.RNG:
                            return 8;

                        case ConvType.Bcut:
                        case ConvType.Len:
                            return 6;

                        case ConvType.Cchg:
                            return 9;

                        case ConvType.If:
                            return 20;

                        case ConvType.Add:
                            return 7;

                        case ConvType.Seq:
                        case ConvType.Fmt:
                        case ConvType.Set:
                            return 4;

                        case ConvType.List:
                            return 16;
                    }
                }

                /// <summary>
                /// 変換のための各パラメーターを保存するクラス
                /// </summary>
                [Serializable]
                public class ConvContents {
                    /// <summary>
                    /// 何回目の変換かを表す数（第 CnvTimes 変換である事を示す）
                    /// </summary>
                    public int CnvTimes = 0;

                    /// <summary>
                    /// 列数
                    /// </summary>
                    public int ItmCnt = 0;

                    /// <summary>
                    /// 変換前の各列において、変換に必要なパラメーターを格納するクラスであるRecoクラスを保持。
                    /// </summary>
                    public CnvMethod.Reco[] aryReco = new CnvMethod.Reco[] { };

                    public ConvContents(int Cnt) {
                        ItmCnt = Cnt;
                        aryReco = new CnvMethod.Reco[Cnt];
                    }

                    public ConvContents() {

                    }
                }

            }
        }
