using System;
using System.Linq;
using OOS.Model;
using OOSCommon;
using OOSCommon.DataContext;

namespace OutOfStock
{
    public class UserLoginManager :IUserLoginManager
    {
        private readonly IOOSEntitiesFactory _oosFactory;
        private readonly IOOSLog _oosLog;

        public UserLoginManager(IOOSEntitiesFactory oosFactory, ILogService oosLog)
        {
            _oosFactory = oosFactory;
            _oosLog = oosLog.GetLogger();
        }

        public void RecordLogin(string username, string region, string store)
        {
            using (var oos = _oosFactory.New())
            {
                try
                {
                    var existingUser = oos.Users.FirstOrDefault(u => u.Username == username);
                    if (existingUser != null)
                    {
                        existingUser.LastLogin = DateTime.Now;
                        existingUser.Region = region;
                        existingUser.Store = store;
                    }
                    if (existingUser == null)
                    {
                        existingUser = new Users() {LastLogin = DateTime.Now, Username = username, Region = region, Store = store};
                        oos.Users.AddObject(existingUser);
                    }

                    oos.SaveChanges();
                }
                catch (Exception ex)
                {
                    _oosLog.Warn($"UserLoginManager.RecordLogin({username}) failed.");
                    _oosLog.Warn(ex.Message);
                }
            }
        }

        public void GetLocationOverrides(string username, out string region, out string store)
        {
            //set defaults.
            region = null;
            store = null;

            using (var oos = _oosFactory.New())
            {
                var existingUser = oos.Users.FirstOrDefault(u => u.Username == username);
                if (existingUser == null) return;
                region = existingUser.RegionOverride;
                store = existingUser.StoreOverride;
            }
        }
    }
}