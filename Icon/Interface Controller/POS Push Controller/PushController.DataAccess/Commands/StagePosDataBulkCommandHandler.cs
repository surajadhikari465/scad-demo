using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Commands
{
    public class StagePosDataBulkCommandHandler : ICommandHandler<StagePosDataBulkCommand>
    {
        private ILogger<StagePosDataBulkCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public StagePosDataBulkCommandHandler(
            ILogger<StagePosDataBulkCommandHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(StagePosDataBulkCommand command)
        {
            if (command.PosData == null || command.PosData.Count == 0)
            {
                logger.Warn("StagePosDataBulkCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return;
            }

            SqlParameter pushDataParameter = new SqlParameter("IrmaPushData", SqlDbType.Structured);
            pushDataParameter.TypeName = "app.IrmaPushType";

            var pushDataForStoredProcedure = command.PosData.ConvertAll(pos => new
                {
                    RegionCode = pos.RegionCode,
                    BusinessUnitId = pos.BusinessUnit_ID,
                    Identifier = pos.Identifier,
                    ChangeType = pos.ChangeType,
                    InsertDate = DateTime.Now,
                    RetailSize = pos.RetailSize,
                    RetailPackageUom = pos.RetailPackageUom,
                    TmDiscountEligible = pos.TMDiscountEligible,
                    CaseDiscount = pos.Case_Discount,
                    AgeCode = pos.AgeCode,
                    Recall = pos.Recall_Flag,
                    RestrictedHours = pos.Restricted_Hours,
                    SoldByWeight = pos.Sold_By_Weight,
                    ScaleForcedTare = pos.ScaleForcedTare,
                    QuantityRequired = pos.Quantity_Required,
                    PriceRequired = pos.Price_Required,
                    QuantityProhibit = pos.QtyProhibit,
                    VisualVerify = pos.VisualVerify,
                    RestrictSale = pos.RestrictSale,
                    PosScaleTare = pos.PosScaleTare,
                    LinkedIdentifier = pos.LinkedIdentifier,
                    Price = pos.Price,
                    RetailUom = pos.RetailUom,
                    Multiple = pos.Multiple,
                    SaleMultiple = pos.SaleMultiple,
                    SalePrice = pos.Sale_Price,
                    SaleStartDate = pos.Sale_Start_Date,
                    SaleEndDate = pos.Sale_End_Date,
                    InProcessBy = pos.InProcessBy,
                    InUdmDate = pos.InUdmDate,
                    EsbReadyDate = pos.EsbReadyDate,
                    UdmFailedDate = pos.UdmFailedDate,
                    EsbReadyFailedDate = pos.EsbReadyFailedDate
                }).ToDataTable();

            pushDataParameter.Value = pushDataForStoredProcedure;

            string sql = "EXEC app.StageIrmaPushData @IrmaPushData";

            context.Context.Database.ExecuteSqlCommand(sql, pushDataParameter);

            logger.Info(String.Format("Successfully bulk inserted {0} IRMAPush record(s).",
                command.PosData.Count, command.PosData[0].IRMAPushID));
        }
    }
}
