using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedKernel;

namespace OOS.Model
{
    public class Sales
    {
        private IRetailItem item;

        public Sales(IRetailItem item)
        {
            this.item = item;
        }

        public double For(int movement)
        {
            return Math.Round(item.Price * movement, 2);
        }
    }
}
