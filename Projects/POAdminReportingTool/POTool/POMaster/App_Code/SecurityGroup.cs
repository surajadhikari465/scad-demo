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
namespace POReports
{
    public class SecurityGroup
    {
        public int SecurityGroupID { get; set; }
        public Boolean canLogin { get; set; }
        public Boolean canViewItems { get; set; }
        public Boolean canEditItems { get; set; }
        public Boolean canEditPrograms { get; set; }
        public Boolean canViewReports { get; set; }
        public Boolean canImport { get; set; }
        public Boolean isAdmin { get; set; }
        public Boolean canSwitchStores { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string AccessList
        {
            get
            {
                string s = "";
                if (canLogin) s += ", Login";
                if (canViewItems) s += ", View Items";
                if (canEditItems) s += ", Edit Items";
                if (canEditPrograms) s += ", Edit Programs";
                if (canViewReports) s += ", View Reports";
                if (canSwitchStores) s += ", Switch Stores";
                if (canImport) s += ", Import";
                if (isAdmin) s = ", Admin Access";

                if (s == "")
                {
                    s = "None";
                }
                else
                {
                    s = s.Substring(2, s.Length - 2);
                }
                return s;
            }
        }
        private void init()
        {
            SecurityGroupID = 0;
            canLogin = true;
            canViewItems = false;
            canEditItems = false;
            canEditPrograms = false;
            canViewReports = false;
            canImport = false;
            isAdmin = false;
            canSwitchStores = false;
            Name = "";
            Priority = 0;
        }

        public SecurityGroup()
        {
            init();
        }

        public SecurityGroup(int ID)
        {
            init();
            SecurityGroupID = ID;
            Load();
        }

        public SecurityGroup(string ID)
        {
            init();
            try
            {
                SecurityGroupID = Convert.ToInt32(ID);
                Load();
            }
            catch
            {
                // Do nothing
            }
        }

        private void Load()
        {
            if (SecurityGroupID <= 0)
            {
                return;
            }

            // Load from database
            string query = "SELECT TOP 1 Name, canLogin, canViewItems, canEditItems, canEditPrograms, canViewReports, canImport, isAdmin, Priority, canSwitchStores FROM POSecurityGroups (nolock) WHERE SecurityGroupID = @SecurityGroupID";
            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@SecurityGroupID", SecurityGroupID);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (rdr.HasRows)
                {
                    rdr.Read();
                    Name = rdr[0].ToString();
                    try { canLogin = Convert.ToBoolean(rdr[1].ToString()); }
                    catch { }
                    try { canViewItems = Convert.ToBoolean(rdr[2].ToString()); }
                    catch { }
                    try { canEditItems = Convert.ToBoolean(rdr[3].ToString()); }
                    catch { }
                    try { canEditPrograms = Convert.ToBoolean(rdr[4].ToString()); }
                    catch { }
                    try { canViewReports = Convert.ToBoolean(rdr[5].ToString()); }
                    catch { }
                    try { canImport = Convert.ToBoolean(rdr[6].ToString()); }
                    catch { }
                    try { isAdmin = Convert.ToBoolean(rdr[7].ToString()); }
                    catch { }
                    try { Priority = Convert.ToInt32(rdr[8].ToString()); }
                    catch { }
                    try { canSwitchStores = Convert.ToBoolean(rdr[9].ToString()); }
                    catch { }

                }
            }
        }

        public int Save()
        {
            string query = "";
            bool IsInsert = false;

            if (SecurityGroupID <= 0)
            {
                // Insert
                query = "INSERT INTO POSecurityGroups (Name, canLogin, canViewItems, canEditItems, canEditPrograms, canViewReports, canImport, isAdmin, Priority, canSwitchStores) VALUES (@Name, @canLogin, @canViewItems, @canEditItems, @canEditPrograms, @canViewReports, @canImport, @isAdmin, @Priority, @canSwitchStores); select [ID] = IDENT_CURRENT('POSecurityGroups');";
                IsInsert = true;
            }
            else
            {
                // Update
                query = "UPDATE POSecurityGroups SET Name=@Name, canLogin=@canLogin, canViewItems=@canViewItems, canEditItems=@canEditItems, canEditPrograms=@canEditPrograms, canViewReports=@canViewReports, canImport=@canImport, isAdmin=@isAdmin, Priority=@Priority, canSwitchStores=@canSwitchStores WHERE SecurityGroupID=@SecurityGroupID";
            }

            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@canLogin", canLogin);
                cmd.Parameters.AddWithValue("@canViewItems", canViewItems);
                cmd.Parameters.AddWithValue("@canEditItems", canEditItems);
                cmd.Parameters.AddWithValue("@canEditPrograms", canEditPrograms);
                cmd.Parameters.AddWithValue("@canViewReports", canViewReports);
                cmd.Parameters.AddWithValue("@canImport", canImport);
                cmd.Parameters.AddWithValue("@isAdmin", isAdmin);
                cmd.Parameters.AddWithValue("@Priority", Priority);
                cmd.Parameters.AddWithValue("@canSwitchStores", canSwitchStores);

                if (!IsInsert)
                {
                    // Update Only Parameters
                    cmd.Parameters.AddWithValue("@SecurityGroupID", SecurityGroupID);
                }

                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (IsInsert && rdr.HasRows)
                {
                    rdr.Read();
                    SecurityGroupID = Convert.ToInt32(rdr[0].ToString());
                }
            }

            return SecurityGroupID;
        }

        public void Delete()
        {
            if (SecurityGroupID == 0)
            {
                return;
            }

            string query = "DELETE FROM POSecurityGroups WHERE SecurityGroupID=@SecurityGroupID";
            string query2 = "UPDATE POSecurity SET SecurityGroupID=1 WHERE SecurityGroupID=@SecurityGroupID";

            using (SqlConnection cn = new SqlConnection(Config.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@SecurityGroupID", SecurityGroupID);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                cmd = new SqlCommand(query2, cn);
                cmd.Parameters.AddWithValue("@SecurityGroupID", SecurityGroupID);
                cn.Open();
                cmd.ExecuteNonQuery();

            }
        }
    }
}