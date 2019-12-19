using System.Web;
using OOS.Model;
using OutOfStock;

namespace OOSCommon
{
    public class Configurator : BasicConfigurator, IConfigurator
    {
        public string GetSessionID()
        {
            var sessionId = " ".PadBoth(24);
            if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                !string.IsNullOrWhiteSpace(HttpContext.Current.Session.SessionID))
                sessionId = HttpContext.Current.Session.SessionID;
            return sessionId;
        }

        public string TemporaryDownloadFilePath()
        {
            return HttpContext.Current.Server.MapPath(string.Empty);
        }
    }
}
