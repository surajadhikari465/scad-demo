using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.Filters
{
    public class FilterBase<TParameters> : IFilter<TParameters>
    {
        private IFilter<TParameters> next;

        public IFilter<TParameters> AddFilter(IFilter<TParameters> filter)
        {
            if (next == null)
            {
                next = filter;
            }
            else
            {
                next.AddFilter(filter);
            }

            return filter;
        }

        public virtual void Execute(TParameters parameters)
        {
            if (next != null)
            {
                next.Execute(parameters);
            }
        }
    }
}
