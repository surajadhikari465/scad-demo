using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.Mvc.Utility
{
    public interface IAttributesHelper
    {
        string CreateCharacterSetRegexPattern(int dataTypeId, List<CharacterSetModel> characterSets, string specialCharacters);
    }
}