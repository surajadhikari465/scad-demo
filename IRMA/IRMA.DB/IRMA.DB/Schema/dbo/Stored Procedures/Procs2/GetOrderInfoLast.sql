CREATE PROCEDURE dbo.GetOrderInfoLast
AS 
   -- **************************************************************************
   -- Procedure: GetOrderInfoLast()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 12/13/2010	BBB		13334	removed deprecated and unused code
   -- 1/18/2011		TTL		759		Added OH.POCostDate to result set.
   -- 7/20/2011		MD		2095	Added OH.ReasonCodeDetailID to result set.
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
    SELECT TOP 1 OrderHeader.OrderHeader_ID, OrderHeader.Temperature, OrderHeader.QuantityDiscount, OrderHeader.Accounting_In_DateStamp,
           Vendor.CompanyName, CB.UserName AS CreatedByName, SubTeam.SubTeam_Name, TTSubTeam.SubTeam_Name AS Transfer_To_SubTeamName,
           OrderHeader.OrderDate, OrderHeader.CloseDate, OrderHeader.SentDate, OrderHeader.Expected_Date, OrderHeader.InvoiceDate, 
           OrderHeader.UploadedDate, OrderHeader.ApprovedDate, OrderHeader.OrderType_ID, ISNULL(OrderHeader.ProductType_ID, 1) as ProductType_ID, 
           OrderHeader.CreatedBy, OrderHeader.Transfer_SubTeam, OrderHeader.Transfer_To_SubTeam, 
           OrderHeader.Vendor_ID, ISNULL(Vendor.Store_No, 0) as Vendor_Store_No, OrderHeader.DiscountType, OrderHeader.PurchaseLocation_ID, 
           OrderHeader.ReceiveLocation_ID, OrderHeader.Fax_Order,  
           OrderHeader.Email_Order, --Alexa Horvath TFS8316 20090107 Pull in for email po functionality
           OrderHeader.Electronic_Order,
           OrderHeader.OverrideTransmissionMethod, --Robert Shurbet TFS8316 20090216 Pull in for email po functionality  
           OrderHeader.Sent, OrderHeader.Return_Order, 
           OrderHeader.OriginalCloseDate, OrderHeader.User_ID, OrderHeader.InvoiceNumber, FromCredit.ReturnOrderHeader_ID AS ReturnOrder_ID,
           OrderHeader.isDropShipment, -- Robin Eudy TFS9750 DropShip
           ToCredit.OrderHeader_ID AS OriginalOrder_ID, ReceiveLocation.Store_No,
           Vendor.POTransmissionTypeID, --Alexa Horvath TFS8316 20090107 Pull in for email po functionality
           ISNULL(VStore.WFM_Store, 0) AS WFM_Store,
    	   ISNULL(VStore.MEGA_Store, 0) AS HFM_Store,
           ISNULL(CASE WHEN VStore.WFM_Store = 1 OR VStore.Mega_Store = 1 THEN 1 ELSE 0 END, 0) As Store_Vend, 
           RecvLog_No, WarehouseSent, WarehouseSentDate, 
           ISNULL(VStore.EXEWarehouse, RStore.EXEWarehouse) As EXEWarehouse,
			CASE 
				WHEN ((SubTeam.SubTeamType_ID = 2) 		-- Manufacturing
						OR (SubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
						OR (SubTeam.SubTeamType_ID = 4)	-- Expense
					 ) THEN 1 -- Unrestricted
				ELSE 0 -- Restricted to retail subteam
			END AS From_SubTeam_Unrestricted,
			CASE 
				WHEN ((TTSubTeam.SubTeamType_ID = 2) 		-- Manufacturing
						OR (TTSubTeam.SubTeamType_ID = 3)	-- RetailManufacturing
						OR (TTSubTeam.SubTeamType_ID = 4)	-- Expense
					 ) THEN 1 -- Unrestricted
				ELSE 0 -- Restricted to retail subteam
			END AS To_SubTeam_Unrestricted,
           	(	SELECT COUNT(*) FROM OrderItem (nolock) 
				WHERE OrderHeader_ID = OrderHeader.OrderHeader_ID AND DateReceived IS NOT NULL
			) As ItemsReceived,
            CASE WHEN OrderHeader.OrderType_ID = 2 -- Distribution
                 THEN CASE WHEN Transfer_SubTeam = Transfer_To_SubTeam 
                           THEN ISNULL(CONVERT(varchar(255), OrderEnd, 108), '0') -- not tranfer order
				           ELSE ISNULL(CONVERT(varchar(255), OrderEndTransfers, 108), '0') -- is transfer order
                           END
                 ELSE '0' END As OrderEnd, 
			CONVERT(varchar(255), GETDATE(), 108) As CurrSysTime,
           	ISNULL(VStore.Distribution_Center, 0) As Distribution_Center, 
           	ISNULL(RStore.Distribution_Center, 0) As ReceivingStore_Distribution_Center,  
			ISNULL(VStore.Manufacturer,0) as Manufacturer, 
			ISNULL(Vendor.WFM, 0) as WFM,
           	(	SELECT COUNT(*) FROM OrderItem (nolock) INNER JOIN Item (nolock) ON Item.Item_Key = OrderItem.Item_Key 
				WHERE OrderHeader_ID = OrderHeader.OrderHeader_ID AND EXEDistributed = 1
			) As IsEXEDistributed, 
	  ISNULL(OI.InvoiceCost, 0) + ISNULL(OI.InvoiceFreight, 0) As InvoiceAmount,
	  ISNULL(CL.UserName, '') As ClosedByUserName,
	  ISNULL(CL2.UserName, '') As ApprovedByUserName, --Dave Stacey TFS 10386
 	  ISNULL(OrderHeader.Freight3Party_OrderCost, 0) as Freight3Party_OrderCost,
      CASE WHEN Vendor.Customer = 0 AND Vendor.InternalCustomer = 0 THEN 1 ELSE 0 END AS IsVendorExternal,
      STSubTeam.SubTeam_Name As SupplyType_SubTeamName,
      OrderHeader.AccountingUploadDate,
      WarehouseCancelled
	,PayByAgreedCost = PayByAgreedCost
	
	, ISNULL((SELECT SUM(OrderItem.LineItemCost)
		FROM OrderItem 
		WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID),0) AS OrderedCost
	, (SELECT SUM(OrderItem.ReceivedItemCost) AS SumOfReceivedItemCost 
			FROM OrderItem 
			WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID) AS AdjustedReceivedCost
	, (SELECT SUM(ISNULL(dbo.fn_GetOrigReceivedCost(dbo.OrderItem.OrderItem_ID),0)) AS FNGetOrigRcvdCost 
		FROM dbo.OrderItem 
		WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID) AS OriginalReceivedCost
	, (CASE WHEN OrderHeader.PayByAgreedCost = 1 AND OrderHeader.UploadedDate IS NOT NULL
			THEN (SELECT SUM(OrderItem.ReceivedItemCost) AS SumOfReceivedItemCost FROM OrderItem WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
			ELSE CASE WHEN OrderHeader.PayByAgreedCost = 0 AND OrderHeader.UploadedDate IS NOT NULL
					THEN OI.InvoiceCost
					ELSE 0 
					END
			END) AS UploadedCost,
		TotalHandlingCharge = ISNULL((SELECT SUM(OrderItem.HandlingCharge * OrderItem.QuantityOrdered)
			FROM OrderItem 
			WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID),0),
		POCostDate = OrderHeader.POCostDate,
		ReasonCodeDetailID = OrderHeader.ReasonCodeDetailID
    FROM OrderHeader
    INNER JOIN Vendor (NOLOCK) 
        ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
    INNER JOIN Users CB (NOLOCK) 
        ON CB.User_ID = OrderHeader.CreatedBy
    INNER JOIN Vendor ReceiveLocation (NOLOCK) 
        ON ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID
    LEFT JOIN ReturnOrderList FromCredit 
        ON FromCredit.OrderHeader_ID = OrderHeader.OrderHeader_ID
    LEFT JOIN ReturnOrderList ToCredit 
        ON ToCredit.ReturnOrderHeader_ID = OrderHeader.OrderHeader_ID
    LEFT JOIN SubTeam (NOLOCK) 
        ON SubTeam.SubTeam_No = OrderHeader.Transfer_SubTeam
    LEFT JOIN SubTeam TTSubTeam (NOLOCK) 
        ON TTSubTeam.SubTeam_No = OrderHeader.Transfer_To_SubTeam
      LEFT JOIN SubTeam STSubTeam (NOLOCK)   
        ON STSubTeam.SubTeam_No = OrderHeader.SupplyTransferToSubTeam 
    LEFT JOIN Store VStore (NOLOCK) 
        ON VStore.Store_No = Vendor.Store_No
    LEFT JOIN Store RStore (NOLOCK) 
        ON RStore.Store_No = ReceiveLocation.Store_No
    LEFT JOIN ZoneSubTeam ZST (nolock)
        ON ZST.Zone_ID = RStore.Zone_ID
        AND ZST.Supplier_Store_No = VStore.Store_No
        AND ZST.SubTeam_No = OrderHeader.Transfer_SubTeam
    LEFT JOIN OrderInvoice OI (nolock)
	    ON OrderHeader.OrderHeader_ID = OI.OrderHeader_ID
	LEFT JOIN Users CL (NOLOCK) 
        ON CL.User_ID = OrderHeader.ClosedBy 
	LEFT JOIN Users CL2 (NOLOCK) 
        ON CL2.User_ID = OrderHeader.ApprovedBy 
    WHERE OrderHeader.OrderHeader_ID = (SELECT Max(OrderHeader_ID) FROM OrderHeader)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoLast] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoLast] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoLast] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoLast] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoLast] TO [IRMAReportsRole]
    AS [dbo];

