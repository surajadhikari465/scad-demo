using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Mappers
{
    public interface IHierarchyClassIdMapper
    {
        void PopulateHierarchyClassDatabaseIds(IEnumerable<GlobalAttributesModel> products);
    }
}
