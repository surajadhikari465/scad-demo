using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using KitBuilderWebApi.Models;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Authentication")]
    public class Authentication : Controller
    {
		private readonly IConfiguration configuration;
		private readonly ILogger<Authentication> logger;

		public Authentication(IConfiguration configuration, ILogger<Authentication> logger)
		{
			this.configuration = configuration;
			this.logger = logger;
		}

		[HttpPost("Login")]
        public IActionResult Login([FromBody]LoginParameters parameters)
        {
            bool isValid = false;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "WFM"))
            {
                // validate the credentials
                 isValid = pc.ValidateCredentials(parameters.Username, parameters.Password);

				if (isValid)
				{
					UserPrincipal user = UserPrincipal.FindByIdentity(pc, parameters.Username);
					string delimiter = configuration["KitBuilderADGroups:delimiter"];
					List<string> fullAccessGroups = configuration["KitBuilderADGroups:FullAccess"].Split(delimiter).ToList();

					foreach (string fullAccessGroup in fullAccessGroups)
					{
						GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, fullAccessGroup);

						if (group == null)
						{
							logger.LogInformation(@"AD group " + fullAccessGroup + " doesn't exist. User " + parameters.Username + " failed to log into Kit Builder Web application.");
						}
						else if (user.IsMemberOf(group))
						{
							return Ok("Authorized");
						}
					}
					// User is not in any of the AD groups that have full access
					logger.LogInformation(@"Unauthorized user " + parameters.Username + " failed to log into the Kit Builder Web application.");
					return Ok("Unauthorizeded");
				}
				else
				{
					logger.LogInformation(@"User " + parameters.Username + " failed to be authenticated.");
					return Ok("Unauthenticated");
				}
            }          

        }

    }
}
