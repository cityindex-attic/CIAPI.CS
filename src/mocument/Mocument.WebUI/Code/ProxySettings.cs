using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;


namespace Mocument.WebUI.Code
{

    public static class ProxySettings
    {
        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static string GetUserId()
        {
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
         
            return CalculateMD5Hash(id.Name);
        }
        public static string GetProxyUrl()
        {
            return HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + ":" + ProxySettings.Port + "/";
        }

        static ProxySettings()
        {
            int port = int.Parse(WebConfigurationManager.AppSettings["proxyPort"]);
            ProxySettings.Port = port;
            bool lockDown = bool.Parse(WebConfigurationManager.AppSettings["proxySecured"]);
            ProxySettings.LockDown = lockDown;

        }
        public static int Port { get; set; }
        public static bool LockDown { get; set; }
        public static string MungTapeId(object id)
        {
            var i = id.ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var username = i[0];
            i.RemoveAt(0);
            var d = string.Join(".", i);
            return username + "/" + d;
        }
        public static string GetTapeId(object id)
        {
            var i = id.ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            i.RemoveAt(0);
            return string.Join(".", i);
        }
    }
}