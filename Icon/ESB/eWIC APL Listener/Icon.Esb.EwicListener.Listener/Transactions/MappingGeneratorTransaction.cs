using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.MappingGenerators;
using Icon.Framework;
using System;

namespace Icon.Esb.EwicAplListener.Transactions
{
    public class MappingGeneratorTransaction : MappingGeneratorTransactionBase
    {
        public MappingGeneratorTransaction(IMappingGenerator generator, IRenewableContext<IconContext> context) : base(generator, context) { }

        public override void GenerateMappings(EwicItemModel item)
        {
            using (var transaction = globalContext.Context.Database.BeginTransaction())
            {
                try
                {
                    generator.GenerateMappings(item);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
