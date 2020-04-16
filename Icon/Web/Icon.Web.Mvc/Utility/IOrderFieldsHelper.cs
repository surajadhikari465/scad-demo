using Icon.Web.Mvc.Models;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Utility
{
    public interface IOrderFieldsHelper
    {
        Dictionary<string, string> OrderAllFields(List<AttributeViewModel> attributeViewModels);
    }
}