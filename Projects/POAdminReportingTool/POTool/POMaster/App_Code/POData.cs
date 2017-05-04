using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for CreditItem
/// </summary>
/// 

namespace POReports
{
    public class POData
    {
        public int PONumber { get; set; }
        public string Suspended { get; set; }
        public DateTime CloseDate { get; set; }
        public string ResolutionCode { get; set; }
        public string AdminNotes { get; set; }
        public string Vendor { get; set; }
        public int Subteam { get; set; }
        public string Store { get; set; }
        public string AdjustedCost { get; set; }
        public string CreditPO { get; set; }
        public string VendorType { get; set; }
        public string POCreator { get; set; }
        public string EInvoiceMatchedToPO { get; set; }
        public string PONotes { get; set; }
        public string ClosedBy { get; set; }
        public string Region { get; set; }

        private void ResetData()
        {
             PONumber = 0;
             Suspended = "N";
             CloseDate = DateTime.MinValue;
             ResolutionCode = "";
             AdminNotes = "";
             Vendor = "";
             Subteam = 0;
             Store = "";
             AdjustedCost = "N";
             CreditPO  = "N";
             VendorType  = "";
             POCreator = "";
             EInvoiceMatchedToPO = "N";
             PONotes = "";
             ClosedBy = "";
             Region = "";
        }

        public POData()
        {
            // New Empty Item
            ResetData();
        }

        public void init()
        {

        }
    }
}