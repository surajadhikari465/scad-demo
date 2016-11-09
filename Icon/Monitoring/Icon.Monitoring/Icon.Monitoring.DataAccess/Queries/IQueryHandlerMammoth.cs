using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Queries
{
    public interface IQueryHandlerMammoth<TParameters, TResult> where TParameters : IQuery<TResult>
    {
        TResult Search(TParameters parameters);
    }
}
