using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalEventController.DataAccess.Queries
{
    /// <summary>
    /// Queries the IRMA database to find the IRMA Brand corresponding with the supplied
    ///   Icon Brands Hierarchy Class ID. Joins the ItemBrand & ValidatedBrand tables
    ///   and searches using the IconBrandId parameter to find the IRMA ItemBrand which
    ///   is cross-referenced with the Icon ID parameter (if any). Returns a default (null)
    ///   object if no matching record is found.
    /// </summary>
    public class GetIrmaBrandQueryHandler : IQueryHandler<GetIrmaBrandQuery, ItemBrand>
    {
        private IrmaContext context;

        public GetIrmaBrandQueryHandler(IrmaContext context)
        {
            this.context = context;
        }
        
        public ItemBrand Handle(GetIrmaBrandQuery parameters)
        {
            var queryResult = ( from vb in context.ValidatedBrand
                join ib in context.ItemBrand on vb.IrmaBrandId equals ib.Brand_ID
                where vb.IconBrandId == parameters.IconBrandId
                select new { ItemBrand = ib, ResultItemCount = ib.Item.Count }
                ).SingleOrDefault();
            if (queryResult == null) return (ItemBrand)null;
            parameters.ResultItemCount = queryResult.ResultItemCount;
            return queryResult.ItemBrand;

            //var lambdaQueryResult = context.ValidatedBrand
            //        .Join(context.ItemBrand, vb => vb.IrmaBrandId, ib => ib.Brand_ID, (vb, ib) => new { vb.IconBrandId, ib })
            //        .Where(anonResult => anonResult.IconBrandId == parameters.IconBrandId)
            //        .Select( anonResult => new { ItemBrand = anonResult.ib, ResultItemcount = anonResult.ib.Item.Count})
            //        .SingleOrDefault()
            //if (lambdaQueryResult == null) return (ItemBrand)null; ;
            //parameters.ResultItemCount = lambdaQueryResult.ResultItemcount;
            //return lambdaQueryResult.ItemBrand;
        }
    }
}
