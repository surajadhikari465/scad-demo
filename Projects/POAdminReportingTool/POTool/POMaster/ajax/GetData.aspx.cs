using System;
using System.Reflection;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

public partial class ajax_GetData : System.Web.UI.Page
{
    public string JSONData;
    public POReports.POSession bb;
    public string ActiveFP;
    public string ActiveFY;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Load Session
        bb = new POReports.POSession(HttpContext.Current);

        ActiveFP = bb.ActiveFP.Period.ToString();
        ActiveFY = bb.ActiveFP.Year.ToString();
        string Action = Request.Params["A"];
        AJAXResult returnData = new AJAXResult();
        string ID;
        POReports.UserRecord security;
        POReports.UserRecord User = new POReports.UserRecord();
        
        int fy = 0;
        int fp = 0;
        int fw = 0;
        string detailType = "";

        // Check if user session is active
        if (bb.isLoggedIn)
        {
            User.LoadUserSecurity(bb.CurrentUser.UserID);

            switch (Action)
            {

                case "revert_region":
                    HttpContext.Current.Session["STORE_OVERRIDE"] = "";
                    HttpContext.Current.Session["AUTH_OVERRIDE"] = "";
                    //bb.CurrentUser.ClearOverride();
                    returnData.msg = "OK";
                    break;
                case "change_region":
                    if (User.isAllowed("ADMIN") || (HttpContext.Current.Session["AUTH_OVERRIDE"] != null && !String.IsNullOrEmpty(HttpContext.Current.Session["AUTH_OVERRIDE"].ToString())))
                    {
                        HttpContext.Current.Session["STORE_OVERRIDE"] = Request.Params["tlc"].ToString();
                        if (Request.Params["auth"] != null)
                        {
                            HttpContext.Current.Session["AUTH_OVERRIDE"] = Request.Params["auth"].ToString();
                        }
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Permission Denied";
                        returnData.result = false;
                    }
                    break;
                
                case "lookup_tms":
                    List<POReports.TeamMember> tms = POReports.Factory.FindTeamMembers(Request.Params["C"]);
                    returnData.data = tms;
                    returnData.msg = "Found " + tms.Count.ToString() + " team members";
                    break;
                case "get_user_sec":
                    string guid;
                    if (String.IsNullOrEmpty(Request.Params["ID"]))
                    {
                        guid = Request.Params["UID"];
                        security = new POReports.UserRecord();
                        security.LoadUserSecurity(guid);
                    }
                    else
                    {
                        ID = Request.Params["ID"];
                        security = new POReports.UserRecord(ID);
                        guid = security.UserID;
                    }

                    returnData.data = security;
                    returnData.msg = security.DisplayName;
                    break;
                case "save_user_sec":
                    ID = Request.Params["ID"].ToString();
                    if (!String.IsNullOrEmpty(ID) && ID != "0")
                    {
                        security = new POReports.UserRecord(ID);
                    }
                    else
                    {
                        security = new POReports.UserRecord();
                        security.LoadUserSecurity(Request.Params["UID"].ToString());
                    }
                    if (string.IsNullOrEmpty(security.UserID))
                    {
                        security.UserID = Request.Params["UID"].ToString().ToUpper();
                        security.Region = ParseRegion(Request.Params["N"].ToString());
                    }
                    if (String.IsNullOrEmpty(security.Region))
                    {
                        security.Region = bb.CurrentUser.Region;
                    }
                    try { security.SecurityGroupID = Convert.ToInt32(Request.Params["AL"].ToString()); }catch{}
                    security.Save();
                    returnData.data = security;
                    break;
                case "delete_user_sec":
                    ID = Request.Params["ID"];
                    if (!String.IsNullOrEmpty(ID) && ID != "0")
                    {
                        security = new POReports.UserRecord(ID);
                        returnData.data = security;
                        security.Delete();
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Missing ID";
                    }
                    break;
                default:
                    returnData.msg = "Invalid action code";
                    break;
            }
        }
        else
        {
            switch (Action)
            {
                
                default:
                    returnData.msg = "Invalid action code";
                    break;
            }
        }

        // Common Actions
        if (returnData.msg == "Invalid action code")
        {
            switch (Action)
            {
                case "export_store_detail":
                    returnData.data = POReports.Factory.ExportStoreDetail(Request.Params["fy"], Request.Params["fp"], Request.Params["fw"], Request.Params["store"], Request.Params["region"]);
                    if (returnData.data != "")
                    {
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Unable to create file";
                    }
                    break;
                case "export_rc_detail":
                    returnData.data = POReports.Factory.ExportRCDetail(Request.Params["fy"], Request.Params["fp"], Request.Params["fw"], Request.Params["rc"], Request.Params["region"]);
                    if (returnData.data != "")
                    {
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Unable to create file";
                    }
                    break;
                case "export_vendor_detail":
                    returnData.data = POReports.Factory.ExportVendorDetail(Request.Params["fy"], Request.Params["fp"], Request.Params["fw"], Request.Params["vendor"], Request.Params["region"]);
                    if (returnData.data != "")
                    {
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Unable to create file";
                    }
                    break;
                case "export_fw_detail":
                    returnData.data = POReports.Factory.ExportFiscalWeekDetail(Request.Params["ds"], Request.Params["type"], Request.Params["region"]);
                    if (returnData.data != "")
                    {
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Unable to create file";
                    }
                    break;
                case "export_fp_detail":
                    returnData.data = POReports.Factory.ExportFiscalPeriodDetail(Request.Params["ds"], Request.Params["type"], Request.Params["region"]);
                    if (returnData.data != "")
                    {
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Unable to create file";
                    }
                    break;
                case "export_global_overview":
                    returnData.data = POReports.Factory.ExportGlobalOverview(Request.Params["fy"].ToString());
                    if (returnData.data != "")
                    {
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Unable to create file";
                    }
                    break;
                case "export_overview":
                    returnData.data = POReports.Factory.ExportFiscalOverview(Request.Params["fy"].ToString(), Request.Params["region"]);
                    if (returnData.data != "")
                    {
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Unable to create file";
                    }
                    break;
                case "get_vendor_trends":
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                    }
                    catch{}

                    if (fy > 0)
                    {
                        returnData.data = POReports.Factory.GetVendorPOTrends(fy, Request.Params["vendor"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Fiscal value cannot be 0";
                    }
                    
                    break;
                case "get_vendor_detail":
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                        fp = Convert.ToInt32(Request.Params["fp"].ToString());
                        fw = Convert.ToInt32(Request.Params["fw"].ToString());
                        detailType = Request.Params["type"];
                    }
                    catch{}

                    if (fp > 0 && fw > 0 && fy > 0 && detailType == "fiscal_week")
                    {
                        POReports.FiscalWeek _tmpWk = POReports.Factory.GetFiscalWeek(fy, fp, fw);
                        returnData.data = POReports.Factory.GetVendorPOsWithRCTotals(_tmpWk.StartDate, _tmpWk.EndDate, Request.Params["vendor"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }
                    else if (fp > 0 && fy > 0 && detailType == "fiscal_period")
                    {
                        POReports.FiscalPeriod _tmpWk = POReports.Factory.GetFiscalPeriod(fy, fp);
                        returnData.data = POReports.Factory.GetVendorPOsWithRCTotals(_tmpWk.StartDate, _tmpWk.EndDate, Request.Params["vendor"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Fiscal value cannot be 0";
                    }
                    
                    break;
                case "get_store_trends":
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                    }
                    catch { }

                    if (fy > 0)
                    {
                        returnData.data = POReports.Factory.GetStorePOTrends(fy, Request.Params["store"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }                   
                    else
                    {
                        returnData.msg = "ERROR: Fiscal value cannot be 0";
                    }

                    break;
                case "get_store_detail":
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                        fp = Convert.ToInt32(Request.Params["fp"].ToString());
                        fw = Convert.ToInt32(Request.Params["fw"].ToString());
                        detailType = Request.Params["type"];
                    }
                    catch { }

                    if (fp > 0 && fw > 0 && fy > 0 && detailType == "fiscal_week")
                    {
                        POReports.FiscalWeek _tmpWk = POReports.Factory.GetFiscalWeek(fy, fp, fw);
                        returnData.data = POReports.Factory.GetStorePOsWithRCTotals(_tmpWk.StartDate, _tmpWk.EndDate, Request.Params["store"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }
                    else if (fp > 0 && fy > 0 && detailType == "fiscal_period")
                    {
                        POReports.FiscalPeriod _tmpWk = POReports.Factory.GetFiscalPeriod(fy, fp);
                        returnData.data = POReports.Factory.GetStorePOsWithRCTotals(_tmpWk.StartDate, _tmpWk.EndDate, Request.Params["store"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Fiscal value cannot be 0";
                    }

                    break;
                case "get_rc_trends":
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                    }
                    catch { }

                    if (fy > 0)
                    {
                        returnData.data = POReports.Factory.GetRCPOTrends(fy, Request.Params["rc"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }                    
                    else
                    {
                        returnData.msg = "ERROR: Fiscal value cannot be 0";
                    }

                    break;
                case "get_rc_detail":
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                        fp = Convert.ToInt32(Request.Params["fp"].ToString());
                        fw = Convert.ToInt32(Request.Params["fw"].ToString());
                        detailType = Request.Params["type"];
                    }
                    catch { }

                    if (fp > 0 && fw > 0 && fy > 0 && detailType == "fiscal_week")
                    {
                        POReports.FiscalWeek _tmpWk = POReports.Factory.GetFiscalWeek(fy, fp, fw);
                        returnData.data = POReports.Factory.GetRCPOs(_tmpWk.StartDate, _tmpWk.EndDate, Request.Params["rc"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }
                    else if (fp > 0 && fy > 0 && detailType == "fiscal_period")
                    {
                        POReports.FiscalPeriod _tmpWk = POReports.Factory.GetFiscalPeriod(fy, fp);
                        returnData.data = POReports.Factory.GetRCPOs(_tmpWk.StartDate, _tmpWk.EndDate, Request.Params["rc"].ToString(), Request.Params["region"]);
                        returnData.msg = "OK";
                    }
                    else
                    {
                        returnData.msg = "ERROR: Fiscal value cannot be 0";
                    }

                    break;
                case "get_fp_detail":
                    string fpDetailType = Request.Params["type"].ToString();
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                        fp = Convert.ToInt32(Request.Params["fp"].ToString());

                        if (fp > 0 && fy > 0)
                        {
                            POReports.FiscalPeriodRowData fprd = new POReports.FiscalPeriodRowData(fy, fp, fpDetailType, Request.Params["region"]);
                            fprd.init();
                            returnData.data = fprd;
                            returnData.msg = "OK";
                        }
                        else
                        {
                            returnData.msg = "ERROR: Fiscal value cannot be 0";
                        }
                    }
                    catch (Exception ex)
                    {
                        returnData.msg = "ERROR: " + ex.Message.ToString();
                    }
                    break;
                case "get_fw_detail":
                    string fwDetailType = Request.Params["type"].ToString();
                    try
                    {
                        fy = Convert.ToInt32(Request.Params["fy"].ToString());
                        fp = Convert.ToInt32(Request.Params["fp"].ToString());
                        fw = Convert.ToInt32(Request.Params["fw"].ToString());
                        
                        if (fp > 0 && fw > 0 && fy > 0)
                        {
                            POReports.FiscalWeekRowData fwrd = new POReports.FiscalWeekRowData(fy, fp, fw, fwDetailType, Request.Params["region"]);
                            fwrd.init();
                            returnData.data = fwrd;
                            returnData.msg = "OK";
                        }
                        else
                        {
                            returnData.msg = "ERROR: Fiscal value cannot be 0";
                        }
                    }
                    catch(Exception ex)                    
                    {
                        returnData.msg = "ERROR: " + ex.Message.ToString();
                    }                    
                    break;
                case "get_global_data":
                    returnData.data = POReports.Factory.GetGlobalData(Request.Params["year"].ToString());
                    returnData.msg = "OK";
                    break;
                case "get_fiscal_periods":
                    returnData.data = POReports.Factory.GetAllFiscalPeriodData(Request.Params["year"].ToString(), Request.Params["region"]);
                    returnData.msg = "OK";
                    break;
                case "get_fiscal_weeks":
                    returnData.data = POReports.Factory.GetFiscalWeekData(Request.Params["year"].ToString(), Request.Params["region"]);
                    returnData.msg = "OK";
                    break;
                case "login":
                    string RETURN_URL = "";

                    try { RETURN_URL = Request.Cookies["ReturnUrl"].Value.ToString(); }
                    catch { RETURN_URL = "Default.aspx"; }
                    POReports.ActiveDirectory ad = new POReports.ActiveDirectory();
                    if (ad.Authenticate(Request.Params["u"], Request.Params["p"]))
                    {
                        if (POReports.Factory.isAuthorized(ad.GUID.ToString()))
                        {
                            HttpContext.Current.Session["USER_ID"] = ad.GUID.ToString();
                            FormsAuthentication.SetAuthCookie(ad.SAMAccountName, false);

                            returnData.data = RETURN_URL;
                            returnData.msg = "OK";
                        }
                        else
                        {
                            returnData.msg = "You are not authorized to access this site.";
                        }
                        
                    }
                    else
                    {
                        returnData.msg = "ERROR: Invalid username or password";
                    }
                    break;
                default:
                    break;
            }
        }

        returnData.result = true;

        // Encode return object to JSON and write to page
        Response.Write(JsonConvert.SerializeObject(returnData));
    }

    public string ParseRegion(string str)
    {
        string region = "";
        string[] arr = str.Split('(');
        if (arr.Length == 2)
        {
            region = arr[1].Substring(0, 2);
        }
        return region;
    }
}

