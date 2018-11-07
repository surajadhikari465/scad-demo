using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

using WFM.Helpers;

namespace IRMAUserAuditConsole
{
    public enum UserUpdateError : int { None = 0, StoreNotFound, TitleNotFound, Other }
    public enum UserRestoreError : int { None = 0, UserNotFound, Other }

    public class UserRepository
    {
        IRMAUserAuditConsoleDataClassesDataContext db;// = new IRMAUserAuditDataClassesDataContext();
        List<User> userCache = new List<User>();
        List<TitlePermissionOverride> tpoCache = new List<TitlePermissionOverride>();
        List<TitleDefaultPermission> tdpCache = new List<TitleDefaultPermission>();
        List<SlimAccess> slimCache = new List<SlimAccess>();
        List<Team> teamCache = new List<Team>();
        List<UserStoreTeamTitle> usttCache = new List<UserStoreTeamTitle>();

        //Log log = new Log(@"C:\temp\UserAudit\Repository.log", true);

        private Dictionary<int, TitleDefaultPermission> titlePermissionCache = new Dictionary<int, TitleDefaultPermission>();

        public UserRepository(string connectionString)
        {
            db = new IRMAUserAuditConsoleDataClassesDataContext(connectionString);
            try
            {
                PopulateDefaultPermissionCache();
                PopulateOverrideCache();
                PopulateUserCache(true);
                PopulateSlimCache();
                PopulateTeamCache();
                PopulateUSTTCache();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserRepository(string connectionString, bool allUsers)
        {
            db = new IRMAUserAuditConsoleDataClassesDataContext(connectionString);
            try
            {
                PopulateDefaultPermissionCache();
                PopulateOverrideCache();
                PopulateUserCache(!allUsers);
                PopulateSlimCache();
                PopulateTeamCache();
                PopulateUSTTCache();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PopulateUSTTCache()
        {
            usttCache = db.UserStoreTeamTitles.ToList();

        }

        private void PopulateTeamCache()
        {
            teamCache = db.Teams.ToList();
        }

        private void PopulateSlimCache()
        {
            slimCache = db.SlimAccesses.ToList();
        }

        private void PopulateDefaultPermissionCache()
        {
            tdpCache = db.TitleDefaultPermissions.ToList();
        }

        private void PopulateOverrideCache()
        {
            tpoCache = db.TitlePermissionOverrides.ToList();
        }

        private void PopulateUserCache(bool enabledOnly)
        {
            if (enabledOnly)
            {
                userCache = db.Users.Where(u => u.AccountEnabled == true).ToList();
            }
            else
            {
                userCache = db.Users.ToList();
            }
        }

        public List<User> GetUsersTable(bool enabled)
        {
            return db.Users.Where(u => u.AccountEnabled == enabled).ToList();
        }

        public List<User> GetUsersTableByStore(bool enabled, int storeLimit)
        {
            if (storeLimit == -1)
                return db.Users.Where(u => u.AccountEnabled == enabled && u.Telxon_Store_Limit == null).ToList();


            return db.Users.Where(u => u.AccountEnabled == enabled && u.Telxon_Store_Limit == storeLimit).ToList();
        }

        public TitleDefaultPermission GetDefaultPermissionsByTitleId(int? id)
        {
            if (id == null)
                return new TitleDefaultPermission();

            if (titlePermissionCache.ContainsKey(id.Value))
            {
                return titlePermissionCache[id.Value];
            }

            TitleDefaultPermission tdp = db.TitleDefaultPermissions.SingleOrDefault(dp => dp.TitleId == id);
            if (tdp != null)
                titlePermissionCache.Add(tdp.TitleId.Value, tdp);

            return tdp;
        }

        public UserInfo GetUserInfo(int id)
        {
            User u = userCache.SingleOrDefault(user => user.User_ID == id);
            UserInfo ui = UserInfoFromUser(u);

            return ui;
        }

        public int? GetTitleIdFromName(string titleName)
        {
            Title title = db.Titles.Where(t => t.Title_Desc.ToLower() == titleName.ToLower()).Take(1).SingleOrDefault();
            if (title != null)
                return title.Title_ID;

            if (titleName == "( None Assigned )")
                return null;

            return -1;
        }

        private int? GetStoreIdFromName(string storeName)
        {
            Store store = db.Stores.Where(s => s.Store_Name.ToLower() == storeName.ToLower()).Take(1).SingleOrDefault();
            if (store != null)
                return store.Store_No;

            if (storeName == "All Stores")
                return null;

            return -1;
        }

        public Store GetStoreFromBusinessUnit(int? businessUnitId)
        {
            if (businessUnitId.HasValue)
            {
                return db.Stores.SingleOrDefault(s => s.BusinessUnit_ID == businessUnitId);
            }
            return null;
        }

        public void AssignDefaultPerms(ref User user, TitleDefaultPermission defaults)
        {
            if (defaults == null)
                defaults = new TitleDefaultPermission();
            user.Accountant = defaults.Accountant ?? false;
            user.ApplicationConfigAdmin = defaults.ApplicationConfigAdmin ?? false;
            user.BatchBuildOnly = defaults.BatchBuildOnly ?? false;
            user.Buyer = defaults.Buyer ?? false;
            user.Coordinator = defaults.Coordinator ?? false;
            user.CostAdmin = defaults.CostAdministrator ?? false;
            user.FacilityCreditProcessor = defaults.FacilityCreditProcessor ?? false;
            user.DCAdmin = defaults.DCAdmin ?? false;
            user.Distributor = defaults.Distributor ?? false;
            user.DeletePO = defaults.DeletePO;
            user.EInvoicing_Administrator = defaults.EInvoicing ?? false;
            user.Inventory_Administrator = defaults.InventoryAdministrator ?? false;
            user.Item_Administrator = defaults.ItemAdministrator ?? false;
            user.JobAdministrator = defaults.JobAdministrator ?? false;
            user.Lock_Administrator = defaults.LockAdministrator ?? false;
            user.SuperUser = defaults.SuperUser ?? false;
            user.PO_Accountant = defaults.POAccountant ?? false;
            user.POApprovalAdmin = defaults.POApprovalAdministrator ?? false;
            user.POEditor = defaults.POEditor;
            user.POSInterfaceAdministrator = defaults.POSInterfaceAdministrator ?? false;
            user.PriceBatchProcessor = defaults.PriceBatchProcessor ?? false;
            user.SecurityAdministrator = defaults.SecurityAdministrator ?? false;
            user.Shrink = defaults.Shrink;
            user.ShrinkAdmin = defaults.ShrinkAdmin;
            user.StoreAdministrator = defaults.StoreAdministrator ?? false;
            user.SystemConfigurationAdministrator = defaults.SystemConfigurationAdministrator ?? false;
            user.TaxAdministrator = defaults.TaxAdministrator;
            user.UserMaintenance = defaults.UserMaintenance ?? false;
            user.Vendor_Administrator = defaults.VendorAdministrator ?? false;
            user.VendorCostDiscrepancyAdmin = defaults.VendorCostDiscrepancyAdmin ?? false;
            user.Warehouse = defaults.Warehouse ?? false;
        }

        public UserRestoreError RestoreUser(User user)
        {
            try
            {
                int result = db.Administration_UserAdmin_UpdateUser(
                    user.AccountEnabled, user.CoverPage, user.EMail,
                    user.Fax_Number, user.FullName, user.Pager_Email,
                    user.Phone_Number, user.Printer, user.RecvLog_Store_Limit,
                    user.Telxon_Store_Limit, user.Title, user.User_ID,
                    user.UserName, user.Accountant, user.ApplicationConfigAdmin,
                    user.BatchBuildOnly, user.Buyer, user.Coordinator, user.CostAdmin,
                    user.FacilityCreditProcessor, user.DataAdministrator, user.DCAdmin,
                    user.Distributor, user.DeletePO, user.EInvoicing_Administrator,
                    user.Inventory_Administrator, user.Item_Administrator, user.JobAdministrator,
                    user.Lock_Administrator, user.SuperUser, user.PO_Accountant,
                    user.POApprovalAdmin, user.POEditor, user.POSInterfaceAdministrator,
                    user.PriceBatchProcessor, user.PromoAccessLevel, user.SecurityAdministrator,
                    user.Shrink, user.ShrinkAdmin, user.StoreAdministrator, user.SystemConfigurationAdministrator,
                    user.TaxAdministrator, user.UserMaintenance, user.Vendor_Administrator,
                    user.VendorCostDiscrepancyAdmin, user.Warehouse
                  );

                //log.Message("UpdateUser result: " + result.ToString());
            }
            catch (Exception ex)
            {
                //log.Error("UpdateUser: " + ex.Message);
                return UserRestoreError.Other;
            }
            return UserRestoreError.None;
        }

        public UserUpdateError UpdateUser(UserInfo newUserInfo, ref Log log)
        {
            //log.Message("updating " + newUserInfo.FullName);
            User user = GetUser(newUserInfo.User_ID);
            // see if title or store has changed, otherwise ignore

            user.AccountEnabled = newUserInfo.User_Disabled.ToUpper() == "YES" ? false : true;


            int? titleId = GetTitleIdFromName(newUserInfo.Title);
            if (titleId.HasValue)
            {
                if (titleId == -1)
                {
                    // can't find the title...
                    return UserUpdateError.TitleNotFound;
                }
            }
            else
            {
                titleId = null;
            }
            if (titleId != null && user.Title != titleId)
            {
                // title has changed.
                //log.Message("Title changing...assigning default perms");
                //TitleDefaultPermission defaultPerms = GetDefaultPermissionsByTitleId(titleId);
                //AssignDefaultPerms(ref user, defaultPerms);
                user.Title1 = db.Titles.SingleOrDefault(t => t.Title_ID == titleId);
                //user.Title = titleId;
            }

            int? storeId = GetStoreIdFromName(newUserInfo.Location);
            if (storeId.HasValue)
            {
                if (storeId == -1)
                {
                    // unable to find store.  Abort! Abort!
                    log.Message("repo: store not found!");
                    return UserUpdateError.StoreNotFound;

                }
            }
            else
            {
                storeId = null;
            }
            if (user.Telxon_Store_Limit != storeId)
            {
                // store has changed
                log.Message("store changing...");
                //user.Telxon_Store_Limit = storeId;
                user.Store1 = db.Stores.SingleOrDefault(s => s.Store_No == storeId);
            }

            log.Message("calling UpdateUser!!");

            try
            {
                int result = db.Administration_UserAudit_UpdateUser(
                    user.AccountEnabled, user.Telxon_Store_Limit, user.Title, user.User_ID);

                log.Message("UpdateUser result: " + result.ToString());
            }
            catch (Exception ex)
            {
                log.Error("UpdateUser: " + ex.Message);
                return UserUpdateError.Other;
            }
            return UserUpdateError.None;
        }

        public bool DeleteUser(int userId, ref Log log)
        {
            try
            {
                log.Message("deleting user with id: " + userId.ToString());
                db.Administration_UserAdmin_DeleteUser(userId);
                log.Message("deleted.");
            }
            catch (Exception ex)
            {
                log.Error("error deleting user!: " + ex.Message);
                throw ex;
            }

            return true;
        }

        public List<Store> GetStores()
        {
            // this join ensures we do not get stores which have no users assigned
            return db.Stores.Where(s => s.Users.Any(u => u.AccountEnabled == true)).Distinct().OrderBy(s => s.Store_Name).ToList();
        }

        private List<User> GetUsersBasedOnRoles(IEnumerable<User> usersCache, IEnumerable<string> userRolesList)
        {
            var users = userCache
                        .Where(u =>u.AccountEnabled =true
                            &&(u.TaxAdministrator && userRolesList.Contains("Tax Administrator".ToUpper())
                            || userRolesList.Contains("Delete PO".ToUpper()) && u.DeletePO
                            || userRolesList.Contains("PO Editor".ToUpper()) && u.POEditor
                            || userRolesList.Contains("Shrink Administrator".ToUpper()) && u.ShrinkAdmin
                            || userRolesList.Contains("Shrink".ToUpper()) && u.Shrink
                            || userRolesList.Contains("User Maintenance".ToUpper()) && u.UserMaintenance
                            || userRolesList.Contains("System Configuration Admin".ToUpper()) && u.SystemConfigurationAdministrator
                            || userRolesList.Contains("Store Administrator".ToUpper()) && u.StoreAdministrator
                            || userRolesList.Contains("Security Administrator".ToUpper()) && u.SecurityAdministrator
                            || userRolesList.Contains("POS Interface Administrator".ToUpper()) && u.POSInterfaceAdministrator
                            || userRolesList.Contains("Data Administrator".ToUpper()) && u.DataAdministrator
                            || userRolesList.Contains("Application Config Admin".ToUpper()) && u.ApplicationConfigAdmin
                            || userRolesList.Contains("Vendor Cost Discrepancy Admin".ToUpper()) && u.VendorCostDiscrepancyAdmin
                            || userRolesList.Contains("EInvoicing Administrator".ToUpper()) && u.EInvoicing_Administrator
                            || userRolesList.Contains("PO Approval Administrator".ToUpper()) && u.POApprovalAdmin
                            || userRolesList.Contains("Cost Administrator".ToUpper()) && u.CostAdmin
                            || userRolesList.Contains("Batch Build Only".ToUpper()) && u.BatchBuildOnly
                            || userRolesList.Contains("Inventory Administrator".ToUpper()) && u.Inventory_Administrator
                            || userRolesList.Contains("Price Batch Processor".ToUpper()) && u.PriceBatchProcessor
                            || userRolesList.Contains("Lock Administrator".ToUpper()) && u.Lock_Administrator
                            || userRolesList.Contains("Vendor Administrator".ToUpper()) && u.Vendor_Administrator
                            || userRolesList.Contains("Item Administrator".ToUpper()) && u.Item_Administrator
                            || userRolesList.Contains("Coordinator".ToUpper()) && u.Coordinator
                            || userRolesList.Contains("Buyer".ToUpper()) && u.Buyer
                            || userRolesList.Contains("Facility Credit Processor".ToUpper()) && u.FacilityCreditProcessor
                            || userRolesList.Contains("Distributor".ToUpper()) && u.Distributor
                            || userRolesList.Contains("Accountant".ToUpper()) && u.Accountant
                            || userRolesList.Contains("PO Accountant".ToUpper()) && u.PO_Accountant
                            || userRolesList.Contains("Super User".ToUpper()) && u.SuperUser
                            || userRolesList.Contains("Job Administrator".ToUpper()) && u.JobAdministrator
                            || userRolesList.Contains("Receiver".ToUpper()) && u.Distributor
                            || userRolesList.Contains("DC Admin".ToUpper()) && u.DCAdmin.HasValue && u.DCAdmin.Value)
                           ).ToList();
            return users;
        }
            
        public List<string> GetStoreNames()
        {
            return (from store in db.Stores
                    select store.Store_Name).ToList();
        }

        public List<UserInfo> GetUsers(IEnumerable<string> userRolesList)
        {
            // use the list of roles (from the config) to determine which users to retrieve
            var userInfos = GetUsersBasedOnRoles(userCache, userRolesList)
                .Select(u => UserInfoFromUser(u, usttCache))
                .ToList();

            return userInfos;
        }

        private UserInfo UserInfoFromUser(User user,
            IEnumerable<UserStoreTeamTitle> teamTitles = null
          )
        {
            if (user == null) return new UserInfo();

            UserInfo ui = new UserInfo
            {
                FullName = user.FullName,
                StoreId = user.Telxon_Store_Limit,
                Location = user.Telxon_Store_Limit.HasValue ? user.Store1.Store_Name : "All Stores",
                Title = user.Title.HasValue ? user.Title1.Title_Desc : "( None Assigned )",
                User_ID = user.User_ID,
                UserName = user.UserName,
                User_Disabled = user.AccountEnabled==true? "No":"Yes"
            };

           return ui;
        }

        public List<UserInfo> GetUsersByStore(int? StoreId, IEnumerable<string> userRolesList)
        {
            if (StoreId.Value == -1)
            {
                StoreId = null;
            }

            var storeUserInfos = GetUsersBasedOnRoles(userCache, userRolesList)
                .Where(u => u.Telxon_Store_Limit == StoreId)
                .Select(u => UserInfoFromUser(u, usttCache))
                .ToList();

            return storeUserInfos;
        }

        /// <summary>
        /// Gets a user's list of overrides as a a string
        /// </summary>
        /// <param name="userId">the user id</param>
        /// <param name="allow">if true, return allowed overrides, deny overrides otherwise</param>
        /// <returns>a string of overrides</returns>
        public string GetUserOverrides(int userId, bool allow)
        {
            var overrides = tpoCache.Where(tpo => tpo.UserId == userId && tpo.PermissionValue == allow);
            StringBuilder orList = new StringBuilder();
            foreach (TitlePermissionOverride tpo in overrides)
            {
                orList.Append(tpo.PermissionName);
                orList.Append(", ");
            }
            if (orList.Length < 2)
                return "None.";

            return orList.Remove(orList.Length - 2, 2).ToString();
        }

        public List<TitlePermissionOverride> GetOverrideList(int userId)
        {
            return tpoCache.Where(tpo => tpo.UserId == userId).ToList();
        }

        public User GetUser(int id)
        {
            return userCache.SingleOrDefault(u => u.User_ID == id);
        }

        public List<string> GetTitles()
        {
            List<string> titles = new List<string>();
            titles.Add("( None Assigned )");
            titles.AddRange((from title in db.Titles
                             where title.Title_Desc.ToLower() != "superuser" && title.Title_Desc.ToLower() != "Store Systems Admin"
                             orderby title.Title_Desc ascending
                             select title.Title_Desc).ToList());
            return titles;
        }

        public int ValidateUser()
        {

            string username = Environment.UserName;
            WindowsIdentity wi = WindowsIdentity.GetCurrent();

            // confirm that logged in user and user running process are the same
            // (not sure if that's important yet, but it seems like it ought to be)
            if (!wi.Name.EndsWith(username))
                return -1;

            User user = db.Users.SingleOrDefault(u => u.UserName == username.ToLower());
            if (user == null)
                return -1;

            return user.User_ID;
        }

        public Store GetUsersPreviousStore(string userId)
        {
            int uid = -1;
            if (!Int32.TryParse(userId, out uid))
                return null;

            int? storeLimit = db.UsersHistories.Where(uh => uh.User_ID == uid).OrderByDescending(uh => uh.Effective_Date).Take(1).SingleOrDefault().Telxon_Store_Limit;
            if (storeLimit.HasValue)
                return db.Stores.SingleOrDefault(s => s.Store_No == storeLimit.Value);

            return null;
        }

        public Store GetStoreFromName(string name)
        {
            if (name == "All Stores")
                return null;

            return db.Stores.Where(s => s.Store_Name.ToLower() == name.ToLower()).Take(1).SingleOrDefault();
        }

        #region configuration related

        public Guid GetAppId(string appName, Guid environmentId)
        {
            return db.AppConfigApps.FirstOrDefault(app => app.Name.ToLower() == appName.ToLower() && app.Deleted == false && app.EnvironmentID == environmentId).ApplicationID;
        }

        public Guid GetEnvId(IRMAEnvironmentEnum environment)
        {
            return db.AppConfigEnvs.SingleOrDefault(env => env.Name.ToUpper() == AuditOptions.ConvertIRMAEnvironmentToString(environment).ToUpper() && env.Deleted == false).EnvironmentID;
        }

        #endregion

        /// <summary>
        /// Maps database column names (dbo.Users) to user-friendly names for IRMA roles
        /// </summary>
        Dictionary<string, string> RoleNameMappingDictionary = new Dictionary<string, string>()
        {
            {nameof(User.TaxAdministrator), "Tax Administrator" },
            {nameof(User.DeletePO), "Delete PO" },
            {nameof(User.POEditor), "PO Editor" },
            {nameof(User.ShrinkAdmin), "Shrink Administrator" },
            {nameof(User.Shrink), "Shrink" },
            {nameof(User.UserMaintenance), "User Maintenance" },
            {nameof(User.SystemConfigurationAdministrator), "System Configuration Admin" },
            {nameof(User.StoreAdministrator), "Store Administrator" },
            {nameof(User.SecurityAdministrator), "Security Administrator" },
            {nameof(User.POSInterfaceAdministrator), "POS Interface Administrator" },
            {nameof(User.JobAdministrator), "Job Administrator" },
            {nameof(User.DataAdministrator), "Data Administrator" },
            {nameof(User.ApplicationConfigAdmin), "Application Config Admin" },
            {nameof(User.VendorCostDiscrepancyAdmin), "Vendor Cost Discrepancy Admin" },
            {nameof(User.EInvoicing_Administrator), "EInvoicing Administrator" },
            {nameof(User.POApprovalAdmin), "PO Approval Administrator" },
            {nameof(User.CostAdmin), "Cost Administrator" },
            {nameof(User.BatchBuildOnly), "Batch Build Only" },
            {nameof(User.Inventory_Administrator), "Inventory Administrator" },
            {nameof(User.PriceBatchProcessor), "Price Batch Processor" },
            {nameof(User.Lock_Administrator), "Lock Administrator" },
            {nameof(User.Vendor_Administrator), "Vendor Administrator" },
            {nameof(User.Item_Administrator), "Item Administrator" },
            {nameof(User.Coordinator), "Coordinator" },
            {nameof(User.Buyer), "Buyer" },
            {nameof(User.FacilityCreditProcessor), "Facility Credit Processor" },
            {nameof(User.Distributor), "Distributor" },
            {nameof(User.Accountant), "Accountant" },
            {nameof(User.PO_Accountant), "PO Accountant" },
            {nameof(User.SuperUser), "Super User" },
           // {nameof(User.Distributor), "Receiver" },
            {nameof(User.DCAdmin), "DC Admin" }
        };
    }
}
