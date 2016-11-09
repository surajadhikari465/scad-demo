using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Core.Wrappers
{
    public class DisposableWrapper<T> : IDisposableWrapper<T>
        where T : IDisposable
    {
        public T Entity { get; set; }

        public DisposableWrapper(T entity)
        {
            Entity = entity;
        }

        public void Dispose()
        {
            Entity.Dispose();
        }
    }
}
