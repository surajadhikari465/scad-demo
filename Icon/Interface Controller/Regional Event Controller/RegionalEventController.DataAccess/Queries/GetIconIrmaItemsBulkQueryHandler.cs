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
    public class GetIconIrmaItemsBulkQueryHandler : IQueryHandler<GetIconIrmaItemsBulkQuery, Dictionary<string, int>>
    {
        private IconContext context;
        public GetIconIrmaItemsBulkQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public Dictionary<string, int> Execute(GetIconIrmaItemsBulkQuery parameters)
        {
            var iconItems = context.ScanCode.Where(sc => parameters.identifiers.Contains(sc.scanCode))
                .Select(i => new {i.itemID, i.scanCode})
                .ToDictionary(i => i.scanCode, i => i.itemID);

            return iconItems;

        }
    }
}
