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
            if (newUserInfo.HasSlimAccess)
            {
                sa = GetUserSlimAccess(newUserInfo.User_ID);
                if (sa != null)
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
                sa = new SlimAccess
                {
                    User_ID = newUserInfo.User_ID,
                    Authorizations = false,
                    IRMAPush = false,
                    ItemRequest = false,
                    RetailCost = false,
                    ScaleInfo = false,
                    StoreSpecials = false,
                    UserAdmin = false,
                    VendorRequest = false,
                    WebQuery = false,
                };
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

        private List<User> GetUsersBasedOnRoles(IEnumerable<User> usersCache, IEnumerable<string> userRolesList)
        {
            var users = userCache.Where(u => 
                userRolesList.Contains(RoleNameMappingDictionary[nameof(u.TaxAdministrator)]) && u.TaxAdministrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.DeletePO)]) && u.DeletePO
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.POEditor)]) && u.POEditor
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.ShrinkAdmin)]) && u.ShrinkAdmin
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Shrink)]) && u.Shrink
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.UserMaintenance)]) && u.UserMaintenance
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.SystemConfigurationAdministrator)]) && u.SystemConfigurationAdministrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.StoreAdministrator)]) && u.StoreAdministrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.POSInterfaceAdministrator)]) && u.POSInterfaceAdministrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.DataAdministrator)]) && u.DataAdministrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.ApplicationConfigAdmin)]) && u.ApplicationConfigAdmin
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.VendorCostDiscrepancyAdmin)]) && u.VendorCostDiscrepancyAdmin
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.EInvoicing_Administrator)]) && u.EInvoicing_Administrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.POApprovalAdmin)]) && u.POApprovalAdmin
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.CostAdmin)]) && u.CostAdmin
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.BatchBuildOnly)]) && u.BatchBuildOnly
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Inventory_Administrator)]) && u.Inventory_Administrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.PriceBatchProcessor)]) && u.PriceBatchProcessor
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Lock_Administrator)]) && u.Lock_Administrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Vendor_Administrator)]) && u.Vendor_Administrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Item_Administrator)]) && u.Item_Administrator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Coordinator)]) && u.Coordinator
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Buyer)]) && u.Buyer
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.FacilityCreditProcessor)]) && u.FacilityCreditProcessor
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Distributor)]) && u.Distributor
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Accountant)]) && u.Accountant
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.PO_Accountant)]) && u.PO_Accountant
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.SuperUser)]) && u.SuperUser
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.Distributor)]) && u.Distributor
                && userRolesList.Contains(RoleNameMappingDictionary[nameof(u.DCAdmin)]) && u.DCAdmin.HasValue && u.DCAdmin.Value
            ).ToList();
            return users;
        }

        //private List<User> GetUsersBasedOnRoles(List<User> cache, Dictionary<string, bool> userRolesDictionary)
        //{
        //    bool test;
        //    return   userCache.Where(u => u.TaxAdministrator = userRolesDictionary.TryGetValue("Tax Administrator", out test) ? true : u.TaxAdministrator
        //                             || (u.DeletePO = userRolesDictionary.TryGetValue("Delete PO", out test)) ? true : u.DeletePO
        //                             || (u.POEditor = userRolesDictionary.TryGetValue("PO Editor", out test)) ? true : u.POEditor
        //                             || (u.ShrinkAdmin = userRolesDictionary.TryGetValue("Shrink Administrator", out test)) ? true : u.ShrinkAdmin
        //                             || (u.Shrink = userRolesDictionary.TryGetValue("Shrink", out test)) ? true : u.Shrink
        //                             || (u.UserMaintenance = userRolesDictionary.TryGetValue("User Maintenance", out test)) ? true : u.UserMaintenance
        //                             || (u.SystemConfigurationAdministrator = userRolesDictionary.TryGetValue("System Configuration Admin", out test)) ? true : u.SystemConfigurationAdministrator
        //                             || (u.StoreAdministrator = userRolesDictionary.TryGetValue("Store Administrator", out test)) ? true : u.StoreAdministrator
        //                             || (u.SecurityAdministrator = userRolesDictionary.TryGetValue("Security Administrator", out test)) ? true : u.SecurityAdministrator
        //                             || (u.POSInterfaceAdministrator = userRolesDictionary.TryGetValue("POS Interface Administrator", out test)) ? true : u.POSInterfaceAdministrator
        //                             || (u.JobAdministrator = userRolesDictionary.TryGetValue("Job Administrator", out test)) ? true : u.JobAdministrator
        //                             || (u.DataAdministrator = userRolesDictionary.TryGetValue("Data Administrator", out test)) ? true : u.DataAdministrator
        //                             || (u.ApplicationConfigAdmin = userRolesDictionary.TryGetValue("Application Config Admin", out test)) ? true : u.ApplicationConfigAdmin
        //                             || (u.VendorCostDiscrepancyAdmin = userRolesDictionary.TryGetValue("Vendor Cost Discrepancy Admin", out test)) ? true : u.VendorCostDiscrepancyAdmin
        //                             || (u.EInvoicing_Administrator = userRolesDictionary.TryGetValue("EInvoicing Administrator", out test)) ? true : u.EInvoicing_Administrator
        //                             || (u.POApprovalAdmin = userRolesDictionary.TryGetValue("PO Approval Administrator", out test)) ? true : u.POApprovalAdmin
        //                             || (u.CostAdmin = userRolesDictionary.TryGetValue("Cost Administrator", out test)) ? true : u.CostAdmin
        //                             || (u.BatchBuildOnly = userRolesDictionary.TryGetValue("Batch Build Only", out test)) ? true : u.BatchBuildOnly
        //                             || (u.Inventory_Administrator = userRolesDictionary.TryGetValue("Inventory Administrator", out test)) ? true : u.Inventory_Administrator
        //                             || (u.PriceBatchProcessor = userRolesDictionary.TryGetValue("Price Batch Processor", out test)) ? true : u.PriceBatchProcessor
        //                             || (u.Lock_Administrator = userRolesDictionary.TryGetValue("Lock Administrator", out test)) ? true : u.Lock_Administrator
        //                             || (u.Vendor_Administrator = userRolesDictionary.TryGetValue("Vendor Administrator", out test)) ? true : u.Vendor_Administrator
        //                             || (u.Item_Administrator = userRolesDictionary.TryGetValue("Item Administrator", out test)) ? true : u.Item_Administrator
        //                             || (u.Coordinator = userRolesDictionary.TryGetValue("Coordinator", out test)) ? true : u.Coordinator
        //                             || (u.Buyer = userRolesDictionary.TryGetValue("Buyer", out test)) ? true : u.Buyer
        //                             || (u.FacilityCreditProcessor = userRolesDictionary.TryGetValue("Facility Credit Processor", out test)) ? true : u.FacilityCreditProcessor
        //                             || (u.Distributor = userRolesDictionary.TryGetValue("Distributor", out test)) ? true : u.Distributor
        //                             || (u.Accountant = userRolesDictionary.TryGetValue("Accountant", out test)) ? true : u.TaxAdministrator
        //                             || (u.PO_Accountant = userRolesDictionary.TryGetValue("PO Accountant", out test)) ? true : u.PO_Accountant
        //                             || (u.SuperUser = userRolesDictionary.TryGetValue("Super User", out test)) ? true : u.SuperUser
        //                             || (u.Distributor = userRolesDictionary.TryGetValue("Receiver", out test)) ? true : u.Distributor
        //                             || (u.DCAdmin.HasValue ? u.DCAdmin == userRolesDictionary.TryGetValue("DC Admin", out test) : false)
        //                                       ).ToList();
        //}

        public List<string> GetStoreNames()
        {
            return (from store in db.Stores
                    select store.Store_Name).ToList();
        }

        public List<UserInfo> GetUsers(IEnumerable<string> userRolesList)
        {
            // use the list of roles (from the config) to determine which users to retrieve
            var userInfos = GetUsersBasedOnRoles(userCache, userRolesList)
                .Select(u => UserInfoFromUser(u, usttCache, slimCache))
                .ToList();

            return userInfos;
        }

        private UserInfo UserInfoFromUser(User user,
            IEnumerable<UserStoreTeamTitle> teamTitles = null,
            IEnumerable<SlimAccess> slimAccesses = null)
        {
            if (user == null) return new UserInfo();

            UserInfo ui = new UserInfo
            {
                FullName = user.FullName,
                StoreId = user.Telxon_Store_Limit,
                StoreLimit = user.Telxon_Store_Limit.HasValue ? user.Store1.Store_Name : "All Stores",
                TitleId = user.Title,
                Title = user.Title.HasValue ? user.Title1.Title_Desc : "( None Assigned )",
                User_ID = user.User_ID,
                UserName = user.UserName,
                OverrideAllow = GetUserOverrides(user.User_ID, true),
                OverrideDeny = GetUserOverrides(user.User_ID, false),
                RDE = user.Item_Administrator ? "Yes" : "No"
            };

            // forgive the hoop jumping here, but apparently it's possible to
            // be assigned to a team which is NOT found in the Team table
            // :(  Sad Panda   
            UserStoreTeamTitle ustt = usttCache?.SingleOrDefault(team => team.User_ID == user.User_ID);
            ui.TeamName = ustt?.Team?.Team_Name ?? "( None Assigned )";

            // SLIM fields
            SlimAccess slimAccess = slimCache?.SingleOrDefault(sa => sa.User_ID == user.User_ID);
            if (slimAccess != null)
            {
                ui.HasSlimAccess = true;
                ui.WebQueryEnabled = (slimAccess.WebQuery.HasValue && slimAccess.WebQuery.Value) ? "Yes" : "No";
                ui.ISSEnabled = (slimAccess.StoreSpecials.HasValue && slimAccess.StoreSpecials.Value) ? "Yes" : "No";
                ui.ItemRequestEnabled = (slimAccess.ItemRequest.HasValue && slimAccess.ItemRequest.Value) ? "Yes" : "No";
            }

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
                .Select(u => UserInfoFromUser(u, usttCache, slimCache))
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
            {nameof(User.Distributor), "Receiver" },
            {nameof(User.DCAdmin), "DC Admin" }
        };
    }
}
