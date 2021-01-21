using IrmaMobile.Domain.Models;
using IrmaMobile.Logging;
using IrmaMobile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IrmaMobile.Controllers
{
    [Route("api/{region}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IIrmaMobileService service;
        private readonly ILogger<UsersController> logger;
        private readonly bool enableAuthentication;
        private readonly string authenticationServiceUrl;

        public UsersController(IIrmaMobileService service, ILogger<UsersController> logger, IOptions<AppConfiguration> options)
        {
            this.service = service;
            this.logger = logger;
            this.enableAuthentication = options.Value.EnableAuthentication;
            this.authenticationServiceUrl = options.Value.AuthenticationServiceUrl;
        }

        public async Task<UserModel> Get([FromRoute]string region, [FromQuery] string userName)
        {
            logger.LogInformation(LoggingEvents.ApiStarted, $"Executing: {nameof(UsersController)}.{nameof(Get)}");

            if (enableAuthentication)
            {
                Microsoft.Extensions.Primitives.StringValues headerValues;

                UserModel blankIrmaUser = new UserModel();

                if (Request.Headers.TryGetValue("Authorization", out headerValues) && headerValues != "undefined")
                {
                    string accessToken = headerValues.First();
                    var userAttribute = await GetUserWithTokenAsync(accessToken);

                    if (userAttribute != null)
                    {
                        JObject wfmUser = JObject.Parse(userAttribute);

                        if (wfmUser["error"] == null)
                        {
                            var irmaUser = await service.GetUser(region, userName);

                            if (irmaUser != null)
                            {
                                if (wfmUser["onPremisesSamAccountName"].ToString() == irmaUser.UserName)
                                {
                                    return irmaUser;
                                }
                                else
                                {
                                    irmaUser.Error = "User AD profile does not match IRMA user profile.";
                                    logger.LogInformation(String.Format("User AD profile does not match IRMA user profile for user {1} in {0} region. ", region, userName));
                                    return irmaUser;
                                }
                            }
                            else
                            { 
                                blankIrmaUser.Error = "IRMA User does not exist.";
                            }
                        }
                        else
                        {
                            JObject wfmUserError = JObject.Parse(wfmUser["error"].ToString());
                            blankIrmaUser.Error = wfmUserError["message"].ToString();
                            logger.LogInformation(String.Format("User {1} in {0} region encountered this login issue: ", region, userName, blankIrmaUser.Error));    
                        }
                    }
                    else
                    {
                        blankIrmaUser.Error = "User profile failed to be retrieved from Graph API";
                    }
                }
                else
                {
                    //blankIrmaUser.Error = "No Authorization token in request header.";
                    //logger.LogError(String.Format("No Authorization token in request header for user {1} in {0} region.", region, userName));
                    logger.LogInformation(String.Format("User {1} in {0} region tried to log in with the old-style user token: ", region, userName, blankIrmaUser.Error));
                    return await service.GetUser(region, userName);
                }

                return blankIrmaUser;
            }
            else
            { 
                return await service.GetUser(region, userName);
            }
        }

        private async Task<string> GetUserWithTokenAsync(string token)
        {
            try
            {
                //get data from API
                HttpClient client = new HttpClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, authenticationServiceUrl);
                message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.SendAsync(message);
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch (Exception ex)
            {
                logger.LogError("[GetUserWithTokenAsync] API call to graph for user profile failed: : " + ex.Message);
                return null;
            }
        }

    }
}