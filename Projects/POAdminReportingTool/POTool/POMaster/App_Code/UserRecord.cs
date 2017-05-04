using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for CakeSecurityGroup
/// </summary>
/// 
namespace POReports{
    public class UserRecord
    {
        public string UserID { get; set; }
        public int SecurityGroupID { get; set; }
        public string DisplayName { get; set; }
        public string StoreTLC { get; set; }
        public string Region { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLogin { get; set; }
        public string LastLoginIPAddress { get; set; }
        public SecurityGroup SecurityGroup { get { return _securityGroup; } }
        public SecurityGroup OriginalSecurityGroup { get { return _originalSecurityGroup; } }
        private SecurityGroup _securityGroup { get; set; }
        private SecurityGroup _originalSecurityGroup { get; set; }

        public void init()
        {
            UserID = "";
            SecurityGroupID = 0;
            Region = "";
            DateCreated = DateTime.Now;
            LastLogin = DateTime.Now;
            LastLoginIPAddress = "";
            // Default to Team Member level (login, catalog, orders)
            _securityGroup = new SecurityGroup(4);
            _originalSecurityGroup = _securityGroup;
        }

        public UserRecord()
        {
  
        }

        public UserRecord(string ID)
        {
            init();
            UserID = ID;
            Load();
        }
        
        public void LoadUserOverride(string UserID, string AuthID)
        {
            if (String.IsNullOrEmpty(UserID) || String.IsNullOrEmpty(AuthID))
            {
                return;
            }

            // Load security record from database using the user ID
            string query = "SELECT TOP 1 UserID, SecurityGroupID, Region, DateCreated, LastLogin, LastLoginIPAddress FROM POUsers (nolock) WHERE UserID = @UserID";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    UserID = rdr[0].ToString();
                    SecurityGroupID = Convert.ToInt32(AuthID);
                    _originalSecurityGroup = new SecurityGroup(Convert.ToInt32(rdr[1].ToString()));
                    Region = rdr[2].ToString();
                    try { DateCreated = Convert.ToDateTime(rdr[3].ToString()); }
                    catch { }
                    try { LastLogin = Convert.ToDateTime(rdr[4].ToString()); }
                    catch { }
                    LastLoginIPAddress = rdr[5].ToString();

                    if (SecurityGroupID > 0)
                    {
                        _securityGroup = new SecurityGroup(SecurityGroupID);
                    }
                }
            }
        }

        public void LoadUserSecurity(string _uid)
        {
            if (String.IsNullOrEmpty(_uid))
            {
                return;
            }
            UserID = _uid;

            // Load security record from database using the user ID
            string query = "SELECT TOP 1 UserID, SecurityGroupID, Region, DateCreated, LastLogin, LastLoginIPAddress FROM POUsers (nolock) WHERE UserID = @UserID";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    SecurityGroupID = Convert.ToInt32(rdr[1].ToString());
                    Region = rdr[2].ToString();
                    try { DateCreated = Convert.ToDateTime(rdr[3].ToString()); }
                    catch { }
                    try { LastLogin = Convert.ToDateTime(rdr[4].ToString()); }
                    catch { }
                    LastLoginIPAddress = rdr[5].ToString();

                    if (SecurityGroupID > 0)
                    {
                        _securityGroup = new SecurityGroup(SecurityGroupID);
                        _originalSecurityGroup = _securityGroup;
                    }
                }
                else
                {
                    cn.Close();                
                    // No special permission set. Use Default
                    SecurityGroupID = 4;
                    Region = "SW";
                    _securityGroup = new SecurityGroup(SecurityGroupID);
                    _originalSecurityGroup = _securityGroup;
                }
            }
        }

        private void Load()
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return;
            }

            // Load from database
            string query = "SELECT TOP 1 SecurityGroupID, Region, DateCreated, LastLogin, LastLoginIPAddress FROM POUsers (nolock) WHERE UserID = @UserID";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    UserID = rdr[0].ToString();
                    SecurityGroupID = Convert.ToInt32(rdr[1].ToString());
                    Region = rdr[2].ToString();
                    try { DateCreated = Convert.ToDateTime(rdr[3].ToString()); }
                    catch { }
                    try { LastLogin = Convert.ToDateTime(rdr[4].ToString()); }
                    catch { }
                    LastLoginIPAddress = rdr[5].ToString();

                    if (SecurityGroupID > 0)
                    {
                        _securityGroup = new SecurityGroup(SecurityGroupID);
                    }
                }
            }
        }

        private bool RecordExists()
        {
            string query = "SELECT UserID FROM POUsers WHERE UserID=@UserID";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@UserID", UserID);

                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return rdr.HasRows;
            }
        }

        public void Save()
        {
            string query = "";
            bool IsInsert = false;

            if (!string.IsNullOrEmpty(UserID))
            {
                return;
            }

            if(!RecordExists()){
                // Insert
                query = "INSERT INTO POUsers (UserID, SecurityGroupID, Region, DateCreated, LastLogin, LastLoginIPAddress) VALUES (@UserID, @SecurityGroupID, @Region, GETDATE(), @LastLogin, @LastLoginIPAddress);";
                IsInsert = true;
            }
            else
            {
                // Update
                query = "UPDATE POUsers SET SecurityGroupID=@SecurityGroupID, Region=@Region, LastLogin=@LastLogin, LastLoginIPAddress=@LastLoginIPAddress WHERE UserID=@UserID";
            }

            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@SecurityGroupID", SecurityGroupID);
                cmd.Parameters.AddWithValue("@Region", Region);
                cmd.Parameters.AddWithValue("@LastLogin", LastLogin);
                cmd.Parameters.AddWithValue("@LastLoginIPAddress", LastLoginIPAddress);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            if (SecurityGroupID > 0)
            {
                _securityGroup = new SecurityGroup(SecurityGroupID);
            }            
            return;
        }

        public void Delete()
        {
            if (string.IsNullOrEmpty(UserID))
            {
                return;
            }
            // The delete function will only flag the cake as deleted so we can still reference this data 
            // for any orders that were placed in the past.
            string query = "DELETE FROM POUsers WHERE UserID=@UserID";

            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cn.Open();
                cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public bool notAllowed(string PAGE_ID, bool original = false)
        {
            // True = not allowed, deny access
            SecurityGroup security;
            if (original)
            {
                security = _originalSecurityGroup;
            }
            else
            {
                security = _securityGroup;
            }


            if (security.isAdmin)
            {
                return false;
            }
            switch (PAGE_ID)
            {
                case "ADMIN":
                    if (security.isAdmin) return false;
                    break;
                case "VIEW_ITEMS":
                    if (security.canViewItems || security.canEditItems) return false;
                    break;
                case "EDIT_ITEMS":
                    if (security.canEditItems) return false;
                    break;
                case "EDIT_PROGRAMS":
                    if (security.canEditPrograms) return false;
                    break;
                case "IMPORT":
                    if (security.canImport) return false;
                    break;
                case "LOGIN":
                    if (security.canLogin) return false;
                    break;
                case "REPORTS":
                    if (security.canViewReports) return false;
                    break;
                case "SWITCH_STORES":
                    if (security.canSwitchStores) return false;
                    break;
                default:
                    break;
            }
            return true;
        }

        public bool isAllowed(string PAGE_ID, bool original = false)
        {
            SecurityGroup security;
            if (original)
            {
                security = _originalSecurityGroup;
            }
            else
            {
                security = _securityGroup;
            }
            // True = not allowed, deny access
            if (security.isAdmin)
            {
                return true;
            }
            switch (PAGE_ID)
            {
                case "ADMIN":
                    if (security.isAdmin) return true;
                    break;
                case "VIEW_ITEMS":
                    if (security.canViewItems || security.canEditItems) return true;
                    break;
                case "EDIT_ITEMS":
                    if (security.canEditItems) return true;
                    break;
                case "EDIT_PROGRAMS":
                    if (security.canEditPrograms) return true;
                    break;
                case "IMPORT":
                    if (security.canImport) return true;
                    break;
                case "LOGIN":
                    if (security.canLogin) return true;
                    break;
                case "REPORTS":
                    if (security.canViewReports) return true;
                    break;
                case "SWITCH_STORES":
                    if (security.canSwitchStores) return true;
                    break;
                default:
                    break;
            }
            return false;
        }


        //! Mapps an bbActiveDirectory object to a bbUser object
        public void LoadUserDataFromActiveDirectory(ActiveDirectory ad)
        {
            UserID = ad.GUID.ToString().ToUpper();
            //Firstname = ad.Firstname;
            //Lastname = ad.Lastname;
            DisplayName = ad.DisplayName;
            //TMNumber = ad.TMNumber;
            //Address = ad.Address;
            //City = ad.City;
            //State = ad.State;
            //Zip = ad.Zip;
            //Email = ad.Email;
            //Country = ad.Country;
            //Company = ad.Company;
            StoreTLC = ad.StoreTLC;
            Region = ad.Region;
            //SAMAccountName = ad.SAMAccountName;
            //DistinguishedName = ad.DistinguishedName;
            //Groups = ad.MemberOf;
        }

        public bool GetUserFromActiveDirectory(string _email)
        {
            ActiveDirectory ad = new ActiveDirectory(true, _email);
            if (ad.isValid)
            {
                LoadUserDataFromActiveDirectory(ad);
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetUserFromActiveDirectory(Guid _userID)
        {
            ActiveDirectory ad = new ActiveDirectory(_userID);
            if (ad.isValid)
            {
                LoadUserDataFromActiveDirectory(ad);
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}