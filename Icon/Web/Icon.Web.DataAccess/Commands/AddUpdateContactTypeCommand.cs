using System.Collections.Generic;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdateContactTypeCommand
    {
         public List<ContactTypeModel> ContactTypes { get; set; } 
    }
}