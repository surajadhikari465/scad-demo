using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.Service.Services
{
    public interface IQueryService<TRequest, TResult> 
    {
        TResult Get(TRequest request);
    }
}
