using System.Collections.Generic;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdateContactCommand
    {
       public string UserName { get; set; }
       public List<ContactModel> Contacts { get; set; } 
    }
}
