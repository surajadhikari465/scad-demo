using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Commands
{
    public class AddUpdateCharacterSetCommand
    {
        public List<CharacterSetModel> CharacterSetModelList { get; set; }
        public int AttributeId { get; set; }
    }
}