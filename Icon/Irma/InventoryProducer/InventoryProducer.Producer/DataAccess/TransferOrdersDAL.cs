using System;
using System.Collections.Generic;
using System.Linq;
using InventoryProducer.Common;
using Icon.DbContextFactory;
using Irma.Framework;
using InventoryProducer.Producer.Model.DBModel;
using System.Data.SqlClient;


namespace InventoryProducer.Producer.DataAccess
{
    // Gets TransferOrders data from IRMA DB
    public class TransferOrdersDAL: ITransferOrdersDAL
    {

        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings inventoryProducerSettings;

        private const int DB_TIMEOUT_SECONDS = 120;

        public TransferOrdersDAL(IDbContextFactory<IrmaContext> irmaContextFactory, 
            InventoryProducerSettings inventoryProducerSettings)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.inventoryProducerSettings = inventoryProducerSettings;
        }

        public IList<TransferOrdersModel> GetTransferOrders(string eventType, int keyId, int? secondaryKeyId)
        {
            if (eventType.Contains(Constants.EventType.TSF_LINE_DEL))
            {
                return this.GetLineDeleteEvent(keyId, secondaryKeyId);
            }
            else if (eventType.Contains(Constants.EventType.TSF_DEL))
            {
                return this.GetPODeleteEvent(keyId);
            }
            else if (!eventType.Contains("DEL")) // usually TSF_CRE eventTypeCode
            {
                return this.GetNonDeleteEvent(keyId);
            }
            throw new NotImplementedException($"EventTypeCode: {eventType} is not implemented");
        }

        private IList<TransferOrdersModel> GetNonDeleteEvent(int keyId)
        {
            using(var irmaContext = irmaContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;
                string getNonDeleteEventQuery = 
                    $@"SELECT
                    oh.OrderHeader_ID as OrderHeaderId,
                    fs.BusinessUnit_ID as FromLocationNumber,
                    fs.Store_Name as FromLocationName,
                    ts.BusinessUnit_ID as ToLocationNumber,
                    ts.Store_Name as ToLocationName,
                    fst.SubDept_No as FromSubTeamNumber,
                    fst.SubTeam_Name as FromSubTeamName,
                    tst.SubDept_No as ToSubTeamNumber,
                    tst.SubTeam_Name as ToSubTeamName,
                    Case
                    When oh.RefuseReceivingReasonID is not null Then 'Refused'
                    When oh.OriginalCloseDate is not null and oh.RefuseReceivingReasonID is null Then 'Closed'
                    Else 'Sent'
                    End as Status,
                    oh.CreatedBy as UserId,
                    uc.UserName as UserName,
                    oh.ApprovedBy as ApproverId,
                    ua.UserName as ApproverName,
                    IsNull(oh.SentDate, oh.OrderDate) as CreateDateTime,
                    oh.ApprovedDate as ApproveDateTime,
                    oi.OrderItem_ID as TransferOrderDetailNumber,
                    oi.Item_Key as SourceItemKey,
                    ii.Identifier as DefaultScanCode,
                    isb.SubDept_No as HostSubTeamNumber,
                    isb.SubTeam_Name as HostSubTeamName,
                    oi.QuantityOrdered,
                    iuo.Unit_Name as OrderedUnit,
                    iuo.Unit_Abbreviation as OrderedUnitCode,
                    oh.Expected_Date as ExpectedArrivalDate,
                    oi.package_desc1 as PackageDesc1,
                    oi.package_desc2 as PackageDesc2
                    FROM OrderHeader oh WITH (NOLOCK)
                    JOIN Vendor rl WITH (NOLOCK) on oh.ReceiveLocation_ID = rl.Vendor_ID
                    JOIN Store ts WITH (NOLOCK) on ts.Store_No = rl.Store_no
                    JOIN Vendor psl WITH (NOLOCK) on oh.Vendor_ID = psl.Vendor_ID
                    JOIN Store fs WITH (NOLOCK) on fs.Store_No = psl.Store_no
                    JOIN SubTeam tst WITH (NOLOCK) on tst.SubTeam_No = oh.Transfer_To_SubTeam
                    JOIN SubTeam fst WITH (NOLOCK) on fst.SubTeam_No = oh.Transfer_SubTeam
                    JOIN Users uc WITH (NOLOCK) on uc.User_ID = oh.CreatedBy
                    LEFT JOIN Users ua WITH (NOLOCK) on ua.User_ID = oh.ApprovedBy
                    JOIN OrderItem oi WITH (NOLOCK) on oh.OrderHeader_ID = oi.OrderHeader_ID
                    JOIN Item i WITH (NOLOCK) on i.Item_Key = oi.Item_Key
                    JOIN ItemIdentifier ii WITH (NOLOCK) on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1
                    JOIN SubTeam isb WITH (NOLOCK) on isb.SubTeam_No = i.SubTeam_No
                    JOIN ItemUnit iuo WITH (NOLOCK) on iuo.Unit_ID = oi.QuantityUnit
                    WHERE oh.OrderHeader_ID = @KeyId
                    AND i.Retail_Sale = 1";

                return irmaContext.Database.SqlQuery<TransferOrdersModel>(
                    getNonDeleteEventQuery,
                    new SqlParameter("@KeyId", keyId)
                    ).ToList();
            }
        }

        private IList<TransferOrdersModel> GetLineDeleteEvent(int keyId, int? secondaryKeyId)
        {
            using(var irmaContext = irmaContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;
                string getLineDeleteEventQuery = 
                    $@"SELECT
                        oh.OrderHeader_ID as OrderHeaderId,
                        NULL as FromLocationNumber,
                        NULL as FromLocationName,
                        ts.BusinessUnit_ID as ToLocationNumber,
                        NULL as ToLocationName,
                        NULL as FromSubTeamNumber,
                        NULL as FromSubTeamName,
                        NULL as ToSubTeamNumber,
                        NULL as ToSubTeamName,
                        NULL as Status,
                        0 as UserId,
                        NULL as UserName,
                        oh.ApprovedBy as ApproverId,
                        NULL as ApproverName,
                        NULL as CreateDateTime,
                        NULL as ApproveDateTime,
                        oi.OrderItem_ID as TransferOrderDetailNumber,
                        oi.Item_Key as SourceItemKey,
                        ii.Identifier as DefaultScanCode,
                        NULL as HostSubTeamNumber,
                        NULL as HostSubTeamName,
                        NULL as QuantityOrdered,
                        NULL as OrderedUnit,
                        NULL as OrderedUnitCode,
                        NULL as ExpectedArrivalDate,
                        NULL as PackageDesc1,
                        NULL as PackageDesc2
                    FROM OrderHeader oh (NOLOCK)
                    JOIN Vendor rl WITH (NOLOCK) on oh.ReceiveLocation_ID = rl.Vendor_ID
                    JOIN Store ts WITH (NOLOCK) on ts.Store_No = rl.Store_no
                    JOIN amz.DeletedOrderItem oi (NOLOCK) on oh.OrderHeader_ID = oi.OrderHeader_ID and oi.OrderItem_ID = @SecondaryKeyId
                    JOIN Item i (NOLOCK) on i.Item_Key = oi.Item_Key
                    JOIN ItemIdentifier ii (NOLOCK) on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1
                    WHERE oh.OrderHeader_ID = @KeyId
                      AND i.Retail_Sale = 1";

                return irmaContext.Database.SqlQuery<TransferOrdersModel>(
                    getLineDeleteEventQuery,
                    new SqlParameter("@KeyId", keyId),
                    new SqlParameter("@SecondaryKeyId", secondaryKeyId)
                    ).ToList();
            }
        }

        private IList<TransferOrdersModel> GetPODeleteEvent(int keyId)
        {
            using(var irmaContext = irmaContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;
                string getPODeleteEventQuery =
                    $@"SELECT
                        oh.OrderHeader_ID as OrderHeaderId,
                        NULL as FromLocationNumber,
                        NULL as FromLocationName,
                        ts.BusinessUnit_ID as ToLocationNumber,
                        NULL as ToLocationName,
                        NULL as FromSubTeamNumber,
                        NULL as FromSubTeamName,
                        NULL as ToSubTeamNumber,
                        NULL as ToSubTeamName,
                        'Deleted' as Status,
                        oh.User_ID as UserId,
                        uc.UserName as UserName,
                        oh.ApprovedBy as ApproverId,
                        NULL as ApproverName,
                        DeleteDate as CreateDateTime,
                        NULL as ApproveDateTime,
                        NULL as TransferOrderDetailNumber,
                        NULL as SourceItemKey,
                        NULL as DefaultScanCode,
                        NULL as HostSubTeamNumber,
                        NULL as HostSubTeamName,
                        NULL as QuantityOrdered,
                        NULL as OrderedUnit,
                        NULL as OrderedUnitCode,
                        NULL as ExpectedArrivalDate,
                        NULL as PackageDesc1,
                        NULL as PackageDesc2
                    FROM DeletedOrder oh (NOLOCK)
                    JOIN Vendor rl WITH (NOLOCK) on oh.ReceiveLocation_ID = rl.Vendor_ID
                    JOIN Store ts WITH (NOLOCK) on ts.Store_No = rl.Store_no
                    JOIN Users uc (NOLOCK) on uc.User_ID = oh.User_ID 
                    WHERE oh.OrderHeader_ID = @KeyId";

                return irmaContext.Database.SqlQuery<TransferOrdersModel>(
                    getPODeleteEventQuery,
                    new SqlParameter("@KeyId", keyId)
                    ).ToList();
            }
        }
    }
}
