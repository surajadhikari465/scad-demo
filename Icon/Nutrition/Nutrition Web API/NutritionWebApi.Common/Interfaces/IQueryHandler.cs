using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionWebApi.Common
{
    public interface IQueryHandler<TParameters, TResult> where TParameters : IQuery<TResult>
    {
        TResult Search(TParameters parameters);
    }
}
