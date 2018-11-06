CREATE PROCEDURE dbo.GetOrderInfoUser
    @CreatedBy int
AS 
   -- **************************************************************************
   -- Procedure: GetOrderInfoUser()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 12/13/2010	BBB		13334	removed deprecated and unused code
   -- 1/18/2011		TTL		759		Added OH.POCostDate to result set.
   -- 1/18/2011		TTL		759		Added OH.POCostDate to result set.
   -- 08/11/2011	DBS		2710	Performance tweaks (aliases, dbo prefix, NOLOCK hints.
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
    SELECT TOP 1 OH.OrderHeader_ID, OH.Temperature, OH.QuantityDiscount, OH.Accounting_In_DateStamp,
           V.CompanyName, CB.UserName AS CreatedByName, SubTeam.SubTeam_Name, TTSubTeam.SubTeam_Name AS Transfer_To_SubTeamName,
           OH.OrderDate, OH.CloseDate, OH.SentDate, OH.Expected_Date, OH.InvoiceDate,
           OH.UploadedDate, OH.ApprovedDate, OH.OrderType_ID, ISNULL(OH.ProductType_ID, 1) as ProductType_ID, 
           OH.CreatedBy, OH.Transfer_SubTeam, OH.Transfer_To_SubTeam, 
           OH.Vendor_ID, ISNULL(V.Store_No, 0) as Vendor_Store_No, OH.DiscountType, OH.PurchaseLocation_ID, 
           OH.ReceiveLocation_ID, OH.Fax_Order, 
           OH.Email_Order, --Alexa Horvath TFS8316 20090107 Pull in for email po functionality 
           OH.Electronic_Order,
           OH.OverrideTransmissionMethod, --Robert Shurbet TFS8316 20090216 Pull in for email po functionality   
           OH.Sent, OH.Return_Order, 
           OH.OriginalCloseDate, OH.User_ID, OH.InvoiceNumber, FromCredit.ReturnOrderHeader_ID AS ReturnOrder_ID,
           OH.isDropShipment, -- Robin Eudy TFS9750 DropShip
           ToCredit.OrderHeader_ID AS OriginalOrder_ID, ReceiveLocation.Store_No, 
           V.POTransmissionTypeID, --Alexa Horvath TFS8316 20090107 Pull in for email po functionality
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
           	(	SELECT COUNT(*) FROM dbo.OrderItem (nolock) 
				WHERE OrderHeader_ID = OH.OrderHeader_ID AND DateReceived IS NOT NULL
			) As ItemsReceived,
            CASE WHEN OH.OrderType_ID = 2 -- Distribution
                 THEN CASE WHEN Transfer_SubTeam = Transfer_To_SubTeam 
                           THEN ISNULL(CONVERT(varchar(255), OrderEnd, 108), '0') -- not tranfer order
				           ELSE ISNULL(CONVERT(varchar(255), OrderEndTransfers, 108), '0') -- is transfer order
                           END
                 ELSE '0' END As OrderEnd, 
			CONVERT(varchar(255), GETDATE(), 108) As CurrSysTime,
           	ISNULL(VStore.Distribution_Center, 0) As Distribution_Center, 
           	ISNULL(RStore.Distribution_Center, 0) As ReceivingStore_Distribution_Center,  
			ISNULL(VStore.Manufacturer,0) as Manufacturer, 
			ISNULL(V.WFM, 0) as WFM,
           	(	SELECT COUNT(*) FROM OrderItem (nolock) INNER JOIN Item (nolock) ON Item.Item_Key = OrderItem.Item_Key 
				WHERE OrderHeader_ID = OH.OrderHeader_ID AND EXEDistributed = 1
			) As IsEXEDistributed, 
	   ISNULL(OI.InvoiceCost, 0) + ISNULL(OI.InvoiceFreight, 0) As InvoiceAmount,
	   ISNULL(CL.UserName, '') As ClosedByUserName,
	   ISNULL(CL2.UserName, '') As ApprovedByUserName, --Dave Stacey TFS 10386
	   ISNULL(OH.Freight3Party_OrderCost, 0) as Freight3Party_OrderCost,
       CASE WHEN V.Customer = 0 AND V.InternalCustomer = 0 THEN 1 ELSE 0 END AS IsVendorExternal,
       STSubTeam.SubTeam_Name As SupplyType_SubTeamName,
       OH.AccountingUploadDate,
       WarehouseCancelled
		,PayByAgreedCost = PayByAgreedCost
	, ISNULL((SELECT SUM(OrderItem.LineItemCost)
		FROM dbo.OrderItem (NOLOCK) 
		WHERE OrderItem.OrderHeader_ID = OH.OrderHeader_ID),0) AS OrderedCost
	, (SELECT SUM(OrderItem.ReceivedItemCost) AS SumOfReceivedItemCost 
			FROM dbo.OrderItem (NOLOCK)
			WHERE OrderItem.OrderHeader_ID = OH.OrderHeader_ID) AS AdjustedReceivedCost
	, (SELECT SUM(ISNULL(dbo.fn_GetOrigReceivedCost(dbo.OrderItem.OrderItem_ID),0)) AS FNGetOrigRcvdCost 
		FROM dbo.OrderItem (NOLOCK) 
		WHERE OrderItem.OrderHeader_ID = OH.OrderHeader_ID) AS OriginalReceivedCost
	, (CASE WHEN OH.PayByAgreedCost = 1 AND OH.UploadedDate IS NOT NULL
			THEN (SELECT SUM(OrderItem.ReceivedItemCost) AS SumOfReceivedItemCost FROM OrderItem WHERE OrderItem.OrderHeader_ID = OH.OrderHeader_ID)
			ELSE CASE WHEN OH.PayByAgreedCost = 0 AND OH.UploadedDate IS NOT NULL
					THEN OI.InvoiceCost
					ELSE 0 
					END
			END) AS UploadedCost,
		TotalHandlingCharge = ISNULL((SELECT SUM(OrderItem.HandlingCharge * OrderItem.QuantityOrdered)
			FROM dbo.OrderItem (NOLOCK)
			WHERE OrderItem.OrderHeader_ID = OH.OrderHeader_ID),0),
		POCostDate = OH.POCostDate,
		ReasonCodeDetailID = OH.ReasonCodeDetailId
    FROM dbo.OrderHeader OH (NOLOCK)
    INNER JOIN dbo.Vendor v (NOLOCK) 
        ON V.Vendor_ID = OH.Vendor_ID
    INNER JOIN dbo.Users CB (NOLOCK) 
        ON CB.User_ID = OH.CreatedBy
    INNER JOIN dbo.Vendor ReceiveLocation (NOLOCK) 
        ON ReceiveLocation.Vendor_ID = OH.ReceiveLocation_ID
    LEFT JOIN dbo.ReturnOrderList FromCredit  (NOLOCK)
        ON FromCredit.OrderHeader_ID = OH.OrderHeader_ID
    LEFT JOIN dbo.ReturnOrderList ToCredit  (NOLOCK)
        ON ToCredit.ReturnOrderHeader_ID = OH.OrderHeader_ID
    LEFT JOIN dbo.SubTeam (NOLOCK) 
        ON SubTeam.SubTeam_No = OH.Transfer_SubTeam
    LEFT JOIN dbo.SubTeam TTSubTeam (NOLOCK) 
        ON TTSubTeam.SubTeam_No = OH.Transfer_To_SubTeam
      LEFT JOIN dbo.SubTeam STSubTeam (NOLOCK)   
        ON STSubTeam.SubTeam_No = OH.SupplyTransferToSubTeam 
    LEFT JOIN dbo.Store VStore (NOLOCK) 
        ON VStore.Store_No = V.Store_No
    LEFT JOIN dbo.Store RStore (NOLOCK) 
        ON RStore.Store_No = ReceiveLocation.Store_No
    LEFT JOIN dbo.ZoneSubTeam ZST (nolock)
        ON ZST.Zone_ID = RStore.Zone_ID
        AND ZST.Supplier_Store_No = VStore.Store_No
        AND ZST.SubTeam_No = OH.Transfer_SubTeam
    LEFT JOIN dbo.OrderInvoice OI (nolock)
	    ON OH.OrderHeader_ID = OI.OrderHeader_ID
	LEFT JOIN dbo.Users CL (NOLOCK) 
        ON CL.User_ID = OH.ClosedBy 
	LEFT JOIN dbo.Users CL2 (NOLOCK) 
        ON CL2.User_ID = OH.ApprovedBy 
    WHERE OH.OrderHeader_ID = (SELECT Max(OrderHeader_ID) FROM dbo.OrderHeader (NOLOCK) WHERE CreatedBy = @CreatedBy)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoUser] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoUser] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoUser] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfoUser] TO [IRMAReportsRole]
    AS [dbo];

