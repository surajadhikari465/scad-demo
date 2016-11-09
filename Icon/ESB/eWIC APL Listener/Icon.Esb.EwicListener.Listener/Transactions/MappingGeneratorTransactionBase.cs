using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.MappingGenerators;
using Icon.Framework;

namespace Icon.Esb.EwicAplListener.Transactions
{
    public abstract class MappingGeneratorTransactionBase : IMappingGenerator
    {
        protected IMappingGenerator generator;
        protected IRenewableContext<IconContext> globalContext;

        public MappingGeneratorTransactionBase(IMappingGenerator generator, IRenewableContext<IconContext> globalContext)
        {
            this.generator = generator;
            this.globalContext = globalContext;
        }

        public abstract void GenerateMappings(EwicItemModel item);
    }
}
