using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Cache
{
    public interface IHierarchyClassCache
    {
        void Initialize();
        Dictionary<string, int> GetTaxDictionary();
    }
}
