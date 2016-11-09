using Mammoth.Esb.ProductListener.Models;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Commands
{
    public class AddOrUpdateProductsCommand
    {
        public List<ProductModel> Products { get; set; }
    }
}