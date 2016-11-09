using Icon.Framework;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Models;
using RegionalEventController.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Reflection;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetBrandNewScanCodesQueryHandler : IQueryHandler<GetBrandNewScanCodesQuery, List<string>>
    {
        private IconContext context;
        public GetBrandNewScanCodesQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public List<string> Execute(GetBrandNewScanCodesQuery parameters)
        {
            List<string> irmaItemsIdentifiers = (from i in context.IRMAItem
                                                 where parameters.scanCodes.Contains(i.identifier)
                                                 select i.identifier).ToList();

            List<string> iconScanCodes = (from s in context.ScanCode
                                          where parameters.scanCodes.Contains(s.scanCode)
                                          select s.scanCode).ToList();

            List<string> remainingScanCode = parameters.scanCodes.Except(irmaItemsIdentifiers).ToList();

            return remainingScanCode.Except(iconScanCodes).ToList();
        }
    }
}
