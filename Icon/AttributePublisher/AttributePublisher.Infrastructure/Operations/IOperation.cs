using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.Infrastructure.Operations
{
    public interface IOperation<TParameters>
    {
        void Execute(TParameters parameters);
    }
}
