using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Managers
{
    public class AddAttributeManager
    {
        public AttributeModel Attribute { get; set; }
        public List<CharacterSetModel> CharacterSetModelList { get; set; }
        public List<PickListModel> PickListModel { get; set; }
    }
}