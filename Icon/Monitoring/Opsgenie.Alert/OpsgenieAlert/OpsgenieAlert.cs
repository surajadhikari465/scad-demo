using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace OpsgenieAlert
{
    public class OpsgenieAlert: IOpsgenieAlert
    {
        public OpsgenieResponse CreateOpsgenieAlert(string message, string description ="", Dictionary<string,
                                        string> details = null, string api = "", string url = "")
        {
            OpsgenieResponse opsGenieResponse = new OpsgenieResponse();

            if (details == null)
                details = new Dictionary<string, string>();

            // Serialize the data to JSON
            var postData = new
            {
                apiKey = api,
                message = message,
                details = details,
                description = description
            };

            var json = JsonConvert.SerializeObject(postData);

            // Set up a client
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("Authorization", $"GenieKey {api}");
         
            try
            {
                var response = client.UploadString(url, json);
                opsGenieResponse = JsonConvert.DeserializeObject<OpsgenieResponse>(response);
                return opsGenieResponse;
            }
            catch (WebException wex)
            {
                using (var stream = wex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    // OpsGenie returns JSON responses for errors
                    var deserializedResponse = JsonConvert.DeserializeObject<IDictionary<string, object>>(reader.ReadToEnd());
                    opsGenieResponse.Error = deserializedResponse["error"].ToString(); ;
                }
                return opsGenieResponse;
            }
           
        }
    }
}