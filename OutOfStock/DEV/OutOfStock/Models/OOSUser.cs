using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OOSCommon.DataContext;
using StructureMap;

namespace OutOfStock.Models
{
    public class OOSUser
    {
        protected const string userRegionSessionKey = "OutOfStock.Models.User.userRegion";
        protected const string userStoreSessionKey = "OutOfStock.Models.User.userStore";
        protected const string userPermissionSessionKey = "OutOfStock.Models.User.userPermission";
        protected const string userADPhysicalDeliveryOfficeName = "OutOfStock.Models.User.physicalDeliveryOfficeName";
        protected const string userADMissingOfficeDetails = "OutOfStock.Models.User.MissingOfficeDetails";

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


        private static bool _hasValidLocationInformation;
        public static bool HasValidLocationInformation { get { return _hasValidLocationInformation; } set { _hasValidLocationInformation = value; } }

        protected static void SetSessionLoggedinUserLocation()
        {
            string userRegionInner = string.Empty;
            string userStoreInner = string.Empty;
            HasValidLocationInformation =  GetLoggedinUserLocation(out userRegionInner, out userStoreInner);
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

           var userName = string.Empty;
            var ForceRegionalUsersList = string.Empty;

            try
            {
                 userName = HttpContext.Current.User.Identity.Name.ToLower();
                 ForceRegionalUsersList = OOSCommon.AppConfig.AppSettings["ForceRegionalUsersList"];

                if (ForceRegionalUsersList != string.Empty)
                {
                    if (ForceRegionalUsersList.Split(',').Any(u => u == userName))
                    {
                        OutOfStock.MvcApplication.oosLog.Info(string.Format(
                            "Forcing user into Regional role.Username: {0}", userName));
                        userPermission = OOSRoles.Store | OOSRoles.Leader | OOSRoles.Regional;
                    }
                }

            }
            catch 
            {
                OutOfStock.MvcApplication.oosLog.Info(string.Format(
                    "Unable to process ForceRegionalUsersList. List: {0} Username: {1}", ForceRegionalUsersList, userName));
            }
            
            
            return userPermission;
        }



        public static  UserLocation GetUsersLocation(string alias)
        {
            UserLocation userResult = null;
            string fullUsername = string.Empty;
            string displayName = string.Empty;
            string storevalue = string.Empty;
            string regionvalue = string.Empty;


            // return new UserLocation() { FriendlyName = "Test User", Region = "SW", Store = "LMR"};
            if (string.IsNullOrEmpty(alias.Trim()))
                throw new Exception("alias cant be empty.");
           

            try
            {
                fullUsername = alias;
                if (alias.StartsWith("WFM"))
                {
                    alias = alias.Split('\\')[1];
                }

                using (
                    DirectoryEntry rootEntry = new DirectoryEntry("LDAP://wfm.pvt")
                    {
                        AuthenticationType = AuthenticationTypes.None
                    })
                {
                    var searcher = new DirectorySearcher(rootEntry);
                    var queryFormat = "(&(objectClass=user)(objectCategory=person)(|(SAMAccountName={0})))";
                    searcher.Filter = string.Format(queryFormat, alias);
                    SearchResultCollection results = searcher.FindAll();


                    if (results.Count > 1)
                    {
                        throw new Exception("Found more than one AD user for [" + alias + "]. Unable to continue.");
                    }

                    else if (results.Count <= 0)
                    {
                        // none found
                        throw new Exception("Found no AD matches for [" + alias + "]. Unable to continue.");
                    }
                    else if (results.Count == 1)
                    {
                        //match
                        if (results[0].Properties["displayName"].Count > 0)
                            displayName = results[0].Properties["displayName"][0].ToString();


                        if (results[0].Properties["WFMDivisionCode"].Count > 0)
                            regionvalue = results[0].Properties["WFMDivisionCode"][0].ToString();


                        if (results[0].Properties["wfmlocationcode"].Count > 0)
                            storevalue = results[0].Properties["wfmlocationcode"][0].ToString();


                        userResult = new UserLocation()
                        {
                            FriendlyName = displayName,
                            Region = regionvalue,
                            Store = storevalue
                        };

                        var regionOverride = string.Empty;
                        var storeOverride = string.Empty;
                        var loginManager = ObjectFactory.GetInstance<IUserLoginManager>();
                        loginManager.GetLocationOverrides(fullUsername, out regionOverride, out storeOverride);

                        if (!string.IsNullOrEmpty(regionOverride)) userResult.Region = regionOverride;
                        if (!string.IsNullOrEmpty(storeOverride)) userResult.Store = storeOverride;
                        if (!string.IsNullOrEmpty(storeOverride) || !string.IsNullOrEmpty(regionOverride))
                        {
                            userResult.LocationOverride = true;
                            MvcApplication.oosLog.Warn(
                                $"Location Override for {alias} from {regionvalue}:{storevalue} to {regionOverride}:{storeOverride}");
                        }
                    }
                }


                return userResult;

            }
            catch (Exception ex)
            {
                // An ill-formed directory entry could cause null exceptions above but is survivable
                MvcApplication.oosLog.Warn(string.Format("==> {0} stack= {1}", ex.Message, ex.StackTrace));
            }
            return new UserLocation();
        }



        protected static bool GetLoggedinUserLocation(out string userRegion, out string userStore)
        {
            bool isOk = true;
            userRegion = string.Empty;
            userStore = string.Empty;
            var userName = string.Empty;
            try
            {
                
                var userInfo = GetUsersLocation(HttpContext.Current.User.Identity.Name);

                userRegion = userInfo.Region;
                userStore = userInfo.Store;

                //string physicalDeliveryOfficeName = GetPhysicalDeliveryOfficeName(userName);
                //string[] words = physicalDeliveryOfficeName.Split(new char[] { ' ' });
                //switch (words.Length)
                //{
                //    // No value happens and it is an error
                //    case 0:
                //        break;
                //    // 1 value happens even though it is an error
                //    case 1:
                //        if (words[0].Length == 2)
                //            userRegion = words[0];
                //        else
                //            userStore = words[0];
                //        break;
                //    default:
                //        userRegion = words[0];
                //        userStore = words[1];
                //        break;
                //}
                if (userRegion.Length != 2 || userStore.Length != 3)
                {
                    HttpContext.Current.Session[userADMissingOfficeDetails] = true;
                    HttpContext.Current.Session[userADPhysicalDeliveryOfficeName] = string.Empty;
                    MvcApplication.oosLog.Warn(string.Format("Invalid AD Office value: [{0} {1}] for {2}", userRegion, userStore, userName));
                    isOk = false;
                }

            }
            catch (Exception ex)
            {
                OutOfStock.MvcApplication.oosLog.Warn(ex.Message + ", Stack=" + ex.StackTrace);
                isOk = false;
            }
            try
            {

                // force CE users to emulate a specific region.
                if (userRegion == "CE") userRegion = OOSCommon.AppConfig.AppSettings["CEDefaultRegion"];

                // some users in the NC region have NP as their Region Abbreviation. NC used to be called NP in the past. 
                // They still consider NP valid in AD. we must hadle it.
                if (userRegion == "NP") userRegion = "NC";

                // UK in AD but EU in the rest of the systems. 
                if (userRegion == "UK") userRegion = "EU";

                
                    

            }
            catch 
            {
                userRegion = "NC";
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

                physicalDeliveryOfficeName = directoryEntry.Properties["physicalDeliveryOfficeName"].Value == null ? string.Empty : directoryEntry.Properties["physicalDeliveryOfficeName"].Value.ToString();
                //physicalDeliveryOfficeName = directoryEntry.Properties["physicalDeliveryOfficeName"].Value.ToString();
            }
            catch (Exception ex)
            {
                // An ill-formed directory entry could cause null exceptions above but is survivable
                MvcApplication.oosLog.Warn(string.Format("GetPhysicalDeliveryOfficeName({0}): {1}; {2}", userName, ex.Message, ex.StackTrace));
            }

            if (physicalDeliveryOfficeName == string.Empty) MvcApplication.oosLog.Warn(string.Format("physicalDeliveryOfficeName was not found in AD for {0}", userName));
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
