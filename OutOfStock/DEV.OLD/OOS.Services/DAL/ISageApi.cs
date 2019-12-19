using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using NLog;
using OOS.Services.DataModels;

namespace OOS.Services.DAL
{
    public interface ISageApi
    {
        List<SageStore> GetModifiedStores();
    }

    public class SageApi : ISageApi
    {

        public SageApi()
        {
            GetResponseToken();
        }

        public string TokenResponse { get; set; }
        private const string PublicKey = "pGsFJU9kYGeOrSLsRZfw7ekFsreZ6u72";
        private const string Secret = "hIuu47QQUBwGWFtr";
        private const string AuthUrl = "https://api.wholefoodsmarket.com/v2/oauth20/";
        private const string StoreUrl = "https://api.wholefoodsmarket.com/v2/stores";

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public List<SageStore> GetModifiedStores()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timeUnix =
                (Convert.ToInt64(((DateTime.Now).AddDays(-7) - epoch).TotalSeconds)).ToString(
                    CultureInfo.InvariantCulture);

            var query = String.Format("{0}?access_token={1}&search=modified_since&timestamp={2}&limit=1000", StoreUrl, TokenResponse,
                timeUnix);
            var request = (HttpWebRequest)WebRequest.Create(query);
            try
            {
                var response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var stuff = reader.ReadToEnd();
                    //Console.Write(stuff);
                    var jsSerializer = new JavaScriptSerializer();

                    var data= jsSerializer.Deserialize<SageStore[]>(stuff).ToList();
                    foreach (var sageStore in data)
                    {
                        if (sageStore.region.ToLower(CultureInfo.InvariantCulture) == "uk") sageStore.region = "EU";
                    }
                    return data;
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var errorText = reader.ReadToEnd();
                    _logger.Error(errorText);
                }
                throw;
            }
        }


        private void GetResponseToken()
        {
            var tokenURL = String.Format("{0}token?client_id={1}&grant_type=client_credentials&client_secret={2}&response_content_type=application/json", AuthUrl, PublicKey, Secret);
            var tokenRequest = (HttpWebRequest)WebRequest.Create(tokenURL);
            try
            {
                var response = tokenRequest.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);

                    XElement xToken = XElement.Parse(reader.ReadToEnd());
                    if (xToken != null)
                    {
                        TokenResponse = xToken.Element("access_token").Element("token").Value;
                    }
                }
            }
            catch (WebException ex)
            {
                var errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    var errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
    }
}
