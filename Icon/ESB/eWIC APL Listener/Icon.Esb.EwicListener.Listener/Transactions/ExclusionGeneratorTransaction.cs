using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.ExclusionGenerators;
using Icon.Framework;
using System;

namespace Icon.Esb.EwicAplListener.Transactions
{
    public class ExclusionGeneratorTransaction : ExclusionGeneratorTransactionBase
    {
        public ExclusionGeneratorTransaction(IExclusionGenerator generator, IRenewableContext<IconContext> globalContext) : base(generator, globalContext) { }

        public override void GenerateExclusions(EwicItemModel item)
        {
            using (var transaction = globalContext.Context.Database.BeginTransaction())
            {
                try
                {
                    generator.GenerateExclusions(item);
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
