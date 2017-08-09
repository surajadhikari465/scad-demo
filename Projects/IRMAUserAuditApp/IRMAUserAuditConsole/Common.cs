using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IRMAUserAuditConsole
{
    public static class Common
    {
        //public static List<string> StoreList = new List<string>();
        //public static List<string> TitleList = new List<string>();
        public static List<string> YesNoList = new List<string>(new string[] { "Yes", "No" });
        public static DateTime currentDateTime = DateTime.Now;

        public static string CreateRegionFilename(string region)
        {
            //DateTime current = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            sb.Append(region.ToUpper());
            sb.Append(currentDateTime.ToString("s"));
            sb.Append(".xlsx");
            sb.Replace(":", "_");
            sb.Replace(" ", "_");
            sb.Replace("/", "-");
            sb.Replace("\\", "-");
            return sb.ToString();
        }

        public static string CreateStoreFilename(Store store)
        {
            //DateTime current = DateTime.Now;
            StringBuilder sb = new StringBuilder();
            if (store.BusinessUnit_ID.HasValue)
            {
                sb.Append(store.BusinessUnit_ID);
                sb.Append("_");
            }
            sb.Append(store.Store_Name.Trim());
            sb.Append("_");
            sb.Append(currentDateTime.ToString("s"));
            sb.Append(".xlsx");
            sb.Replace(":", "_");
            sb.Replace(" ", "_");
            sb.Replace("/", "-");
            sb.Replace("\\", "-");
            return sb.ToString();
        }

        public static SpreadsheetManager SetupStoreSpreadsheet(string fileName, List<string> StoreList, List<string> TitleList)
        {
            SpreadsheetManager ssm = new SpreadsheetManager(fileName);
            ssm.CreateWorksheet("Users");
            ssm.CreateHeader("Users", (new string[] { "User_ID", "UserName", "FullName", "Title", "StoreLimit", "Override Allow", "Override Deny", "User Edited?", "Delete User?" }).ToList());
            ssm.AddDropdown("Users", "TitlesDropdown", "X", 1, TitleList.ToArray());
            ssm.AddDropdown("Users", "StoresDropdown", "Y", 1, StoreList.ToArray());
            ssm.AddDropdown("Users", "YesNoDropdown", "Z", 1, YesNoList.ToArray());

            // no need to hide dropdowns since they are on a separate (hidden) sheet
            //ssm.HideColumn("Users", 24);
            //ssm.HideColumn("Users", 25);
            //ssm.HideColumn("Users", 26);

            // SLIM sheet
            ssm.CreateWorksheet("SLIM");
            ssm.CreateHeader("SLIM", (new string[] { "User_ID", "UserName", "FullName", "WebQuery", "ItemRequest", "ISS", "Store", "Team", "User Edited?" }).ToList());
            ssm.AddDropdown("SLIM", "YesNoDropdown", "Z", 1, YesNoList.ToArray());

            // no need to hide columns now that formulas are on a separate sheet
            //ssm.HideColumn("SLIM", 26);

            return ssm;
        }

        public static void AddUserToSpreadsheet(ref SpreadsheetManager ssm, UserInfo ui)
        {
            List<object> items = new List<object>();
            items.Add(ui.User_ID);
            items.Add(ui.UserName);
            items.Add(ui.FullName);
            items.Add(ui.Title);
            items.Add(ui.Location);
            items.Add(ui.User_Disabled);
            items.Add("No");
            items.Add("No");
            int row = ssm.GetCurrentRow("Users");
            ssm.AssignFormulaToCell("Users", "TitlesDropdown", "D" + row.ToString());
            ssm.AssignFormulaToCell("Users", "StoresDropdown", "E" + row.ToString());
            ssm.AssignFormulaToCell("Users", "YesNoDropdown", "H" + row.ToString());
            ssm.AssignFormulaToCell("Users", "YesNoDropdown", "I" + row.ToString());
            ssm.AddRow("Users", items.ToArray());

        }

        public static void CreateFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                di.Create();
            }
        }

        public static string CreateFiscalYearString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("FY");
            sb.Append(FiscalYear.Year());
            sb.Append("_Q");
            sb.Append(FiscalYear.Quarter());

            return sb.ToString();
        }
    }
}
