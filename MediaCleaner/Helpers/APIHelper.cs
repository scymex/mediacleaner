using System;
using System.Security.Cryptography;
using System.Text;


namespace MediaCleaner
{
    public static class APIHelper
    {
        /*public bool CheckSettings()
        {
            var validSettings = true;
            if (!CheckApikey() || Properties.Settings.Default.emby_userid == "" || Properties.Settings.Default.emby_accesstoken == "" || Int32.Parse(Properties.Settings.Default.episodesToKeep) < 0 || Int32.Parse(Properties.Settings.Default.interval) < 0)
                validSettings = false;

            return validSettings;
        }*/

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        public static string SHA1Hash(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        public static string MD5Hash(string input)
        {
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            return HexStringFromBytes(bytes);
        }

        public static int DateTimeToInt(DateTime theDate)
        {
            return (int)(theDate.Date - new DateTime(1900, 1, 1)).TotalDays + 2;
        }
    }
}
