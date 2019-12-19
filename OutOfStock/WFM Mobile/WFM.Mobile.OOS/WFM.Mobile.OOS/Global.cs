using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace WFM.Mobile.OOS
{
    static class Global
    {

        public static string RegionCode { get; set; }
        public static string ServiceURI { get; set; }
        public static bool UserAuthenticated { get; set; }
        public static string UserName { get; set; }
        public static string UserEmail { get; set; }
        public static string PluginName { get; set; }
        public static string AssemblyVersion { get; set; }
        public static string StoreAbbrev { get; set; }
        public static string OOSWebService { get; set; }
        public static string BizTalkWebService { get; set; }
        public static bool ForceClose { get; set; }
        


        public const string  ConfigFilePath = "OOSSettings.xml";

        public static bool isNSC2(string upc)
        {
            return new System.Text.RegularExpressions.Regex("^02[0-9]{10}$").Match(upc).Success;
        }

        public static string convertToNSC2(string upc)
        {
            return string.Format("{0}00000", upc.Substring(0, 7));
        }

        public static bool TcpSocketTest()
        {
            var returnvalue = true;
            try
            {
                var client = new TcpClient("www.google.com", 80);
                client.Close();
            }
            catch (Exception)
            {
                returnvalue = false;
            }
            return returnvalue;
        }
    }
}
