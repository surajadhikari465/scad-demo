using System.Web;
using OOS.Model;
using OOSCommon;

namespace OutOfStock
{
    public class Configurator : BasicConfigurator, IConfigurator
    {
        public string GetSessionID()
        {
            string sessionID = " ".PadBoth(24);
            if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                !string.IsNullOrWhiteSpace(HttpContext.Current.Session.SessionID))
                sessionID = HttpContext.Current.Session.SessionID;
            return sessionID;
        }

        public string TemporaryDownloadFilePath()
        {
            return HttpContext.Current.Server.MapPath(string.Empty);
        }
    }
}
