using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.Wrappers
{
    public interface IDisposableWrapper<T> : IDisposable 
        where T : IDisposable
    {
        T Entity { get; set; }
    }
}
