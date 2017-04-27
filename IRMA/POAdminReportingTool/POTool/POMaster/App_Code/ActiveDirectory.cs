using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Hosting;

using System.Runtime.InteropServices; // DllImport
using System.Security.Principal; // WindowsImpersonationContext
using System.Security.Permissions; // PermissionSetAttribute

namespace POReports
{
    public class ActiveDirectory
    {
        // Public vars
        public Guid GUID { get; set; }
        public string SAMAccountName { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Region { get; set; }
        public string StoreTLC { get; set; }
        public string DistinguishedName { get; set; }
        public string MemberOf { get; set; }
        public string DisplayName { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string TMNumber { get; set; }
        public string ConflictStore { get; set; }
        public string ConflictRegion { get; set; }
        public static string DomainName = "wfm.pvt";
        public static string ADContainer = "DC=wfm,DC=pvt";

        public bool isValid { get { return _valid; } }
        public bool isAuthenticated { get { return _authenticated; } }

        // Private vars
        private bool _authenticated { get; set; }
        private bool _valid { get; set; }

        private void ResetData()
        {
            LoginName = "";
            UserName = "";
            SAMAccountName = "";
            Firstname = "";
            Lastname = "";
            Region = "";
            DistinguishedName = "";
            MemberOf = "";
            Email = "";
            GUID = new Guid();
            DisplayName = "";
            TMNumber = "";
            _valid = false;
            _authenticated = false;
            ConflictStore = "";
            ConflictRegion = "";
        }

        public ActiveDirectory()
        {
            ResetData();
        }

        public ActiveDirectory(string _LoginName)
        {
            ResetData();
            LoginName = _LoginName;
            UserName = ExtractUserName(LoginName);
            if (IsExistInAD())
            {
                _valid = true;
            }
            else
            {
                _valid = false;
            }
        }

        public ActiveDirectory(bool isEmail, string _email)
        {
            ResetData();
            Email = _email;
            if (IsExistInAD())
            {
                _valid = true;
            }
            else
            {
                _valid = false;
            }
        }

        public ActiveDirectory(Guid objGuid)
        {
            ResetData();
            GUID = objGuid;

            if (IsExistInAD())
            {
                _valid = true;
            }
            else
            {
                _valid = false;
            }
        }

        private void LoadUserData(UserPrincipal up)
        {
            if (up != null && up.Guid != null)
            {

                UserName = up.SamAccountName;
                SAMAccountName = up.SamAccountName;
                DistinguishedName = up.DistinguishedName;
                Firstname = up.GivenName;
                Lastname = up.Surname;
                DisplayName = up.DisplayName;
                Email = up.EmailAddress;
                GUID = up.Guid.Value;

                Zip = up.GetProperty("postalcode");
                Company = up.GetProperty("company");
                State = up.GetProperty("st");
                Address = up.GetProperty("streetaddress");
                Country = up.GetProperty("c");
                City = up.GetProperty("l");
                TMNumber = up.GetProperty("employeeid");
                MemberOf = up.GetMemberOfString();

                // Parse meta data (store, region)
                ParseMetaData();
            }
        }

        //! Authenticate the user's credientals against active directory
        public bool Authenticate(string Username, string Password)
        {
            _authenticated = false;
            _valid = false;
            // Attempt to authenticate user
            PrincipalContext adContext = new PrincipalContext(ContextType.Domain);

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, DomainName, ADContainer))
            {
                Username = Username.Replace(@"WFM\", "");
                Username = Username.Replace(@"wfm\", "");
                Username = Username.Replace("@wfm.pvt", "");
                Username = Username.Replace("@WFM.PVT", "");
                if (pc.ValidateCredentials(Username, Password))
                {
                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, Username);
                    if (up != null && up.Guid != null)
                    {
                        LoadUserData(up);

                        // Update/Save user data
                        string UserID = GUID.ToString().ToUpper();
                        UserRecord _user = Factory.GetUser(UserID, false);
                        if (_user == null) _user = new UserRecord();
                        _user.LoadUserDataFromActiveDirectory(this);
                        _user.UserID = UserID;
                        _user.Save();

                        // Set valid and authenticated
                        _valid = true;
                        _authenticated = true;
                    }
                }
            }

            return isAuthenticated;
        }

        private string ExtractUserName(string path)
        {
            string[] userPath = path.Split(new char[] { '\\' });
            return userPath[userPath.Length - 1];
        }

        public bool IsExistInAD()
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, DomainName, ADContainer))
            {
                UserPrincipal up = null;

                if (GUID.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    up = UserPrincipal.FindByIdentity(pc, IdentityType.Guid, GUID.ToString());
                }
                else if (!String.IsNullOrEmpty(UserName))
                {
                    up = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, UserName);
                }
                else if (!String.IsNullOrEmpty(Email))
                {
                    up = new UserPrincipal(pc);
                    up.EmailAddress = Email;

                    // create a principal searcher for running a search operation
                    PrincipalSearcher pS = new PrincipalSearcher(up);

                    // run the query
                    PrincipalSearchResult<Principal> results = pS.FindAll();

                    if (results.Count<Principal>() > 0)
                    {
                        // only reference the first one
                        up = results.ElementAt<Principal>(0) as UserPrincipal;
                    }
                }

                if (up != null)
                {
                    LoadUserData(up);
                    return true;
                }
                else
                {
                    Firstname = "Unknown";
                    Lastname = "User";
                    return false;
                }
            }
        }

        private void ParseMetaData()
        {
            if (String.IsNullOrEmpty(DistinguishedName))
            {
                return;
            }
            // Parse Region
            string[] arr = DistinguishedName.Split(',');
            // -3
            int pos = arr.Length - 4;
            Region = arr[pos].Replace("OU=", "");
            // Compensate for renamed northern pacific region
            if (Region == "NP")
            {
                Region = "NC";
            }

            // Parse Store TLC
            arr = DistinguishedName.Split(',');
            // -3
            pos = arr.Length - 5;
            StoreTLC = arr[pos].Replace("OU=", "");

            // Check for conflict against DisplayName
            string SecondaryTLC = "";
            string SecondaryRegion = "";

            try
            {
                arr = DisplayName.Split('(');
                string s = arr[1].Replace(")", string.Empty);
                arr = s.Split(' ');
                SecondaryRegion = arr[0];
                SecondaryTLC = arr[1];
            }
            catch { }

            if (!String.IsNullOrEmpty(SecondaryTLC) && (SecondaryTLC != StoreTLC))
            {
                // CONFLICT FOUND!
                ConflictStore = SecondaryTLC;

                // Check groups to see if the primary store is still used
                if (MemberOf.Contains(StoreTLC + " "))
                {
                    // Primary store still exists in groups so let things be.
                }
                else if (MemberOf.Contains(ConflictStore))
                {
                    // Primary store not found. Assume AD updated incorrectly and assign
                    // Conflict store as the primary store.
                    StoreTLC = ConflictStore;
                }
                else
                {
                    // Neither store found. NO CHANGE
                }
            }

            if (!String.IsNullOrEmpty(SecondaryRegion) && (SecondaryRegion != Region))
            {
                // CONFLICT FOUND!
                ConflictRegion = SecondaryRegion;
            }
        }
    }

    //! Extension for the account management class that allow you get additional properties from a UserPrincipal object
    public static class AccountManagementExtensions
    {

        public static String GetProperty(this Principal principal, String property)
        {
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            if (directoryEntry.Properties.Contains(property))
                return directoryEntry.Properties[property].Value.ToString();
            else
                return String.Empty;
        }

        public static String GetMemberOfString(this Principal principal)
        {
            DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
            if (directoryEntry.Properties.Contains("memberof"))
            {
                string x = "";
                for (var i = 0; i < directoryEntry.Properties["memberof"].Count; i++)
                {
                    try
                    {
                        string[] y = directoryEntry.Properties["memberof"][i].ToString().Split(',');
                        if (i > 0)
                        {
                            x += ",";
                        }
                        x += y[0].Replace("CN=", "");
                    }
                    catch { }
                }
                return x;

            }
            else
                return String.Empty;
        }

        public static String GetCompany(this Principal principal)
        {
            return principal.GetProperty("company");
        }

        public static String GetDepartment(this Principal principal)
        {
            return principal.GetProperty("department");
        }

    }
}