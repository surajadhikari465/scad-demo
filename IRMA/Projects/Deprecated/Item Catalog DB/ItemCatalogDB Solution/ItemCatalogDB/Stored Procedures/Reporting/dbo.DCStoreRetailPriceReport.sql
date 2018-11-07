IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DCStoreRetailPriceReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DCStoreRetailPriceReport]
GO

/****** Object:  StoredProcedure [dbo].[DCStoreRetailPriceReport]    Script Date: 07/25/2012 14:57:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DCStoreRetailPriceReport]
	@Vendor_ID		int,
	@Zone_ID		int,
	@IsRegional		bit,
	@Store_No_List	varchar(8000),
	@SubTeam_No		int,
	@Identifier		varchar(14)
AS 
    -- **************************************************************************
   -- Procedure: DCStoreRetailPriceReport()
   --    Author: Billy Blackerby
   --      Date: 01/14/2009
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 01/14/2009  BBB	Took copy of Reporting_VendorItems to create base report; cleaned
   --					up SQL and formatted; added in calls from WarehouseMovement report
   -- 01/15/2009  BBB	Continued development on the report by adding in calls to
   --					Store, Price, and PriceChangeType
   -- 01/19/2009  BBB	Started from ground up with query removing extraneous joins 
   --					and cross applies slowing performance
   -- 01/20/2009  BBB	Added in updated WarehouseMovement code to handle OnOrder and
   --					FutureArrival based upon the DC being the vendor; added in calls
   --					to ItemVendor to return Item_ID
   -- 01/22/2009  BBB	Added in manual call to AvgCostHistory and changes StoreNo to pull
   --					from vendor warehouse storeno instead of storeno; corrected 
   --					markup issue with using store instead of warehouse
   -- 5/6/2009	   MU	(for bug 9617) Fixed StoreItemVendor join in #DCRPWeekly section to 
   --					store table	rather than the vendor table.
   --					Also, added PriceMultiple column to report and margin calculation
   -- 5/7/2009	   MU	changed @VendorID to @Vendor_ID to be consistent with the report parameter
   --
   -- 10/2/2012	   DF	Removed all references to ItemCaseHistory.  The "ItemCaseHistrionics", 
   --					if you will.
   -- 01/11/2013   MZ   TFS 8755 - Replace Item.Discontinue_Item with a function call to 
   --                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON  

	--**************************************************************************
	-- Create internal SP variables
	--************************************************************************** 
	DECLARE @Identifier2	varchar(16)
	DECLARE @tblStoreList	table
							(
							Store_No	int
							)

	--**************************************************************************
	-- Add wildcards for searching if none exist within these parameters
	--**************************************************************************
	IF CHARINDEX('%', @Identifier) = 0
		SELECT @Identifier2 = '%' + @Identifier + '%'
	ELSE
		SELECT @Identifier2 = @Identifier
		
	--**************************************************************************
	-- Verify the UPC (if specified)
	--**************************************************************************
	IF @Identifier2 IS NOT NULL
		IF NOT EXISTS	(
						SELECT 
							* 
						FROM 
							ItemIdentifier 
						WHERE 
							Identifier LIKE @Identifier2
						)
			BEGIN
				RAISERROR('No items found matching the UPC ''%s''!', 16, 1, @Identifier)
			END

	--**************************************************************************
	-- Verify the VendorID exists (if specified)
	--**************************************************************************
	IF NOT EXISTS	(
					SELECT 
						* 
					FROM 
						Vendor 
					WHERE 
						Vendor_ID = ISNULL(@Vendor_ID, Vendor_ID)
					)
		BEGIN
			RAISERROR('No Vendor found with Vendor_ID = %d!', 16, 1, @Vendor_ID)
		END

	--**************************************************************************
	-- Get the stores to use in the report
	--**************************************************************************
   	IF @IsRegional = 1
		BEGIN
			INSERT INTO @tblStoreList
				SELECT 
					Store_No 
				FROM 
					Store
				WHERE
					Distribution_Center <>  1
		END
	ELSE IF @Zone_ID IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT 
					Store_No 
				FROM 
					Store
				WHERE 
					Zone_ID					=	@Zone_ID
					AND Distribution_Center <>  1
		END
	ELSE IF @Store_No_List IS NOT NULL
		BEGIN
			INSERT INTO @tblStoreList
				SELECT 
					Key_Value
				FROM 
					fn_Parse_List(@Store_No_List, ',') FN
				WHERE 
					EXISTS	(
							SELECT 
								* 
							FROM 
								Store 
							WHERE 
								Store_No = FN.Key_Value
							)
		END

	--**************************************************************************
	-- Verify stores were selected to use
	--**************************************************************************
	IF NOT EXISTS (SELECT * FROM @tblStoreList)
		BEGIN
			RAISERROR('No stores have been selected for the report!', 16, 1)
		END

	--**************************************************************************
	--Select Future Arrival for the DC based upon Today's date
	--**************************************************************************
    CREATE TABLE #DCRPFuture
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			FutureQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max),
			VendorItem	varchar(20)
			)

    INSERT INTO #DCRPFuture
		SELECT 
			[Item_Key]		=	oi.Item_Key,
			[Identifier]	=	ii.Identifier,
			[SubTeam]		=	oh.Transfer_To_SubTeam,
			[Qty]			=	SUM(oi.QUantityOrdered),
			[PackSize]		=	oi.Package_Desc1,
			[PackType]		=	iu.Unit_Abbreviation,
			[VendorItem]	=	iv.Item_ID
		FROM
			OrderHeader						(nolock) oh
			INNER JOIN OrderItem			(nolock) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
			INNER JOIN ItemIdentifier		(nolock) ii		ON	oi.Item_Key				= ii.Item_Key
															AND ii.Default_Identifier	= 1
			INNER JOIN Vendor				(nolock) v		ON	oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN Vendor				(nolock) vw		ON	oh.PurchaseLocation_ID	= vw.Vendor_ID
			INNER JOIN ItemVendor			(nolock) iv		ON	oi.Item_Key				= iv.Item_Key
															AND v.Vendor_ID				= iv.Vendor_ID
			INNER JOIN ItemUnit				(nolock) iu		ON	oi.CostUnit				= iu.Unit_ID
		WHERE 
			oh.CloseDate									IS		NULL 
			AND	oi.DateReceived								IS		NULL 
			AND oh.Expected_Date							>=		GETDATE()
			AND oh.Return_Order								=		0 
			AND CONVERT(smalldatetime, GETDATE(), 101)		<		ISNULL(iv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
			AND vw.Store_No									IN		(SELECT Store_No FROM Store WHERE Distribution_Center = 1)
			AND oh.Transfer_To_SubTeam						=		ISNULL(@SubTeam_No, oh.Transfer_To_SubTeam)
			AND v.Vendor_ID									=		ISNULL(@Vendor_ID, v.Vendor_ID)
			AND ii.Identifier								LIKE	ISNULL(@Identifier2, ii.Identifier) 
		GROUP BY 
			oi.Item_Key, 
			ii.Identifier,
			oh.Transfer_To_SubTeam, 
			oi.Package_Desc1,
			iu.Unit_Abbreviation,
			iv.Item_ID

	--**************************************************************************
	--Select Store On Order from the DC totals based upon allocation and order status
	--**************************************************************************
    CREATE TABLE #DCRPOnOrder
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			OnOrderQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max),
			VendorItem	varchar(20)
			)

	INSERT INTO #DCRPOnOrder
		SELECT
			[Item_Key]		=	oi.Item_Key,
			[Identifier]	=	ii.Identifier,
			[SubTeam]		=	oh.Transfer_To_SubTeam,
			[Qty]			=	SUM(oi.QUantityOrdered),
			[PackSize]		=	oi.Package_Desc1,
			[PackType]		=	iu.Unit_Abbreviation,
			[VendorItem]	=	iv.Item_ID
		FROM
			OrderHeader						(nolock) oh
			INNER JOIN OrderItem			(nolock) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
			INNER JOIN ItemIdentifier		(nolock) ii		ON	oi.Item_Key				= ii.Item_Key
															AND ii.Default_Identifier	= 1
			INNER JOIN Vendor				(nolock) vw		ON	oh.Vendor_ID			= vw.Vendor_ID
			INNER JOIN Vendor				(nolock) vs		ON	oh.ReceiveLocation_ID	= vs.Vendor_ID
			INNER JOIN StoreItemVendor		(nolock) siv	ON	oi.Item_Key				= siv.Item_Key
															AND vw.Store_No				= siv.Store_No
															AND siv.PrimaryVendor		= 1
			INNER JOIN ItemUnit				(nolock) iu		ON	oi.CostUnit				= iu.Unit_ID
			INNER JOIN ItemVendor			(nolock) iv		ON	siv.Vendor_ID			= iv.Vendor_ID
															AND	siv.Item_Key			= iv.Item_Key
		WHERE 
			oh.CloseDate									IS		NULL
			AND oh.WareHouseSentDate						IS		NOT NULL
			AND CONVERT(smalldatetime, GETDATE(), 101)		<		ISNULL(siv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
			AND CONVERT(smalldatetime, GETDATE(), 101)		<		ISNULL(iv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
			AND vw.Store_No									IN		(SELECT Store_No FROM Store WHERE Distribution_Center = 1)
			AND vs.Store_No									IN		(SELECT Store_No FROM @tblStoreList)
			AND oh.Transfer_To_SubTeam						=		ISNULL(@SubTeam_No, oh.Transfer_To_SubTeam)
			AND siv.Vendor_ID								=		ISNULL(@Vendor_ID, siv.Vendor_ID)
			AND ii.Identifier								LIKE	ISNULL(@Identifier2, ii.Identifier) 
		GROUP BY 
			oi.Item_Key, 
			ii.Identifier,
			oh.Transfer_To_SubTeam, 
			oi.Package_Desc1,
			iu.Unit_Abbreviation,
			iv.Item_ID

	--**************************************************************************
	--Select OnHand Totals
	--**************************************************************************
    CREATE TABLE #DCRPOnHand
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			OnHandQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max),
			VendorItem	varchar(20)
			)

    INSERT INTO #DCRPOnHand
		SELECT
			[Item_Key]		=	i.Item_Key,
			[Identifier]	=	ii.Identifier,
			[SubTeam]		=	st.SubTeam_No,
			[Qty]			=	coh.OnHand,
			[PackSize]		=	coh.Pack,
			[PackType]		=	coh.PackUOM,
			[VendorItem]	=	iv.Item_ID
		FROM
			Item								(nolock) i		
			INNER JOIN		ItemIdentifier		(nolock) ii		ON	ii.Item_Key					= i.Item_Key 
																AND ii.Default_Identifier		= 1
			INNER JOIN		ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
			INNER JOIN		SubTeam				(nolock) st		ON	st.SubTeam_No				= st.SubTeam_No
			INNER JOIN		Store				(nolock) s		ON	s.Store_No					= s.Store_No
			INNER JOIN		StoreItemVendor		(nolock) siv	ON	siv.Store_No				= s.Store_No
																AND siv.PrimaryVendor			= 1
																AND siv.Item_Key				= i.Item_Key
			INNER JOIN		Vendor				(nolock) v		ON  v.Vendor_ID					= siv.Vendor_ID
			INNER JOIN		ItemVendor			(nolock) iv		ON	siv.Vendor_ID				= iv.Vendor_ID
																AND	siv.Item_Key				= iv.Item_Key
			INNER JOIN		(
							SELECT 
									[Store_No]		=	s.Store_No,
									[Item_Key]		=	ih.Item_Key, 
									[Pack]			=	i.Package_Desc1,
									[OnHand]		=	SUM(CASE 
																WHEN ISNULL(ih.Quantity, 0) > 0 THEN 
																	ih.Quantity / 
																							CASE 
																								WHEN i.Package_Desc1 <> 0 THEN 
																									i.Package_Desc1 
																								ELSE 
																									1 
																							END
																ELSE 
																	ISNULL(ih.Weight, 0) / 
																							CASE 
																								WHEN i.Package_Desc1 * i.Package_Desc2 <> 0 THEN 
																									(i.Package_Desc1 * i.Package_Desc2) 
																								ELSE 
																									1 
																							END 
															END
																	 * ia.Adjustment_Type),
									[PackUOM]		=	dbo.fn_GetRetailUnitAbbreviation(ih.Item_Key),
									[QuantityUOM]	=	dbo.fn_GetDistributionUnitAbbreviation(ih.Item_Key),
									[SubTeam_No]	=	ISNULL(ih.SubTeam_No, i.SubTeam_No)
								FROM 
									OnHand						(nolock) oh
									CROSS JOIN Store			(nolock) s
									INNER JOIN ItemHistory		(nolock) ih		ON	ih.Item_Key				= oh.Item_Key 
																				AND ih.Store_No				= oh.Store_No 
																				AND ih.SubTeam_No			= oh.SubTeam_No
									INNER JOIN ItemAdjustment	(nolock) ia		ON	ih.Adjustment_ID		= ia.Adjustment_ID
									INNER JOIN Item				(nolock) i		ON	i.Item_Key				= oh.Item_Key
									INNER JOIN ItemIdentifier	(nolock) ii		ON	ii.Item_Key				= i.Item_Key 
																				AND ii.Default_Identifier	= 1
									--LEFT JOIN ItemCaseHistory   (nolock) ich	ON	ich.ItemHistoryID		= ih.ItemHistoryID
								WHERE 
									oh.Store_No			=		s.Store_No
									AND oh.SubTeam_No	=		ISNULL(ih.SubTeam_No, i.SubTeam_No)
									AND ih.DateStamp	>=		ISNULL(oh.LastReset, ih.DateStamp)
									AND ii.Identifier	LIKE	ISNULL(@Identifier2, ii.Identifier)
									AND Deleted_Item = 0
								GROUP BY 
									s.Store_No, 
									ih.Item_Key, 
									i.Package_Desc1, 
									ISNULL(ih.SubTeam_No, i.SubTeam_No)
								HAVING 
									SUM(CASE 
											WHEN ISNULL(ih.Quantity, 0) > 0 THEN 
												ih.Quantity /	
																		CASE 
																			WHEN i.Package_Desc1 <> 0 THEN 
																				i.Package_Desc1 
																			ELSE 
																				1 
																		END
											ELSE 
												ISNULL(ih.Weight, 0) / 
																				CASE 
																					WHEN i.Package_Desc1 * i.Package_Desc2 <> 0 THEN 
																						(i.Package_Desc1 * i.Package_Desc2) 
																					ELSE 
																						1 
																				END 
										END * ia.Adjustment_Type) <> 0
							)	AS						 coh	ON	coh.Item_Key				= i.Item_Key
																AND coh.Store_No				= s.Store_No
																AND coh.SubTeam_No				= st.SubTeam_No
		WHERE 
			s.Store_No										IN		(SELECT Store_No FROM Store WHERE Distribution_Center = 1)
			AND st.SubTeam_No								=		ISNULL(@SubTeam_No, st.SubTeam_No)
			AND siv.Vendor_ID								=		ISNULL(@Vendor_ID, siv.Vendor_ID)
			AND ii.Identifier								LIKE	ISNULL(@Identifier2, ii.Identifier)
			AND CONVERT(smalldatetime, GETDATE(), 101)		<		ISNULL(siv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
			AND CONVERT(smalldatetime, GETDATE(), 101)		<		ISNULL(iv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))

	--**************************************************************************
	--Select This Week, Last Week, 2Week, 3Week movement totals for full fiscal 
	--week based upon today's date, and select 4Week and 8Week Avg
	--**************************************************************************
	DECLARE @LastWeekDate varchar(12)
	SET @LastWeekDate= CONVERT(varchar(12), DATEADD("d", -7, DATEADD("d",-(DATEPART(dw,GETDATE()) -2),GETDATE()))   , 101)

    CREATE TABLE #DCRPWeekly
			(
			Item_Key		int,
			Identifier		varchar(max),
			SubTeam_No		int, 
			ThisWeekQty		decimal(18,4), 
			LastWeekQty		decimal(18,4),
			TwoWeekTotal	decimal(18,4),
			ThreeWeekTotal	decimal(18,4),
			FourWeekAvg		decimal(18,4),
			EightWeekAvg	decimal(18,4),
			PackSize		int,
			PackType		varchar(max),
			VendorItem		varchar(20)
			)

	INSERT INTO #DCRPWeekly
		SELECT
			[Item_Key]		= Item_Key,
			[Identifier]	= Identifier,
			[SubTeam]		= SubTeam,
			[QtyThisWeek]	= SUM(
								CASE 
									WHEN WeekNo = 1 THEN
										Qty
								END),
			[QtyLastWeek]	= SUM(
								CASE 
									WHEN WeekNo = -1 THEN
										Qty
								END),
			[QtyTwoWeek]	= SUM(
								CASE 
									WHEN WeekNo = -2 THEN
										Qty
								END),
			[QtyThreeWeek]	= SUM(
								CASE 
									WHEN WeekNo = -3 THEN
										Qty
								END),
			[QtyFourWeek]	= SUM(
								CASE 
									WHEN FourWeekAvg = 1 THEN
										Qty
								END) / 4,
			[QtyEightWeek]	= SUM(Qty) / 8,
			[PackSize]		= PackSize,
			[PackType]		= PackType,
			[VendorItem]	= VendorItem
		FROM
			(
				SELECT
					[Item_Key]		=	oi.Item_Key,
					[Identifier]	=	ii.Identifier,
					[SubTeam]		=	oh.Transfer_To_SubTeam,
					[Qty]			=	SUM(oi.QuantityOrdered),
					[PackSize]		=	oi.Package_Desc1,
					[PackType]		=	iu.Unit_Abbreviation,
					[WeekNo]		=	CASE
											WHEN oh.CloseDate >= DATEADD("d", +7, @LastWeekDate) THEN
												1
											WHEN oh.CloseDate >= @LastWeekDate AND oh.CloseDate <= DATEADD("d", +7, @LastWeekDate) THEN
												-1
											WHEN oh.CloseDate >= DATEADD("d", -7, @LastWeekDate) AND oh.CloseDate <= @LastWeekDate THEN
												-2
											WHEN oh.CloseDate >= DATEADD("d", -14, @LastWeekDate) AND oh.CloseDate <= DATEADD("d", -7, @LastWeekDate) THEN
												-3
										END,
					[FourWeekAvg]	=	CASE
											WHEN  oh.CloseDate >= DATEADD("d", -21, @LastWeekDate) THEN
												1
											ELSE
												0
										END,
					[VendorItem]	=	iv.Item_ID
				FROM
					OrderHeader						(nolock) oh		
					INNER JOIN OrderItem			(nolock) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
					INNER JOIN ItemIdentifier		(nolock) ii		ON	oi.Item_Key				= ii.Item_Key
																	AND ii.Default_Identifier	= 1
					INNER JOIN Vendor				(nolock) vw		ON	oh.Vendor_ID			= vw.Vendor_ID
					INNER JOIN Vendor				(nolock) vs		ON	oh.ReceiveLocation_ID	= vs.Vendor_ID
					INNER JOIN StoreItemVendor		(nolock) siv	ON	oi.Item_Key				= siv.Item_Key
																	AND vs.Store_No				= siv.Store_No
																	AND siv.PrimaryVendor		= 1
					INNER JOIN ItemUnit				(nolock) iu		ON	oi.CostUnit				= iu.Unit_ID
					INNER JOIN ItemVendor			(nolock) iv		ON	siv.Vendor_ID			= iv.Vendor_ID
																	AND	siv.Item_Key			= iv.Item_Key
				WHERE 
					oh.CloseDate									IS		NOT NULL
					AND oh.CloseDate								>=		DATEADD("d", -49, @LastWeekDate)
					AND CONVERT(smalldatetime, GETDATE(), 101)		<		ISNULL(siv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
					AND CONVERT(smalldatetime, GETDATE(), 101)		<		ISNULL(iv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
					AND vw.Store_No									IN		(SELECT Store_No FROM Store WHERE Distribution_Center = 1)
					AND vs.Store_No									IN		(SELECT Store_No FROM @tblStoreList)
					AND oh.Transfer_To_SubTeam						=		ISNULL(@SubTeam_No, oh.Transfer_To_SubTeam)
					AND siv.Vendor_ID								=		ISNULL(@Vendor_ID, siv.Vendor_ID)
					AND ii.Identifier								LIKE	ISNULL(@Identifier2, ii.Identifier)
				GROUP BY 
					oi.Item_Key, 
					ii.Identifier,
					oh.Transfer_To_SubTeam, 
					oi.Package_Desc1,
					iu.Unit_Abbreviation,
					oh.CloseDate,
					iv.Item_ID
			) AS inner_result
		GROUP BY
			Item_Key,
			Identifier,
			SubTeam,
			PackSize,
			PackType,
			VendorItem

	--**************************************************************************
	--Select #Future, #OnHand, #ThisWeek, #LastWeek, #2Week, #3Week, #4Week, #8Week values into report output
	--**************************************************************************
	SELECT
		[Class]				=	ic.Category_Name,
		[Item_Description]	=	i.Item_Description, 
		[SubTeam]			=	st.SubTeam_Abbreviation,
		[Identifier]		=	REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier),
		[Brand]				=	ib.Brand_Name,
		[ItemID]			=	ISNULL(wk.VendorItem, npd.VendorItem),
		[PackSize]			=	ISNULL(wk.PackSize, npd.PackSize),
		[PackType]			=	ISNULL(wk.PackType, npd.PackType),
		[FutureQty]			=	SUM(ISNULL(fu.FutureQty, 0)),
		[OnHandQty]			=	SUM(ISNULL(oh.OnHandQty, 0)),
		[OnOrderQty]		=	SUM(ISNULL(oo.OnOrderQty, 0)),
		[ThisWeekQty]		=	SUM(ISNULL(wk.ThisWeekQty, 0)),
		[LastWeekQty]		=	SUM(ISNULL(wk.LastWeekQty, 0)),
		[TwoWeekTotal]		=	SUM(ISNULL(wk.TwoWeekTotal,0)),
		[ThreeWeekTotal]	=	SUM(ISNULL(wk.ThreeWeekTotal,0)),
		[FourWeekAvg]		=	SUM(ISNULL(wk.FourWeekAvg, 0)),
		[EightWeekAvg]		=	SUM(ISNULL(wk.EightWeekAvg, 0)),
		[Store]				=	npd.StoreName,
		[PriceMultiple]		=	npd.PriceMultiple,
		[RetailPrice]		=	npd.PriceIncVAT,
		[PromoStatus]		=	npd.PromotionStatus,
		[Margin]			=	ISNULL	(
										CASE 
											WHEN ISNULL((npd.PriceExcVAT/npd.PriceMultiple), 0) <> 0 THEN 
												ISNULL(((npd.PriceExcVAT/npd.PriceMultiple) - (npd.DeliveredCost / npd.PackSize)) / (npd.PriceExcVAT/npd.PriceMultiple), 0)
										END
										, 0),
		[DeliveredCost]		=	npd.DeliveredCost
	FROM
		Item							(nolock) i
		INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key					= ii.Item_Key
														AND ii.Default_Identifier		= 1
		INNER JOIN	ItemBrand			(nolock) ib		ON	i.Brand_ID					= ib.Brand_ID
		INNER JOIN	SubTeam				(nolock) st		ON  i.SubTeam_No				= st.SubTeam_No
		LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
		LEFT JOIN	#DCRPWeekly			(nolock) wk		ON	i.Item_Key					= wk.Item_Key
		LEFT JOIN	#DCRPOnOrder		(nolock) oo		ON	i.Item_Key					= oo.Item_Key
														AND	oo.PackSize					= wk.PackSize
		LEFT JOIN	#DCRPFuture			(nolock) fu		ON	i.Item_Key					= fu.Item_Key
														AND fu.PackSize					= wk.PackSize
		LEFT JOIN	#DCRPOnHand			(nolock) oh		ON	i.Item_Key					= oh.Item_Key
														AND oh.PackSize					= wk.PackSize
		CROSS APPLY
					(
					SELECT 
						[DeliveredCost]		=	CASE  
													WHEN svch.CaseUpchargePct > 0 THEN
														(ISNULL(
																(
																SELECT TOP 1 
																	AvgCost
																FROM 
																	AvgCostHistory (nolock)
																WHERE 
																	Item_Key			=	svch.Item_Key
																	AND Store_No		=	(SELECT Store_No FROM Store WHERE Distribution_Center = 1)
																	AND SubTeam_No		=	i.SubTeam_No
																	AND Effective_Date	<=	GETDATE()
																ORDER BY 
																	Effective_Date DESC
																),0) * svch.PackSize) * svch.CaseUpchargePct
													ELSE
														(ISNULL(
																(
																SELECT TOP 1 
																	AvgCost
																FROM 
																	AvgCostHistory (nolock)
																WHERE 
																	Item_Key			=	svch.Item_Key
																	AND Store_No		=	(SELECT Store_No FROM Store WHERE Distribution_Center = 1)
																	AND SubTeam_No		=	i.SubTeam_No
																	AND Effective_Date	<=	GETDATE()
																ORDER BY 
																	Effective_Date DESC
																),0) * svch.PackSize) + svch.CaseUpchargeAmt
												END,
						[PromotionStatus]	=	pct.PriceChgTypeDesc,
						[PriceMultiple]			=	CASE 
													WHEN pct.On_Sale = 1 THEN 
														p.Sale_Multiple
													ELSE 
														p.Multiple
												END,
						[PriceExcVAT]		=	CASE 
													WHEN pct.On_Sale = 1 THEN 
														dbo.fn_Price(p.PriceChgTypeId, p.Multiple, p.Price, p.PricingMethod_ID, p.Sale_Multiple, p.Sale_Price) 
													ELSE 
														p.Price
												END,
						[PriceIncVAT]		=
												CASE 
													WHEN pct.On_Sale = 1 THEN 
														dbo.fn_Price(p.PriceChgTypeId, p.Multiple, p.POSPrice, p.PricingMethod_ID, p.Sale_Multiple, p.POSSale_Price) 
													ELSE
														p.POSPrice
												END,
						[StoreName]			=	s.Store_Name,
						[PackSize]			=	svch.PackSize,
						[PackType]			=	svch.PackType,
						[VendorItem]		=	svch.VendorItem
					FROM
						Store						(nolock) s		
						INNER JOIN	Price			(nolock) p		ON	s.Store_No				= p.Store_No 
																	AND i.Item_Key				= p.Item_Key
						INNER JOIN	PriceChgType	(nolock) pct	ON	pct.PriceChgTypeID		= p.PriceChgTypeID
						CROSS APPLY
									(
									SELECT TOP 1 
										[PackSize]			=	vch.Package_Desc1,
										[PackType]			=	iu.Unit_Abbreviation,
										[Item_Key]			=	siv.Item_Key,
										[Store_No]			=	siv.Store_No,
										[VendorItem]		=	iv.Item_ID,
										[CaseUpchargePct]	=	ISNULL(dbo.fn_CaseUpchargePct(i.SubTeam_No), 0),
										[CaseUpchargeAmt]	=	CASE ISNULL(iv.CaseDistHandlingChargeOverride, 0)
																	WHEN 0 THEN
																		ISNULL(v.CaseDistHandlingCharge, 0)
																	ELSE
																		iv.CaseDistHandlingChargeOverride
																END
									FROM 
										VendorCostHistory			(nolock) vch
										INNER JOIN	StoreItemVendor (nolock) siv	ON	siv.StoreItemVendorID	= vch.StoreItemVendorID
																					AND siv.PrimaryVendor		= 1
																					AND siv.DeleteDate			IS NULL
										INNER JOIN	ItemUnit		(nolock) iu		ON	CostUnit_ID				= iu.Unit_ID
										INNER JOIN	ItemVendor		(nolock) iv		ON	siv.Vendor_ID			= iv.Vendor_ID
																					AND	siv.Item_Key			= iv.Item_Key
										INNER JOIN	Vendor			(nolock) v		ON	iv.Vendor_ID			= v.Vendor_ID
									WHERE
										siv.Item_Key		= p.Item_Key
										AND siv.Store_No	= s.Store_No
										AND siv.Vendor_ID	= ISNULL(@Vendor_ID, siv.Vendor_ID)
										AND
										(
											(CONVERT(smalldatetime, GETDATE(), 101)		>=	vch.StartDate) 
											AND 
											(CONVERT(smalldatetime, GETDATE(), 101)		<=	vch.EndDate)
										)
										AND CONVERT(smalldatetime, GETDATE(), 101)		<	ISNULL(siv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
										AND CONVERT(smalldatetime, GETDATE(), 101)		<	ISNULL(iv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
									ORDER BY 
										vch.Promotional DESC, 
										vch.VendorCostHistoryID DESC
									) svch
					WHERE
						s.Store_No IN (SELECT Store_No FROM @tblStoreList)
					) npd
	WHERE
		dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, @Vendor_ID) <>		1
		AND i.Remove_Item		<>		1
		AND i.Deleted_Item		<>		1
		AND i.SubTeam_No		=		ISNULL(@SubTeam_No, i.SubTeam_No)
		AND ii.Identifier		LIKE	ISNULL(@Identifier2, ii.Identifier)
		AND i.EXEDistributed	=		1
	GROUP BY
		ic.Category_Name,
		i.Item_Description, 
		st.SubTeam_Abbreviation,
		ii.Identifier,
		ib.Brand_Name,
		wk.VendorItem, 
		npd.VendorItem,
		wk.PackSize, 
		npd.PackSize,
		wk.PackType, 
		npd.PackType,
		npd.StoreName,
		npd.PriceMultiple,
		npd.PriceIncVAT,
		npd.PromotionStatus,
		npd.PriceExcVAT,
		npd.DeliveredCost

	--**************************************************************************
	--Drop temp tables
	--**************************************************************************
	DROP TABLE #DCRPFuture
	DROP TABLE #DCRPOnHand
	DROP TABLE #DCRPOnOrder
	DROP TABLE #DCRPWeekly
	SET NOCOUNT OFF
	
	--**************************************************************************
	--Remove pants
	--**************************************************************************
	
END