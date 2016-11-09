using Infor.Services.NewItem.Models;
using System;
using System.Collections.Generic;

namespace Infor.Services.NewItem.Cache
{
    public interface IIconCache
    {
        Dictionary<string, int> TaxClassCodesToIdDictionary { get; }
        Dictionary<string, int> NationalClassCodesToIdDictionary { get; }
        Dictionary<int, string> BrandIdToAbbreviationDictionary { get; }
        Dictionary<int, BrandModel> BrandDictionary { get; }
        Dictionary<string, TaxClassModel> TaxDictionary { get; }
        Dictionary<string, NationalClassModel> NationalClassModels { get; }
        Dictionary<string, SubTeamModel> SubTeamModels { get; }
    }
}