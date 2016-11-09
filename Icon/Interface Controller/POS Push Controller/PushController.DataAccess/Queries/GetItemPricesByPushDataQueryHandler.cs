using Icon.RenewableContext;
using Icon.Framework;
using MoreLinq;
using PushController.Common;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetItemPricesByPushDataQueryHandler : IQueryHandler<GetItemPricesByPushDataQuery, List<ItemPriceModel>>
    {
        private IRenewableContext<IconContext> context;

        public GetItemPricesByPushDataQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public List<ItemPriceModel> Execute(GetItemPricesByPushDataQuery parameters)
        {
            SqlParameter irmaPushRows = new SqlParameter("IrmaPushRows", SqlDbType.Structured);
            irmaPushRows.TypeName = "app.IrmaPushType";
            irmaPushRows.Value = parameters.IrmaPushList.ConvertAll(p => new
                {
                    RegionCode = p.RegionCode,
                    BusinessUnitId = p.BusinessUnit_ID,
                    Identifier = p.Identifier,
                    ChangeType = p.ChangeType,
                    InsertDate = p.InsertDate,
                    RetailSize = p.RetailSize,
                    RetailPackageUom = p.RetailPackageUom,
                    TmDiscountEligible = p.TMDiscountEligible,
                    CaseDiscount = p.Case_Discount,
                    AgeCode = p.AgeCode,
                    Recall = p.Recall_Flag,
                    RestrictedHours = p.Restricted_Hours,
                    SoldByWeight = p.Sold_By_Weight,
                    ScaleForcedTare = p.ScaleForcedTare,
                    QuantityRequired = p.Quantity_Required,
                    PriceRequired = p.Price_Required,
                    QuantityProhibit = p.Quantity_Required,
                    VisualVerify = p.VisualVerify,
                    RestrictSale = p.RestrictSale,
                    PosScaleTare = p.PosScaleTare,
                    LinkedIdentifier = p.LinkedIdentifier,
                    Price = p.Price,
                    RetailUom = p.RetailUom,
                    Multiple = p.Multiple,
                    SaleMultiple = p.SaleMultiple,
                    SalePrice = p.Sale_Price,
                    SaleStartDate = p.Sale_Start_Date,
                    SaleEndDate = p.Sale_End_Date,
                    InProcessBy = p.InProcessBy,
                    InUdmDate = p.InUdmDate,
                    EsbReadyDate = p.EsbReadyDate,
                    UdmFailedDate = p.UdmFailedDate,
                    EsbReadyFailedDate = p.EsbReadyFailedDate
                })
                .ToDataTable();

            string sql = "EXEC app.GetItemPrices @IrmaPushRows";

            var queryResults = context.Context.Database.SqlQuery<ItemPriceModel>(sql, irmaPushRows).ToList();

            return queryResults;
        }
    }
}
