using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.Filters
{
    public class PipeLine<TParameters> : IFilter<TParameters>
    {
        private IFilter<TParameters> head;

        public IFilter<TParameters> AddFilter(IFilter<TParameters> filter)
        {
            if(head == null)
            {
                head = filter;
            }
            else
            {
                head.AddFilter(filter);
            }

            return this;
        }

        public void Execute(TParameters parameters)
        {
            if(head != null)
            {
                head.Execute(parameters);
            }
        }
    }
}
