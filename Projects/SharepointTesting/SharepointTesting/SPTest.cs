using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SharepointTesting
{
    class SPTest
    {
        public void TestIt()
        {
            IRMAdws.Dws dws = new IRMAdws.Dws();

            try
            {
                dws.Credentials = CredentialCache.DefaultCredentials;
                //dws.Url = "http://cewp1605/sites/DefaultCollection/IRMA/General%20Documents/Forms/_vti_bin/dws.asmx";
                dws.PreAuthenticate = true;
                string strResult = "";
                strResult = dws.GetDwsMetaData("General%20Documents/IRMA%20User%20Audit/FY2012_Q2/MW/10035_MN_ST_PAUL_(STP)2012-06-19T11_46_09.xlsx", null, false);
                if (IsDwsErrorResult(strResult))
                {
                    int intErrorID = 0;
                    string strErrorMsg = "";
                    ParseDwsErrorResult(strResult, out intErrorID,
                        out strErrorMsg);
                    Console.WriteLine
                        ("A document workspace error occurred.\r\n" +
                        "Error number: " + intErrorID.ToString() + "\r\n" +
                        "Error description: " + strErrorMsg);
                }
                else
                {
                    string strOutputFile = "C:\\GetDwsMetaData.xml";
                    System.IO.TextWriter tw =
                        System.IO.File.CreateText(strOutputFile);
                    tw.WriteLine(strResult);
                    tw.Close();
                    System.Diagnostics.Process.Start("iexplore.exe", "file://" + strOutputFile);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("An exception occurred.\r\n" +
                    "Description: " + exc.Message);
            }
            /*
            try
            {
                dws.Credentials = CredentialCache.DefaultNetworkCredentials;
                dws.PreAuthenticate = true;
                string result = dws.GetDwsData("General%20Documents/IRMA%20User%20Audit/FY2012_Q2/MW/10035_MN_ST_PAUL_(STP)2012-06-19T11_46_09.xlsx", null);
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
             * 
             */
        }

        private bool IsDwsErrorResult(string ResultFragment)
        {
            System.IO.StringReader srResult =
                new System.IO.StringReader(ResultFragment);
            System.Xml.XmlTextReader xtr =
                new System.Xml.XmlTextReader(srResult);
            xtr.Read();
            if (xtr.Name == "Error")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ParseDwsErrorResult(string ErrorFragment, out int ErrorID,
            out string ErrorMsg)
        {
            System.IO.StringReader srError =
                new System.IO.StringReader(ErrorFragment);
            System.Xml.XmlTextReader xtr =
                new System.Xml.XmlTextReader(srError);
            xtr.Read();
            xtr.MoveToAttribute("ID");
            xtr.ReadAttributeValue();
            ErrorID = System.Convert.ToInt32(xtr.Value);
            ErrorMsg = xtr.ReadString();
        }

        private string ParseDwsSingleResult(string ResultFragment)
        {
            System.IO.StringReader srResult =
                new System.IO.StringReader(ResultFragment);
            System.Xml.XmlTextReader xtr =
                new System.Xml.XmlTextReader(srResult);
            xtr.Read();
            return xtr.ReadString();
        }

        public XmlNode GetFolderQuery(string folder)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml("<Query><Where><And><Eq><FieldRef Name='FileLeafRef'><Value Type='Text'>" + folder + "</Value><FieldRef Name='FSObjType'/><Value Type='Text'>1</Value></Eq></And></Where></Query>");
            return xdoc;
        }
    }
}
