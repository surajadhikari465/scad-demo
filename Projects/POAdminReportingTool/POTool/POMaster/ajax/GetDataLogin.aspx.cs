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

public partial class login_ajax_GetData : System.Web.UI.Page
{
    public string JSONData;
    public POReports.POSession bb;
    public string ActiveFP;
    public string ActiveFY;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Load Core Class
        bb = new POReports.POSession(HttpContext.Current);

        string Action = Request.Params["A"];
        AJAXResult returnData = new AJAXResult();

        // Common 
        
        switch (Action)
        {
            case "login":
                //string RETURN_URL = "";

                //try { RETURN_URL = Request.Cookies["ReturnUrl"].Value.ToString(); }
                //catch { RETURN_URL = "Default.aspx"; }
                POReports.ActiveDirectory ad = new POReports.ActiveDirectory();
                if (ad.Authenticate(Request.Params["u"], Request.Params["p"]))
                {
                    if (POReports.Factory.isAuthorized(ad.GUID.ToString()))
                    {
                        HttpContext.Current.Session["USER_ID"] = ad.GUID.ToString();
                        
                        returnData.data = "Default.aspx";
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

        returnData.result = true;
        JSONData = returnData.getJSON();
    }
}

