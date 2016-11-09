using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.Factories
{
    public interface IGenericFactory<TResult>
    {
        TResult Create();
    }

    public interface IGenericFactory<TResult, TParameters>
    {
        TResult Create(TParameters paramters);
    }
}
