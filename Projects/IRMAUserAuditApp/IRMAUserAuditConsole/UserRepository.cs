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
            if(storeLimit == -1)
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
            UserInfo ui = new UserInfo();
            if (u != null)
            {
                ui.User_ID = u.User_ID;
                ui.UserName = u.UserName;
                ui.TitleId = u.Title;
                ui.Title = u.Title.HasValue ? u.Title1.Title_Desc : "( None Assigned )";
                ui.StoreLimit = u.Telxon_Store_Limit.HasValue ? u.Store1.Store_Name : "All Stores";
                ui.StoreId = u.Telxon_Store_Limit;
                ui.FullName = u.FullName;

                SlimAccess sa = slimCache.Where(slim => slim.User_ID == u.User_ID).Take(1).SingleOrDefault();
                return ui;
            }

            return null;
            
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
            user.Accountant = defaults.Accountant.HasValue ? defaults.Accountant.Value : false;
            user.ApplicationConfigAdmin = defaults.ApplicationConfigAdmin.HasValue ? defaults.ApplicationConfigAdmin.Value : false;
            user.BatchBuildOnly = defaults.BatchBuildOnly.HasValue ? defaults.BatchBuildOnly.Value : false;
            user.Buyer = defaults.Buyer.HasValue ? defaults.Buyer.Value : false;
            user.Coordinator = defaults.Coordinator.HasValue ? defaults.Coordinator.Value : false;
            user.CostAdmin = defaults.CostAdministrator.HasValue ? defaults.CostAdministrator.Value : false;
            user.FacilityCreditProcessor = defaults.FacilityCreditProcessor.HasValue ? defaults.FacilityCreditProcessor.Value : false;
            user.DCAdmin = defaults.DCAdmin.HasValue ? defaults.DCAdmin.Value : false;
            user.Distributor = defaults.Distributor.HasValue ? defaults.Distributor.Value : false;
            user.DeletePO = defaults.DeletePO;
            user.EInvoicing_Administrator = defaults.EInvoicing.HasValue ? defaults.EInvoicing.Value : false;
            user.Inventory_Administrator = defaults.InventoryAdministrator.HasValue ? defaults.InventoryAdministrator.Value : false;
            user.Item_Administrator = defaults.ItemAdministrator.HasValue ? defaults.ItemAdministrator.Value : false;
            user.JobAdministrator = defaults.JobAdministrator.HasValue ? defaults.JobAdministrator.Value : false;
            user.Lock_Administrator = defaults.LockAdministrator.HasValue ? defaults.LockAdministrator.Value : false;
            user.SuperUser = defaults.SuperUser.HasValue ? defaults.SuperUser.Value : false;
            user.PO_Accountant = defaults.POAccountant.HasValue ? defaults.POAccountant.Value : false;
            user.POApprovalAdmin = defaults.POApprovalAdministrator.HasValue ? defaults.POApprovalAdministrator.Value : false;
            user.POEditor = defaults.POEditor ? defaults.POEditor : false;
            user.POSInterfaceAdministrator = defaults.POSInterfaceAdministrator.HasValue ? defaults.POSInterfaceAdministrator.Value : false;
            user.PriceBatchProcessor = defaults.PriceBatchProcessor.HasValue ? defaults.PriceBatchProcessor.Value : false;
            user.SecurityAdministrator = defaults.SecurityAdministrator.HasValue ? defaults.SecurityAdministrator.Value : false;
            user.Shrink = defaults.Shrink;
            user.ShrinkAdmin = defaults.ShrinkAdmin;
            user.StoreAdministrator = defaults.StoreAdministrator.HasValue ? defaults.StoreAdministrator.Value : false;
            user.SystemConfigurationAdministrator = defaults.SystemConfigurationAdministrator.HasValue ? defaults.SystemConfigurationAdministrator.Value : false;
            user.TaxAdministrator = defaults.TaxAdministrator;
            user.UserMaintenance = defaults.UserMaintenance.HasValue ? defaults.UserMaintenance.Value : false;
            user.Vendor_Administrator = defaults.VendorAdministrator.HasValue ? defaults.VendorAdministrator.Value : false;
            user.VendorCostDiscrepancyAdmin = defaults.VendorCostDiscrepancyAdmin.HasValue ? defaults.VendorCostDiscrepancyAdmin.Value : false;
            user.Warehouse = defaults.Warehouse.HasValue ? defaults.Warehouse.Value : false;
        }

        public SlimAccess GetUserSlimAccess(int userId)
        {
            return slimCache.Where(sc => sc.User_ID == userId).Take(1).SingleOrDefault();
        }

        public UserRestoreError RestoreUser(User user, SlimAccess sa)
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
                    user.VendorCostDiscrepancyAdmin, user.Warehouse,
                    sa.Authorizations, sa.IRMAPush, sa.ItemRequest, sa.RetailCost,
                    sa.ScaleInfo, sa.StoreSpecials, sa.UserAdmin, sa.VendorRequest,
                    sa.WebQuery);

                //log.Message("UpdateUser result: " + result.ToString());
            }
            catch (Exception ex)
            {
                //log.Error("UpdateUser: " + ex.Message);
                return UserRestoreError.Other;
            }
            return UserRestoreError.None;
        }

        public UserUpdateError UpdateSlim(UserInfo newUserInfo, ref Log log)
        {
            User user = GetUser(newUserInfo.User_ID);
            SlimAccess sa = null;
            if (newUserInfo.HasSlimAccess)
            {
                sa = GetUserSlimAccess(newUserInfo.User_ID);
                if (sa != null)
                {
                    log.Message("updating SLIM fields...");
                    sa.WebQuery = newUserInfo.WebQueryEnabled.ToLower() == "yes" ? true : false;
                    sa.StoreSpecials = newUserInfo.ISSEnabled.ToLower() == "yes" ? true : false;
                    sa.ItemRequest = newUserInfo.ItemRequestEnabled.ToLower() == "yes" ? true : false;
                }
            }
            

            log.Message("calling UpdateUser!!");
            // Do it.  DO IT!!!
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
                    user.VendorCostDiscrepancyAdmin, user.Warehouse,
                    sa.Authorizations, sa.IRMAPush, sa.ItemRequest, sa.RetailCost,
                    sa.ScaleInfo, sa.StoreSpecials, sa.UserAdmin, sa.VendorRequest,
                    sa.WebQuery);

                log.Message("UpdateSLIM result: " + result.ToString());
            }
            catch (Exception ex)
            {
                log.Error("UpdateSLIM: " + ex.Message);
                return UserUpdateError.Other;
            }
            return UserUpdateError.None;
        }

        public UserUpdateError UpdateUser(UserInfo newUserInfo, bool updateSlim, ref Log log)
        {
            //log.Message("updating " + newUserInfo.FullName);
            User user = GetUser(newUserInfo.User_ID);
            // see if title or store has changed, otherwise ignore
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
            if (user.Title != titleId)
            {
                // title has changed.
                //log.Message("Title changing...assigning default perms");
                TitleDefaultPermission defaultPerms = GetDefaultPermissionsByTitleId(titleId);
                AssignDefaultPerms(ref user, defaultPerms);
                user.Title1 = db.Titles.SingleOrDefault(t => t.Title_ID == titleId);
            }

            int? storeId = GetStoreIdFromName(newUserInfo.StoreLimit);
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

            SlimAccess sa = null;
            if(newUserInfo.HasSlimAccess)
            {
                sa = GetUserSlimAccess(newUserInfo.User_ID);
                if(sa != null)
                {
                    if (updateSlim)
                    {
                        log.Message("updating SLIM fields...");
                        sa.WebQuery = newUserInfo.WebQueryEnabled.ToLower() == "yes" ? true : false;
                        sa.StoreSpecials = newUserInfo.ISSEnabled.ToLower() == "yes" ? true : false;
                        sa.ItemRequest = newUserInfo.ItemRequestEnabled.ToLower() == "yes" ? true : false;
                    }
                }
            }
            else
            {
                log.Message("assigning SLIM defaults...");
                sa = new SlimAccess();
                sa.User_ID = newUserInfo.User_ID;
                sa.Authorizations = false;
                sa.IRMAPush = false;
                sa.ItemRequest = false;
                sa.RetailCost = false;
                sa.ScaleInfo = false;
                sa.StoreSpecials = false;
                sa.UserAdmin = false;
                sa.VendorRequest = false;
                sa.WebQuery = false;
            }

            log.Message("calling UpdateUser!!");
            // Do it.  DO IT!!!
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
                    user.VendorCostDiscrepancyAdmin, user.Warehouse,
                    sa.Authorizations, sa.IRMAPush, sa.ItemRequest, sa.RetailCost,
                    sa.ScaleInfo, sa.StoreSpecials, sa.UserAdmin, sa.VendorRequest,
                    sa.WebQuery);

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

        public List<string> GetStoreNames()
        {
            return (from store in db.Stores
                    select store.Store_Name).ToList();
        }

        public List<UserInfo> GetUsers()
        {
            List<UserInfo> userinfos = new List<UserInfo>();

            foreach (User u in userCache)
            {
                UserInfo ui = new UserInfo();
                ui.FullName = u.FullName;
                ui.StoreId = u.Telxon_Store_Limit;
                if (u.Telxon_Store_Limit.HasValue)
                    ui.StoreLimit = u.Store1.Store_Name;
                else
                    ui.StoreLimit = "All Stores";

                ui.TitleId = u.Title;

                if (u.Title.HasValue)
                    ui.Title = u.Title1.Title_Desc;
                else
                    ui.Title = "( None Assigned )";

                ui.User_ID = u.User_ID;
                ui.UserName = u.UserName;
                ui.OverrideAllow = GetUserOverrides(u.User_ID, true);
                ui.OverrideDeny = GetUserOverrides(u.User_ID, false);

                // forgive the hoop jumping here, but apparently it's possible to
                // be assigned to a team which is NOT found in the Team table
                // :(  Sad Panda

                UserStoreTeamTitle ustt = (usttCache.Where(team => team.User_ID == u.User_ID)).Take(1).SingleOrDefault();
                if (ustt != null)
                {
                    if (ustt.Team != null)
                        ui.TeamName = ustt.Team.Team_Name;
                }
                else
                {
                    ui.TeamName = "( None Assigned )";
                }


                // SLIM fields
                ui.RDE = u.Item_Administrator ? "Yes" : "No";
                SlimAccess slimAccess = slimCache.Where(sa => sa.User_ID == u.User_ID).Take(1).SingleOrDefault();
                if (slimAccess != null)
                {
                    ui.HasSlimAccess = true;
                    ui.WebQueryEnabled = (slimAccess.WebQuery.HasValue && slimAccess.WebQuery.Value) ? "Yes" : "No";
                    ui.ISSEnabled = (slimAccess.StoreSpecials.HasValue && slimAccess.StoreSpecials.Value) ? "Yes" : "No";
                    ui.ItemRequestEnabled = (slimAccess.ItemRequest.HasValue && slimAccess.ItemRequest.Value) ? "Yes" : "No";
                }

                userinfos.Add(ui);

            }
            return userinfos;
        }

        public List<UserInfo> GetUsersByStore(int? StoreId)
        {
            if (StoreId.Value == -1)
                StoreId = null;

            List<UserInfo> userinfos = new List<UserInfo>();

            var storeUsers = userCache.Where(user => user.Telxon_Store_Limit == StoreId);

            foreach (User u in storeUsers)
            {
                UserInfo ui = new UserInfo();
                ui.FullName = u.FullName;
                ui.StoreId = u.Telxon_Store_Limit;
                if (u.Telxon_Store_Limit.HasValue)
                    ui.StoreLimit = u.Store1.Store_Name;
                else
                    ui.StoreLimit = "All Stores";

                ui.TitleId = u.Title;

                if (u.Title.HasValue)
                    ui.Title = u.Title1.Title_Desc;
                else
                    ui.Title = "( None Assigned )";

                ui.User_ID = u.User_ID;
                ui.UserName = u.UserName;
                ui.OverrideAllow = GetUserOverrides(u.User_ID, true);
                ui.OverrideDeny = GetUserOverrides(u.User_ID, false);

                // forgive the hoop jumping here, but apparently it's possible to
                // be assigned to a team which is NOT found in the Team table
                // :(  Sad Panda

                UserStoreTeamTitle ustt = (usttCache.Where(team => team.User_ID == u.User_ID)).Take(1).SingleOrDefault();
                if (ustt != null)
                {
                    if (ustt.Team != null)
                        ui.TeamName = ustt.Team.Team_Name;
                }
                else
                {
                    ui.TeamName = "( None Assigned )";
                }
               
                
                // SLIM fields
                ui.RDE = u.Item_Administrator ? "Yes" : "No";
                SlimAccess slimAccess = slimCache.Where(sa => sa.User_ID == u.User_ID).Take(1).SingleOrDefault();
                if (slimAccess != null)
                {
                    ui.HasSlimAccess = true;
                    ui.WebQueryEnabled = (slimAccess.WebQuery.HasValue && slimAccess.WebQuery.Value) ? "Yes" : "No";
                    ui.ISSEnabled = (slimAccess.StoreSpecials.HasValue && slimAccess.StoreSpecials.Value) ? "Yes" : "No";
                    ui.ItemRequestEnabled = (slimAccess.ItemRequest.HasValue && slimAccess.ItemRequest.Value) ? "Yes" : "No";
                }
                

                userinfos.Add(ui);
                
            }
            return userinfos;
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
    }
}
