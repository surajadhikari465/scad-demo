using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class DeleteContactCommand
    {
        public string UserName { get; set; }
        public List<int> ContactIds { get; set; } 
    }
}
