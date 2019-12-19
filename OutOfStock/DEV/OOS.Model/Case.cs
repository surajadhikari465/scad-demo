using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class Case : ICase
    {
        private IProduct product;

        public Case(IProduct product)
        {
            this.product = product;
        }

        public int Size
        {
            get
            {
                try
                {
                    return Convert.ToInt32(product.Size);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

    }
}
