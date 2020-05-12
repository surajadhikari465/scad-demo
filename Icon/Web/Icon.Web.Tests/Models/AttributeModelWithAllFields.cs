using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Models;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Models
{
    public class AttributeModelWithAllFields
    {
        public List<AttributeViewModel> Attributes;
        public List<ItemColumnOrderModel> OrderOfFields;
    }
}