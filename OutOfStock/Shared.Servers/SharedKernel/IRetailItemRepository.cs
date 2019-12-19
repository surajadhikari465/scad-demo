using System.Collections.Generic;

namespace SharedKernel
{
    public interface IRetailItemRepository
    {
        IRetailItem For(string upc, string storeAbbrev);
        List<IRetailItem> For(IEnumerable<string> upcs, string storeAbbrev);
    }
}
