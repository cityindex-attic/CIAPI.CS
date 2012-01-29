using System.Globalization;

namespace SOAPI2.DocScraper
{
    /// <summary>
    /// from http://theburningmonk.com/2010/08/dotnet-tips-string-totitlecase-extension-methods/
    /// </summary>
    public static class StringExtension
    {
        public static string PascalCased(this string str)
        {
            str = str.Replace("-", " ");
            str = str.Replace("_", " ");
            str = str.ToTitleCase();
            str = str.Replace(" ", "");
            return str;
        }

        /// <summary>
        /// Use the current thread's culture info for conversion
        /// </summary>
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the culture info with the specified name
        /// </summary>
        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            var cultureInfo = new CultureInfo(cultureInfoName);
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        /// <summary>
        /// Overload which uses the specified culture info
        /// </summary>
        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}