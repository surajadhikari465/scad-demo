
CREATE PROCEDURE [dbo].[GetSuspendedOrderSearch]
	@OrderHeader_ID					int,
	@OrderInvoice_ControlGroup_ID	int,
	@Vendor_ID						int,
	@Vendor_Key						varchar(10),
	@InvoiceNumber					varchar(16),
	@InvoiceDateStart				smalldatetime,	
	@InvoiceDateEnd					smalldatetime,	
	@OrderDateStart					smalldatetime,
	@OrderDateEnd					smalldatetime,	
	@StoreList						varchar(MAX),
	@EInvoiceOnly					bit,
	@ResolutionCodeID				int,
	@Identifier						varchar(15),
	@VIN							varchar(15)

AS 

-- **************************************************************************
-- Procedure: GetSuspendedOrderSearch()
--    Author: Tom Lux (rewritten for performance)
--    Date: 10/21/2013

-- ** Two procs were combined to create this proc:
-- 1) GetAllSuspendedOrderSearch
-- 2) GetAllRemediatedSuspendedOrderSearch

-- Description:
-- Returns a list of orders and related info for one of two types of searches from the SPOT screen:
-- 1) Suspended Orders
-- 2) Remediated Orders
--
-- Modification History:
-- Date: YMD   	Init  	TFS   	Comment
-- 03/28/2011	BBB   	1746	Updated for code standards and readability;Parameter sniffing adjustment;
-- 06/03/2011	DBS   	2189	Adjusted cost sum so the invoice cost doesn't get multiplied by the number of invoice items;
-- 07/24/2011   FA      2408    Fixed the calculation of doctype 'Other/None' flag
-- 2011/12/14	KM		3744	Repalced SUM(oi.ReceivedItemCost) with calls to oh.AdjustedReceivedCost
-- 2011/12/16	KM		3744	Added oh.AdjustedReceivedCost to GROUP BY clause
-- 05/03/2012   VAA		5880    Added index to OrderItem to avoid seach timeout
-- 2012/10/10	HK		7419	Added select oh.DSDorder 
-- 10/24/2012   FA      7552    Modifed date range filters for optimization 
-- 10/29/2012	HK		8327	Changed end date to from 12/31/2999 to 12/31/2078. end year 2999 throw exceptions
-- 11/05/2012	HK		8329	Add partial shippment in select column
-- 11/28/2012	HK		8329	Add oh.DSDOrder=1, so it will show in spot tool
-- 12/13/2012   FA      9462    Modified the logic for DocTypeOther for DSD orders
-- 2013/01/03   HK      9695    Add PO with invoiceDate is null will be pulled out 
-- 2013/01/29   FA      9716    Added eInvoiceRequired column
-- 2013/02/21   FA      9402    Added SourcePONumber column
-- 2013/03/11	FA		8325	Added Total Refused column
-- 2013/10/21	Lux		12017	Rewrote for performance.  Combined suspended and remediated procs because they were very similar.  Added detailed notes in code.  Change to ALTER instead of drop & create.
-- 2013/11/13	KM		14464	Exclude transfer orders from the initial #orders table;
-- **************************************************************************

BEGIN

	SET NOCOUNT ON;

	-- **************************************************************************
	-- Added in local variables to eliminate parameter sniffing causing issues on some DB servers
	-- **************************************************************************
	declare @LocalOrderHeader_ID int
	set @LocalOrderHeader_ID=@OrderHeader_ID
	
	declare @LocalOrderInvoice_ControlGroup_ID int
	Set @LocalOrderInvoice_ControlGroup_ID=@OrderInvoice_ControlGroup_ID
	
	declare @LocalVendor_ID int
	set @LocalVendor_ID=@Vendor_ID
		
	declare @LocalVendor_Key varchar(10)
	Set @LocalVendor_Key=@Vendor_Key
		
	declare @LocalInvoiceNumber varchar(16)
	set @LocalInvoiceNumber=@InvoiceNumber

	declare @LocalInvoiceDateStart smalldatetime	
	set @LocalInvoiceDateStart=ISNULL(@InvoiceDateStart, '1/1/1900')
	
	declare @LocalInvoiceDateEnd smalldatetime	
	set @LocalInvoiceDateEnd=ISNULL(@InvoiceDateEnd, '12/31/2078')

	declare @LocalOrderDateStart smalldatetime
	set @LocalOrderDateStart=ISNULL(@OrderDateStart, '1/1/1900')

	declare @LocalOrderDateEnd smalldatetime
	set @LocalOrderDateEnd=ISNULL(@OrderDateEnd, '12/31/2078')	

	declare @LocalStoreList varchar(MAX)
	set @LocalStoreList=@StoreList
		
	declare @LocalEInvoiceOnly bit	
	set @LocalEInvoiceOnly=@EInvoiceOnly

	declare @LocalResolutionCodeID int
	set @LocalResolutionCodeID = @ResolutionCodeID

	declare @LocalIdentifier varchar(15)
	set @LocalIdentifier = @Identifier

	declare @LocalVIN varchar(15)
	set @LocalVIN = @VIN

	--**************************************************************************
	--Set variables
	--**************************************************************************
	IF @OrderDateStart is NULL and @OrderDateEnd is null
		BEGIN
			set @LocalOrderDateEnd = GETDATE() 
			set @LocalOrderDateStart = DateADD(day, -60, GETDATE())
		END 

	---------------------------------------------------------------------
	---------------------------------------------------------------------
	---------------------------------------------------------------------

	if object_id('tempdb..#orders') is not null
	begin
		drop table #orders
	end
	create table #orders (
		orderheader_id int not null
		PRIMARY KEY (
			orderheader_id
		)
	)

	-- Build table to house updated sets of orders after applying search filters to main list of orders in #orders table.
	if object_id('tempdb..#filteredOrders') is not null
	begin
		drop table #filteredOrders
	end
	create table #filteredOrders (
		orderheader_id int not null
		PRIMARY KEY (
			orderheader_id
		)
	)



	/*
	This initial pass to identify a target set of orders includes these search filters, if specified:
	- PO #
	- Ordered-Date Range
	- Inv #
	- Invoiced-Date Range
	- Vendor ID
	- Vendor Key
	- eInv-Only Flag
	- Closed POs (must have closed date)
	- Approved POs (must have approved date)
	- PO-level resolution code


	The general idea here is we want to restrict our target list of orders down quickly/efficiently using
	fast/key indexes.  Since we're certainly working with the OrderHeader table, we should apply any order-level
	filtering here.

	NOTE: If a Resolution Code is specified, we want to return any order that has that resolution code at the order or item level.
	We are banking on the hope that res-codes are rarely specified as a criteria.  As such, we make this initial pass at the order level
	only (no OrderItem join) and only hit OrderItem if/when a res-code is specified.  This is an extra pass when a res-code is specified
	but it's worth the savings if, most of the time, a res-code is not specified.

	*/
	/*
	The two SPOT search options:
	1) Suspended Orders
			oh.CloseDate IS NOT NULL
			AND oh.ApprovedDate IS NULL
			AND ISNULL(oh.UploadedDate, oh.AccountingUploadDate) IS NULL   
	2) Remediated Orders
			oh.CloseDate IS NOT NULL
			AND oh.ApprovedDate IS NOT NULL
			AND (
				oh.ResolutionCodeID IS NOT NULL
				OR
				oi.ResolutionCodeID IS NOT NULL
			)

	*/
	if @LocalResolutionCodeID = -1
	begin
		-- Grab Suspended Orders
		insert into #orders
			select
				oh.OrderHeader_ID
			from 
				dbo.OrderHeader										(nolock) oh
				INNER JOIN	dbo.Vendor								(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
				INNER JOIN	dbo.Vendor								(nolock) vs		ON	oh.PurchaseLocation_ID				= vs.Vendor_ID
				INNER JOIN	dbo.Store								(nolock) s		ON	vs.Store_No							= s.Store_No
				INNER JOIN  fn_Parse_List(@LocalStoreList, '|')				 sl		ON	sl.Key_Value						= vs.Store_No 
				LEFT JOIN	dbo.Store								(nolock) vendst ON	vendst.Store_No						= v.Store_No
			where
				-- Purchase Order Number search criteria 
				(@LocalOrderHeader_ID IS NULL
					OR (@LocalOrderHeader_ID IS NOT NULL 
						AND (oh.OrderHeader_ID = @LocalOrderHeader_ID or oh.OrderExternalSourceOrderID = @LocalOrderHeader_ID)))
				-- Vendor Invoice Number search criteria
				AND (@LocalInvoiceNumber IS NULL
					OR (@LocalInvoiceNumber IS NOT NULL 
						AND oh.InvoiceNumber = @LocalInvoiceNumber))
				-- Vendor ID search criteria
				AND (@LocalVendor_ID IS NULL
					OR (@LocalVendor_ID IS NOT NULL 
						AND v.Vendor_ID = @LocalVendor_ID))
				-- Vendor Key search criteria
				AND (@LocalVendor_Key IS NULL
					OR (@LocalVendor_Key IS NOT NULL
						AND v.Vendor_Key = @LocalVendor_Key))
				-- eInv-Only search criteria
				AND (@LocalEInvoiceOnly = 0
					OR (@LocalEInvoiceOnly = 1 AND oh.eInvoice_Id is not null))
				-- Invoice Date search criteria
				AND ((oh.InvoiceDate BETWEEN @LocalInvoiceDateStart AND @LocalInvoiceDateEnd)
					OR oh.invoiceDate is null)
				-- Order Date search criteria
				AND oh.OrderDate BETWEEN @LocalOrderDateStart AND @LocalOrderDateEnd
				AND ISNULL(vendst.Manufacturer, 0) = 0
				AND oh.OrderType_ID <> 3	
				-- Suspended-PO criteria.
				AND oh.CloseDate IS NOT NULL
				AND oh.ApprovedDate IS NULL
				AND ISNULL(oh.UploadedDate, oh.AccountingUploadDate) IS NULL   
	end
	else
	begin
		-- Grab Remediated Orders
		insert into #orders
			select
				oh.OrderHeader_ID
			from 
				dbo.OrderHeader										(nolock) oh
				INNER JOIN	dbo.Vendor								(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
				INNER JOIN	dbo.Vendor								(nolock) vs		ON	oh.PurchaseLocation_ID				= vs.Vendor_ID
				INNER JOIN	dbo.Store								(nolock) s		ON	vs.Store_No							= s.Store_No
				INNER JOIN  fn_Parse_List(@LocalStoreList, '|')				 sl		ON	sl.Key_Value						= vs.Store_No 
				LEFT JOIN	dbo.Store								(nolock) vendst ON	vendst.Store_No						= v.Store_No
			where
				-- Purchase Order Number search criteria 
				(@LocalOrderHeader_ID IS NULL
					OR (@LocalOrderHeader_ID IS NOT NULL 
						AND (oh.OrderHeader_ID = @LocalOrderHeader_ID or oh.OrderExternalSourceOrderID = @LocalOrderHeader_ID)))
				-- Vendor Invoice Number search criteria
				AND (@LocalInvoiceNumber IS NULL
					OR (@LocalInvoiceNumber IS NOT NULL 
						AND oh.InvoiceNumber = @LocalInvoiceNumber))
				-- Vendor ID search criteria
				AND (@LocalVendor_ID IS NULL
					OR (@LocalVendor_ID IS NOT NULL 
						AND v.Vendor_ID = @LocalVendor_ID))
				-- Vendor Key search criteria
				AND (@LocalVendor_Key IS NULL
					OR (@LocalVendor_Key IS NOT NULL
						AND v.Vendor_Key = @LocalVendor_Key))
				-- eInv-Only search criteria
				AND (@LocalEInvoiceOnly = 0
					OR (@LocalEInvoiceOnly = 1 AND oh.eInvoice_Id is not null))
				-- Invoice Date search criteria
				AND ((oh.InvoiceDate BETWEEN @LocalInvoiceDateStart AND @LocalInvoiceDateEnd)
					OR oh.invoiceDate is null)
				-- Order Date search criteria
				AND oh.OrderDate BETWEEN @LocalOrderDateStart AND @LocalOrderDateEnd
				AND ISNULL(vendst.Manufacturer, 0) = 0
				AND oh.OrderType_ID <> 3	
				-- Remediated-PO criteria (also should include order-item-level resolution codes, but that's done in a later query)
				AND oh.CloseDate IS NOT NULL
				AND oh.ApprovedDate IS NOT NULL
				AND ((@LocalResolutionCodeID = 0 and oh.ResolutionCodeID IS NOT NULL)
					OR (@LocalResolutionCodeID > 0 and oh.ResolutionCodeID = @LocalResolutionCodeID)
				)
	end



	/*
	=========================================================================================================================
	Check for a specific item-level Resolution Code.
	=========================================================================================================================

	** Lots of design notes.  Worth reading...?  Probably.

	This filter has a value of '0' if no resolution code was specified as input,
	so we apply a filter when the value is greater than '0'.

	Any Resolution Code was already applied at PO-level during query that pulled our initial set of target orders.

	This query is expensive because it's at the order-item level, so we only want to do it if a res-code was specified.

	If we find orders, we ADD them to our target-orders list because the search criteria allows the res-code at the
	order leve OR item level.  As such, we must apply the same order-level filters as our original query to
	maintain the OR operation.

	Yes, this is duplicated effort because, when a res-code is specified, we've already made a pass through with
	all this criteria at the order level, but it's worth the extra overhead under the rare case that a res-code is
	specified.

	Unfortunately, there are cases where an item-level res-code exists but there is no res-code at the order level.
	As such, we must still apply this filter to the item-level (if item-level res-code couldn't exist without order-level,
	we could check only at order level and be done).

	We've already done order-level res-code filtering in our initial order-list query,
	so we DO NOT redo that here, as we only care about item-level filtering for this pass.

	Make sure the resolution code filter at item level is skipped for suspended orders, as it does not apply.

	=========================================================================================================================

	A good enhancement would be to default to only order-level res-code filtering and force the user to specify
	when they want to ALSO apply that filter at the item level.

	*/

	if @LocalResolutionCodeID > 0
	begin

		-- Clear filtering table.
		truncate table #filteredOrders

		insert into #filteredOrders
			-- *NOTE: Exactly the same remediated-order query as above, when pulling the initial order list, but here, we filter to item-level res-code.
			select
				oh.OrderHeader_ID
			from 
				dbo.OrderHeader										(nolock) oh
				join orderitem										(nolock) oi		ON	oh.OrderHeader_Id					= oi.OrderHeader_Id
				INNER JOIN	dbo.Vendor								(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
				INNER JOIN	dbo.Vendor								(nolock) vs		ON	oh.PurchaseLocation_ID				= vs.Vendor_ID
				INNER JOIN	dbo.Store								(nolock) s		ON	vs.Store_No							= s.Store_No
				INNER JOIN  fn_Parse_List(@LocalStoreList, '|')				 sl		ON	sl.Key_Value						= vs.Store_No 
				LEFT JOIN	dbo.Store								(nolock) vendst ON	vendst.Store_No						= v.Store_No
			where
				-- Purchase Order Number search criteria 
				(@LocalOrderHeader_ID IS NULL
					OR (@LocalOrderHeader_ID IS NOT NULL 
						AND (oh.OrderHeader_ID = @LocalOrderHeader_ID or oh.OrderExternalSourceOrderID = @LocalOrderHeader_ID)))
				-- Vendor Invoice Number search criteria
				AND (@LocalInvoiceNumber IS NULL
					OR (@LocalInvoiceNumber IS NOT NULL 
						AND oh.InvoiceNumber = @LocalInvoiceNumber))
				-- Vendor ID search criteria
				AND (@LocalVendor_ID IS NULL
					OR (@LocalVendor_ID IS NOT NULL 
						AND v.Vendor_ID = @LocalVendor_ID))
				-- Vendor Key search criteria
				AND (@LocalVendor_Key IS NULL
					OR (@LocalVendor_Key IS NOT NULL
						AND v.Vendor_Key = @LocalVendor_Key))
				-- eInv-Only search criteria
				AND (@LocalEInvoiceOnly = 0
					OR (@LocalEInvoiceOnly = 1 AND oh.eInvoice_Id is not null))
				-- Invoice Date search criteria
				AND ((oh.InvoiceDate BETWEEN @LocalInvoiceDateStart AND @LocalInvoiceDateEnd)
					OR oh.invoiceDate is null)
				-- Order Date search criteria
				AND oh.OrderDate BETWEEN @LocalOrderDateStart AND @LocalOrderDateEnd
				AND ISNULL(vendst.Manufacturer, 0) = 0	
				-- Remediated-PO criteria (also should include order-item-level resolution codes, but that's done in a later query)
				AND oh.CloseDate IS NOT NULL
				AND oh.ApprovedDate IS NOT NULL
				-- We've already done order-level res-code filtering in our initial order-list query,
				-- so we DO NOT redo that here, as we only care about item-level filtering for this pass.
				AND oi.ResolutionCodeID = @LocalResolutionCodeID
			group by
				oh.OrderHeader_ID -- We need only distinct order numbers from item-level data.

	-- Add any orders we found to our main order list, ensuring no dups.
		insert into #orders
			select foh.orderheader_id
			from #filteredOrders foh left join #orders oh on foh.orderheader_id = oh.orderheader_id
			where foh.orderheader_id is not null -- Add filtered order #s that are not in the main list.

	end -- [END::if @LocalResolutionCodeID is not null]



	/*

	We do not need to join to the item-level tables if such filters were not pass, so we skip that
	query all together and save lots of overhead.

	*/
	if @LocalIdentifier is not null
	begin

		-- Clear filtering table because regardless of what happens below, any results in this table becomes our new list of target orders.
		truncate table #filteredOrders

		-- Find item key by identifier.
		declare @localIdentifierItemKey int
		select @localIdentifierItemKey = item_key from dbo.ItemIdentifier (nolock) where Identifier = @LocalIdentifier

		if @localIdentifierItemKey is not null
		begin

			insert into #filteredOrders
				select oh.orderheader_id
				from #orders oh
				join orderheader									(nolock)		on	oh.orderheader_id					= orderheader.orderheader_id
				join orderitem										(nolock) oi		ON	oh.OrderHeader_Id					= oi.OrderHeader_Id
				join ItemIdentifier									(nolock) ii		ON	ii.Item_Key							= oi.Item_Key
																															AND ii.Default_Identifier = 1
				where
					ii.item_key = @localIdentifierItemKey

		end

		-- Set our filtered list of orders as our new, main list of target orders.
		truncate table #orders;
		insert into #orders
			select orderheader_id from #filteredOrders

	end -- [END::if @LocalIdentifier is not null]


	/*
	Check for a specific VIN.
	*/
	if @LocalVIN is not null
	begin

		-- Clear filtering table because regardless of what happens below, any results in this table becomes our new list of target orders.
		truncate table #filteredOrders

		-- Find item key by VIN.
		declare @localVINItemKey int
		select @localVINItemKey = item_key from dbo.ItemVendor (nolock) where item_id = @LocalVIN

		-- Nothing to do if the VIN isn't found.
		if @localVINItemKey is not null
		begin

			insert into #filteredOrders
				select oh.orderheader_id
				from #orders oh
				-- We have to join back to OH to get Vendor_ID for join to ItemVendor because our temp table only has OHID.
				join orderheader									(nolock)		on	oh.orderheader_id					= orderheader.orderheader_id
				join orderitem										(nolock) oi		ON	oh.OrderHeader_Id					= oi.OrderHeader_Id
				join ItemVendor										(nolock) iv		ON	iv.Item_Key							= oi.Item_Key
																					AND iv.Vendor_ID						= orderheader.Vendor_ID
				where
					iv.item_key = @localVINItemKey
		end

		-- Set our filtered list of orders as our new, main list of target orders.
		truncate table #orders;
		insert into #orders
			select orderheader_id from #filteredOrders

	end -- [END::if @LocalVIN is not null]


	/*
	Check for a specific Control Group ID.
	*/
	if @LocalOrderInvoice_ControlGroup_ID is not null
	begin

		-- Clear filtering table because regardless of what happens below, any results in this table becomes our new list of target orders.
		truncate table #filteredOrders

			insert into #filteredOrders
				select oh.orderheader_id
				from #orders oh
			LEFT JOIN	dbo.OrderInvoice_ControlGroupInvoice	(nolock) cgi	ON	oh.OrderHeader_ID					= cgi.OrderHeader_ID
			LEFT JOIN	dbo.OrderInvoice_ControlGroup			(nolock) cg		ON	cgi.OrderInvoice_ControlGroup_ID	= cg.OrderInvoice_ControlGroup_ID 
				where
					cg.OrderInvoice_ControlGroup_ID = @LocalOrderInvoice_ControlGroup_ID
					AND cg.OrderInvoice_ControlGroupStatus_ID = 2
					AND (
						cgi.ValidationCode = 0
						OR
						dbo.fn_IsWarningValidationCode(cgi.ValidationCode) = 1
					)

		-- Set our filtered list of orders as our new, main list of target orders.
		truncate table #orders;
		insert into #orders
			select orderheader_id from #filteredOrders

	end -- [END::if @LocalOrderInvoice_ControlGroup_ID is not null]


	---------------------------------------------------------------------
	---------------------------------------------------------------------
	---------------------------------------------------------------------

	/*
		Build table to house...
		1) "is qty mismatch" assessment without having to call fn_IsQuantityMismatch.
		2) OI-aggregates, which are the attributes returned for the order itemself (order level)
		   that are determined by checking all the items in the order (item level).
	*/
	if object_id('tempdb..#oiAggregates') is not null
	begin
		drop table #oiAggregates
	end
	create table #oiAggregates (
		orderheader_id int not null
		,eInvoice_Id int null -- OH
		,QtyMismatch varchar(5) not null default 'N'
		,eInvoiceQuantity decimal(18,4) null
		,QuantityReceived decimal(18,4) null
		-- The following five fields are consumed as Integers because that's how they were being served originally (directly from SELECT clause, not in table or variables).
		,DiscountType int not null default 0 -- OH
		,AdjustedCost int null
		,CbW int null
		,CostNotByCase int null
		,OrderUnitMismatch int null
		PRIMARY KEY (
			orderheader_id
		)
	)


	/*
	Get data for qty-mismatch assessment.
	We're doing this here, rather than via FN-call because this is far more efficient than calling FN for each target order.
	The general process is we will pull the necessary data into a temp table, then make a pass through that extracted data
	to build the Qty-Mismatch value: Y, N, or N/A.

	We perform the 'N/A' assessment during the initial query and default to no mismatch ('N') for all others,
	then we update mismatched entries with 'Y'.
	*/

	-- NEW, qty mismatch combined with other OI aggregates

	insert into #oiAggregates(orderheader_id, eInvoiceQuantity, QuantityReceived, AdjustedCost, CbW, CostNotByCase, OrderUnitMismatch)
		select
			oh.orderheader_id
			,eInvoiceQuantity = sum(isnull(oi.eInvoiceQuantity, 0.0))
			,QuantityReceived = sum(isnull(oi.QuantityReceived, 0.0))
			,[AdjustedCost]					=	CASE 
													WHEN 0 < SUM(oi.DiscountType) Then 1 
													WHEN 0 < SUM(oi.AdjustedCost) Then 1 
													ELSE 0
													END
			,[CbW]							=	case when SUM(cast(I.CostedByWeight as Int) + cast(I.CatchweightRequired as Int)) > 0 then 1 else 0 end
			,[CostNotByCase]				=	case when sum(case
													when IU.Unit_Name <> 'CASE' AND OI.Package_Desc1 > 1 
													Then 1
													Else 0
													End) > 0
												then 1
												else 0
												end
			,[OrderUnitMismatch]			=	case when sum(Case 
													When OI.QuantityUnit <> I.Vendor_Unit_ID
													Then 1
													Else 0
													End) > 0
												then 1
												else 0
												end
		from
			#orders foh -- Filtered order list.
			join orderheader oh (nolock)
				on foh.orderheader_id = oh.orderheader_id
			join orderitem oi (nolock)
				on oh.orderheader_id = oi.orderheader_id
			join Item I (nolock)
				on OI.Item_Key = I.Item_Key
			join ItemUnit IU (nolock)
				on IU.Unit_ID = OI.CostUnit
	group by
		oh.orderheader_id


	-- We handled the n/a case above and defaulted everything else to "N", so we just have to apply the "Y" values now.
	update #oiAggregates
	set
		QtyMismatch = case
						when isnull(oh.eInvoice_Id, 0) = 0
							then 'N/A'
						when ISNULL(eInvoiceQuantity, 0.0) - ISNULL(QuantityReceived, 0.0) <> 0.0
							then 'Y'
						else 'N'
						end
		/*
			The following AdjustedCost value means we get the adjusted-cost "flag" from either the existing value,
			which was pulled from the order-item level, or from the order level.
		*/
		,AdjustedCost = AdjustedCost | oh.DiscountType
	from #oiAggregates oia
			join orderheader oh (nolock)
				on oia.orderheader_id = oh.orderheader_id



	/*
	==========================================================================
	Main SQL
	==========================================================================
	
	The culprit for making this query run a few times slower than it should is this:
	[QtyMismatch]				=	dbo.fn_IsQuantityMismatch(oh.OrderHeader_ID),

	So, this was pulled out into logic above that is applied once, rather than for each row (order) returned.

	*/
	SELECT
		[OrderHeader_ID]			=	oh.OrderHeader_ID,
		[SourcePONumber]            =	CONVERT(varchar(15), ISNULL(oh.OrderExternalSourceOrderID, oh.OrderHeader_ID)) + ' - (' + ISNULL(os.Description, 'IRMA') + ')',
		[CloseDate]					=	oh.CloseDate,
		[VStoreName]				=	vs.CompanyName,
		[SubTeamName]				=	st.SubTeam_Name,
		[GLAccount]					=	CASE oh.ProductType_ID
											WHEN 1 THEN 
												ISNULL(CONVERT(varchar(6), st.GLPurchaseAcct), '500000')
											WHEN 2 THEN 
												ISNULL(CONVERT(varchar(6), st.GLPackagingAcct), '510000')
											WHEN 3 THEN 
												ISNULL(CONVERT(varchar(6), st.GLSuppliesAcct), '800000')
											ELSE 
												'0' 
										END,
		[Vendor_ID]					=	oh.Vendor_ID,
		[VendorName]				=	V.CompanyName,					    	
		[VendorPSExportID]			=	V.PS_Export_Vendor_ID,
		[Vendor_Key]				=	V.Vendor_Key,
		[InvoiceNumber]				=	oh.InvoiceNumber,
		[MatchingValidationCode]	=	oh.MatchingValidationCode,
		[MatchingDate]				=	oh.MatchingDate,
		[MatchingUser_ID]			=	oh.MatchingUser_ID,
		[MatchingValidationDesc]	=	VC.Description,
		[InvoiceSubteam_No]			=	ov.SubTeam_No,
		[vStoreID]					=	vs.Store_No,                            
		[VState]					=	vs.State,									
		[VZoneID]					=	S.Zone_ID,									
		[DocumentDataExists]		=	CASE ISNULL(oh.VendorDoc_ID, '')		
											WHEN '' THEN 
												0
											ELSE 
												1
										END,
		[TotalInvoiceCost]			=	ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0),
		[TotalInvoiceCostNoCharges] =	round(ISNULL(ov.InvoiceTotalCost, 0), 2, 1),
		[TotalInvoiceFreight]		=	ISNULL(ov.InvoiceFreight, 0),
		[TotalOrderCost]			=	round(
											CASE ISNULL(oh.DiscountType, -1)
												WHEN 1 THEN 
													ISNULL(oh.AdjustedReceivedCost, 0) - oh.QuantityDiscount
												WHEN 2 THEN 
													ISNULL(oh.AdjustedReceivedCost, 0) - (ISNULL(oh.AdjustedReceivedCost, 0) * (oh.QuantityDiscount / 100))
												WHEN 4 THEN 
													ISNULL(oh.AdjustedReceivedCost, 0) - (ISNULL(oh.AdjustedReceivedCost, 0) * (oh.QuantityDiscount / 100))
												ELSE 
													ISNULL(oh.AdjustedReceivedCost, 0)
											END
											,2, 1),
		[TotalCostDiff]				=	CASE ISNULL(oh.DiscountType, -1)
											WHEN 1 THEN 
												ABS(ISNULL(oh.AdjustedReceivedCost, 0) - oh.QuantityDiscount) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
											WHEN 2 THEN 
												ABS((ISNULL(oh.AdjustedReceivedCost, 0)) - ((ISNULL(oh.AdjustedReceivedCost, 0)) * (oh.QuantityDiscount / 100))) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
											WHEN 4 THEN 
												ABS((ISNULL(oh.AdjustedReceivedCost, 0)) - ((ISNULL(oh.AdjustedReceivedCost, 0)) * (oh.QuantityDiscount / 100))) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
											ELSE 
												ABS((ISNULL(oh.AdjustedReceivedCost, 0))) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
										END,
		[OrderDate]					=	oh.OrderDate,								
		[InvoiceDate]				=	oh.InvoiceDate,
		[SentDate]					=	oh.SentDate,
		[POCostEffectiveDate]		=	oh.POCostDate,
		[POCreator]					=	uo.FullName, 
		[POCloser]					=	uc.FullName, 
		[MatchedeEInvoiceExists]	=	CASE ISNULL(oh.eInvoice_Id, '')
											WHEN '' THEN 
												0
											ELSE
												1
										END,
		[POTransmissionType]		=	CASE 
											WHEN oh.Fax_Order = 1 THEN 
												'Fax'
											WHEN oh.Email_Order = 1 THEN 
												'Email'
											WHEN oh.Electronic_Order = 1 THEN 
												'Electronic'
											ELSE 
												'Manual'
										END, 
		[Notes]						=	oh.OrderHeaderDesc,
		[PaymentTerms]				=	PT.Description,
		[PayByAgreedCost]			=	oh.PayByAgreedCost,										
		[PaymentType]				=   case when oh.PayByAgreedCost = 1 then 'Pay Agreed Cost' else 'Pay By Invoice' end,										
		[NoOfDaysSuspended]			=	DATEDIFF(DAY, oh.CloseDate, GETDATE()),	
		[Beverage]					=	ST.Beverage,
		[CreditPO]					=	oh.Return_Order,
		[AdjustedCost]				=	oia.AdjustedCost,
		[DocTypeOther]				=	CASE ISNULL(oh.VendorDoc_ID, '')		
											WHEN '' THEN 
												CASE WHEN oh.InvoiceNumber is null or oh.InvoiceNumber = '' THEN
													1
												ELSE
													CASE WHEN oh.DSDOrder = 1 and oh.eInvoice_Id is null THEN
														1
													ELSE 
														0
													END
												END		
											ELSE 
												1										
										END,
		[CbW]						=	oia.CbW,
		[QtyMismatch]				=	oia.QtyMismatch,
		[CostNotByCase]				=	oia.CostNotByCase,
		[OrderUnitMismatch]			=	oia.OrderUnitMismatch,
		[Charges]					=	ROUND(ISNULL((SELECT SUM(Value) FROM OrderInvoiceCharges WHERE OrderHeader_Id = oh.OrderHeader_ID AND IsAllowance = 0), 0), 2, 1),
		[POAdminNotes]				=	oh.AdminNotes,
		[ResolutionCode]			=	rc.ReasonCodeDesc,
		[ResolutionCodeId]			=	rc.reasoncodedetailid,
		[OrderType_ID]				=	oh.OrderType_ID,
		[eInvoice_Id]				=	oh.eInvoice_Id,
		[CreatedBy]					=	oh.CreatedBy,
		[ClosedBy]					=	oh.ClosedBy,
		[Status]					=	case when oh.MatchingValidationCode > 0 and oh.InReview = 0 and oh.ApprovedDate IS NULL then 'Open'
										when oh.MatchingValidationCode > 0 and oh.InReview =1 and oh.ApprovedDate IS NULL then 'In Review'
										when oh.MatchingValidationCode > 0 and oh.InReview =0 and oh.ResolutionCodeID IS NOT NULL then 'Resolved'
										end,
		[InReviewUsername]			=	uir.username,
		[InReviewFullname]			=	uir.fullname,
		[ReceivingDocument]			=	oh.dsdorder,
		[PartialShipment]			=	oh.PartialShipment,
		[EinvoiceRequired]          =   v.EinvoiceRequired,
		[TotalRefused]				=	oh.TotalRefused
	FROM
		#orders toh -- target order list
		join #oiAggregates oia												ON	toh.OrderHeader_Id					= oia.OrderHeader_Id
		join dbo.OrderHeader								(nolock) oh		ON	toh.OrderHeader_Id					= oh.OrderHeader_Id
		LEFT JOIN	dbo.OrderInvoice						(nolock) ov		ON	oh.OrderHeader_Id					= ov.OrderHeader_Id
		INNER JOIN	dbo.Vendor								(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
		INNER JOIN	dbo.Subteam								(nolock) st		ON	oh.Transfer_to_Subteam				= st.Subteam_No
		INNER JOIN	dbo.Vendor								(nolock) vs		ON	oh.PurchaseLocation_ID				= vs.Vendor_ID
		INNER JOIN	dbo.Store								(nolock) s		ON	vs.Store_No							= s.Store_No
		INNER JOIN  fn_Parse_List(@LocalStoreList, '|')				 sl		ON	sl.Key_Value						= vs.Store_No 
		LEFT JOIN	dbo.ReasonCodeDetail					(nolock) rc		ON	oh.ResolutionCodeID					= rc.ReasonCodeDetailID
		LEFT JOIN	dbo.Users								(nolock) uo		ON	oh.createdby						= uo.User_ID
		LEFT JOIN	dbo.Users								(nolock) uc		ON	oh.ClosedBy							= uc.User_ID
		LEFT JOIN	dbo.Users								(nolock) uir	ON	oh.InReviewUser						= uir.User_ID
		LEFT JOIN	dbo.VendorPaymentTerms					(nolock) pt		ON	v.PaymentTermID						= pt.PaymentTermID
		LEFT JOIN	dbo.ValidationCode						(nolock) vc		ON	oh.MatchingValidationCode			= vc.ValidationCode
		LEFT JOIN	dbo.OrderExternalSource					(nolock) os     ON	oh.OrderExternalSourceID			= os.ID			
	ORDER BY 
		oh.OrderHeader_ID

	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSuspendedOrderSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSuspendedOrderSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSuspendedOrderSearch] TO [IRMAReportsRole]
    AS [dbo];

