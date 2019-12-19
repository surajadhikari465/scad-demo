using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public interface IProductFactory
    {
        IEnumerable<IProduct> Reconstitute(IEnumerable<string> upcs, IEnumerable<ProductDTO> productDtos);
    }
}
