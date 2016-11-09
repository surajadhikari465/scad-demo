using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.Filters
{
    public interface IFilter<TParameters>
    {
        void Execute(TParameters parameters);
        IFilter<TParameters> AddFilter(IFilter<TParameters> filter);
    }
}
