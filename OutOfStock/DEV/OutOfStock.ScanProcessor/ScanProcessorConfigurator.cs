using OOS.Model;
using OOSCommon;

namespace OutOfStock.ScanProcessor
{
    public class ScanProcessorConfigurator : BasicConfigurator, IConfigurator
    {
        public string GetSessionID()
        {
            //var sessionId = " ".PadBoth(24);
            //if (HttpContext.Current != null && HttpContext.Current.Session != null &&
            //    !string.IsNullOrWhiteSpace(HttpContext.Current.Session.SessionID))
            //    sessionId = HttpContext.Current.Session.SessionID;
            //return sessionId;
            return "[sessionid]";
        }

        public string TemporaryDownloadFilePath()
        {
            return "c:\\";
        }
    }
}