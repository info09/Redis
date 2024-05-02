namespace IdcCommon.CommonMethods
{
    /// <summary>
    /// 言語圏クラス（静的メソッド）
    /// </summary>
    public static class CurrentCulture
    {
        /// <summary>
        /// 言語が日本語かを判定します。（true:日本語）
        /// </summary>
        public static bool IsCultureJa = System.Globalization.CultureInfo.CurrentUICulture.Name.Contains("ja");
    }
}
