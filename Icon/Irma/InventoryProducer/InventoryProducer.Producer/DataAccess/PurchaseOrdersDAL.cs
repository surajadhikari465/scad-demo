﻿using Icon.DbContextFactory;
using InventoryProducer.Common;
using InventoryProducer.Producer.Model.DBModel;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.DataAccess
{
    public class PurchaseOrdersDAL : IPurchaseOrdersDAL
    {
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings inventoryProducerSettings;

        private const int DB_TIMEOUT_SECONDS = 120;

        public PurchaseOrdersDAL(IDbContextFactory<IrmaContext> irmaContextFactory,
            InventoryProducerSettings inventoryProducerSettings)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.inventoryProducerSettings = inventoryProducerSettings;
        }

        public IList<PurchaseOrdersModel> GetPurchaseOrders(string eventType, int keyId, int? secondaryKeyId)
        {
            if (eventType.Contains(Constants.EventType.PO_LINE_DEL))
            {
                return GetLineDeleteEvent(keyId, secondaryKeyId);
            }
            else if (eventType.Contains(Constants.EventType.PO_DEL))
            {
                return GetPODeleteEvent(keyId);
            }
            else if (!eventType.Contains(Constants.EventType.DEL)) // usually TSF_CRE eventTypeCode
            {
                return GetNonDeleteEvent(keyId);
            }
            throw new NotImplementedException($"EventTypeCode: {eventType} is not implemented");
        }

        private IList<PurchaseOrdersModel> GetNonDeleteEvent(int keyId)
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;
                string getNonDeleteEventQuery =
                    @"SELECT
                        oh.OrderHeader_ID,
                        e.ExternalOrder_Id,
                        'AMAZON' as ExternalSource,
                        'Purchase Order' as PurchaseType,
                        v.PS_Vendor_ID as SupplierNumber,
                        s.BusinessUnit_ID as LocationNumber,
                        s.Store_Name as LocationName,
                        sb.SubDept_No as OrderSubTeam_No,
                        sb.SubTeam_Name as OrderSubTeam_Name,
                        t.Team_No as OrderTeam_No,
                        t.Team_Name as OrderTeam_Name,
                        Case
                            When oh.RefuseReceivingReasonID is not null Then 'Refused'
                            When oh.OriginalCloseDate is not null and oh.ApprovedDate is null Then 'Suspended'
                            When oh.ApprovedDate is not null Then 'Closed'
                            Else 'Sent'
                        End as Status,
                        oh.CreatedBy as UserId,
                        uc.UserName as UserName,
                        oh.ApprovedBy as ApproverId,
                        ua.UserName as ApproverName,
                        ISNULL(oh.SentDate, oh.OrderDate) as CreateDateTime,
                        oh.ApprovedDate as ApproveDateTime,
                        oh.OriginalCloseDate as CloseDateTime,
                        oh.AdminNotes as PurchaseOrderComments,
                        oi.OrderItem_ID as PurchaseOrderDetailNumber,
                        oi.Item_Key as SourceItemKey,
                        ii.Identifier as DefaultScanCode,
                        isb.SubDept_No as HostSubTeam_No,
                        isb.SubTeam_Name as HostSubTeam_Name,
	                    CASE oh.Return_Order 
		                    WHEN 0
			                    THEN oi.QuantityOrdered
		                    ELSE oi.QuantityOrdered * -1 
	                    END as QuantityOrdered,
                        iuo.Unit_Abbreviation as OrderedUnitCode,
                        iuo.Unit_Name as OrderedUnit,
                        CAST(oi.Package_Desc1 as int) as PackSize1,
                        CAST(oi.Package_Desc2 as int) as PackSize2,
                        iui.Unit_Name as RetailUnit,
                        oi.UnitCost as ItemCost,
                        oi.DateReceived as EarliestArrivalDate,
                        oh.Expected_Date as ExpectedArrivalDate,
	                    oi.eInvoiceQuantity,
	                    oi.eInvoiceWeight,
	                    oh.OrderExternalSourceOrderID,
	                    CASE
		                    WHEN oh.OrderExternalSourceOrderID IS NOT NULL THEN oes.Description
		                    ELSE NULL
	                    END as OtherExternalSourceDescription
                    FROM OrderHeader oh (NOLOCK)
                    join Vendor psl (NOLOCK) on oh.PurchaseLocation_ID = psl.Vendor_ID
                    JOIN Store s (NOLOCK) on s.Store_No = psl.Store_no
                    JOIN Vendor v (NOLOCK) on oh.Vendor_ID = v.Vendor_ID
                    JOIN SubTeam sb (NOLOCK) on sb.SubTeam_No = oh.Transfer_To_SubTeam
                    JOIN Team t (NOLOCK) on t.Team_No = sb.Team_No
                    JOIN Users uc (NOLOCK) on uc.User_ID = oh.CreatedBy 
                    LEFT JOIN Users ua (NOLOCK) on ua.User_ID = oh.ApprovedBy
                    LEFT JOIN ExternalOrderInformation e (NOLOCK) on e.OrderHeader_Id = oh.OrderHeader_ID 
	                    and e.ExternalSource_Id = (SELECT ID FROM OrderExternalSource (NOLOCK) WHERE Description = 'AMAZON')
                    LEFT JOIN OrderExternalSource oes on oh.OrderExternalSourceID = oes.ID
                    JOIN OrderItem oi (NOLOCK) on oh.OrderHeader_ID = oi.OrderHeader_ID 
                    JOIN Item i (NOLOCK) on i.Item_Key = oi.Item_Key
                    JOIN ItemIdentifier ii (NOLOCK) on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1
                    JOIN SubTeam isb (NOLOCK) on isb.SubTeam_No = i.SubTeam_No
                    JOIN ItemUnit iuo (NOLOCK) on iuo.Unit_ID = oi.QuantityUnit
                    JOIN itemUnit iui (NOLOCK) on iui.Unit_ID = i.Retail_Unit_ID
                    WHERE oh.OrderHeader_ID = @KeyId";

                return irmaContext.Database.SqlQuery<PurchaseOrdersModel>(
                    getNonDeleteEventQuery,
                    new SqlParameter("@KeyId", keyId)
                    ).ToList();
            }
        }

        private IList<PurchaseOrdersModel> GetPODeleteEvent(int keyId)
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;
                string getPoDeleteEventQuery =
                @"
                SELECT
	                oh.OrderHeader_ID,
	                e.ExternalOrder_Id,
	                'AMAZON' as ExternalSource,
	                NULL as PurchaseType,
	                NULL as SupplierNumber,
	                s.BusinessUnit_ID as LocationNumber,
	                NULL as LocationName,
	                NULL as OrderSubTeam_No,
	                NULL as OrderSubTeam_Name,
	                NULL as OrderTeam_No,
	                NULL as OrderTeam_Name,
	                'Deleted' as Status,
	                oh.User_ID as UserId,
	                uc.UserName as UserName,
	                NULL as ApproverId,
	                NULL as ApproverName,
	                NULL as CreateDateTime,
	                NULL as ApproveDateTime,
	                NULL as CloseDateTime,
	                NULL as PurchaseOrderComments,
	                NULL as PurchaseOrderDetailNumber,
	                NULL as SourceItemKey,
	                NULL as DefaultScanCode,
	                NULL as HostSubTeam_No,
	                NULL as HostSubTeam_Name,
	                NULL as QuantityOrdered,
	                NULL as OrderedUnitCode,
	                NULL as OrderedUnit,
	                NULL as PackSize1,
	                NULL as PackSize2,
	                NULL as RetailUnit,
	                NULL as ItemCost,
	                NULL as EarliestArrivalDate,
	                NULL as ExpectedArrivalDate,
	                NULL as eInvoiceQuantity,
	                NULL as eInvoiceWeight,
	                oh.OrderExternalSourceOrderID,
	                CASE
		                WHEN oh.OrderExternalSourceOrderID IS NOT NULL THEN oes.Description
		                ELSE NULL
	                END as OtherExternalSourceDescription
                FROM DeletedOrder oh (NOLOCK)
                join Vendor psl (NOLOCK) on oh.PurchaseLocation_ID = psl.Vendor_ID
                JOIN Store s (NOLOCK) on s.Store_No = psl.Store_no
                JOIN Users uc (NOLOCK) on uc.User_ID = oh.User_ID 
                LEFT JOIN ExternalOrderInformation e (NOLOCK) on e.OrderHeader_Id = oh.OrderHeader_ID 
	                AND e.ExternalSource_Id = (SELECT ID FROM OrderExternalSource WHERE Description = 'AMAZON')
                LEFT JOIN OrderExternalSource oes on oh.OrderExternalSourceID = oes.ID
                WHERE oh.OrderHeader_ID = @KeyId
                ";

                return irmaContext.Database.SqlQuery<PurchaseOrdersModel>(
                   getPoDeleteEventQuery,
                   new SqlParameter("@KeyId", keyId)
                   ).ToList();
            }
        }

        private IList<PurchaseOrdersModel> GetLineDeleteEvent(int keyId, int? secondaryKeyId)
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;
                string getPoDeleteEventQuery =
                @"
                SELECT oh.OrderHeader_ID,
	                    e.ExternalOrder_Id,
	                    'AMAZON' as ExternalSource,
	                    NULL as PurchaseType,
	                    NULL as SupplierNumber,
	                    s.BusinessUnit_ID as LocationNumber,
	                    NULL as LocationName,
	                    NULL as OrderSubTeam_No,
	                    NULL as OrderSubTeam_Name,
	                    NULL as OrderTeam_No,
	                    NULL as OrderTeam_Name,
	                    Case
		                    When oh.RefuseReceivingReasonID is not null Then 'Refused'
		                    When oh.OriginalCloseDate is not null and oh.ApprovedDate is null Then 'Suspended'
		                    When oh.ApprovedDate is not null Then 'Closed'
		                    Else 'Sent'
	                    End as Status,
	                    0 as UserId,
	                    NULL as UserName,
	                    NULL as ApproverId,
	                    NULL as ApproverName,
	                    NULL as CreateDateTime,
	                    NULL as ApproveDateTime,
	                    NULL as CloseDateTime,
	                    NULL as PurchaseOrderComments,
	                    oi.OrderItem_ID as PurchaseOrderDetailNumber,
	                    oi.Item_Key as SourceItemKey,
	                    ii.Identifier as DefaultScanCode,
	                    NULL as HostSubTeam_No,
	                    NULL as HostSubTeam_Name,
	                    NULL as QuantityOrdered,
	                    NULL as OrderedUnitCode,
	                    NULL as OrderedUnit,
	                    NULL as PackSize1,
	                    NULL as PackSize2,
	                    NULL as RetailUnit,
	                    NULL as ItemCost,
	                    NULL as EarliestArrivalDate,
	                    NULL as ExpectedArrivalDate,
	                    NULL as eInvoiceQuantity,
	                    NULL as eInvoiceWeight,
	                    oh.OrderExternalSourceOrderID,
	                    CASE
		                    WHEN oh.OrderExternalSourceOrderID IS NOT NULL THEN oes.Description
		                    ELSE NULL
	                    END as OtherExternalSourceDescription
                    FROM OrderHeader oh (NOLOCK)
                    join Vendor psl (NOLOCK) on oh.PurchaseLocation_ID = psl.Vendor_ID
                    JOIN Store s (NOLOCK) on s.Store_No = psl.Store_no
                    LEFT JOIN ExternalOrderInformation e (NOLOCK) on e.OrderHeader_Id = oh.OrderHeader_ID 
	                    AND e.ExternalSource_Id = (SELECT ID FROM OrderExternalSource (NOLOCK) WHERE Description = 'AMAZON')
                    LEFT JOIN OrderExternalSource oes on oh.OrderExternalSourceID = oes.ID
                    JOIN amz.DeletedOrderItem oi (NOLOCK) on oh.OrderHeader_ID = oi.OrderHeader_ID and oi.OrderItem_ID = @SecondaryKeyId
                    JOIN Item i (NOLOCK) on i.Item_Key = oi.Item_Key
                    JOIN ItemIdentifier ii (NOLOCK) on oi.Item_Key = ii.Item_key and ii.Default_Identifier = 1
                    WHERE oh.OrderHeader_ID = @KeyId";

                return irmaContext.Database.SqlQuery<PurchaseOrdersModel>(
                   getPoDeleteEventQuery,
                   new SqlParameter("@KeyId", keyId),
                    new SqlParameter("@SecondaryKeyId", secondaryKeyId)
                   ).ToList();
            }
        }
    }
}
