using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdatePickListDataCommand
    {
        public List<PickListModel> PickListModel { get; set; }
        public int AttributeId { get; set; }
    }
}
