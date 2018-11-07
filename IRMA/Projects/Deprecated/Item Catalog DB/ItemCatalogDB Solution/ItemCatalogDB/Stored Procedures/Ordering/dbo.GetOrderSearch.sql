/****** Object:  StoredProcedure [dbo].[GetOrderSearch]    Script Date: 11/07/2012 14:43:05 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrderSearch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrderSearch]
GO

/****** Object:  StoredProcedure [dbo].[GetOrderSearch]    Script Date: 11/07/2012 14:43:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetOrderSearch]
    @OrderHeader_ID				int,
    @OrderDate					datetime = NULL,
	@SentDate					datetime = NULL,
    @InvoiceNumber				varchar(255),
    @Sent						int,
    @OrderStatus				int,
    @OrderType_ID				int,
    @Type3						int,
    @Type4						int,
    @Vendor						varchar(50),
    @Created_By					int,
    @Transfer_SubTeam			int,
    @Transfer_To_SubTeam		int,
    @ReceiveLocation			int,
    @Identifier					varchar(13),
    @Item_Description			varchar(50),
    @WarehouseSentDate			datetime = NULL,
	@Expected_Date				datetime = NULL,
    @LotNo						varchar(12),
    @FromQueue					bit,
    @EInvoice					bit,
	@SourceID					int,
	@PartialShipment			bit,
	@RefuseReceivingReasonID	bit

AS

   --**************************************************************************
   -- Procedure: GetOrderSearch()
   --    Author: SO
   --      Date: waaaay back.
   --
   -- Description:
   -- This procedure finds and returns the Orders that match the specified criteria.
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 5/21/2008		RE		xxxxx	Modified Searching by date fields to truncate any timestamps that are passed in and just search using the date part.
   --								DVO orders were being excluded from search results because they had timestamps associated with them. 
   --   
   -- 11/19/2007	RS				Added OR statement to OrderHeader_ID where condition to
   --								also check for the order number at the start of the
   --								description field for DVO imported order numbers.
   --
   -- 09/17/2009	BSR				Changed OR statement from looking at OrderHeaderDesc to look at OrderExternalSourceOrderID
   --								also added join to Outer join to OrderExternalSource table and Source output	
   -- 09/21/2009	BSR				Added @SourceID criteria and search clause for external source 					
   -- 10/13/2009	TL				Added @EInvoice param and search filter.
   -- 12/07/2011	BBB		3744    added OrderedCost column;	
   -- 10/24/2012	RDE				added support for ExternalOrderInformation table. 
   -- 11/07/2012	AB		8312	added search for deleted orders
   -- 2012/12/25	KM		8780	Search ItemOverride for Item_Description;
   -- 2013/01/09	KM		8780	Resolve ambiguous CompanyName references;
   -- 2013/01/17	DN		9703	1. Changed the data type of the date parameters from CHAR(10) to DATETIME
   --								2. Updated the comparsion expressions from @OrderDate <> NULL to @OrderDate IS NOT NULL
   -- 2013/03/07	KM		11474	Correct a misspelling of "OrderHeaderId" in the Source section of the WHERE clause;
   -- 2013/04/03	KM		11815	Separate the identifier and description joins to improve performance; Correct the ItemOverride join;
   -- 2013/06/19	BS		12769	Updated SubTeam_Name to just SubTeam in creation of #OrderSearch to match the DataTable setup in .NET code
   --**************************************************************************
 
BEGIN
    SET NOCOUNT ON
    
    SET TRANSACTION ISOLATION LEVEL SNAPSHOT 
  
	--Create Temp table to keep Search Results  
  	CREATE TABLE #OrderSearch
	(
		OrderHeader_ID	int NOT NULL,
		CompanyName		varchar(50) NULL,
		OrderDate		smalldatetime NOT NULL,
		Expected_Date	smalldatetime NULL,
		SentDate		smalldatetime NULL,
		SubTeam			varchar(100) NULL,
		CloseDate		datetime NULL,
		Return_Order	bit NULL,
		eInvoice		bit NULL,
		OrderedCost		money NULL,
		Source			varchar(20) NULL,
		Jurisdiction	varchar(6) NOT NULL,
		DeleteDate		smalldatetime NULL
	)

	CREATE TABLE #ValidSearchOrders (OrderHeaderId int PRIMARY KEY) 
	
	IF (@OrderHeader_ID <> 0) AND (@SourceID = 0)
		BEGIN 
			INSERT INTO #ValidSearchOrders 
			SELECT @OrderHeader_ID
			UNION
			SELECT DISTINCT eoi.OrderHeader_Id FROM dbo.ExternalOrderInformation eoi WHERE eoi.ExternalOrder_Id = @OrderHeader_ID
		END
	
	ELSE IF (@OrderHeader_ID <> 0) AND (@SourceID <> 0)  
		BEGIN
			INSERT INTO #ValidSearchOrders 
			SELECT @OrderHeader_ID
			UNION
			SELECT DISTINCT eoi.OrderHeader_Id FROM dbo.ExternalOrderInformation eoi WHERE eoi.ExternalOrder_Id = @OrderHeader_ID AND eoi.ExternalSource_Id = @SourceID    
		END
	
	ELSE IF (@OrderHeader_ID = 0) AND (@SourceID <> 0)
		BEGIN
			-- Only select records where OrderExternalSourceID is not null (Not created in IRMA)
			INSERT INTO #ValidSearchOrders 
				SELECT distinct eoi.OrderHeader_Id FROM dbo.ExternalOrderInformation eoi inner join OrderHeader OH on OH.OrderHeader_ID = eoi.OrderHeader_Id
				 WHERE eoi.ExternalSource_Id = @SourceID and OH.OrderExternalSourceID is not null
				  union				 
				SELECT distinct eoi.OrderHeader_Id FROM dbo.ExternalOrderInformation eoi inner join DeletedOrder OH on OH.OrderHeader_ID = eoi.OrderHeader_Id
				 WHERE eoi.ExternalSource_Id = @SourceID and OH.OrderExternalSourceID is not null          
		END  
	
	DECLARE @SQL varchar(3000),@beginCounter int,@endCounter int
	-- We have 4 diiferent scenarios
	-- 1 Everything - we run loop twice to search both tables OrderHeader and DeletedOrders
	-- 2 Open orders - we run loop once to search only table OrderHeader
	-- 3 Closed orders - we run loop once to search only table OrderHeader
	-- 4 Deleted orders - we run loop once to search only table DeletedOrders
	
	SELECT 
	@beginCounter	=	CASE @OrderStatus
							WHEN 4 THEN 1
							ELSE 0
						END,	
	@endCounter		=	CASE @OrderStatus
							WHEN 4 THEN 2
							WHEN 1 THEN 2
							ELSE 1
						END	

	WHILE @beginCounter < @endCounter
		BEGIN
			SET @beginCounter = @beginCounter + 1
			
			SELECT @SQL = 'Insert Into #OrderSearch '
			SELECT @SQL = @SQL + 'SELECT TOP 1000 OrderHeader_ID, Vendor.CompanyName, OrderDate, Expected_Date, SentDate, st.Subteam_Name, CloseDate, ISNULL(Return_Order, 0), CASE WHEN eInvoice_Id IS NOT NULL THEN 1 ELSE 0 END AS eInvoice, '
			SELECT @SQL = @SQL + 'OH.OrderedCost, '
			SELECT @SQL = @SQL + 'CASE WHEN OES.Description IS NULL THEN ''IRMA'' ELSE OES.Description END AS Source, sj.StoreJurisdictionDesc, '
			SELECT @SQL = @SQL + CASE @beginCounter WHEN 2 THEN 'DeleteDate ' ELSE 'null ' END
			SELECT @SQL = @SQL + 'DeleteDate FROM '
			SELECT @SQL = @SQL + CASE @beginCounter WHEN 2 THEN 'DeletedOrder ' ELSE 'OrderHeader ' END
			SELECT @SQL = @SQL + ' OH (NOLOCK)  INNER JOIN Subteam st ON st.subteam_no = OH.Transfer_to_SubTeam INNER JOIN Vendor (NOLOCK) ON (Vendor.Vendor_ID = OH.Vendor_ID) '
	
			IF (@Identifier <> '') OR (@Item_Description <> '')
				BEGIN
					IF (@Identifier <> '') 
						BEGIN
							SELECT @SQL = @SQL + 'INNER JOIN ( '
							SELECT @SQL = @SQL + '  SELECT DISTINCT OrderHeader_ID AS OHID '
							SELECT @SQL = @SQL + '  FROM OrderItem (NOLOCK) oi 
																	WHERE oi.Item_Key IN  ( '
																		 SELECT @SQL = @SQL + '  SELECT Item.Item_Key '
																		 SELECT @SQL = @SQL + '  FROM ItemIdentifier (NOLOCK) INNER JOIN Item (NOLOCK) ON (ItemIdentifier.Item_Key = Item.Item_Key) '
																		 SELECT @SQL = @SQL + '  WHERE Identifier LIKE ''%' + @Identifier + '%'''
																		 SELECT @SQL = @SQL + ' )' 
							IF @LotNo <> '' SELECT @SQL = @SQL + ' AND ISNULL(Lot_No, '''') = ''' + @LotNo + '''' 
							SELECT @SQL = @SQL + ') IdentifierMatches ON (IdentifierMatches.OHID = OH.OrderHeader_ID) '
						END
					IF (@Item_Description <> '')
						BEGIN 
							SELECT @SQL = @SQL + 'INNER JOIN ( '
							SELECT @SQL = @SQL + '  SELECT DISTINCT OrderHeader_ID AS OHID '
							SELECT @SQL = @SQL + '  FROM OrderItem (NOLOCK) oi 
																	WHERE oi.Item_Key IN  ( '
																		 SELECT @SQL = @SQL + '  SELECT Item.Item_Key FROM Item '
																		 SELECT @SQL = @SQL + '  LEFT JOIN ItemOverride (NOLOCK) iov ON Item.Item_Key = iov.Item_Key '
																		 SELECT @SQL = @SQL + '  WHERE Item.Item_Description LIKE ''%' + @Item_Description + '%'' OR iov.Item_Description LIKE ''%' + @Item_Description + '%'' '         
																		 SELECT @SQL = @SQL + ' )' 
							IF @LotNo <> '' SELECT @SQL = @SQL + ' AND ISNULL(Lot_No, '''') = ''' + @LotNo + '''' 
							SELECT @SQL = @SQL + ') DescriptionMatches ON (DescriptionMatches.OHID = OH.OrderHeader_ID) '
						END
				END
			ELSE IF @LotNo <> '' 
				BEGIN
					SELECT @SQL = @SQL + 'INNER JOIN ( '
					SELECT @SQL = @SQL + '  SELECT DISTINCT OrderHeader_ID AS OHID '
					SELECT @SQL = @SQL + '  FROM OrderItem (NOLOCK) WHERE '
					SELECT @SQL = @SQL + ' ISNULL(Lot_No, '''') = ''' + @LotNo + '''' 
					SELECT @SQL = @SQL + ') LotNo ON (LotNo.OHID = OH.OrderHeader_ID) '
				END
				
			SELECT @SQL = @SQL + 'LEFT OUTER JOIN OrderExternalSource (NOLOCK) OES '
			SELECT @SQL = @SQL + 'ON OH.OrderExternalSourceID = OES.ID '
			SELECT @SQL = @SQL + '  join Vendor (nolock) vr				on oh.ReceiveLocation_ID	= vr.Vendor_ID
									join Store (nolock) s				on vr.Store_no				= s.Store_No
									join StoreJurisdiction (nolock) sj	on s.StoreJurisdictionID	= sj.StoreJurisdictionID '

			SELECT @SQL = @SQL + 'WHERE 1 = 1 '
			IF (@OrderHeader_ID <> 0) SELECT @SQL = @SQL + 'AND ( OrderHeader_ID in (select OrderHeaderID from #ValidSearchOrders )) '
			IF (@OrderDate IS NOT NULL) SELECT @SQL = @SQL + 'AND DATEDIFF(d,OrderDate, ''' + RTRIM(CONVERT(VARCHAR(30),@OrderDate)) + ''') = 0 ' 
			IF (@SentDate IS NOT NULL) SELECT @SQL = @SQL + 'AND DATEDIFF(d, SentDate, ''' + RTRIM(CONVERT(VARCHAR(30),@SentDate)) + ''') = 0 '
			IF (@InvoiceNumber <> '') SELECT @SQL = @SQL + 'AND InvoiceNumber LIKE ''' + @InvoiceNumber + '%'' '
			IF (@Sent <> 0) SELECT @SQL = @SQL + 'AND Sent = 1 ' 
			IF (@OrderStatus = 2) SELECT @SQL = @SQL + 'AND CloseDate IS NULL ' 
			IF (@OrderStatus = 3) SELECT @SQL = @SQL + 'AND CloseDate IS NOT NULL ' 
			IF (@OrderType_ID > 0) SELECT @SQL = @SQL + 'AND OrderType_ID = '	+ CONVERT(VARCHAR(10), @OrderType_ID) + ' '
			IF (@Type3 <> 0) SELECT @SQL = @SQL + 'AND Return_Order = 1 '
			IF (@Type4 <> 0) SELECT @SQL = @SQL + 'AND Return_Order = 0 '
			IF (@Vendor <> '') SELECT @SQL = @SQL + 'AND Vendor.CompanyName LIKE ''%' + @Vendor + '%'' ' 
			IF (@Created_By <> 0) SELECT @SQL = @SQL + 'AND CreatedBy = ' + CONVERT(VARCHAR(10),@Created_By) + ' '
			IF (@Transfer_SubTeam <> 0) SELECT @SQL = @SQL + 'AND ISNULL(Transfer_SubTeam, -1) = ' + CONVERT(VARCHAR(10),@Transfer_SubTeam) + ' '
			IF (@Transfer_To_SubTeam <> 0) SELECT @SQL = @SQL + 'AND Transfer_To_SubTeam = ' + CONVERT(VARCHAR(10),@Transfer_To_SubTeam) + ' '
			IF (@ReceiveLocation <> 0) SELECT @SQL = @SQL + 'AND ReceiveLocation_ID = ' + CONVERT(VARCHAR(10),@ReceiveLocation) + ' '
			IF (@WarehouseSentDate IS NOT NULL) SELECT @SQL = @SQL + 'AND DATEDIFF(d, WarehouseSentDate, ''' + RTRIM(CONVERT(VARCHAR(30),@WarehouseSentDate)) + ''') = 0 '
			IF (@Expected_Date IS NOT NULL) SELECT @SQL = @SQL + 'AND DATEDIFF(d, Expected_Date, ''' + RTRIM(CONVERT(VARCHAR(30),@Expected_Date)) + ''') = 0 '  
			IF (@FromQueue = 1) SELECT @SQL = @SQL + 'AND FromQueue = 1 ' 
			IF (@EInvoice = 1) SELECT @SQL = @SQL + 'AND EInvoice_Id is not null ' 
			IF (@SourceID <> 0) SELECT @SQL = @SQL + 'AND OrderHeader_Id in (select OrderHeaderId from #ValidSearchOrders)'
			IF (@PartialShipment =1) SELECT @SQL = @SQL + ' AND PartialShipment = 1 '
			IF (@RefuseReceivingReasonID =1) SELECT @SQL = @SQL + ' AND RefuseReceivingReasonID IS NOT NULL '
			SELECT @SQL = @SQL + 'ORDER BY OrderHeader_ID DESC'
			
			--PRINT(@SQL)
			EXECUTE(@SQL)
		END

	SELECT * FROM #OrderSearch ORDER BY OrderDate DESC
	DROP TABLE #ValidSearchOrders
	DROP TABLE #OrderSearch
    SET NOCOUNT OFF 

END
GO