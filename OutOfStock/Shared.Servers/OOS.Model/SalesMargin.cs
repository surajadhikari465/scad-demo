using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class SalesMargin
    {
        private IRetailItem item;

        public SalesMargin(IRetailItem item)
        {
            this.item = item;
        }

        public double Margin()
        {
            var price = item.Price;
            var unitCost = new Cost(item).UnitCost;
            return (price == 0) ? 0 : (price - unitCost)/price;
        }

    }
}
