using System;
using System.IO;

namespace IdcCommon.CommonMethods
{
    public static class Roaming
    {
        private static readonly string IcspRoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ICSP";
        private static readonly string IdcRoamingPath = IcspRoamingPath + @"\IDC";
        public static readonly string IdcIniRoamingPath = IdcRoamingPath + @"\INI";

        /// <summary>
        /// Roamingフォルダ直下にIDCのINIフォルダがあるかをチェックし、なければフォルダ作成・及びRegistryClass.IniFolderにあるINIフォルダをコピー。
        /// コピー失敗した場合はfalseを返します。
        /// </summary>
        public static bool CheckExistInifileFolder()
        {
            try
            {
                if (!Directory.Exists(IcspRoamingPath))
                {
                    Directory.CreateDirectory(IcspRoamingPath);
                }
                if (!Directory.Exists(IdcRoamingPath))
                {
                    Directory.CreateDirectory(IdcRoamingPath);
                }
                if (!Directory.Exists(IdcIniRoamingPath))
                {
                    CopyDirectory(Registry.IniFolder, IdcIniRoamingPath);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static void CopyDirectory(string sourceDirName, string destDirName)
        {
            //コピー先のディレクトリがないときは作る
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                //属性もコピー
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
            { destDirName += Path.DirectorySeparatorChar; }

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            { File.Copy(file, destDirName + Path.GetFileName(file), true); }


            //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            { CopyDirectory(dir, destDirName + Path.GetFileName(dir)); }
        }
    }
}
