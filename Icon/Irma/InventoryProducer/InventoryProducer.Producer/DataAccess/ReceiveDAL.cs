using System;
using System.Collections.Generic;
using System.Linq;
using InventoryProducer.Common;
using Icon.DbContextFactory;
using Irma.Framework;
using InventoryProducer.Producer.Model.DBModel;
using System.Data.SqlClient;
using Apache.NMS;


namespace InventoryProducer.Producer.DataAccess
{
    // Gets InventoryReceive data from IRMA DB
    public class ReceiveDAL : IDAL<ReceiveModel>
    {
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings inventoryProducerSettings;
        private const int DB_TIMEOUT_SECONDS = 120;

        public ReceiveDAL(
            IDbContextFactory<IrmaContext> irmaContextFactory,
            InventoryProducerSettings inventoryProducerSettings
            )
        {
            this.irmaContextFactory = irmaContextFactory;
            this.inventoryProducerSettings = inventoryProducerSettings;
        }

        public IList<ReceiveModel> Get(string eventType, int keyId, int? secondaryKeyId)
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = DB_TIMEOUT_SECONDS;
                string getEventsReceiveQuery =
                    $@"SELECT  
	                oh.OrderHeader_ID as OrderHeaderId,
	                oi.OrderItem_ID as OrderItemId,
	                id.Identifier as Identifier,
	                id.Item_Key as ItemKey,
	                st.SubTeam_Name as HostSubTeam,
	                st.SubDept_No as HostSubTeamNumber,
	                sth.SubTeam_Name as SubTeam,
	                sth.SubDept_No as SubTeamNumber,
	                oi.DateReceived as DateReceived,
	                CAST(oh.Return_Order as int) as CreditPO, 
	                CAST(ISNULL(oi.QuantityReceived, 0) as int) as QuantityReceived,
	                CAST(oi.QuantityOrdered as int) as QuantityOrdered,
	                oi.Package_Desc1 as PackageDesc1,
	                oi.Package_Desc2 as PackageDesc2,
	                Oh.RecvLogUser_ID as RecvLogUserId,
	                IsNull(u.UserName, '') as RecvUserName,
	                IsNull(u.User_ID, 0)  as RecvUSerID,
	                iu.Unit_Abbreviation as OrderUom,
	                iu.Unit_Abbreviation as ReceiptUom,
	                CASE WHEN IsNull(QuantityReceived,0) = QuantityOrdered THEN 'Received'  
	                	WHEN oh.PartialShipment = 1 AND IsNull(QuantityReceived,0) > 0 THEN 'Partially Received'
	                	WHEN oh.PartialShipment = 0 AND IsNull(QuantityReceived,0) > 0 THEN 'Received' 
	                	ELSE 'Not Received' END AS ReceiptStatus,
	                ISNULL(iv.item_id, 0) as VIN,
	                s.BusinessUnit_ID as StoreNumber,
                        s.Store_Name as StoreName,
	                ohe.PastReceiptDate,
	                v.PS_Vendor_ID as SupplierNumber,
	                ISNULL(oh.SentDate, oh.OrderDate) as CreateDateTime
	                FROM 
	                dbo.OrderHeader oh WITH (NOLOCK)
	                JOIN Vendor psl WITH (NOLOCK) on oh.PurchaseLocation_ID = psl.Vendor_ID 
	                JOIN Store s  WITH (NOLOCK) on s.Store_No = psl.Store_no 
	                JOIN dbo.OrderItem as oi  WITH (NOLOCK) on oh.OrderHeader_ID = oi.OrderHeader_ID
	                JOIN ItemIdentifier id WITH (NOLOCK) on id.Item_Key = oi.Item_Key
	                JOIN item i WITH (NOLOCK) on i.Item_key = id.Item_Key
	                JOIN SubTeam st WITH (NOLOCK) on st.SubTeam_No = i.SubTeam_No
	                JOIN SubTeam sth WITH (NOLOCK) on sth.Subteam_No = oh.Transfer_To_SubTeam
	                LEFT JOIN dbo.Users u WITH (NOLOCK) on oh.ApprovedBy = u.User_ID
	                JOIN ItemUnit iu WITH (NOLOCK) on iu.Unit_ID = oi.QuantityUnit
	                JOIN Vendor v WITH (NOLOCK) on  v.Vendor_ID = oh.Vendor_ID
	                LEFT JOIN StoreItemVendor siv WITH (NOLOCK) on siv.Store_No = s.Store_No and siv.Item_Key = i.Item_Key and siv.PrimaryVendor = 1
	                LEFT JOIN OrderHeaderExtended ohe on ohe.OrderHeader_Id = oh.OrderHeader_Id
	                LEFT JOIN ItemVendor iv WITH (NOLOCK) on iv.Item_Key = id.Item_Key and iv.Vendor_ID = 
	                					case 
	                						when oh.OrderType_ID = 1
	                						then v.Vendor_ID
	                					else siv.Vendor_ID
	                					end
	                WHERE 
	                  i.Retail_Sale = 1
	                  AND id.Default_Identifier = 1
	                  AND oh.OrderHeader_ID = @KeyId";
                return irmaContext.Database.SqlQuery<ReceiveModel>(
                    getEventsReceiveQuery,
                    new SqlParameter("@KeyId", keyId)
                    ).ToList();
            }
        }
    }
}
