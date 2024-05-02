using System;
using System.Collections.Generic;
using System.Linq;

namespace IdcRecordConvert.classes
{
    public static class INIenum
    {
        interface IConv
        {
            string EnumToStr();
            Enum StrToEnum(string str);
        }

        public enum enumFileType
        {
            csv,
            tsv,
            fix
        }
        
        public enum enumRecordEnd
        {
            none,
            cr0D,
            lf0A,
            crlf
        }

        public enum enumFileEnd
        {
            none,
            gs1A
        }

        public enum enumAfter
        {
            none,
            time,
            del
        }
        public enum enumUseEncode
        {
            SJIS = 932,
            Unicode = 1200,
            UTF8 = 65001,
            ANSI = 20127
        }
        public enum enumMatrix
        {
            Xstart,
            Xcount,
            Ystart,
            Ycount,
            XDstart,
            XDcount,
            YDstart,
        }
        public static class eFileEndCls
        {
            

            private static Dictionary<enumFileEnd, string> _dicEnumStr = new Dictionary<enumFileEnd, string>()
            {
                { enumFileEnd.none,"none" },
                { enumFileEnd.gs1A,"1A" }                
            };

            private static Dictionary<enumFileEnd, string> _dicEnumChar = new Dictionary<enumFileEnd, string>()
            {
                { enumFileEnd.none,"" },
                { enumFileEnd.gs1A,((char)26).ToString() }
        
            };

            public static enumFileEnd RetEnum(string s)
            {
                if (!_dicEnumStr.ContainsValue(s)) { return enumFileEnd.none; }
                return _dicEnumStr.First(x => x.Value == s).Key;
            }

            public static string Retstr(enumFileEnd enm)
            {
                return _dicEnumStr[enm];
            }

            public static string RetChar(enumFileEnd enm)
            {
                return _dicEnumChar[enm];
            }

        }

        public static class eAfterCls
        {
            private static Dictionary<enumAfter, string> _dicEnumStr = new Dictionary<enumAfter, string>()
            {
                { enumAfter.none,"none" },
                { enumAfter.time,"time" },
                { enumAfter.del,"del" }
            };

            public static enumAfter RetEnum(string s)
            {
                if (!_dicEnumStr.ContainsValue(s)) { return enumAfter.none;  }
                return _dicEnumStr.First(x => x.Value == s).Key;
            }

            public static string Retstr(enumAfter enm)
            {
                return _dicEnumStr[enm];
            }

        }



        public static class eEncodeCls
        {
            private static Dictionary<enumUseEncode, string> _dicEnumStr = new Dictionary<enumUseEncode, string>()
            {
                { enumUseEncode.SJIS,"sjis" },
                { enumUseEncode.Unicode,"unicode" },
                { enumUseEncode.UTF8,"utf8" },
                { enumUseEncode.ANSI,"ansi" }
            };

            public static enumUseEncode RetEnum(string s)
            {
                if (!_dicEnumStr.ContainsValue(s)) { return enumUseEncode.SJIS; }
                return _dicEnumStr.First(x => x.Value == s).Key;
            }

            public static string Retstr(enumUseEncode enm)
            {
                return _dicEnumStr[enm];
            }
        }


        public static class eRecordEndCls
        {
            private static Dictionary<enumRecordEnd, string> _dicEnumStr = new Dictionary<enumRecordEnd, string>()
            {
                { enumRecordEnd.none,"none" },
                { enumRecordEnd.cr0D,"0D" },
                { enumRecordEnd.lf0A,"0A" },
                { enumRecordEnd.crlf,"0D0A" }
            };

            private static Dictionary<enumRecordEnd, string> _dicEnumChar = new Dictionary<enumRecordEnd, string>()
            {
                { enumRecordEnd.none,"" },
                { enumRecordEnd.cr0D,"\r" },
                { enumRecordEnd.lf0A,"\n" },
                { enumRecordEnd.crlf,"\r\n" }
            };


            public static enumRecordEnd RetEnum(string s)
            {
                if (!_dicEnumStr.ContainsValue(s)) { return enumRecordEnd.none; }
                return _dicEnumStr.First(x => x.Value == s).Key;
            }

            public static string Retstr(enumRecordEnd enm)
            {
                return _dicEnumStr[enm];
            }

            public static string RetChar(enumRecordEnd enm)
            {
                return _dicEnumChar[enm];
            }



        }



        public static class eFileTypeCls
        {

            private static Dictionary<enumFileType, string> _dicEnumStr = new Dictionary<INIenum.enumFileType, string>()
            {
                { enumFileType.csv,"csv" },
                { enumFileType.tsv,"tsv" },
                { enumFileType.fix,"fix" }
            };

            private static Dictionary<enumFileType, string> _dicEnumChar = new Dictionary<INIenum.enumFileType, string>()
            {
                { enumFileType.csv,"," },
                { enumFileType.tsv,"\t" },
                { enumFileType.fix,"" }
            };

            public static enumFileType RetEnum(string s)
            {
                if (!_dicEnumStr.ContainsValue(s)) { return enumFileType.csv; }
                return _dicEnumStr.First(x => x.Value == s).Key;
            }

            public static string Retstr(enumFileType enm)
            {
                return _dicEnumStr[enm];
            }

            public static string RetChar(enumFileType enm)
            {
                return _dicEnumChar[enm];
            }
        }


    }
}
