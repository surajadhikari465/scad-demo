using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Models
{
    public class AttributePickListCharacterSetModel
    {
        public AttributeModel AttributeModel { get; set; }
        public List<PickListModel> PickList { get; set; }
        public List<AttributeCharacterSetModel> AttributeCharacterSetList { get; set; }

    }
}