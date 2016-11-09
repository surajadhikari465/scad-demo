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
    public class GetValidatedItemsQueryHandler : IQueryHandler<GetValidatedItemsQuery, Dictionary<string, int>>
    {
        private IconContext context;
        public GetValidatedItemsQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public Dictionary<string, int> Execute(GetValidatedItemsQuery parameters)
        {
            var items = (from i in context.Item
                        join it in context.ItemTrait on i.itemID equals it.itemID
                        join sc in context.ScanCode.AsQueryable<ScanCode>() on i.itemID equals sc.itemID
                        join t in context.Trait on it.traitID equals t.traitID
                        where parameters.identifiers.Contains(sc.scanCode) && t.traitCode == TraitCodes.ValidationDate

                         select new
                         {
                             itemID = i.itemID,
                             scanCode = i.ScanCode.Select(s => s.scanCode).FirstOrDefault()
                         }).ToList();

            var mapping = new Dictionary<string, int>();
            foreach (var item in items)
            {
                mapping.Add(item.scanCode, item.itemID);
            }

            return mapping;
        }
    }
}
