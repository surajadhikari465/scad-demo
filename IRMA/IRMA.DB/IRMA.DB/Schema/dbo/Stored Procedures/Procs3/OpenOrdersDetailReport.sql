CREATE PROCEDURE dbo.OpenOrdersDetailReport
    @ReceiveLocation_ID int,
    @SubTeam_No int,
    @Expected_DateStart varchar(12),
    @Expected_DateEnd varchar(12),
    @OrderDateStart varchar(12),
    @OrderDateEnd varchar(12),
    @Return_Orders int,
    @User_ID int,
    @Vendor_ID int,
    @Pre_Order bit,
    @IncludeBlankPOs bit
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON
    
	IF @IncludeBlankPOs = 0
		BEGIN
			SELECT Vendor.CompanyName AS VendorName, ReceiveLocation.CompanyName AS ReceiveLocationName, 
				   OrderHeader.OrderHeader_ID, OrderHeader.OrderDate, OrderHeader.SentDate, OrderHeader.Expected_Date,
				   Item.Item_Description, Identifier, 
				   Item.Package_Desc1,
				   OrderItem.QuantityOrdered,
				   ItemUnit.Unit_Name,
				   SubTeam_Name, UserName, OrderHeader.Return_Order, OrderHeader.OrderHeaderDesc as Notes
			FROM Users (nolock) INNER JOIN (
				  ItemUnit (nolock) INNER JOIN (
				   SubTeam (nolock) INNER JOIN (
					 ItemIdentifier (nolock) INNER JOIN (
					   Item (nolock) INNER JOIN (
						 OrderItem (nolock) INNER JOIN (
						   Vendor ReceiveLocation (nolock) INNER JOIN (
							 Vendor (nolock) INNER JOIN OrderHeader (nolock) ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
						   ) ON (ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID)
						 ) ON (OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID)
					   ) ON (Item.Item_Key = OrderItem.Item_Key)
					 ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
				   ) ON (SubTeam.SubTeam_No = Transfer_To_SubTeam)
				 ) ON (ItemUnit.Unit_ID = OrderItem.QuantityUnit)
			   ) ON (Users.User_ID = OrderHeader.CreatedBy)
			WHERE OrderHeader.ReceiveLocation_ID = ISNULL(@ReceiveLocation_ID, OrderHeader.ReceiveLocation_ID) AND
				  Orderheader.CloseDate IS NULL AND Transfer_To_SubTeam = ISNULL(@SubTeam_No, Transfer_To_SubTeam) AND
				  (ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) >= ISNULL(@Expected_DateStart, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0)) AND ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) <= ISNULL(@Expected_DateEnd, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0))) AND
				  (DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) >= ISNULL(@OrderDateStart, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)) AND DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) <= ISNULL(@OrderDateEnd, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0))) AND
				  OrderHeader.Return_Order = ISNULL(@Return_Orders,OrderHeader.Return_Order) AND
				  ISNULL(@User_ID, Users.User_ID) = Users.User_ID AND
				  OrderHeader.Vendor_ID = ISNULL(@Vendor_ID,OrderHeader.Vendor_ID) AND
				  Item.Pre_Order = ISNULL(@Pre_Order,0)
		END
	ELSE
		BEGIN
			SELECT Vendor.CompanyName AS VendorName, ReceiveLocation.CompanyName AS ReceiveLocationName, 
				   OrderHeader.OrderHeader_ID, OrderHeader.OrderDate, OrderHeader.SentDate, OrderHeader.Expected_Date,
				   Item.Item_Description, Identifier, 
				   Item.Package_Desc1,
				   OrderItem.QuantityOrdered,
				   ItemUnit.Unit_Name,
				   SubTeam_Name, UserName, OrderHeader.Return_Order, OrderHeader.OrderHeaderDesc as Notes
			FROM OrderHeader (nolock) 
  				 LEFT JOIN OrderItem (nolock) ON OrderItem.OrderHeader_Id = OrderHeader.OrderHeader_Id
				 LEFT JOIN Item ON Item.Item_Key = OrderItem.Item_Key
				 LEFT JOIN ItemUnit (nolock) ON ItemUnit.Unit_ID = OrderItem.QuantityUnit
				 LEFT JOIN SubTeam (nolock) ON SubTeam.SubTeam_No = Transfer_To_SubTeam
				 LEFT JOIN ItemIdentifier (nolock) ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
				 LEFT JOIN Vendor ReceiveLocation (nolock) ON ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID
				 LEFT JOIN Vendor (nolock) ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
				 LEFT JOIN Users (nolock) ON Users.User_ID = OrderHeader.CreatedBy
			WHERE OrderHeader.ReceiveLocation_ID = ISNULL(@ReceiveLocation_ID, OrderHeader.ReceiveLocation_ID) AND
				  Orderheader.CloseDate IS NULL AND Transfer_To_SubTeam = ISNULL(@SubTeam_No, Transfer_To_SubTeam) AND
				  (ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) >= ISNULL(@Expected_DateStart, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0)) AND ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0) <= ISNULL(@Expected_DateEnd, ISNULL(DATEADD(dd, DATEDIFF(dd, 0, Expected_Date), 0), 0))) AND
				  (DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) >= ISNULL(@OrderDateStart, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0)) AND DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0) <= ISNULL(@OrderDateEnd, DATEADD(dd, DATEDIFF(dd, 0, OrderDate), 0))) AND
				  OrderHeader.Return_Order = ISNULL(@Return_Orders,OrderHeader.Return_Order) AND
				  ISNULL(@User_ID, Users.User_ID) = Users.User_ID AND
				  OrderHeader.Vendor_ID = ISNULL(@Vendor_ID,OrderHeader.Vendor_ID) AND
				  ISNULL(Item.Pre_Order,0) = ISNULL(@Pre_Order,0)		
		END
		
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersDetailReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersDetailReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersDetailReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OpenOrdersDetailReport] TO [IRMAReportsRole]
    AS [dbo];

