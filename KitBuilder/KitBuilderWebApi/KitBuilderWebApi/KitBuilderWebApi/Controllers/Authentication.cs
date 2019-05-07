using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace KitBuilderWebApi.Controllers
{
    [Route("api/Authentication")]
    public class Authentication : Controller
    {

        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginParameters parameters)
        {
            bool isValid = false;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "WFM"))
            {
                // validate the credentials
                 isValid = pc.ValidateCredentials(parameters.Username, parameters.Password);
            }


            return Ok(isValid); 
          

        }

    }

    public class LoginParameters
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
