using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Data.Entity;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetItemByScanCodeQuery : IQueryHandler<GetItemByScanCodeParameters, Item>
    {
        private ILogger<GetItemByScanCodeQuery> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetItemByScanCodeQuery(
            ILogger<GetItemByScanCodeQuery> logger,
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public Item Search(GetItemByScanCodeParameters parameters)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                var scanCode = context.ScanCode
                    .Include(sc => sc.Item.ItemType)
                    .SingleOrDefault(sc => sc.scanCode == parameters.ScanCode);

                if (scanCode == null)
                {
                    logger.Error(string.Format("Failed to find an Item record for ScanCode {0}.", parameters.ScanCode));
                    throw new ArgumentException(string.Format("No item records match scan code {0}.", parameters.ScanCode));
                }
                else
                {
                    return scanCode.Item;
                }
            }
        }
    }
}
