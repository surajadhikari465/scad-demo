using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributePublisher.Infrastructure.Operations
{
    public abstract class OperationBase<TParameters> : IOperation<TParameters>
    {
        protected IOperation<TParameters> next;

        public OperationBase(IOperation<TParameters> next)
        {
            this.next = next;
        }

        public void Execute(TParameters parameters)
        {
            ExecuteImplementation(parameters);

            if (next != null)
            {
                next.Execute(parameters);
            }
        }

        protected abstract void ExecuteImplementation(TParameters parameters);
    }
}
