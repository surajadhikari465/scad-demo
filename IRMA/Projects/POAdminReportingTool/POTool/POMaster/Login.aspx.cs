using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

public partial class Default : System.Web.UI.Page
{
    public string MESSAGE = "";
    public string SUPPORT_EMAIL = "";
    public string SERVER_NAME = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        POReports.POSession bb = new POReports.POSession(HttpContext.Current);
        SUPPORT_EMAIL = ConfigurationManager.AppSettings["SUPPORT_EMAIL"].ToString();
        SERVER_NAME = System.Environment.MachineName.ToString();

        string Action = "";
        try { Action = Request.Params["A"].ToString(); }
        catch { }

        if (Action == "logout")
        {
            bb.logout();

            // Logout Proceedure
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            //Response.Redirect("Login.aspx");
        }        
        else if (Action == "not_authorized")
        {
            MESSAGE = "<p class=\"error center\">You are not authorized to access this site.</p>";
        }
    }
}
