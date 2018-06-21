using Icon.Framework;
using Icon.Logging;
using RegionalEventController.Common;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertIrmaItemsToIconBulkCommandHandler : IBulkCommandHandler<InsertIrmaItemsToIconBulkCommand>
    {
        private ILogger<InsertIrmaItemsToIconBulkCommandHandler> logger;
        private IconContext context;

        public InsertIrmaItemsToIconBulkCommandHandler(ILogger<InsertIrmaItemsToIconBulkCommandHandler> logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(InsertIrmaItemsToIconBulkCommand command)
        {
            int returnVal = 0;
            if (command.irmaNewItems == null || command.irmaNewItems.Count == 0)
            {
                logger.Warn("InsertIrmaItemsToIconBulkCommandHandler was called with a null or empty list.  Check execution logic in the calling method.");
                return returnVal;
            }

            SqlParameter messagesParameter = new SqlParameter("IRMAItems", SqlDbType.Structured);
            messagesParameter.TypeName = "app.IRMAItemType";

            messagesParameter.Value = command.irmaNewItems.ConvertAll(i => new
            {
                RegionCode = i.regioncode,
                Identifier = i.identifier,
                DefaultIdentifier = i.defaultIdentifier,
                BrandName = i.brandName,
                ItemDescription = i.itemDescription,
                PosDescription = i.posDescription,
                PackageUnit = i.packageUnit,
                RetailSize = i.retailSize,
                RetailUom = i.retailUom,
                FoodStamp = i.foodStamp,
                PosScaleTare = i.posScaleTare,
                DepartmentSale = i.departmentSale,
                GiftCard = i.giftCard,
                TaxClassID = i.taxClassID,
                MerchandiseClassID = i.merchandiseClassID,
                IrmaSubTeamName = i.irmaSubTeamName,
                NationalClassId = i.nationalClassID
            }).ToDataTable();

            string sql = "EXEC app.InsertIRMAItems @IRMAItems ";

            returnVal = context.Database.ExecuteSqlCommand(sql, messagesParameter);

            logger.Info(String.Format("Successfully generated {0} IrmaItem entries.",
                returnVal.ToString()));

            return returnVal;
        }
    }
}
