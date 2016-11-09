using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.ExclusionGenerators;
using Icon.Framework;

namespace Icon.Esb.EwicAplListener.Transactions
{
    public abstract class ExclusionGeneratorTransactionBase : IExclusionGenerator
    {
        protected IExclusionGenerator generator;
        protected IRenewableContext<IconContext> globalContext;

        public ExclusionGeneratorTransactionBase(IExclusionGenerator generator, IRenewableContext<IconContext> globalContext)
        {
            this.generator = generator;
            this.globalContext = globalContext;
        }

        public abstract void GenerateExclusions(EwicItemModel item);
    }
}
