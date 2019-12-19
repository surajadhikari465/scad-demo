using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public interface IProductRepository
    {
        IProduct For(string upc);
        IEnumerable<IProduct> For(IEnumerable<string> upcs);
    }

}
