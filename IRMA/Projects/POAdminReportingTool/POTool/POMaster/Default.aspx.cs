using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Default : System.Web.UI.Page
{
    public string CurrentFiscalYear = "";
    public string CurrentFiscalWeek = "";    
    public string USER_STRING = "";
    public string CurrentRegion = "";
    public string CurrentRegionName = "";
    public string SUPPORT_EMAIL = "";
    public string YEAR_OPTIONS = "";
    public string FP_OPTIONS = "";
    public int START_YEAR = 0;
    public string SERVER_NAME = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        // Setup the session object
        POReports.POSession bb = new POReports.POSession(HttpContext.Current);
        SUPPORT_EMAIL = ConfigurationManager.AppSettings["SUPPORT_EMAIL"].ToString();
        SERVER_NAME = System.Environment.MachineName.ToString();

        int[] YearRange = POReports.Factory.GetYearDataRange();
        try { START_YEAR = YearRange[YearRange.Length - 1]; }
        catch { START_YEAR = Convert.ToInt32(DateTime.Now.ToString("yyyy")); }
        for (int i = YearRange[0]; i <= YearRange[1]; i++)
        {
            string selected = "";
            if (START_YEAR == i)
            {
                selected = " selected=\"selected\"";
            }
            YEAR_OPTIONS += string.Format("<option value=\"{0}\" {1}>{0}</option>", i, selected);
        }

        for (int i = 1; i < 14; i++)
        {
            FP_OPTIONS += string.Format("<option value=\"{0}\">{0}</option>", i);
        }

        // Check if the session contains a valid login
        if (bb.isLoggedIn)
        {
            POReports.PORegion Region = POReports.Factory.GetRegion(bb.CurrentUser.Region);
            CurrentRegion = bb.CurrentUser.Region;            
            CurrentRegionName = Region.Name;
            CurrentFiscalYear = String.Format("{0}", bb.FPData.CurrentPeriod.Year);

            if (!String.IsNullOrEmpty(Request.Params["y"]))
            {
                CurrentFiscalYear = Request.Params["y"];
            }

            CurrentFiscalWeek = string.Format("Current: FY{0} FP{1} FW{2}", bb.FPData.CurrentWeek.Year, bb.FPData.CurrentWeek.Period, bb.FPData.CurrentWeek.Week);

            // Check if authorized
            if (!POReports.Factory.isAuthorized(bb.CurrentUser.UserID))
            {
                Response.Redirect("Login.aspx?A=not_authorized&type=form");
            }
            else
            {
                USER_STRING = bb.CurrentUser.DisplayName;
            }
        }
        else
        {
            // Redirect to the login form
            Response.Redirect("~/Login.aspx?ref=" + HttpContext.Current.Request.Url.AbsoluteUri);
        }
    }
}
