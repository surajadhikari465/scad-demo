using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetItemByScanCodeQuery : IQueryHandler<GetItemByScanCodeParameters, Item>
    {
        private ILogger<GetItemByScanCodeQuery> logger;
        private IRenewableContext<IconContext> globalContext;

        public GetItemByScanCodeQuery(
            ILogger<GetItemByScanCodeQuery> logger,
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public Item Search(GetItemByScanCodeParameters parameters)
        {
            var scanCode = globalContext.Context.ScanCode
                .Include(sc => sc.Item.ItemType)
                .SingleOrDefault(sc => sc.scanCode == parameters.ScanCode);

            if (scanCode == null)
            {
                logger.Error(String.Format("Failed to find an Item record for ScanCode {0}.", parameters.ScanCode));
                throw new ArgumentException(String.Format("No item records match scan code {0}.", parameters.ScanCode));
            }
            else
            {
                return scanCode.Item;
            }
        }
    }
}
