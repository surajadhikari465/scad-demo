using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// AJAX JSON Result Template Class
/// </summary>
public class AJAXResult
{
    public bool result;
    public string msg;
    public object data;

	public AJAXResult()
	{
        result = false;
        msg = "";
	}

    public string getJSON()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string arrayJson = serializer.Serialize(this);
        return arrayJson;
    }
}