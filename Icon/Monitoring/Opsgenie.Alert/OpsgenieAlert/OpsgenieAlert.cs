using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace OpsgenieAlert
{
    public class OpsgenieAlert: IOpsgenieAlert
    {
        public OpsgenieResponse CreateOpsgenieAlert( string api, string url, string message, string description ="",
                                                    Dictionary<string, string> details = null)
        {
            OpsgenieResponse opsGenieResponse = new OpsgenieResponse();
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            if (details == null)
                details = new Dictionary<string, string>();

            // Serialize the data to JSON
            var opsgenieRequest = new OpsgenieRequest()
            {
                ApiKey = api,
                Message = message,
                Details = details,
                Description = description
            };

            var json = JsonConvert.SerializeObject(opsgenieRequest, serializerSettings);

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