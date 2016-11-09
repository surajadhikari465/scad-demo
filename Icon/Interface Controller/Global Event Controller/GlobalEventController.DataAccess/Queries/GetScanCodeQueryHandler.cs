using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetScanCodeQueryHandler : IQueryHandler<GetScanCodeQuery, List<ScanCode>>
    {
        private readonly IconContext context;

        public GetScanCodeQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public List<ScanCode> Handle(GetScanCodeQuery parameters)
        {
            List<ScanCode> scanCodes = context.ScanCode
                .AsNoTracking()
                .Include(sc => sc.Item.ItemHierarchyClass.Select(ihc => ihc.HierarchyClass.Hierarchy))
                .Include(sc => sc.Item.ItemHierarchyClass.Select(ihc => ihc.HierarchyClass.HierarchyClassTrait.Select(hct => hct.Trait)))
                .Include(sc => sc.Item.ItemTrait.Select(it => it.Trait))
                .Where(sc => parameters.ScanCodes.Contains(sc.scanCode))
                .ToList();

            if (scanCodes.Count == 0)
            {
                throw new ArgumentException(String.Format("Scan codes {0} could not be found in Icon.", parameters.ScanCodes));
            }
            else
            {
                return scanCodes;
            }
        }
    }
}
