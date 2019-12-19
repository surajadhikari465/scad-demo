using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class Cost
    {
        private IRetailItem item;

        public Cost(IRetailItem item)
        {
            this.item = item;
        }

        public double For(int movement)
        {
            return Math.Round(UnitCost * movement, 2);
        }

        public double UnitCost
        {
            get
            {
                var size = new Case(item).Size;
                if (size == 0) return item.Cost;
                return item.Cost/size;
            }
        }
    }
}
