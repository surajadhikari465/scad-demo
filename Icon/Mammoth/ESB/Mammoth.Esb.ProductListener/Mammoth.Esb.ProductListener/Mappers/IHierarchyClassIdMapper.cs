using Mammoth.Esb.ProductListener.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Esb.ProductListener.Mappers
{
    public interface IHierarchyClassIdMapper
    {
        void PopulateHierarchyClassDatabaseIds(IEnumerable<GlobalAttributesModel> products);
    }
}
