using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for Config
/// </summary>
/// 
namespace POReports
{
    public class Config
    {
        public static string ConnectionString {
            get
            {
                return ConfigurationManager.ConnectionStrings["POConnectionString"].ToString();
            }
        }

        public static string DataFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteDataFolder"].ToString();
            }
        }

        public static string DataURL
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteDataURL"].ToString();
            }
        }

        public static System.Web.SessionState.HttpSessionState Session
        {
            get
            {
                if (Context == null) return null;
                else return Context.Session;
            }
        }
        public static System.Web.HttpContext Context = null;

        public static string GetSessionProperty(string name)
        {
            if (Session == null || Session[name] == null)
            {
                return string.Empty;
            }
            else
            {
                return Session[name].ToString();
            }
        }
    }
}