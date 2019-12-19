using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OOSCommon.DataContext;

namespace OutOfStock.Models
{
    public class OOSUser
    {
        protected const string userRegionSessionKey = "OutOfStock.Models.User.userRegion";
        protected const string userStoreSessionKey = "OutOfStock.Models.User.userStore";
        protected const string userPermissionSessionKey = "OutOfStock.Models.User.userPermission";

        [Flags]
        public enum OOSRoles : int { None = 0, Store = 1, Leader = 2, Regional = 4, Central = 8 };

        protected static string[] securityGroupsRegional
        {
            get
            {
                if (_securityGroupsRegional == null)
                    _securityGroupsRegional = GetListFromSetting("SecurityGroupsRegional");
                return _securityGroupsRegional;
            }
        } static string[] _securityGroupsRegional = null;
        protected static string[] securityStoresRegional
        {
            get
            {
                if (_securityStoresRegional == null)
                    _securityStoresRegional = GetListFromSetting("SecurityStoresRegional");
                return _securityStoresRegional;
            }
        } static string[] _securityStoresRegional = null;
        protected static string[] securityGroupsCentral
        {
            get
            {
                if (_securityGroupsCentral == null)
                    _securityGroupsCentral = GetListFromSetting("SecurityGroupsCentral");
                return _securityGroupsCentral;
            }
        } static string[] _securityGroupsCentral = null;
        protected static string[] securityStoresCentral
        {
            get
            {
                if (_securityStoresCentral == null)
                    _securityStoresCentral = GetListFromSetting("SecurityStoresCentral");
                return _securityStoresCentral;
            }
        } static string[] _securityStoresCentral = null;

        public static string userRegion
        {
            get
            {
                object obj = HttpContext.Current.Session[userRegionSessionKey];
                if (obj != null)
                    return (string)obj;
                SetSessionLoggedinUserLocation();
                return (string)HttpContext.Current.Session[userRegionSessionKey];
            }
            set { HttpContext.Current.Session[userRegionSessionKey] = value; }
        }
        public static string userStore
        {
            get
            {
                object obj = HttpContext.Current.Session[userStoreSessionKey];
                if (obj != null)
                    return (string)obj;
                SetSessionLoggedinUserLocation();
                return (string)HttpContext.Current.Session[userStoreSessionKey];
            }
            set { HttpContext.Current.Session[userStoreSessionKey] = value; }
        }
        public static OOSRoles userPermissions
        {
            get
            {
                object obj = HttpContext.Current.Session[userPermissionSessionKey];
                if (obj != null)
                    return (OOSRoles)obj;
                SetSessionLoggedinUserPermission();
                return (OOSRoles)HttpContext.Current.Session[userPermissionSessionKey];
            }
            set { HttpContext.Current.Session[userPermissionSessionKey] = value; }
        }
        public static bool isStoreLevel
        {
            get { return true; }
        }
        public static bool isRegionalBuyer
        {
            get { return (userPermissions & (OOSRoles.Regional | OOSRoles.Central)) != OOSRoles.None; }
            set { userPermissions = userPermissions | OOSRoles.Regional; }
        }
        public static bool isCentral
        {
            get { return (userPermissions & OOSRoles.Central) != OOSRoles.None; }
            set { userPermissions = userPermissions | OOSRoles.Central; }
        }

        public static bool EnableUDPLoggingForUser()
        {
            var result = false;
            using (var context = new OOSEntities(MvcApplication.oosEFConnectionString))
             {
                 const string sql = "select ConfigKey +':'+ConfigValue AS Value from ApplicationConfig where configgroup = 'UDPLogging'";
                 var lookuptable = context.ExecuteStoreQuery<string>(sql).ToList().ToLookup(item => item.Split(':')[0], item => item.Split(':')[1]);
                 
                 if (lookuptable["UDPFilter"].ToString().ToLower().Equals("on"))
                 {
                     result = true;
                 }
            }

            return true;
        }
        /// <summary>
        /// Get logged in user's user name (may include the domain)
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            string UserName = string.Empty;
            try
            {
                UserName = HttpContext.Current.User.Identity.Name;
            }
            catch (Exception ex)
            {
                OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                return null; 
            }
            return UserName;
        }

        protected static void SetSessionLoggedinUserLocation()
        {
            string userRegionInner = string.Empty;
            string userStoreInner = string.Empty;
            GetLoggedinUserLocation(out userRegionInner, out userStoreInner);
            HttpContext.Current.Session[userRegionSessionKey] = userRegionInner;
            HttpContext.Current.Session[userStoreSessionKey] = userStoreInner;
            OutOfStock.MvcApplication.oosLog.Info("User=" + GetUserName() +
                ", Region=" + userRegionInner + ", Store=" + userStoreInner);
        }

        protected static void SetSessionLoggedinUserPermission()
        {
            OOSRoles userPermissionInner = GetLoggedinUserPermission();
            HttpContext.Current.Session[userPermissionSessionKey] = userPermissionInner;
            {
                string userPermissionText = string.Empty;
                foreach (OOSRoles oosRolesEnum in Enum.GetValues(typeof(OOSRoles)))
                {
                    if ((userPermissionInner & oosRolesEnum) != OOSRoles.None)
                    {
                        if (userPermissionText.Length > 0)
                            userPermissionText += "|";
                        userPermissionText += oosRolesEnum.ToString();
                    }
                }
                if (userPermissionText.Length < 1)
                    userPermissionText = OOSRoles.None.ToString();
                OutOfStock.MvcApplication.oosLog.Info("User=" + GetUserName() + ", Permissions=" + userPermissionText);
            }
        }

        protected static OOSRoles GetLoggedinUserPermission()
        {
            OOSRoles userPermission = OOSRoles.Store | OOSRoles.Leader;
            if (securityStoresCentral.Contains(userStore))
                userPermission |= OOSRoles.Regional | OOSRoles.Central;
            else if (securityGroupsCentral.Where(role => Roles.IsUserInRole(role)).Any())
                userPermission |= OOSRoles.Regional | OOSRoles.Central;
            else if (securityStoresRegional.Contains(userStore))
                userPermission |= OOSRoles.Regional;
            else if (securityGroupsRegional.Where(role => Roles.IsUserInRole(role)).Any())
                userPermission |= OOSRoles.Regional;
            return userPermission;
        }

        protected static bool GetLoggedinUserLocation(out string userRegion, out string userStore)
        {
            bool isOk = true;
            userRegion = string.Empty;
            userStore = string.Empty;
            try
            {
                string userName = HttpContext.Current.User.Identity.Name;
                string physicalDeliveryOfficeName = GetPhysicalDeliveryOfficeName(userName);
                string[] words = physicalDeliveryOfficeName.Split(new char[] { ' ' });
                switch (words.Length)
                {
                    // No value happens and it is an error
                    case 0:
                        break;
                    // 1 value happens even though it is an error
                    case 1:
                        if (words[0].Length == 2)
                            userRegion = words[0];
                        else
                            userStore = words[0];
                        break;
                    default:
                        userRegion = words[0];
                        userStore = words[1];
                        break;
                }
                if (words.Length != 2 || userRegion.Length != 2 || userStore.Length != 3)
                    OutOfStock.MvcApplication.oosLog.Warn("Invalid AD Office value, \"" +
                        physicalDeliveryOfficeName + "\" for " + userName);
            }
            catch (Exception ex)
            {
                OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                isOk = false;
            }
            return isOk;
        }

        /// <summary>
        /// Return current Region and Store for user userName
        /// "CE CEN" is region CE, store "CEN"
        /// </summary>
        /// <returns></returns>
        protected static string GetPhysicalDeliveryOfficeName(string userName)
        {
            string physicalDeliveryOfficeName = string.Empty;
            try
            {
                // Strip all but username
                {
                    string[] userNameParts = userName.Split(new char[] { '\\' });
                    if (userNameParts.Length > 1)
                        userName = userNameParts[userNameParts.Length - 1];
                }
                DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
                string defaultNamingContext = rootDSE.Properties["DefaultNamingContext"].Value.ToString();
                DirectoryEntry root = new DirectoryEntry("LDAP://" + defaultNamingContext);
                DirectorySearcher searcher = new DirectorySearcher(root);
                searcher.Filter = "(&(ObjectCategory=person)(objectClass=User)(samAccountName=" + userName + "))";
                searcher.SearchScope = SearchScope.Subtree;
                SearchResult searchResult = searcher.FindOne();
                string distinguishedName = searchResult.Properties["distinguishedName"][0].ToString();
                DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + distinguishedName);
                physicalDeliveryOfficeName = directoryEntry.Properties["physicalDeliveryOfficeName"].Value.ToString();
            }
            catch (Exception ex)
            {
                // An ill-formed directory entry could cause null exceptions above but is survivable
                OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
            }
            return physicalDeliveryOfficeName;
        }

        /// <summary>
        /// Get an array of strings from a web.config appsetting
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        protected static string[] GetListFromSetting(string settingName)
        {
            string[] settings;
            try
            {
                string setting = OOSCommon.AppConfig.AppSettings[settingName];
                settings = setting.Split(new char[] { ',' });
                for (int ix = 0; ix < settings.Length; ++ix)
                    settings[ix] = settings[ix].Trim();
                if (settings.Length == 1 && settings[0].Length == 0)
                    settings = new string[] { };
            }
            catch (Exception)
            {
                settings = new string[] { };
            }
            return settings;
        }

    }
}
