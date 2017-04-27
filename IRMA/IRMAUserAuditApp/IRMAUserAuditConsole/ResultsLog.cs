using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using OfficeOpenXml;

namespace IRMAUserAuditConsole
{
    enum ResultType : int { Error = 1, Imported, NoChange };
    class ResultsLog
    {
        public string fileName { get; set; }
        private ExcelPackage package;
        private ExcelHelper helper;
        private FileInfo fi;
        // these start at 3 because Excel rows are 1-based, 
        //  and because row 1 is the header and row 2 is blank
        private int errorRowCount = 3;
        private int importRowCount = 3;
        private int nochangeRowCount = 3;

        private string storeFormula;
        private string titleFormula;
        private string yesNoFormula;

        public ResultsLog(string _fileName)
        {
            fileName = _fileName;
            fi = new FileInfo(fileName);
            //fi.Open(FileMode.OpenOrCreate);
            package = new ExcelPackage(fi);
            helper = new ExcelHelper();
            CreateWorksheets();
            CreateHeaders();
        }

        public void CreateWorksheets()
        {
            helper.NewWorksheet(package, "Errors");
            helper.NewWorksheet(package, "Imported");
            helper.NewWorksheet(package, "No Change");
        }

        public void CreateHeaders()
        {
            helper.CreateHeader(package, "Imported", new string[] { "User_ID", "UserName", "FullName", "Title", "StoreLimit", "Override Allow", "Override Deny", "User Edited?", "Delete User?", "Action Taken" }, 1);
            helper.CreateHeader(package, "Errors", new string[] { "User_ID", "UserName", "FullName", "Title", "StoreLimit", "Override Allow", "Override Deny", "User Edited?", "Delete User?", "Error" }, 1);
            helper.CreateHeader(package, "No Change", new string[] { "User_ID", "UserName", "FullName", "Title", "StoreLimit", "Override Allow", "Override Deny", "User Edited?", "Delete User?" }, 1);
        }

        public void CreateDropdowns(ref List<string> stores, ref List<string> titles, ref List<string> yesno)
        {
            foreach (string ws in new string[] { "Imported", "Errors", "No Change" })
            {
                yesNoFormula = helper.EmbedListOptions(package, ws, "X", 3, yesno.ToArray());
                storeFormula = helper.EmbedListOptions(package, ws, "Y", 3, stores.ToArray());
                titleFormula = helper.EmbedListOptions(package, ws, "Z", 3, titles.ToArray());
            }
        }

        public void HideDropdowns()
        {
            foreach (string ws in new string[] { "Imported", "Errors", "No Change" })
            {
                helper.HideColumn(package, ws, 24);
                helper.HideColumn(package, ws, 25);
                helper.HideColumn(package, ws, 26);
            }
        }

        public void Close()
        {
            package.Save();
        }

        public void Add(ResultType type, List<object> rowData)
        {
            string worksheet = "Imported";
            int row = importRowCount;
            if (type == ResultType.Error)
            {
                worksheet = "Errors";
                row = errorRowCount;
            }
            else if (type == ResultType.NoChange)
            {
                worksheet = "No Change";
                row = nochangeRowCount;
            }

            try
            {
                helper.AddRow(package, worksheet, row, rowData.ToArray());
                IncrementRowCount(worksheet);
            }
            catch (Exception ex)
            {
                // TODO:  umm...do something with this?
                // not sure what to do with an error 
                // thrown while writing to the error log  :)
                // You have errored!  And I will Error AGAIN!
            }
        }

        private void IncrementRowCount(string worksheet)
        {
            if (worksheet == "Imported")
                importRowCount++;
            else if (worksheet == "Error")
                errorRowCount++;
            else
                nochangeRowCount++;
        }
    }
}
