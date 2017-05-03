SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAPUploads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetAPUploads]
GO

CREATE PROCEDURE [dbo].[GetAPUploads]
     @Region_Code As varchar(3)
AS
	-- ******************************************************************************************************
	-- Procedure: GetAPUploads()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is utilized by the PeopleSoftUploadJob.vb to provide a
	-- delimited data feed to PeopleSoft.
	--
	-- Modification History:
	-- Date       	Init  	TFS   	Comment
	-- 07/14/2009	BBB				Added left joins to Currency table from OrderHeader in
	--								all SQL calls; added CurrencyCode value to output for 
	--								TXN_Currency_cd, Base_Currency, Currency_CD
	-- 09/03/2009	BBB				Added additional left join to Currency table from Store
	--								in all SQL calls; added 4 columns to output;
	-- 09/04/2009	BBB				Corrected Currency join to compare BU against Order instead
	--								of Vendor against Order;
	-- 11/01/2010	MU		13630	Joined 3rd Party Freight temp table to @UploadOrders
	--								to apply the PS Upload delay so the 3PF invoices
	--								upload the same day as the associated product invoices
	-- 12/03/2010	DBS		13776	Add Sales_Account to Primary Key on @SalesAccountCost temp table
	-- 04/06/2011	BBB		1744	modified conversion of SalesAcct to varchar(7); modified output of
	--								@OrderInvoice to be OI.InvoiceCost as SAC charges are done in other logic;
	--								made corrections to DIST_MERH_AMT value calculations based upon misunderstanding;
	-- 2011/12/21	KM		3744	extension change;
	-- 2012/01/31	KM		4484	Change InvoiceCost calculation logic to reference new 4.4 column TotalPaidCost; included
	--								new calls to OIC for Allocated Charges to be added to GROSS_AMT, MERCHANDISE_AMT, and DIST_MERCHANDISE_AMT
	-- 2012/02/01	BBB		4484	Removed addition of AllocatedCharges for PayByInvoice orders as InvoiceCost is business as usual;
	-- 05/28/2013	DN		12488	Removed the CONVERT() funtion to allow the store procedure to return ENTERED_DT field as Date type.
	-- ******************************************************************************************************
BEGIN
	SET NOCOUNT ON  
	
	DECLARE @Error_No             INT,
	        @CurrDate             DATETIME,
	        @ThisPeriodBeginDate  DATETIME,
	        @LastPeriodBeginDate  DATETIME  
	
	SELECT @Error_No = 0 
	
	-- Get this period begin date  
	SELECT @CurrDate = DATEADD(d, -2, GETDATE())
	
	EXEC GetBeginPeriodDate @CurrDate,
	     @BP_Date = @ThisPeriodBeginDate OUTPUT
	
	SELECT @Error_No = @@ERROR  
	
	IF @Error_No <> 0
	BEGIN
	    SET NOCOUNT OFF 
	    RAISERROR (
	        'GetAPUploads, GetBeginPeriodDate (This Period), failed with Error = %d',
	        16,
	        1,
	        @Error_No
	    ) 
	    RETURN
	END 

	-- Get ID for allocated SACType  
	DECLARE @SACAllocType_Id AS INT  
	
	SELECT @SACAllocType_Id = SACType_Id
	FROM   dbo.einvoicing_sactypes (NOLOCK)
	WHERE  SACType = 'Allocated' 
		
	-- Get ID for non-allocated SACType  
	DECLARE @SACType_Id AS INT  
	
	SELECT @SACType_Id = SACType_Id
	FROM   dbo.einvoicing_sactypes (NOLOCK)
	WHERE  SACType = 'Not Allocated' 
	
	-- Get last period begin date  
	SELECT @CurrDate = DATEADD(mi, -1, @ThisPeriodBeginDate)  
	
	EXEC GetBeginPeriodDate @CurrDate,
	     @BP_Date = @LastPeriodBeginDate OUTPUT
	
	SELECT @Error_No = @@ERROR  
	
	IF @Error_No <> 0
	BEGIN
	    SET NOCOUNT OFF 
	    RAISERROR (
	        'GetAPUploads, GetBeginPeriodDate (Last Period), failed with Error = %d',
	        16,
	        1,
	        @Error_No
	    ) 
	    RETURN
	END 
	
	-- Read the regional cofiguration data from the database
	-- Read the PS_SetID value from the InstanceData table   
	DECLARE @PS_SetID AS VARCHAR(10)  
	SELECT @PS_SetID = PS_SetID
	FROM   dbo.InstanceData 
	
	-- Read the PS_OprID and UploadDelay values from the PSUploadConfig table   
	DECLARE @PS_OprID           AS VARCHAR(8),
	        @UploadDelay_Sun    INT,
	        @UploadDelay_Mon    INT,
	        @UploadDelay_Tues   INT,
	        @UploadDelay_Wed    INT,
	        @UploadDelay_Thurs  INT,
	        @UploadDelay_Fri    INT,
	        @UploadDelay_Sat    INT
	
	SELECT @PS_OprID = PS_OPRID,
	       @UploadDelay_Sun = ISNULL(UploadDelay_Sun, 0),
	       @UploadDelay_Mon = ISNULL(UploadDelay_Mon, 0),
	       @UploadDelay_Tues = ISNULL(UploadDelay_Tues, 0),
	       @UploadDelay_Wed = ISNULL(UploadDelay_Wed, 0),
	       @UploadDelay_Thurs = ISNULL(UploadDelay_Thurs, 0),
	       @UploadDelay_Fri = ISNULL(UploadDelay_Fri, 0),
	       @UploadDelay_Sat = ISNULL(UploadDelay_Sat, 0)
	FROM   dbo.PSUploadConfig
	WHERE  Region_Code = @Region_Code 
	
	-- Get list of Orders to be uploaded so we do not have to repeat the nasty conditions later  
	DECLARE @UploadOrders TABLE (OrderHeader_ID INT PRIMARY KEY)  
	
	INSERT INTO @UploadOrders
	SELECT DISTINCT OrderInvoice.OrderHeader_ID
	FROM   OrderInvoice
	       INNER JOIN OrderHeader
	            ON  OrderInvoice.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       INNER JOIN Vendor(NOLOCK)
	            ON  OrderHeader.Vendor_ID = Vendor.Vendor_ID
	       INNER JOIN Vendor ReceiveLocation(NOLOCK)
	            ON  ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID
	       INNER JOIN Store(NOLOCK)
	            ON  Store.Store_No = ReceiveLocation.Store_no
	       INNER JOIN -- This is a new JOIN added to V3 to support multiple regions in one instance of IRMA  
	            StoreRegionMapping(NOLOCK)
	            ON  Store.Store_No = StoreRegionMapping.Store_no
	            AND @Region_Code = StoreRegionMapping.Region_Code
	       INNER JOIN StoreSubTeam(NOLOCK)
	            ON  Store.Store_No = StoreSubTeam.Store_No
	            AND OrderInvoice.SubTeam_No = StoreSubTeam.SubTeam_No
	WHERE  (
	           -- Comparing the DAY for today and the modified RecvLogDate or CloseDate to see if they are the same;
	           -- the OrderHeader is included when they match.
	           -- Changed in V3 to support a regionally configurable delay between the closing of the order and the sending
	           -- of the order to PS instead of having a hard-coded delay based on the day of the week.  
	           DATEDIFF(
	               d,
	               GETDATE(),
	               (
	                   -- Modifies the RecvLogDate or CloseDate based on the weekday
	                   -- SET DATEFIRST command can be used to set the first day of the week.  
	                   CASE DATEPART(dw, ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)) 
	                        -- MONDAY
	                        WHEN 2 THEN DATEADD(
	                                 d,
	                                 @UploadDelay_Mon,
	                                 ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)
	                             ) 
	                             -- TUESDAY
	                        WHEN 3 THEN DATEADD(
	                                 d,
	                                 @UploadDelay_Tues,
	                                 ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)
	                             ) 
	                             -- WEDNESDAY
	                        WHEN 4 THEN DATEADD(
	                                 d,
	                                 @UploadDelay_Wed,
	                                 ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)
	                             ) 
	                             -- THURSDAY
	                        WHEN 5 THEN DATEADD(
	                                 d,
	                                 @UploadDelay_Thurs,
	                                 ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)
	                             ) 
	                             -- FRIDAY
	                        WHEN 6 THEN DATEADD(
	                                 d,
	                                 @UploadDelay_Fri,
	                                 ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)
	                             ) 
	                             -- SATURDAY
	                        WHEN 7 THEN DATEADD(
	                                 d,
	                                 @UploadDelay_Sat,
	                                 ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)
	                             ) 
	                             -- SUNDAY
	                        WHEN 1 THEN DATEADD(
	                                 d,
	                                 @UploadDelay_Sun,
	                                 ISNULL(OrderHeader.RecvLogDate, OrderHeader.CloseDate)
	                             )
	                   END
	               )
	           ) <= 0
	       )
	       AND OrderHeader.Transfer_SubTeam IS NULL
	       AND OrderHeader.CloseDate IS NOT NULL
	       AND OrderHeader.ApprovedDate IS NOT NULL -- Changed in V3; only upload approved orders (not suspended orders)
	       AND OrderHeader.UploadedDate IS NULL
		   AND OrderHeader.RefuseReceivingReasonID IS NULL --Refuse Receiving Orders should not be uploaded TFS 2460
	       AND OrderHeader.InvoiceNumber IS NOT NULL -- only upload orders that were approved with invoice data (not document data)
	       AND (
	               Vendor.PS_Export_Vendor_ID IS NOT NULL
	               AND -- Changed in V3 to support parent-child vendor relationships used by some vendors   
	                   Vendor.PS_Location_Code IS NOT NULL
	               AND Vendor.PS_Address_Sequence IS NOT NULL
	           )
	       AND EXISTS (
	               SELECT *
	               FROM   OrderItem
	               WHERE  (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
	                      AND DATEDIFF(d, OrderHeader.CloseDate, OrderItem.DateReceived) 
	                          >= 0
	           ) 
	
	-- Get list of Orders to be uploaded so we do not have to repeat the nasty conditions later  
	DECLARE @UploadSACOrders    TABLE (
	            OrderHeader_ID INT,
	            OrderSACID INT,
	            SubTeam_No INT,
	            GLPurchaseAcct VARCHAR(7),
	            SACTotal MONEY,
	            PRIMARY KEY(OrderHeader_ID, SubTeam_No, GLPurchaseAcct)
	        )
	
	DECLARE @SACHeaderID        INT,
	        @OrderSACID         INT,
	        @SACSubTeam_No      INT,
	        @SACHeaderID2       INT,
	        @SACGLPurchaseAcct  VARCHAR(7),
	        @SACOrderValue      MONEY
	
	SELECT @SACHeaderID2 = 2  
	DECLARE curSACOrders  CURSOR  
	FOR
	    SELECT UO.OrderHeader_ID,
	           ST         .SubTeam_No,
	           CONVERT    (VARCHAR(7), ST.GLPurchaseAcct),
	           SUM        (
	               OIC.Value * (CASE WHEN oh.Return_Order = 1 THEN -1 ELSE 1 END)
	           )
	    FROM   @UploadOrders UO
	           JOIN dbo.OrderHeader oh
	                ON  oh.OrderHeader_ID = uo.OrderHeader_ID
	           JOIN dbo.OrderInvoiceCharges OIC(NOLOCK)
	                ON  OIC.OrderHeader_ID = UO.OrderHeader_ID
	           JOIN dbo.SubTeam ST(NOLOCK)
	                ON  ST.SubTeam_No = OIC.SubTeam_No
	    WHERE  ST.GLPurchaseAcct IS NOT NULL
	           AND OIC.SACType_Id = @SACType_Id
	    GROUP BY
	           UO.OrderHeader_ID,
	           ST         .SubTeam_No,
	           ST         .GLPurchaseAcct
	    ORDER BY
	           UO.OrderHeader_ID,
	           ST         .SubTeam_No,
	           ST         .GLPurchaseAcct
	
	OPEN curSACOrders 
	FETCH NEXT FROM curSACOrders 
	INTO @SACHeaderID, @SACSubTeam_No, @SACGLPurchaseAcct, @SACOrderValue                                                      
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	    IF @SACHeaderID <> @SACHeaderID2
	    BEGIN
	        SELECT @OrderSACID = 2
	    END
	    ELSE
	    BEGIN
	        SELECT @OrderSACID = @OrderSACID + 1
	    END   
	    
	    INSERT INTO @UploadSACOrders
	      (
	        OrderHeader_ID,
	        OrderSACID,
	        SubTeam_No,
	        GLPurchaseAcct,
	        SACTotal
	      )
	    -- OrderInvoice - Items with Special Charges  
	    SELECT @SACHeaderID,
	           @OrderSACID,
	           @SACSubTeam_No,
	           @SACGLPurchaseAcct,
	           @SACOrderValue
	    
	    SELECT @SACHeaderID2 = @SACHeaderID 
	    
	    FETCH NEXT FROM curSACOrders 
	    INTO @SACHeaderID, @SACSubTeam_No, @SACGLPurchaseAcct, @SACOrderValue
	END 
	CLOSE curSACOrders 
	DEALLOCATE curSACOrders 
	
	-- Create sum of Allocated Charges to be added back to the PO, since those values are no longer consumed as part of the eInvoice (4.4)
	DECLARE @UploadSACAllocOrders    TABLE (
	        OrderHeader_ID INT,
	        SACTotal MONEY,
	        PRIMARY KEY(OrderHeader_ID)
	    )

	INSERT INTO @UploadSACAllocOrders
			SELECT 
				UO.OrderHeader_ID,
				SUM(OIC.Value * (CASE WHEN oh.Return_Order = 1 THEN -1 ELSE 1 END))
			FROM   
				@UploadOrders UO
				   JOIN dbo.OrderHeader oh
						ON  oh.OrderHeader_ID = uo.OrderHeader_ID
				   JOIN dbo.OrderInvoiceCharges OIC(NOLOCK)
						ON  OIC.OrderHeader_ID = UO.OrderHeader_ID
			WHERE
				OIC.SACType_Id = @SACAllocType_Id
				AND ISNULL(OIC.IsAllowance, 0) = 0
				AND oh.PayByAgreedCost = 1
			GROUP BY
				   UO.OrderHeader_ID
			ORDER BY
				   UO.OrderHeader_ID
	
	-- Add up received cost for items with Sales Accounts and treat as the invoice cost - must be separated in AP  
	DECLARE @SalesAccountCost TABLE (
	            OrderHeader_ID INT,
	            Sales_Account VARCHAR(7),
	            PS_DEPT INT,
	            PS_PRODUCT INT,
	            SubTeam_No INT,
	            InvoiceCost SMALLMONEY,
	            InvoiceFreight SMALLMONEY,
	            InvoiceNumber VARCHAR(20) PRIMARY KEY NONCLUSTERED(OrderHeader_ID, Sales_Account, SubTeam_No)
	        )  
	
	INSERT INTO @SalesAccountCost
	SELECT OrderHeader.OrderHeader_ID,
	       Item.Sales_Account,
	       NULL,
	       NULL,
	       OrderHeader.Transfer_To_SubTeam,
	       SUM(OrderItem.ReceivedItemCost),
	       SUM(OrderItem.ReceivedItemFreight),
	       OrderHeader.InvoiceNumber
	FROM   OrderHeader
	       INNER JOIN @UploadOrders UO
	            ON  OrderHeader.OrderHeader_ID = UO.OrderHeader_ID
	       INNER JOIN OrderItem
	            ON  (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
	       INNER JOIN Item
	            ON  (OrderItem.Item_Key = Item.Item_Key)
	WHERE  Item.Sales_Account IS NOT NULL
	GROUP BY
	       OrderHeader.OrderHeader_ID,
	       Item.Sales_Account,
	       OrderHeader.Transfer_To_SubTeam,
	       OrderHeader.InvoiceNumber 
	
	
	-- Final OrderInvoice data using Sales Account stuff from above and the actual OrderInvoice table  
	DECLARE @OrderInvoice TABLE (
	            OrderHeader_ID INT,
	            Sales_Account VARCHAR(7),
	            PS_DEPT INT,
	            PS_PRODUCT INT,
	            SubTeam_No INT,
	            InvoiceCost SMALLMONEY,
	            InvoiceFreight SMALLMONEY,
	            InvoiceNumber VARCHAR(20)
	        )  
	
	INSERT INTO @OrderInvoice
	-- OrderInvoice without OrderItems that have Sales Accounts  
	SELECT OI.OrderHeader_ID,
	       CASE OrderHeader.ProductType_ID
				WHEN 1 THEN ISNULL(CONVERT(varchar(7), SubTeam.GLPurchaseAcct), '500000')
				WHEN 2 THEN ISNULL(CONVERT(varchar(7), SubTeam.GLPackagingAcct), '510000')
				WHEN 3 THEN ISNULL(CONVERT(varchar(7), SubTeam.GLSuppliesAcct), '800000')
			ELSE '0' END,
			CASE WHEN dbo.fn_GLAcctIncludesTeamSubteam(SubTeamType_ID, OrderHeader.ProductType_ID, OI.OrderHeader_ID) = 1 THEN StoreSubTeam.PS_Team_No ELSE NULL END,
            CASE WHEN dbo.fn_GLAcctIncludesTeamSubteam(SubTeamType_ID, OrderHeader.ProductType_ID, OI.OrderHeader_ID) = 1 THEN StoreSubTeam.PS_SubTeam_No ELSE NULL END,
	        OI.SubTeam_No,
	        (CASE
				WHEN (OrderHeader.Return_Order = 0) AND (OrderHeader.PayByAgreedCost = 1) THEN 						
					CASE 
						WHEN (OrderHeader.eInvoice_ID IS NOT NULL) THEN 
							(
								SELECT
									TotalPaidCost
								FROM
									OrderHeader (nolock) oh
								WHERE
									UO.OrderHeader_ID = oh.OrderHeader_ID 
							)
						ELSE 
							CASE 
								WHEN ROUND(ReceivedItemCostTotal - ISNULL(SA.SA_InvoiceCost, 0), 2) <  ROUND(OI.InvoiceCost  - ISNULL(SA.SA_InvoiceCost, 0), 2) THEN 
									ROUND(ReceivedItemCostTotal - ISNULL(SA.SA_InvoiceCost, 0), 2)
								ELSE ROUND(OI.InvoiceCost  - ISNULL(SA.SA_InvoiceCost, 0), 2)
							END
					END
				ELSE ROUND(OI.InvoiceCost  - ISNULL(SA.SA_InvoiceCost, 0), 2) 
			END)  AS InvoiceCost,
	        (OI.InvoiceFreight - ISNULL(SA.SA_InvoiceFreight, 0)) AS InvoiceFreight,
	        OrderHeader.InvoiceNumber
	FROM   dbo.OrderInvoice OI (NOLOCK)
			INNER JOIN @UploadOrders UO
				ON  OI.OrderHeader_ID = UO.OrderHeader_ID
			LEFT JOIN -- OrderItems with Sales Account so amount can be subtracted  
				(
					SELECT OrderHeader_ID,
						   SubTeam_No,
						   SUM(InvoiceCost) AS SA_InvoiceCost,
						   SUM(InvoiceFreight) AS SA_InvoiceFreight
					FROM   @SalesAccountCost
					GROUP BY
						   OrderHeader_ID,
						   SubTeam_No
				) SA
				ON  SA.OrderHeader_ID = OI.OrderHeader_ID
				AND SA.SubTeam_No = OI.SubTeam_No
			INNER JOIN dbo.SubTeam (NOLOCK)
				ON  SubTeam.SubTeam_No = OI.SubTeam_No      
			INNER JOIN 
				(SELECT OrderHeader.OrderHeader_ID, Vendor_ID, ReceiveLocation_ID, ProductType_ID, Return_Order, SentDate, 
                SUM(ReceivedItemCost) As ReceivedItemCostTotal, PayByAgreedCost, eInvoice_ID, SupplyTransferToSubTeam, InvoiceNumber
				FROM OrderHeader (nolock)
				INNER JOIN OrderItem (nolock) ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
				GROUP BY OrderHeader.OrderHeader_ID, Vendor_ID, ReceiveLocation_ID, ProductType_ID, Return_Order, SentDate, PayByAgreedCost, eInvoice_ID, SupplyTransferToSubTeam, InvoiceNumber) OrderHeader
				ON OrderHeader.OrderHeader_ID = OI.OrderHeader_ID
	       INNER JOIN Vendor ReceiveLocation(NOLOCK)
	            ON  ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID
	       INNER JOIN Store(NOLOCK)
	            ON  Store.Store_No = ReceiveLocation.Store_no
			INNER JOIN
				StoreSubTeam (NOLOCK)
				ON Store.Store_No = StoreSubTeam.Store_No AND 
				   ISNULL(OrderHeader.SupplyTransferToSubTeam, OI.SubTeam_No) = StoreSubTeam.SubTeam_No    
	WHERE  ((OI.InvoiceCost - ISNULL(SA.SA_InvoiceCost, 0)) <> 0)
	       OR  ((OI.InvoiceFreight - ISNULL(SA.SA_InvoiceFreight, 0)) <> 0)
	       OR  (OI.InvoiceCost = 0 AND ISNULL(SA.SA_InvoiceCost, 0) = 0) 
	UNION
	-- OrderItem totals for OrderItems with Sales Accounts  
	SELECT OrderHeader_ID,
	       Sales_Account,
	       PS_DEPT,
	       PS_PRODUCT,
	       SubTeam_No,
	       InvoiceCost,
	       InvoiceFreight,
	       InvoiceNumber
	FROM   @SalesAccountCost 
	
	-- Changed in V3: Populate a temp table with the third party freight invoices that are being uploaded
	-- Include all 3rd party freight invoices that have not already been uploaded in this file  
	DECLARE @OrderInvoiceFreight3Party TABLE (
	            OrderHeader_ID INT,
	            Sales_Account VARCHAR(7),
	            PS_DEPT INT,
	            PS_PRODUCT INT,
	            SubTeam_No INT,
	            InvoiceCost SMALLMONEY,
	            InvoiceFreight SMALLMONEY,
	            InvoiceNumber VARCHAR(20),
	            InvoiceDate SMALLDATETIME,
	            Vendor_ID INT
	        )
	
	INSERT INTO @OrderInvoiceFreight3Party
	SELECT OI.OrderHeader_ID,
	       CASE OH.ProductType_ID
				WHEN 1 THEN ISNULL(CONVERT(varchar(7), ST.GLPurchaseAcct), '500000')
				WHEN 2 THEN ISNULL(CONVERT(varchar(7), ST.GLPackagingAcct), '510000')
				WHEN 3 THEN ISNULL(CONVERT(varchar(7), ST.GLSuppliesAcct), '800000')
			ELSE '0' END,
			CASE WHEN dbo.fn_GLAcctIncludesTeamSubteam(SubTeamType_ID, OH.ProductType_ID, OI.OrderHeader_ID) = 1 THEN StoreSubTeam.PS_Team_No ELSE NULL END,
            CASE WHEN dbo.fn_GLAcctIncludesTeamSubteam(SubTeamType_ID, OH.ProductType_ID, OI.OrderHeader_ID) = 1 THEN StoreSubTeam.PS_SubTeam_No ELSE NULL END,
	       OH.Transfer_To_SubTeam,
	       ISNULL(OI.InvoiceCost, 0) AS InvoiceCost,
	       0 AS InvoiceFreight,
	       OI.InvoiceNumber,
	       OI.InvoiceDate,
	       OI.Vendor_ID
	FROM   dbo.OrderInvoice_Freight3Party OI
	       INNER JOIN dbo.OrderHeader OH
	            ON  OH.OrderHeader_ID = OI.OrderHeader_ID
	       INNER JOIN dbo.SubTeam ST
	            ON  ST.SubTeam_No = OH.Transfer_To_SubTeam
	       INNER JOIN Vendor ReceiveLocation(NOLOCK)
	            ON  ReceiveLocation.Vendor_ID = OH.ReceiveLocation_ID
	       INNER JOIN Store(NOLOCK)
	            ON  Store.Store_No = ReceiveLocation.Store_no
		   INNER JOIN
				StoreSubTeam (NOLOCK)
				ON Store.Store_No = StoreSubTeam.Store_No AND 
				   OH.Transfer_To_SubTeam = StoreSubTeam.SubTeam_No   
		   INNER JOIN @UploadOrders UO --per bug 13630, 3PF invoices need to upload the same day as the product invoices
	            ON  OH.OrderHeader_ID = UO.OrderHeader_ID   
	WHERE OI.UploadedDate IS NULL
	
	
	-- Main query
	-- Return the invoice data for APPROVED orders - Header Row  
	SELECT '000' AS VCHR_HDR_ROW_ID,
	       Store.BusinessUnit_ID AS BUSINESS_UNIT_ID,
	       NULL AS VOUCHER_ID,
	       OrderInvoice.InvoiceNumber AS INVOICE_ID,
	       OrderHeader.INVOICEDATE AS INVOICE_DT,
	       @PS_SetID AS VENDOR_SETID,	-- Changed in V3 to remove hard-coded US value  
	       Vendor.PS_Export_Vendor_ID AS VENDOR_ID,	-- Changed in V3 to support parent-child vendor relationships used by some vendors   
	       'DEFAULT' AS VNDR_LOC,
	       Vendor.PS_Address_Sequence AS ADDRESS_SEQ_NUM,
	       NULL AS GRP_AP_ID,
	       @Region_Code AS ORIGIN,	-- Changed in V3 to remove hard-coded region value; supports multiple regions in one instance of IRMA   
	       ISNULL(@PS_OprID, 'IRMA') AS OPRID,	-- Changed in V3 to remove hard-coded region value; IRMA was the value supplied by the PS team  
	       1 AS VCHR_TTL_LINES,
	       -- If the Order CloseDate is not in this period and it is the first Wed. or after in this period
	       -- or if the Close date is older than last period
	       -- report the current date rather than the CloseDate as the Accounting Date  
	       CASE 
	            WHEN CLOSEDATE < @ThisPeriodBeginDate THEN GETDATE()
	            ELSE CLOSEDATE
	       END AS ACCOUNTING_DT,
	       NULL AS POST_VOUCHER,
	       NULL AS DST_CNTRL_ID,
	       NULL AS VOUCHER_ID_RELATED,
	       (
	           SELECT
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACO.SACTotal), 0)
	                          FROM   @UploadSACOrders SACO
	                          WHERE  OrderHeader.OrderHeader_ID = SACO.OrderHeader_ID
	                      ),
	                      2
	                  ) +
	                  SUM(
	                      ROUND(
	                          OrdInv.InvoiceCost * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      ) + ROUND(
	                          OrdInv.InvoiceFreight * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      )
	                  ) +
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACA.SACTotal), 0)
	                          FROM   @UploadSACAllocOrders SACA
	                          WHERE  OrderHeader.OrderHeader_ID = SACA.OrderHeader_ID
	                      ),
	                      2
	                  )
	           FROM   @OrderInvoice OrdInv
	                  INNER JOIN OrderHeader OH
	                       ON  OH.OrderHeader_ID = OrdInv.OrderHeader_ID
	           WHERE  OH.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       ) AS GROSS_AMT,
	       NULL AS DSCNT_AMT,
	       NULL AS USETAX_CD,
	       NULL AS SALETX_AMT,
	       NULL AS SALETX_CD,
	       NULL AS FREIGHT_AMT,
	       NULL AS DUE_DT,
	       NULL AS DSCNT_DUE_DT,
	       NULL AS PYMNT_TERMS_CD,
	       GETDATE() AS ENTERED_DT,
	       Currency.CurrencyCode AS TXN_CURRENCY_CD,
	       NULL AS RT_TYPE,
	       NULL AS RATE_MULT,
	       NULL AS RATE_DIV,
	       NULL AS VAT_ENTRD_AMT,
	       NULL AS MATCH_ACTION,
	       NULL AS MATCH_STATUS_VCHR,
	       NULL AS BCM_TRAN_TYPE,
	       NULL AS CNTRCT_ID,
	       Vendor.PS_Address_Sequence AS REMIT_ADDR_SEQ_NUM,
	       NULL AS CUR_RT_SOURCE,
	       NULL AS DSCNT_AMT_FLG,
	       NULL AS DUE_DT_FLG,
	       NULL AS VCHR_APPRVL_FLG,
	       NULL AS BUSPROCNAME,
	       NULL AS APPR_RULE_SET,
	       NULL AS VAT_DCLRTN_POINT,
	       NULL AS VAT_CALC_TYPE,
	       NULL AS VAT_ENTITY,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS TAX_CD_VAT,
	       NULL AS VAT_RCRD_INPT_FLG,
	       NULL AS VAT_RCRD_OUTPT_FLG,
	       NULL AS VAT_RECOVERY_PCT,
	       NULL AS VAT_CALC_GROSS_NET,
	       NULL AS VAT_RECALC_FLG,
	       NULL AS VAT_CALC_FRGHT_FLG,
	       NULL AS VAT_RGSTRN_SELLER,
	       NULL AS COUNTRY_SHIP_FROM,
	       NULL AS COUNTRY_SHIP_TO,
	       NULL AS COUNTRY_VAT_BILLFR,
	       NULL AS COUNTRY_VAT_BILLTO,
	       NULL AS VAT_TREATMENT_PUR,
	       NULL AS VAT_EXCPTN_TYPE,
	       NULL AS VAT_EXCPTN_CERTIF,
	       NULL AS VAT_USE_ID,
	       NULL AS DSCNT_PRORATE_FLG,
	       NULL AS USETAX_PRORATE_FLG,
	       NULL AS SALETX_PRORATE_FLG,
	       NULL AS FRGHT_PRORATE_FLG,
	       NULL AS IST_TXN_FLG,
	       NULL AS DOC_TYPE,
	       NULL AS DOC_SEQ_DATE,
	       NULL AS DOC_SEQ_NBR,
	       NULL AS VAT_CF_ANLSYS_TYPE,
	       OrderHeader.OrderHeader_ID AS DESCR254_MIXED,	--TFS 6926 Add PO_ID to AP Upload File  
	       '001' AS VCHR_LINE_ROW_ID,
	       Store.BusinessUnit_ID AS LINE_BUSINESS_UNIT_ID,
	       NULL AS LINE_VOUCHER_ID,
	       1 AS VOUCHER_LINE_NUM,
	       (
	           (
	               SELECT COUNT(*)
	               FROM   @OrderInvoice TOI
	               WHERE  TOI.OrderHeader_ID = OrderHeader.OrderHeader_ID
	           ) 
	           + (
	               SELECT COUNT(*)
	               FROM   @UploadSACOrders SACO
	               WHERE  OrderHeader.OrderHeader_ID = SACO.OrderHeader_ID
	           )
	       ) AS TOTAL_DISTRIBS,
	       NULL AS BUSINESS_UNIT_PO,
	       NULL AS PO_ID,
	       NULL AS LINE_NBR,
	       NULL AS SCHED_NBR,
	       NULL AS DESCR,
	       (
	           SELECT 
			   ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACO.SACTotal), 0)
	                          FROM   @UploadSACOrders SACO
	                          WHERE  OrderHeader.OrderHeader_ID = SACO.OrderHeader_ID
	                      ),
	                      2
	                  ) + 
					  SUM(
	                      ROUND(
	                          OrdInv.InvoiceCost * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      ) + ROUND(
	                          OrdInv.InvoiceFreight * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      )
	                  ) +
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACA.SACTotal), 0)
	                          FROM   @UploadSACAllocOrders SACA
	                          WHERE  OrderHeader.OrderHeader_ID = SACA.OrderHeader_ID
	                      ),
	                      2
	                  )
	           FROM   @OrderInvoice OrdInv
	                  INNER JOIN OrderHeader OH
	                       ON  OH.OrderHeader_ID = OrdInv.OrderHeader_ID
	           WHERE  OH.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       ) AS MERCHANDISE_AMT,
	       NULL AS ITEM_SETID,
	       NULL AS INV_ITEM_ID,
	       NULL AS QTY_VCHR,
	       NULL AS STATISTIC_AMT,
	       NULL AS UNIT_OF_MEASURE,
	       NULL AS UNIT_PRICE,
	       NULL AS SALETX_APPL_FLG,
	       NULL AS USETAX_APPL_FLG,
	       NULL AS FRGHT_PRORATE_FLG,
	       NULL AS DSCNT_APPL_FLG,
	       NULL AS WTHD_SW,
	       NULL AS TAX_CD_VAT,
	       NULL AS VAT_RECOVERY_PCT,
	       NULL AS BUSINESS_UNIT_RECV,
	       NULL AS RECEIVER_ID,
	       NULL AS RECV_LN_NBR,
	       NULL AS RECV_SHIP_SEQ_NBR,
	       NULL AS MATCH_LINE_OPT,
	       NULL AS DISTRIB_MTHD_FLG,
	       Currency.CurrencyCode AS TXN_CURRENCY_CD,
	       Currency.CurrencyCode AS BASE_CURRENCY,
	       Currency.CurrencyCode AS CURRENCY_CD,
	       NULL AS SHIPTO_ID,
	       NULL AS SUT_BASE_ID,
	       NULL AS TAX_CD_SUT,
	       NULL AS SUT_EXCPTN_TYPE,
	       NULL AS SUT_EXCPTN_CERTIF,
	       NULL AS SUT_APPLICABILITY,
	       @PS_SetID AS WTHD_SETID,	-- Changed in V3 to remove hard-coded US value  
	       NULL AS WTHD_CD,
	       NULL AS VAT_APPL_FLG,
	       NULL AS VAT_APPLICABILITY,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS NATURE_OF_TXN1,
	       NULL AS NATURE_OF_TXN2,
	       NULL AS VAT_USE_ID,
	       '002' AS VCHR_DIST_ROW_ID,
	       Store.BusinessUnit_ID AS DIST_BUSINESS_UNIT_ID,
	       NULL AS DIST_VOUCHER_ID,
	       1 AS DIST_VOUCHER_LINE_NUM,
	       (
	           SELECT COUNT(*) + 1
	           FROM   @OrderInvoice TOI
	           WHERE  TOI.OrderHeader_ID = OrderHeader.OrderHeader_ID
	                  AND (
	                          (TOI.SubTeam_No < OrderInvoice.SubTeam_No)
	                          OR (TOI.Sales_Account < OrderInvoice.Sales_Account)
	                      )
	       ) AS DISTRIB_LINE_NUM,
	       Store.BusinessUnit_ID AS BUSINESS_UNIT_GL,
	       OrderInvoice.Sales_Account AS ACCOUNT,
	       NULL AS STATISTIC_CODE,
	       NULL AS STATISTIC_AMOUNT,
	       NULL AS QTY_VCHR,
	       NULL AS JRNL_LN_REF,
	       NULL AS OPEN_ITEM_STATUS,
	       NULL AS DISTRIB_DESCR,
	       (
	           ROUND(
	               OrderInvoice.InvoiceCost * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           ) + ROUND(
	               OrderInvoice.InvoiceFreight * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           ) +
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACA.SACTotal), 0)
	                          FROM   @UploadSACAllocOrders SACA
	                          WHERE  OrderHeader.OrderHeader_ID = SACA.OrderHeader_ID
	                      ),
	                      2
	                  )
	       ) AS DIST_MERCHANDISE_AMT,
	       NULL AS BUSINESS_UNIT_PO,
	       NULL AS DISTRIB_PO_ID,
	       NULL AS DISTRIB_LINE_NBR,
	       NULL AS DISTRIB_SCHED_NBR,
	       NULL AS PO_DIST_LINE_NUM,
	       NULL AS BUSINESS_UNIT_PC,
	       NULL AS ACTIVITY_ID,
	       NULL AS ANALYSIS_TYPE,
	       NULL AS RESOURCE_TYPE,
	       NULL AS RESOURCE_CATEGORY,
	       NULL AS RESOURCE_SUB_CAT,
	       NULL AS ASSET_FLG,
	       NULL AS BUSINESS_UNIT_AM,
	       NULL AS ASSET_ID,
	       NULL AS PROFILE_ID,
	       NULL AS FREIGHT_AMT,
	       NULL AS SALETX_AMT,
	       NULL AS USETAX_AMT,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS VAT_INV_AMT,
	       NULL AS VAT_NONINV_AMT,
	       NULL AS BUSINESS_UNIT_RECV,
	       NULL AS RECEIVER_ID,
	       NULL AS RECV_LN_NBR,
	       NULL AS RECV_SHIP_SEQ_NBR,
	       NULL AS RECV_DIST_LINE_NUM,
	       /* 
	       This code was modified for V3.4 to support sending packaging and supplies to PS
	       --CASE 
	       --     WHEN Sales_Account = '500000' THEN StoreSubTeam.PS_Team_No -- Changed in V3 to return the PS department (called team in IRMA) value; PS values do not have to match IRMA values
	       --     ELSE NULL
	       --END AS DEPTID,
	       --CASE 
	       --     WHEN Sales_Account = '500000' THEN StoreSubTeam.PS_SubTeam_No -- Changed in V3 to return the PS product (called subteam in IRMA) value; PS values do not have to match IRMA values
	       --     ELSE NULL
	       --END AS PRODUCT,
	       */
	       PS_DEPT AS DEPTID,
	       PS_PRODUCT AS PRODUCT,
	       NULL AS PROEJCT_ID,
	       NULL AS AFFILIATE,
	       NULL AS VAT_APORT_CNTRL,
	       OrderHeader.OrderHeader_ID,
	       0 AS Freight3Party,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'006'
		   END AS VCHR_CURR_ROW_ID,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				Store.BusinessUnit_ID
		   END AS CURR_BUSINESS_UNIT_ID,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'USD'
		   END AS CURR_VENDOR_CODE,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'ROYAL'
		   END AS CURR_BU_CODE
	FROM   @OrderInvoice AS OrderInvoice
	       INNER JOIN OrderHeader
	            ON  OrderInvoice.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       INNER JOIN Vendor(NOLOCK)
	            ON  OrderHeader.Vendor_ID = Vendor.Vendor_ID
	       INNER JOIN Vendor ReceiveLocation(NOLOCK)
	            ON  ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID
	       INNER JOIN Store(NOLOCK)
	            ON  Store.Store_No = ReceiveLocation.Store_no
	       INNER JOIN StoreSubTeam(NOLOCK)
	            ON  Store.Store_No = StoreSubTeam.Store_No
	            AND OrderInvoice.SubTeam_No = StoreSubTeam.SubTeam_No 
	       LEFT JOIN Currency (NOLOCK)
				ON  OrderHeader.CurrencyID = Currency.CurrencyID
		   LEFT JOIN Currency (NOLOCK) Currency2
				ON  ReceiveLocation.CurrencyID = Currency2.CurrencyID
	-- Changed in V3 to include 3rd party freight invoices    -- Changed in V3 to include 3rd party freight invoices  
	UNION 
	-- Return the invoice data for 3rd party freight invoices;  this processing is outside of the ordering workflow.
	-- Any pending invoices are sent to PeopleSoft by the next run of the AP Upload job, regardless of the state of the associated order.  
	SELECT '000' AS VCHR_HDR_ROW_ID,
	       Store.BusinessUnit_ID AS BUSINESS_UNIT_ID,
	       NULL AS VOUCHER_ID,
	       OrderInvoice.InvoiceNumber AS INVOICE_ID,
	       OrderInvoice.InvoiceDate AS INVOICE_DT,
	       @PS_SetID AS VENDOR_SETID,
	       Vendor.PS_Export_Vendor_ID AS VENDOR_ID,
	       'DEFAULT' AS VNDR_LOC,
	       Vendor.PS_Address_Sequence AS ADDRESS_SEQ_NUM,
	       NULL AS GRP_AP_ID,
	       @Region_Code AS ORIGIN,
	       ISNULL(@PS_OprID, 'IRMA') AS OPRID,
	       1 AS VCHR_TTL_LINES,
	       -- If the Order CloseDate is not in this period and it is the first Wed. or after in this period
	       -- or if the Close date is older than last period
	       -- report the current date rather than the CloseDate as the Accounting Date  
	       CASE 
	            WHEN ISNULL(OrderHeader.CloseDate, GETDATE()) < @ThisPeriodBeginDate THEN 
	                 GETDATE()
	            ELSE ISNULL(OrderHeader.CloseDate, GETDATE())
	       END AS ACCOUNTING_DT,
	       NULL AS POST_VOUCHER,
	       NULL AS DST_CNTRL_ID,
	       NULL AS VOUCHER_ID_RELATED,
	       (
	           ROUND(
	               OrderInvoice.InvoiceCost * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           ) 
	           + ROUND(
	               OrderInvoice.InvoiceFreight * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           )
	       ) AS GROSS_AMT,
	       NULL AS DSCNT_AMT,
	       NULL AS USETAX_CD,
	       NULL AS SALETX_AMT,
	       NULL AS SALETX_CD,
	       NULL AS FREIGHT_AMT,
	       NULL AS DUE_DT,
	       NULL AS DSCNT_DUE_DT,
	       NULL AS PYMNT_TERMS_CD,
	       GETDATE() AS ENTERED_DT,
	       Currency.CurrencyCode AS TXN_CURRENCY_CD,
	       NULL AS RT_TYPE,
	       NULL AS RATE_MULT,
	       NULL AS RATE_DIV,
	       NULL AS VAT_ENTRD_AMT,
	       NULL AS MATCH_ACTION,
	       NULL AS MATCH_STATUS_VCHR,
	       NULL AS BCM_TRAN_TYPE,
	       NULL AS CNTRCT_ID,
	       Vendor.PS_Address_Sequence AS REMIT_ADDR_SEQ_NUM,
	       NULL AS CUR_RT_SOURCE,
	       NULL AS DSCNT_AMT_FLG,
	       NULL AS DUE_DT_FLG,
	       NULL AS VCHR_APPRVL_FLG,
	       NULL AS BUSPROCNAME,
	       NULL AS APPR_RULE_SET,
	       NULL AS VAT_DCLRTN_POINT,
	       NULL AS VAT_CALC_TYPE,
	       NULL AS VAT_ENTITY,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS TAX_CD_VAT,
	       NULL AS VAT_RCRD_INPT_FLG,
	       NULL AS VAT_RCRD_OUTPT_FLG,
	       NULL AS VAT_RECOVERY_PCT,
	       NULL AS VAT_CALC_GROSS_NET,
	       NULL AS VAT_RECALC_FLG,
	       NULL AS VAT_CALC_FRGHT_FLG,
	       NULL AS VAT_RGSTRN_SELLER,
	       NULL AS COUNTRY_SHIP_FROM,
	       NULL AS COUNTRY_SHIP_TO,
	       NULL AS COUNTRY_VAT_BILLFR,
	       NULL AS COUNTRY_VAT_BILLTO,
	       NULL AS VAT_TREATMENT_PUR,
	       NULL AS VAT_EXCPTN_TYPE,
	       NULL AS VAT_EXCPTN_CERTIF,
	       NULL AS VAT_USE_ID,
	       NULL AS DSCNT_PRORATE_FLG,
	       NULL AS USETAX_PRORATE_FLG,
	       NULL AS SALETX_PRORATE_FLG,
	       NULL AS FRGHT_PRORATE_FLG,
	       NULL AS IST_TXN_FLG,
	       NULL AS DOC_TYPE,
	       NULL AS DOC_SEQ_DATE,
	       NULL AS DOC_SEQ_NBR,
	       NULL AS VAT_CF_ANLSYS_TYPE,
	       OrderHeader.RecvLog_No AS DESCR254_MIXED,
	       '001' AS VCHR_LINE_ROW_ID,
	       Store.BusinessUnit_ID AS LINE_BUSINESS_UNIT_ID,
	       NULL AS LINE_VOUCHER_ID,
	       1 AS VOUCHER_LINE_NUM,
	       (
	           SELECT COUNT(*)
	           FROM   @OrderInvoiceFreight3Party TOI
	           WHERE  TOI.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       ) AS TOTAL_DISTRIBS,
	       NULL AS BUSINESS_UNIT_PO,
	       NULL AS PO_ID,
	       NULL AS LINE_NBR,
	       NULL AS SCHED_NBR,
	       NULL AS DESCR,
	       (
	           ROUND(
	               OrderInvoice.InvoiceCost * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           ) 
	           + ROUND(
	               OrderInvoice.InvoiceFreight * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           )
	       ) AS MERCHANDISE_AMT,
	       NULL AS ITEM_SETID,
	       NULL AS INV_ITEM_ID,
	       NULL AS QTY_VCHR,
	       NULL AS STATISTIC_AMT,
	       NULL AS UNIT_OF_MEASURE,
	       NULL AS UNIT_PRICE,
	       NULL AS SALETX_APPL_FLG,
	       NULL AS USETAX_APPL_FLG,
	       NULL AS FRGHT_PRORATE_FLG,
	       NULL AS DSCNT_APPL_FLG,
	       NULL AS WTHD_SW,
	       NULL AS TAX_CD_VAT,
	       NULL AS VAT_RECOVERY_PCT,
	       NULL AS BUSINESS_UNIT_RECV,
	       NULL AS RECEIVER_ID,
	       NULL AS RECV_LN_NBR,
	       NULL AS RECV_SHIP_SEQ_NBR,
	       NULL AS MATCH_LINE_OPT,
	       NULL AS DISTRIB_MTHD_FLG,
	       Currency.CurrencyCode AS TXN_CURRENCY_CD,
	       Currency.CurrencyCode AS BASE_CURRENCY,
	       Currency.CurrencyCode AS CURRENCY_CD,
	       NULL AS SHIPTO_ID,
	       NULL AS SUT_BASE_ID,
	       NULL AS TAX_CD_SUT,
	       NULL AS SUT_EXCPTN_TYPE,
	       NULL AS SUT_EXCPTN_CERTIF,
	       NULL AS SUT_APPLICABILITY,
	       @PS_SetID AS WTHD_SETID,
	       NULL AS WTHD_CD,
	       NULL AS VAT_APPL_FLG,
	       NULL AS VAT_APPLICABILITY,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS NATURE_OF_TXN1,
	       NULL AS NATURE_OF_TXN2,
	       NULL AS VAT_USE_ID,
	       '002' AS VCHR_DIST_ROW_ID,
	       Store.BusinessUnit_ID AS DIST_BUSINESS_UNIT_ID,
	       NULL AS DIST_VOUCHER_ID,
	       1 AS DIST_VOUCHER_LINE_NUM,
	       (
	           SELECT COUNT(*) + 1
	           FROM   @OrderInvoiceFreight3Party TOI
	           WHERE  TOI.OrderHeader_ID = OrderHeader.OrderHeader_ID
	                  AND (
	                          (TOI.SubTeam_No < OrderHeader.Transfer_To_SubTeam)
	                          OR (TOI.Sales_Account < OrderInvoice.Sales_Account)
	                      )
	       ) AS DISTRIB_LINE_NUM,
	       Store.BusinessUnit_ID AS BUSINESS_UNIT_GL,
	       OrderInvoice.Sales_Account AS ACCOUNT,
	       NULL AS STATISTIC_CODE,
	       NULL AS STATISTIC_AMOUNT,
	       NULL AS QTY_VCHR,
	       NULL AS JRNL_LN_REF,
	       NULL AS OPEN_ITEM_STATUS,
	       NULL AS DISTRIB_DESCR,
	       (
	           ROUND(
	               OrderInvoice.InvoiceCost * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           ) 
	           + ROUND(
	               OrderInvoice.InvoiceFreight * (CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END),
	               2
	           )
	       ) AS DIST_MERCHANDISE_AMT,
	       NULL AS BUSINESS_UNIT_PO,
	       NULL AS DISTRIB_PO_ID,
	       NULL AS DISTRIB_LINE_NBR,
	       NULL AS DISTRIB_SCHED_NBR,
	       NULL AS PO_DIST_LINE_NUM,
	       NULL AS BUSINESS_UNIT_PC,
	       NULL AS ACTIVITY_ID,
	       NULL AS ANALYSIS_TYPE,
	       NULL AS RESOURCE_TYPE,
	       NULL AS RESOURCE_CATEGORY,
	       NULL AS RESOURCE_SUB_CAT,
	       NULL AS ASSET_FLG,
	       NULL AS BUSINESS_UNIT_AM,
	       NULL AS ASSET_ID,
	       NULL AS PROFILE_ID,
	       NULL AS FREIGHT_AMT,
	       NULL AS SALETX_AMT,
	       NULL AS USETAX_AMT,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS VAT_INV_AMT,
	       NULL AS VAT_NONINV_AMT,
	       NULL AS BUSINESS_UNIT_RECV,
	       NULL AS RECEIVER_ID,
	       NULL AS RECV_LN_NBR,
	       NULL AS RECV_SHIP_SEQ_NBR,
	       NULL AS RECV_DIST_LINE_NUM,
	       /* 
	       This code was modified for V3.4 to support sending packaging and supplies to PS
	       --CASE 
	       --     WHEN Sales_Account = '500000' THEN StoreSubTeam.PS_Team_No -- Changed in V3 to return the PS department (called team in IRMA) value; PS values do not have to match IRMA values
	       --     ELSE NULL
	       --END AS DEPTID,
	       --CASE 
	       --     WHEN Sales_Account = '500000' THEN StoreSubTeam.PS_SubTeam_No -- Changed in V3 to return the PS product (called subteam in IRMA) value; PS values do not have to match IRMA values
	       --     ELSE NULL
	       --END AS PRODUCT,
	       */
	       PS_DEPT AS DEPTID,
	       PS_PRODUCT AS PRODUCT,
	       NULL AS PROEJCT_ID,
	       NULL AS AFFILIATE,
	       NULL AS VAT_APORT_CNTRL,
	       OrderHeader.OrderHeader_ID,
	       1 AS Freight3Party,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'006'
		   END AS VCHR_CURR_ROW_ID,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				Store.BusinessUnit_ID
		   END AS CURR_BUSINESS_UNIT_ID,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'USD'
		   END AS CURR_VENDOR_CODE,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'ROYAL'
		   END AS CURR_BU_CODE
	FROM   @OrderInvoiceFreight3Party AS OrderInvoice
	       INNER JOIN dbo.OrderHeader
	            ON  OrderInvoice.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       INNER JOIN dbo.Vendor (NOLOCK)
	            ON  OrderInvoice.Vendor_ID = Vendor.Vendor_ID
	       INNER JOIN dbo.Vendor ReceiveLocation(NOLOCK)
	            ON  ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID
	       INNER JOIN dbo.Store (NOLOCK)
	            ON  Store.Store_No = ReceiveLocation.Store_no
	       INNER JOIN dbo.StoreSubTeam (NOLOCK)
	            ON  Store.Store_No = StoreSubTeam.Store_No
	            AND OrderHeader.Transfer_To_SubTeam = StoreSubTeam.SubTeam_No 
	       LEFT JOIN Currency (NOLOCK)
				ON  OrderHeader.CurrencyID = Currency.CurrencyID
		   LEFT JOIN Currency (NOLOCK) Currency2
				ON  ReceiveLocation.CurrencyID = Currency2.CurrencyID
	--    -- Return the SAC Invoice orders  
	UNION  
	SELECT '000' AS VCHR_HDR_ROW_ID,
	       Store.BusinessUnit_ID AS BUSINESS_UNIT_ID,
	       NULL AS VOUCHER_ID,
	       OrderInvoice.InvoiceNumber AS INVOICE_ID,
	       OrderHeader.INVOICEDATE AS INVOICE_DT,
	       @PS_SetID AS VENDOR_SETID,	-- Changed in V3 to remove hard-coded US value  
	       Vendor.PS_Export_Vendor_ID AS VENDOR_ID,	-- Changed in V3 to support parent-child vendor relationships used by some vendors   
	       'DEFAULT' AS VNDR_LOC,
	       Vendor.PS_Address_Sequence AS ADDRESS_SEQ_NUM,
	       NULL AS GRP_AP_ID,
	       @Region_Code AS ORIGIN,	-- Changed in V3 to remove hard-coded region value; supports multiple regions in one instance of IRMA   
	       ISNULL(@PS_OprID, 'IRMA') AS OPRID,	-- Changed in V3 to remove hard-coded region value; IRMA was the value supplied by the PS team  
	       1 AS VCHR_TTL_LINES,
	       -- If the Order CloseDate is not in this period and it is the first Wed. or after in this period
	       -- or if the Close date is older than last period
	       -- report the current date rather than the CloseDate as the Accounting Date  
	       CASE 
	            WHEN CLOSEDATE < @ThisPeriodBeginDate THEN GETDATE()
	            ELSE CLOSEDATE
	       END AS ACCOUNTING_DT,
	       NULL AS POST_VOUCHER,
	       NULL AS DST_CNTRL_ID,
	       NULL AS VOUCHER_ID_RELATED,
	       (
	           SELECT
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACO.SACTotal), 0)
	                          FROM   @UploadSACOrders SACO
	                          WHERE  OrderHeader.OrderHeader_ID = SACO.OrderHeader_ID
	                      ),
	                      2
	                  ) +
	                  SUM(
	                      ROUND(
	                          OrdInv.InvoiceCost * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      ) + ROUND(
	                          OrdInv.InvoiceFreight * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      )
	                  ) +
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACA.SACTotal), 0)
	                          FROM   @UploadSACAllocOrders SACA
	                          WHERE  OrderHeader.OrderHeader_ID = SACA.OrderHeader_ID
	                      ),
	                      2
	                  )
	           FROM   @OrderInvoice OrdInv
	                  INNER JOIN OrderHeader OH
	                       ON  OH.OrderHeader_ID = OrdInv.OrderHeader_ID
	           WHERE  OH.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       ) AS GROSS_AMT,
	       NULL AS DSCNT_AMT,
	       NULL AS USETAX_CD,
	       NULL AS SALETX_AMT,
	       NULL AS SALETX_CD,
	       NULL AS FREIGHT_AMT,
	       NULL AS DUE_DT,
	       NULL AS DSCNT_DUE_DT,
	       NULL AS PYMNT_TERMS_CD,
	       GETDATE() AS ENTERED_DT,
	       Currency.CurrencyCode AS TXN_CURRENCY_CD,
	       NULL AS RT_TYPE,
	       NULL AS RATE_MULT,
	       NULL AS RATE_DIV,
	       NULL AS VAT_ENTRD_AMT,
	       NULL AS MATCH_ACTION,
	       NULL AS MATCH_STATUS_VCHR,
	       NULL AS BCM_TRAN_TYPE,
	       NULL AS CNTRCT_ID,
	       Vendor.PS_Address_Sequence AS REMIT_ADDR_SEQ_NUM,
	       NULL AS CUR_RT_SOURCE,
	       NULL AS DSCNT_AMT_FLG,
	       NULL AS DUE_DT_FLG,
	       NULL AS VCHR_APPRVL_FLG,
	       NULL AS BUSPROCNAME,
	       NULL AS APPR_RULE_SET,
	       NULL AS VAT_DCLRTN_POINT,
	       NULL AS VAT_CALC_TYPE,
	       NULL AS VAT_ENTITY,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS TAX_CD_VAT,
	       NULL AS VAT_RCRD_INPT_FLG,
	       NULL AS VAT_RCRD_OUTPT_FLG,
	       NULL AS VAT_RECOVERY_PCT,
	       NULL AS VAT_CALC_GROSS_NET,
	       NULL AS VAT_RECALC_FLG,
	       NULL AS VAT_CALC_FRGHT_FLG,
	       NULL AS VAT_RGSTRN_SELLER,
	       NULL AS COUNTRY_SHIP_FROM,
	       NULL AS COUNTRY_SHIP_TO,
	       NULL AS COUNTRY_VAT_BILLFR,
	       NULL AS COUNTRY_VAT_BILLTO,
	       NULL AS VAT_TREATMENT_PUR,
	       NULL AS VAT_EXCPTN_TYPE,
	       NULL AS VAT_EXCPTN_CERTIF,
	       NULL AS VAT_USE_ID,
	       NULL AS DSCNT_PRORATE_FLG,
	       NULL AS USETAX_PRORATE_FLG,
	       NULL AS SALETX_PRORATE_FLG,
	       NULL AS FRGHT_PRORATE_FLG,
	       NULL AS IST_TXN_FLG,
	       NULL AS DOC_TYPE,
	       NULL AS DOC_SEQ_DATE,
	       NULL AS DOC_SEQ_NBR,
	       NULL AS VAT_CF_ANLSYS_TYPE,
	       OrderHeader.OrderHeader_ID AS DESCR254_MIXED,	--TFS 6926 Add PO_ID to AP Upload File  
	       '001' AS VCHR_LINE_ROW_ID,
	       Store.BusinessUnit_ID AS LINE_BUSINESS_UNIT_ID,
	       NULL AS LINE_VOUCHER_ID,
	       1 AS VOUCHER_LINE_NUM,
	       (
	           (
	               SELECT COUNT(*)
	               FROM   @OrderInvoice TOI
	               WHERE  TOI.OrderHeader_ID = OrderHeader.OrderHeader_ID
	           ) 
	           + (
	               SELECT COUNT(*)
	               FROM   @UploadSACOrders SACO
	               WHERE  OrderHeader.OrderHeader_ID = SACO.OrderHeader_ID
	           )
	       ) AS TOTAL_DISTRIBS,
	       NULL AS BUSINESS_UNIT_PO,
	       NULL AS PO_ID,
	       NULL AS LINE_NBR,
	       NULL AS SCHED_NBR,
	       NULL AS DESCR,
	       (
	           SELECT
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACO.SACTotal), 0)
	                          FROM   @UploadSACOrders SACO
	                          WHERE  OrderHeader.OrderHeader_ID = SACO.OrderHeader_ID
	                      ),
	                      2
	                  ) +
					   SUM(
	                      ROUND(
	                          OrdInv.InvoiceCost * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      ) + ROUND(
	                          OrdInv.InvoiceFreight * (CASE WHEN OH.Return_Order = 1 THEN -1 ELSE 1 END),
	                          2
	                      )
	                  ) +
				ROUND(
	                      (
	                          SELECT ISNULL(SUM(SACA.SACTotal), 0)
	                          FROM   @UploadSACAllocOrders SACA
	                          WHERE  OrderHeader.OrderHeader_ID = SACA.OrderHeader_ID
	                      ),
	                      2
	                  )
	           FROM   @OrderInvoice OrdInv
	                  INNER JOIN OrderHeader OH
	                       ON  OH.OrderHeader_ID = OrdInv.OrderHeader_ID
	           WHERE  OH.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       ) AS MERCHANDISE_AMT,
	       NULL AS ITEM_SETID,
	       NULL AS INV_ITEM_ID,
	       NULL AS QTY_VCHR,
	       NULL AS STATISTIC_AMT,
	       NULL AS UNIT_OF_MEASURE,
	       NULL AS UNIT_PRICE,
	       NULL AS SALETX_APPL_FLG,
	       NULL AS USETAX_APPL_FLG,
	       NULL AS FRGHT_PRORATE_FLG,
	       NULL AS DSCNT_APPL_FLG,
	       NULL AS WTHD_SW,
	       NULL AS TAX_CD_VAT,
	       NULL AS VAT_RECOVERY_PCT,
	       NULL AS BUSINESS_UNIT_RECV,
	       NULL AS RECEIVER_ID,
	       NULL AS RECV_LN_NBR,
	       NULL AS RECV_SHIP_SEQ_NBR,
	       NULL AS MATCH_LINE_OPT,
	       NULL AS DISTRIB_MTHD_FLG,
	       Currency.CurrencyCode AS TXN_CURRENCY_CD,
	       Currency.CurrencyCode AS BASE_CURRENCY,
	       Currency.CurrencyCode AS CURRENCY_CD,
	       NULL AS SHIPTO_ID,
	       NULL AS SUT_BASE_ID,
	       NULL AS TAX_CD_SUT,
	       NULL AS SUT_EXCPTN_TYPE,
	       NULL AS SUT_EXCPTN_CERTIF,
	       NULL AS SUT_APPLICABILITY,
	       @PS_SetID AS WTHD_SETID,	-- Changed in V3 to remove hard-coded US value  
	       NULL AS WTHD_CD,
	       NULL AS VAT_APPL_FLG,
	       NULL AS VAT_APPLICABILITY,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS NATURE_OF_TXN1,
	       NULL AS NATURE_OF_TXN2,
	       NULL AS VAT_USE_ID,
	       '002' AS VCHR_DIST_ROW_ID,	--Delivers only the 002 lines for SAC charges, nothing else  
	       Store.BusinessUnit_ID AS DIST_BUSINESS_UNIT_ID,
	       NULL AS DIST_VOUCHER_ID,
	       1 AS DIST_VOUCHER_LINE_NUM,
	       SACO.OrderSACID AS DISTRIB_LINE_NUM,
	       Store.BusinessUnit_ID AS BUSINESS_UNIT_GL,
	       SACO.GLPurchaseAcct AS ACCOUNT,
	       NULL AS STATISTIC_CODE,
	       NULL AS STATISTIC_AMOUNT,
	       NULL AS QTY_VCHR,
	       NULL AS JRNL_LN_REF,
	       NULL AS OPEN_ITEM_STATUS,
	       NULL AS DISTRIB_DESCR,
	       (ROUND(SACO.SACTotal, 2)) AS DIST_MERCHANDISE_AMT,	-- DIST AMT for sum of SAC charges for PSTeam/Subteam/GLAccount combo for invoice  
	       NULL AS BUSINESS_UNIT_PO,
	       NULL AS DISTRIB_PO_ID,
	       NULL AS DISTRIB_LINE_NBR,
	       NULL AS DISTRIB_SCHED_NBR,
	       NULL AS PO_DIST_LINE_NUM,
	       NULL AS BUSINESS_UNIT_PC,
	       NULL AS ACTIVITY_ID,
	       NULL AS ANALYSIS_TYPE,
	       NULL AS RESOURCE_TYPE,
	       NULL AS RESOURCE_CATEGORY,
	       NULL AS RESOURCE_SUB_CAT,
	       NULL AS ASSET_FLG,
	       NULL AS BUSINESS_UNIT_AM,
	       NULL AS ASSET_ID,
	       NULL AS PROFILE_ID,
	       NULL AS FREIGHT_AMT,
	       NULL AS SALETX_AMT,
	       NULL AS USETAX_AMT,
	       NULL AS VAT_TXN_TYPE_CD,
	       NULL AS VAT_INV_AMT,
	       NULL AS VAT_NONINV_AMT,
	       NULL AS BUSINESS_UNIT_RECV,
	       NULL AS RECEIVER_ID,
	       NULL AS RECV_LN_NBR,
	       NULL AS RECV_SHIP_SEQ_NBR,
	       NULL AS RECV_DIST_LINE_NUM,
	       StoreSubTeam.PS_Team_No AS DEPTID,	-- Changed back in V3.5 for non allocated charges (AZ-08/06/09) -- Changed in V3 to return the PS department (called team in IRMA) value; PS values do not have to match IRMA values  
	       StoreSubTeam.PS_SubTeam_No AS PRODUCT,	 -- Changed back in V3.5 for non allocated charges  (AZ-08/06/09) ---Changed in V3 to return the PS product (called subteam in IRMA) value; PS values do not have to match IRMA values  
	       NULL AS PROEJCT_ID,
	       NULL AS AFFILIATE,
	       NULL AS VAT_APORT_CNTRL,
	       OrderHeader.OrderHeader_ID,
	       0 AS Freight3Party,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'006'
		   END AS VCHR_CURR_ROW_ID,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				Store.BusinessUnit_ID
		   END AS CURR_BUSINESS_UNIT_ID,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'USD'
		   END AS CURR_VENDOR_CODE,
		   CASE 
			WHEN Currency.CurrencyCode = 'USD' AND Currency2.CurrencyCode = 'CAD' THEN
				'ROYAL'
		   END AS CURR_BU_CODE
	FROM   @OrderInvoice AS OrderInvoice
	       INNER JOIN OrderHeader --OH
	            ON  OrderInvoice.OrderHeader_ID = OrderHeader.OrderHeader_ID
	       JOIN @UploadSACOrders SACO
	            ON  OrderHeader.OrderHeader_ID = SACO.OrderHeader_ID
	       INNER JOIN Vendor(NOLOCK)
	            ON  OrderHeader.Vendor_ID = Vendor.Vendor_ID
	       INNER JOIN Vendor ReceiveLocation(NOLOCK)
	            ON  ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID
	       INNER JOIN Store(NOLOCK)
	            ON  Store.Store_No = ReceiveLocation.Store_no
	       INNER JOIN StoreSubTeam(NOLOCK)
	            ON  Store.Store_No = StoreSubTeam.Store_No
	            AND SACO.SubTeam_No = StoreSubTeam.SubTeam_No
	       LEFT JOIN Currency (NOLOCK)
				ON  OrderHeader.CurrencyID = Currency.CurrencyID
		   LEFT JOIN Currency (NOLOCK) Currency2
				ON  ReceiveLocation.CurrencyID = Currency2.CurrencyID
	ORDER BY
	       OrderHeader.OrderHeader_ID,
	       DISTRIB_LINE_NUM,
	       Business_Unit_ID,
	       Vendor.PS_Export_Vendor_ID,
	       OrderInvoice.InvoiceNumber,
	       OrderInvoice.Sales_Account  
	
	
	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO