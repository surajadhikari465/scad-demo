CREATE PROCEDURE [dbo].[OpenOrdersReport]
    @ReceiveLocation_ID int,
    @SubTeam_No			int,
    @Expected_DateStart	varchar(12),
    @Expected_DateEnd	varchar(12),
    @OrderDateStart		varchar(12),
    @OrderDateEnd		varchar(12),
    @Return_Orders		int,
    @User_ID			int,
    @Vendor_ID			int,
    @Pre_Order			bit,
    @IncludeBlankPOs	bit

AS

-- ****************************************************************************************************************
-- Procedure: OpenOrdersReport()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- This procedure is called from a single RDL file and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2008-05-23	RE		6555	Changed the WHERE clause for OrderDate and ExpectdDate to Ignore TimeStamps on dates in the OrderHeader Record. 
--								DVO Orders Contained Timestamps and the search functionality of the report would exclude them. DVO orders will now be included in results.
-- 2013-01-12	KM		9476	Add update history template; convert varchar input parameters to date types to assist
--								with cultural date formatting;
-- 2013-09-12   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- ****************************************************************************************************************

BEGIN  
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON  

	DECLARE 
		@Expected_DateStart_datetime	datetime2,
		@Expected_DateEnd_datetime		datetime2,
		@OrderDateStart_datetime		datetime2,
		@OrderDateEnd_datetime			datetime2
			    
	SET @Expected_DateStart_datetime	= CONVERT(datetime2, @Expected_DateStart)
	SET @Expected_DateEnd_datetime		= CONVERT(datetime2, @Expected_DateEnd)
	SET @OrderDateStart_datetime		= CONVERT(datetime2, @OrderDateStart)
	SET @OrderDateEnd_datetime			= CONVERT(datetime2, @OrderDateEnd)
	
    IF @IncludeBlankPOs = 0  
		BEGIN  
			SELECT 
				Vendor.CompanyName AS VendorName, 
				ReceiveLocation.CompanyName AS ReceiveLocationName,   
				OrderHeader.OrderHeader_ID, 
				OrderHeader.OrderDate, 
				OrderHeader.SentDate, 
				OrderHeader.Expected_Date, 
				SubTeam_Name,   
				UserName, 
				OrderHeader.Return_Order, 
				OrderingStore.CompanyName AS OrderingStore, 
				COUNT(OrderItem.OrderItem_ID) AS ItemCount, 
				OrderHeader.OrderHeaderDesc AS Notes  
		
			FROM 
				Users (NOLOCK) INNER JOIN (  
				SubTeam (NOLOCK) INNER JOIN (  
				Item (NOLOCK) INNER JOIN (  
				OrderItem (NOLOCK) INNER JOIN (  
				Vendor ReceiveLocation (NOLOCK) INNER JOIN (  
				Vendor (NOLOCK) INNER JOIN OrderHeader (NOLOCK) ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)  
				) ON (ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID)  
				) ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)  
				) ON (Item.Item_Key = OrderItem.Item_Key)  
				) ON (SubTeam.SubTeam_No = Transfer_To_SubTeam)  
				) ON (Users.User_ID = OrderHeader.CreatedBy)  
				INNER JOIN Vendor OrderingStore ON OrderingStore.Vendor_ID = OrderHeader.ReceiveLocation_ID  
	   
			WHERE 
				OrderHeader.ReceiveLocation_ID = ISNULL(@ReceiveLocation_ID, OrderHeader.ReceiveLocation_ID)  
				AND Orderheader.CloseDate IS NULL   
				AND Transfer_To_SubTeam = ISNULL(@SubTeam_No, Transfer_To_SubTeam)  
				AND (ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) >= ISNULL(@Expected_DateStart_datetime, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0)) AND ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) <= ISNULL(@Expected_DateEnd_datetime, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0)))    
				AND (DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) >= ISNULL(@OrderDateStart_datetime, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)) AND DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) <= ISNULL(@OrderDateEnd_datetime, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)))     
				AND OrderHeader.Return_order = ISNULL(@Return_Orders, OrderHeader.Return_Order)  
				AND ISNULL(@User_ID, Users.User_ID) = Users.User_ID  
				AND OrderHeader.Vendor_ID = ISNULL(@Vendor_ID,OrderHeader.Vendor_ID)  
				AND Item.Pre_Order = ISNULL(@Pre_Order,0)  
		
			GROUP BY 
				Vendor.CompanyName, 
				ReceiveLocation.CompanyName, 
				OrderHeader.OrderHeader_ID,   
				OrderHeader.OrderDate, 
				OrderHeader.SentDate, 
				OrderHeader.Expected_Date, 
				SubTeam_Name, 
				UserName, 
				OrderHeader.Return_Order, 
				OrderingStore.CompanyName, 
				OrderHeader.OrderHeaderDesc
		
			ORDER BY  
				SubTeam_Name,  
				Vendor.CompanyName,  
				OrderHeader.Return_Order,  
				OrderHeader.OrderDate  
		END  
	ELSE  
		BEGIN  
			SELECT 
				Vendor.CompanyName AS VendorName, 
				ReceiveLocation.CompanyName AS ReceiveLocationName,   
				OrderHeader.OrderHeader_ID, 
				OrderHeader.OrderDate, 
				OrderHeader.SentDate, 
				OrderHeader.Expected_Date, 
				SubTeam_Name,   
				UserName, 
				OrderHeader.Return_Order, 
				OrderingStore.CompanyName AS OrderingStore, 
				COUNT(OrderItem.OrderItem_ID) AS ItemCount, 
				OrderHeader.OrderHeaderDesc as Notes  
			
			FROM 
				Users (NOLOCK) LEFT JOIN (  
				SubTeam (NOLOCK) LEFT JOIN (  
				Vendor ReceiveLocation (NOLOCK) LEFT JOIN (  
				Vendor (NOLOCK) LEFT JOIN OrderHeader (NOLOCK) ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)  
				) ON (ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID)  
				) ON (SubTeam.SubTeam_No = Transfer_To_SubTeam)  
				) ON (Users.User_ID = OrderHeader.CreatedBy)  
				INNER JOIN Vendor OrderingStore ON OrderingStore.Vendor_ID = OrderHeader.ReceiveLocation_ID  
				LEFT JOIN OrderItem OrderItem ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID  
		   
			WHERE 
				OrderHeader.ReceiveLocation_ID = ISNULL(@ReceiveLocation_ID, OrderHeader.ReceiveLocation_ID)  
				AND Orderheader.CloseDate IS NULL   
				AND Transfer_To_SubTeam = ISNULL(@SubTeam_No, Transfer_To_SubTeam)  
				AND (ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) >= ISNULL(@Expected_DateStart_datetime, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0)) AND ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) <= ISNULL(@Expected_DateEnd_datetime, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0)))    
				AND (DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) >= ISNULL(@OrderDateStart_datetime, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)) AND DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) <= ISNULL(@OrderDateEnd_datetime, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)))     
				AND OrderHeader.Return_order = ISNULL(@Return_Orders, OrderHeader.Return_Order)  
				AND ISNULL(@User_ID, Users.User_ID) = Users.User_ID  
				AND OrderHeader.Vendor_ID = ISNULL(@Vendor_ID,OrderHeader.Vendor_ID)  
			
			GROUP BY 
				Vendor.CompanyName, 
				ReceiveLocation.CompanyName, 
				OrderHeader.OrderHeader_ID,   
				OrderHeader.OrderDate, 
				OrderHeader.SentDate, 
				OrderHeader.Expected_Date, 
				SubTeam_Name, 
				UserName, 
				OrderHeader.Return_Order, 
				OrderingStore.CompanyName, 
				OrderHeader.OrderHeaderDesc  
			
			ORDER BY  
				SubTeam_Name,  
				Vendor.CompanyName,  
				OrderHeader.Return_Order,  
				OrderHeader.OrderDate  
		END  
       
    SET NOCOUNT OFF  
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersReport] TO [IRMAReportsRole]
    AS [dbo];

