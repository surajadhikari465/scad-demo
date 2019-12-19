using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class LossOfSale
    {
        private IRetailItem item;
        private IProduct product;

        public LossOfSale(IRetailItem item, IProduct product)
        {
            this.item = item;
            this.product = product;
        }

        public double For(int movementProjection)
        {
            var loss = movementProjection * Case.Size * item.Price;
            return Math.Round(loss, 2);
        }

        private ICase Case
        {
            get { return new Case(product); }
        }

    }
}
