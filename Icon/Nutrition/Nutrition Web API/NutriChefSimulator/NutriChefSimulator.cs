using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NutritionWebApi.Common.Models;
using System.Net;
using System.Web.Script.Serialization;

using System.Security.Principal;
using System.Runtime.InteropServices;

namespace NutriChefSimulator
{
    public partial class NutriChefSimulator : Form
    {
        //Dev server
        string devNutritionUrl = "http://cewd6589/Nutrition-Dev/api/Nutrition/";
        string localNutritionUrl ="http://localhost:12663/api/Nutrition/";

        //Test server settings
        string testNutritionUrl = "http://icon-nutrition-test/api/Nutrition/";

        //QA server
        string qaNutritionUrl = "http://icon-nutrition-qa/api/Nutrition/";      
        string devSlawUrl = "http://cewd6017/slaw/printbatch/";

        string devUser = "NutriconServiceDev";
        string devPasswrod = "wAb1TI*M81";
        string testUser = "NutriconServiceTest";
        string testPassword = "7M6n*YE00d";
        string qaUser = "NutriconServiceQA";
        string qaPaswword = "38Cr$0Ud2!";
        string devRegion = "Dev";
        string testRegion = "Test";
        string qaRegion = "Qa";
        public NutriChefSimulator()
        {
            InitializeComponent();
        }

        private void btnSendToICon_Click(object sender, EventArgs e)
        {
            List<NutritionItemModel> itemList = new List<NutritionItemModel>();
            NutritionItemModel nutritionItemModel = new NutritionItemModel();
            nutritionItemModel.Plu = txtBoxPlu.Text;
            nutritionItemModel.RecipeName = txtBoxRecipeName.Text;
            nutritionItemModel.HshRating = Int32.Parse(txtBoxHshRating.Text);
            nutritionItemModel.ServingsPerPortion = float.Parse(txtBoxServingsPerPortion.Text);
            nutritionItemModel.Ingredients = "KK sim Test Ingredients";
            nutritionItemModel.Allergens = "KK sim test Allergens";
            itemList.Add(nutritionItemModel);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(itemList);
            string region = devRegion;

            if (!ImpersonationUtil.Impersonate(GetUserName(region), GetPassword(region), "WFM"))
            {
                MessageBox.Show("Impersonation failed.");
                return;
            }

            //Then send the POST request to the server:
            using (var client = new WebClient())
            {
                client.UseDefaultCredentials = true;

                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                //client.Headers[HttpRequestHeader.Authorization] = "NTLM " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("WFM\\NutriconServiceDev:wAb1TI*M81"));
                //client.Headers[HttpRequestHeader.Authorization] = "NTLM " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("WFM\\NutriconServiceTest:7M6n*YE00d"));
                 try
                {
                    string result = client.UploadString(new Uri(GetUrl(region)), "POST", json);
                    
                    //SLAW
                    //json = @"{""Application"": ""application X"",""BatchName"": ""XX BatchName"",""BatchType"": 1,""BusinessUnitId"": 1,""EffectiveDate"": ""2015-10-07T00:00:00.0000000-06:00"",""Event"": ""XX Event"",""IsAdHoc"": false,""PriceChangeBatch"": 0,""SourceSystem"": ""IRMA"",""BatchItems"": [{""Identifier"": ""alpha"",""PrintOrder"": 1,""Template"": ""1alpha""}]}";

                }
                catch (Exception exception)
                {
                    Console.Write(exception.Message);
                }
            }

            //After your task, undo impersonate:
            ImpersonationUtil.UnImpersonate();
            
            //Below worked for logged user (not for no-logged user)
            //client.UseDefaultCredentials = true;            
            // client.Headers[HttpRequestHeader.Authorization] = "NTLM " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("WFM\\kasthuri.kona:wholefoods5"));             
        }

        private string GetUserName(string region)
        {
            switch(region)
            {
                case "Dev": return devUser;
                case "Test": return testUser;
                case "Qa": return qaUser;
                default: return devUser;
            }
        }

        private string GetPassword(string region)
        {
            switch (region)
            {
                case "Dev": return devPasswrod;
                case "Test": return testPassword;
                case "Qa": return qaPaswword;
                default: return devPasswrod;
            }
        }

        private string GetUrl(string region)
        {
            switch (region)
            {
                case "Dev": return devNutritionUrl;
                case "Test": return testNutritionUrl;
                case "Qa": return qaNutritionUrl;
                default: return devNutritionUrl;
            }
        }

        private void txtBoxHshRating_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtBoxServingsPerPortion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void txtBoxPlu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }








    /// <summary>
    /// Impersonate a windows logon.
    /// </summary>
    public class ImpersonationUtil
    {

        /// <summary>
        /// Impersonate given logon information.
        /// </summary>
        /// <param name="logon">Windows logon name.</param>
        /// <param name="password">password</param>
        /// <param name="domain">domain name</param>
        /// <returns></returns>
        public static bool Impersonate(string logon, string password, string
        domain)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (LogonUser(logon, domain, password, LOGON32_LOGON_INTERACTIVE,
            LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            {

                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    impersonationContext = tempWindowsIdentity.Impersonate();
                    if (null != impersonationContext) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Unimpersonate.
        /// </summary>
        public static void UnImpersonate()
        {
            impersonationContext.Undo();
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern int LogonUser(
        string lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);

        [DllImport("advapi32.dll",
        CharSet = System.Runtime.InteropServices.CharSet.Auto,
        SetLastError = true)]
        public extern static int DuplicateToken(
        IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_LOGON_NETWORK_CLEARTEXT = 4;
        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private static WindowsImpersonationContext impersonationContext;
    }
}
