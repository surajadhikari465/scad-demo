using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.DirectoryServices.AccountManagement;
using System.Xml;
using System.Xml.XPath;

namespace POReports
{

    public class POSession
    {
        // Public Vars
        public UserRecord CurrentUser { get; set; }
        public FiscalData FPData { get; set; }
        public bool isLoggedIn { get { return _isLoggedIn; } }
        public FiscalPeriod ActiveFP { get; set; }
        public FiscalPeriod UserFP { get; set; }

        // Private Vars
        private bool _isLoggedIn { get; set; }
        private string _userID { get; set; }

        private void ResetData()
        {
            _isLoggedIn = false;
            _userID = "";
            CurrentUser = null;
            FPData = null;
            ActiveFP = null;
            UserFP = null;
        }

        public POSession(HttpContext ctx)
        {
            ResetData();
            Config.Context = ctx;
            init();
        }

        //! Code that runs when the class is instanciated
        public void init()
        {
            _userID = "";
            // Check if a session is active and has a user id associated with it
            try { _userID = Config.GetSessionProperty("USER_ID"); }
            catch { }
            if (String.IsNullOrEmpty(_userID))
            {
                // No session user ID was passed, try to use NTLM
                CurrentUser = NTLMLogin();

                if (CurrentUser != null)
                {
                    _isLoggedIn = true;
                }
            }
            else
            {
                // We have a session user ID. Use it.
                CurrentUser = Factory.GetUser(_userID);
                if (CurrentUser != null)
                {
                    _isLoggedIn = true;
                }
            }


            FPData = new FiscalData();
            FPData.init();

            // Set the active FP to the previous FP for CreditMaster only
            ActiveFP = FPData.CurrentPeriod;            
        }

        #region Login Methods

        public bool Login(string u, string p)
        {
            ActiveDirectory ad = new ActiveDirectory();
            if (ad.Authenticate(u, p))
            {
                CurrentUser = Factory.GetUser(ad.GUID.ToString());
                _isLoggedIn = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        //! Attempt to login using NTLM (Windows) Authentication
        private UserRecord NTLMLogin()
        {
            string LoginName = Config.Context.User.Identity.Name;
            if (string.IsNullOrEmpty(LoginName)) return null;

            ActiveDirectory ad = new ActiveDirectory(LoginName);
            if (ad.isValid)
            {
                return Factory.GetUser(ad.GUID.ToString());
            }
            else
            {
                return null;
            }
        }

        public void logout()
        {
            _isLoggedIn = false;
            _userID = "";
            CurrentUser = null;
        }

        #endregion

        #region Fiscal Period Functions
        // FISCAL PERIOD FUNCTIONS

        public void SetActiveFiscalPeriod(int fp, int fy)
        {
            if (fp <= 0 || fy <= 0)
            {
                ActiveFP = FPData.CurrentPeriod;
            }
            else
            {
                LoadUserFP(fp, fy);
                if (UserFP != null)
                {
                    ActiveFP = UserFP;
                }
            }
        }

        private void LoadUserFP(int fp, int fy)
        {
            if (UserFP == null || (UserFP.Period != fp && UserFP.Year != fy))
            {
                UserFP = new FiscalPeriod();
                using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 FiscalPeriodNumber, FiscalYear, Quarter, FPStartDate, FPEndDate FROM FiscalPeriods WHERE FiscalPeriodNumber = N'" + fp.ToString() + "' AND FiscalYear = N'" + fy.ToString() + "'", cn);
                    cn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        UserFP = Factory.GetAs<FiscalPeriod>(rdr);
                        UserFP.init();
                    }
                }
            }
        }

        #endregion
        
        public static string parseRegionFromDisplayName(string str)
        {
            if (str.IndexOf("(") != -1)
            {
                return str.Substring(str.IndexOf("("), 2);
            }
            else
            {
                return "";
            }
        }

        public static string parseStoreFromDisplayName(string str)
        {
            if (str.IndexOf("(") != -1)
            {
                return str.Substring(str.IndexOf("(") + 3, 3);
            }
            else
            {
                return "";
            }
        }
    }   
}