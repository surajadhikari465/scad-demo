using System;
using System.Reflection;
using System.Data;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Web.Script.Serialization;

public partial class ajax_KeepAlive : System.Web.UI.Page
{
    public string JSONData;

    protected void Page_Load(object sender, EventArgs e)
    {
        JSONData = "OK";
        POReports.POSession bb = new POReports.POSession(HttpContext.Current);
    }
}

