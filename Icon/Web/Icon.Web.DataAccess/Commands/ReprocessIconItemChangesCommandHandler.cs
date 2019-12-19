using Icon.Common.DataAccess;
using Irma.Framework;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessIconItemChangesCommandHandler : ICommandHandler<ReprocessIconItemChangesCommand>
    {
        private IrmaContext context;

        public void Execute(ReprocessIconItemChangesCommand data)
        {
            if (String.IsNullOrWhiteSpace(data.RegionalConnectionStringName))
            {
                throw new ArgumentException("Regional connection string cannot be null, empty, or whitespace.", "RegionalConnectionStringName");
            }

            using (context = new IrmaContext(data.RegionalConnectionStringName))
            {
                var failedItemUpdates = context.IconItemChangeQueue.
                    Where(ic => data.IconItemChangeIds.Contains(ic.QID));

                foreach (var failedItemUpdate in failedItemUpdates)
                {
                    failedItemUpdate.ProcessFailedDate = null;
                }

                context.SaveChanges();
            }
        }
    }
}
