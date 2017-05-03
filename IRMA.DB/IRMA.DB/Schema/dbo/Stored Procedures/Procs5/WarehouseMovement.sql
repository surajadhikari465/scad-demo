CREATE PROCEDURE [dbo].[WarehouseMovement]
    @Store_No int,
    @SubTeam_No int,
    @VendorID int,
	@Identifier varchar(13)
AS
   -- **************************************************************************
   -- Procedure: WarehouseMovement()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 09/30/2008  BBB	Updated SP to be more readable and updated table calls to
   --					reflect the location of data requested by LP and Jon. Removed
   --					extraneous table lookups and provided efficienies in joins
   --					and conditional parameters. Primarily switched to case/weight data
   --					as was requested in the initial bug fix.
   -- 10/09/2008  BBB	Modified to pull SUM data for 2weeks ago (>= -21 and <= -14)
   --					and for 3weeks ago (>=-28 and <=-21)
   -- 10/31/2008  BBB	Converted all aggregate functions to utilize query elements from 
   --					InventoryValueReport; optimized query and remove extraneous joins
   -- 11/10/2008  BBB	Movement functions should be based upon OrderItem/OrderHeader and
   --					not OnHand data. Modified sub queries to leverage those tables
   -- 11/11/2008  BBB	Added a null capture on PackSize/PackType by calling the 
   --					values for OnHand when missing from the eight week movement subquery
   -- 11/21/2008  BBB	Added SOO subquery which aggregates EXE sent and not closed orders;
   --					Added aggregate of current week movement; returning all items in
   --					the system regardless of OnHand or Movement; added in catch of EXE
   --					items; added in call to VendorCostHistory for all items without
   --					any ordering or onhand entries
   -- 11/22/2008  BBB	Updated 2Week and 3Week movement totals to reflect data for those
   --					7day week periods only and not a cumulative to date
   -- 12/29/2008  BBB	Modified OnOrder query joins to pull orders currently on order from
   --					the DC to a Store. Modified FutureArrival query joins to pull orders
   --					made by the DC to an external vendor. Combined LastWeek, ThisWeek, 
   --					2Week, 3Week, 4Week, and 8Week subqueries into one optimized query.
   -- 01/13/2009  BBB	Removed redundant calls to VCH and other data tables. Corrected issue
   --					in final summation table with joining on item key and not taking into
   --					account the correct packsize.
   -- 01/16/2009  BBB	Corrected issue with vendor parameter not being assessed correctly within
   --					OnOrder subquery
   -- 02/25/2009  BBB	Corrected issue with join on SIV table within FutureArrival query where
   --					the DC was being treated as the vendor and the recipient
   -- 06/03/2009  BBB	Modified final query to use a distinct pull from VCH to join all subqueries
   --					instead of weekly, based upon PackSize and Item_Key, resolving issue with
   --					items with weekly packSize that didn't match other packsize pulls
   -- 06/05/2009  BBB	Removed ExpectedDate from FutureArrival WHERE clause
   -- 06/22/2009  BSR	Added output of vendor ID and Vendor name to final select statement
   -- 06/29/2009  BSR	Changed Criteria from Transfer_To_Subteam to Transfer_Subteam on all accounts
   -- 07/02/2009  BSR	Reversed previous change ONLY in Futures Table calculation
   -- 07/07/2009  BSR	Changed Futures Vendor criteria from siv to oh vendor_id
   -- 07/07/2009  BSR	Changed mvt quantity to sum OrderItem.QuantityReceived instead of OrderItem.QuantityOrdered
   -- 07/13/2009  BSR   Changed the 4wkavg output from SUM(ISNULL(wk.FourWeekAvg, 0)) to ISNULL(wk.FourWeekAvg, 0)
   --					EXEC WarehouseMovement 808,1, NULL, '9948242224'
   -- 08/14/2009  CV	Added siv.PrimaryVendor = 1 to report output query to ensure the ‘most 
   --					recent vendor purchased from’ is the one displayed for each item.    
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
   -- 01/14/2013  BAS	TFS 8755: Update i.Discontinue_Item to dbo.fn_GetDiscontinueStatus
   --					because of schema change
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON

	--**************************************************************************
	--Select Future Arrival for the DC based upon Today's date
	--**************************************************************************
    CREATE TABLE #WHMFuture
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			FutureQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max)
			)

    INSERT INTO #WHMFuture
		SELECT 
			oi.Item_Key,
			ii.Identifier,
			oh.Transfer_To_Subteam,
			SUM(oi.QuantityOrdered),
			oi.Package_Desc1,
			iu.Unit_Abbreviation
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
		WHERE 
			oh.CloseDate				IS	NULL 
			AND	oi.DateReceived			IS	NULL 
			AND oh.Return_Order			=	0 
			AND (
				siv.Deletedate			IS	NULL 
				OR siv.Deletedate		>	GETDATE()
				) 
			AND vs.Store_No				=	ISNULL(@Store_No, vs.Store_No)
			AND oh.Transfer_To_Subteam	=	ISNULL(@SubTeam_No, oh.Transfer_To_Subteam)
			AND oh.Vendor_ID			=	ISNULL(@VendorID, oh.Vendor_ID)
			AND ii.Identifier			=	ISNULL(@Identifier, ii.Identifier)
		GROUP BY 
			oi.Item_Key, 
			ii.Identifier,
			oh.Transfer_To_Subteam, 
			oi.Package_Desc1,
			iu.Unit_Abbreviation

	--**************************************************************************
	--Select Store On Order from the DC totals based upon allocation and order status
	--**************************************************************************
    CREATE TABLE #WHMOnOrder
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			OnOrderQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max)
			)

	INSERT INTO #WHMOnOrder
		SELECT
			[Item_Key]		=	oi.Item_Key,
			[Identifier]	=	ii.Identifier,
			[SubTeam]		=	oh.Transfer_Subteam,
			[Qty]			=	SUM(oi.QUantityOrdered),
			[PackSize]		=	oi.Package_Desc1,
			[PackType]		=	iu.Unit_Abbreviation
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
		WHERE 
			oh.CloseDate				IS	NULL
			AND oh.WareHouseSentDate	IS	NOT NULL
			AND (
				siv.Deletedate			IS	NULL 
				OR siv.Deletedate		>	GETDATE()
				) 
			AND vw.Store_No				=	ISNULL(@Store_No, vw.Store_No)
			AND oh.Transfer_Subteam 	=	ISNULL(@SubTeam_No, oh.Transfer_Subteam)
			AND siv.Vendor_ID			=	ISNULL(@VendorID, siv.Vendor_ID)
			AND ii.Identifier			=	ISNULL(@Identifier, ii.Identifier)
		GROUP BY 
			oi.Item_Key, 
			ii.Identifier,
			oh.Transfer_Subteam, 
			oi.Package_Desc1,
			iu.Unit_Abbreviation

	--**************************************************************************
	--Select OnHand Totals
	--**************************************************************************
    CREATE TABLE #WHMOnHand
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			OnHandQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max)
			)

    INSERT INTO #WHMOnHand
		SELECT
			i.Item_Key,	
			ii.Identifier,
			st.SubTeam_No,
			coh.OnHand,
			coh.Pack,
			coh.PackUOM																				
		FROM
			Item								(nolock) i		
			INNER JOIN		ItemIdentifier		(nolock) ii		ON	ii.Item_Key					= i.Item_Key 
																AND ii.Default_Identifier		= 1
			INNER JOIN		ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
			INNER JOIN		SubTeam				(nolock) st		ON	st.SubTeam_No				= st.SubTeam_No
			INNER JOIN		Store				(nolock) s		ON	s.Store_No					= ISNULL(@Store_No, s.Store_No)
			INNER JOIN		StoreItemVendor		(nolock) siv	ON	siv.Store_No				= s.Store_No
																AND siv.Item_Key				= i.Item_Key
																AND siv.PrimaryVendor			= 1
			INNER JOIN		(
							SELECT 
									[Store_No]		=	s.Store_No,
									[Item_Key]		=	ih.Item_Key, 
									[Pack]			=	 i.Package_Desc1,
									[OnHand]		=	SUM(															
																	CASE 
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
								WHERE 
									oh.Store_No			=	s.Store_No
									AND oh.SubTeam_No	=	ISNULL(ih.SubTeam_No, i.SubTeam_No)
									AND ih.DateStamp	>=	ISNULL(oh.LastReset, ih.DateStamp)
									AND ii.Identifier	=	ISNULL(@Identifier, ii.Identifier)
									AND Deleted_Item = 0
								GROUP BY 
									s.Store_No, 
									ih.Item_Key, 
									 i.Package_Desc1, 
									ISNULL(ih.SubTeam_No, i.SubTeam_No)
								HAVING 
									SUM( 
															CASE 
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
			(
			siv.Deletedate			IS	NULL 
			OR siv.Deletedate		>	GETDATE()
			) 
			AND s.Store_No		= ISNULL(@Store_No, s.Store_No)
			AND st.SubTeam_No	= ISNULL(@SubTeam_No, st.SubTeam_No)
			AND siv.Vendor_ID	= ISNULL(@VendorID, siv.Vendor_ID)
			AND ii.Identifier	= ISNULL(@Identifier, ii.Identifier)

	--**************************************************************************
	--Select This Week, Last Week, 2Week, 3Week movement totals for full fiscal 
	--week based upon today's date, and select 4Week and 8Week Avg
	--**************************************************************************
	DECLARE @LastWeekDate varchar(12)
	SET @LastWeekDate= CONVERT(varchar(12), DATEADD("d", -7, DATEADD("d",-(DATEPART(dw,GETDATE()) -2),GETDATE()))   , 101)

    CREATE TABLE #WHMWeekly
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
			PackType		varchar(max)
			)

	INSERT INTO #WHMWeekly
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
			[PackType]		= PackType
		FROM
			(
				SELECT
					[Item_Key]		=	oi.Item_Key,
					[Identifier]	=	ii.Identifier,
					[SubTeam]		=	oh.Transfer_Subteam,
					[Qty]			=	SUM(oi.QuantityReceived),
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
					[FourWeekAvg]			=	CASE
											WHEN  oh.CloseDate >= DATEADD("d", -21, @LastWeekDate) THEN
												1
											ELSE
												0
										END
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
				WHERE 
					oh.CloseDate				IS	NOT NULL
					AND oh.CloseDate			>=	DATEADD("d", -49, @LastWeekDate)
					AND (
						siv.Deletedate			IS	NULL 
						OR siv.Deletedate		>	GETDATE()
						) 
					AND vw.Store_No				=	ISNULL(@Store_No, vw.Store_No)
					AND oh.Transfer_Subteam	=	ISNULL(@SubTeam_No, oh.Transfer_Subteam)
					AND siv.Vendor_ID			=	ISNULL(@VendorID, siv.Vendor_ID)
					AND ii.Identifier			=	ISNULL(@Identifier, ii.Identifier)
				GROUP BY 
					oi.Item_Key, 
					ii.Identifier,
					oh.Transfer_Subteam, 
					oi.Package_Desc1,
					iu.Unit_Abbreviation,
					oh.CloseDate
			) AS inner_result
		GROUP BY
			Item_Key,
			Identifier,
			SubTeam,
			PackSize,
			PackType

	--**************************************************************************
	--Select #Future, #OnHand, #ThisWeek, #LastWeek, #2Week, #3Week, #4Week, #8Week values into report output
	--**************************************************************************
	SELECT
		[Item_Key]			=	i.Item_Key,
		[VendorID]			=   npd.VendorID,
		[VendorName]		=   npd.VendorName,
		[Identifier]		=	REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier),
		[Item_Description]	=	i.Item_Description, 
		[Tie]				=	i.Tie, 
		[High]				=	i.High,  
		[PackSize]			=	npd.PackSize,
		[PackType]			=	npd.PackType,
		[WFM_Item]			=	i.WFM_Item,
		[HFM_Item]			=	i.HFM_Item,
		[Category_Name]		=	ic.Category_Name,
		[Level3]			=	lv3.Description,
		[Level4]			=	lv4.Description,
		[FutureQty]			=	SUM(ISNULL(fu.FutureQty, 0)),
		[OnHandQty]			=	SUM(ISNULL(oh.OnHandQty, 0)),
		[OnOrderQty]		=	SUM(ISNULL(oo.OnOrderQty, 0)),
		[ThisWeekQty]		=	SUM(ISNULL(wk.ThisWeekQty, 0)),
		[LastWeekQty]		=	SUM(ISNULL(wk.LastWeekQty, 0)),
		[TwoWeekTotal]		=	SUM(ISNULL(wk.TwoWeekTotal,0)),
		[ThreeWeekTotal]	=	SUM(ISNULL(wk.ThreeWeekTotal,0)),
		[FourWeekAvg]		=	ISNULL(wk.FourWeekAvg, 0),
		[EightWeekAvg]		=	ISNULL(wk.EightWeekAvg, 0)
	FROM
		Item							(nolock) i
		INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key					= ii.Item_Key
														AND ii.Default_Identifier		= 1
		INNER JOIN 
					(
					SELECT DISTINCT
						[PackSize]	=	vch.Package_Desc1,
						[PackType]	=	iu.Unit_Abbreviation,
						[Item_Key]	=	siv.Item_Key,
						[Store_No]	=	siv.Store_No,
						[VendorName] = RTRIM(v.CompanyName),
						[VendorID] = siv.Vendor_ID
					FROM 
						VendorCostHistory			(nolock) vch
						INNER JOIN	StoreItemVendor (nolock) siv	ON	siv.StoreItemVendorID	= vch.StoreItemVendorID
																	AND	siv.Store_No			= ISNULL(@Store_No,siv.Store_No)
																	AND siv.Vendor_ID			= ISNULL(@VendorID,siv.Vendor_ID)										
																	AND siv.DeleteDate			IS NULL
						INNER JOIN	ItemUnit		(nolock) iu		ON	CostUnit_ID				= iu.Unit_ID
						INNER JOIN Vendor v ON siv.Vendor_ID = v.Vendor_ID
					WHERE
						(
							(CONVERT(smalldatetime, GETDATE(), 101)		>= StartDate) 
							AND 
							(CONVERT(smalldatetime, GETDATE(), 101)		<= EndDate)
						)
						AND CONVERT(smalldatetime, GETDATE(), 101)		< ISNULL(DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
						AND siv.PrimaryVendor = 1						
					) npd								ON	i.Item_Key					= npd.Item_Key
		LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
		LEFT JOIN	#WHMWeekly			(nolock) wk		ON	i.Item_Key					= wk.Item_Key
														AND	npd.PackSize				= wk.PackSize
		LEFT JOIN	#WHMFuture			(nolock) fu		ON	i.Item_Key					= fu.Item_Key
														AND npd.PackSize				= fu.PackSize
		LEFT JOIN	#WHMOnHand			(nolock) oh		ON	i.Item_Key					= oh.Item_Key
														AND npd.PackSize				= oh.PackSize
		LEFT JOIN	#WHMOnOrder			(nolock) oo		ON	i.Item_Key					= oo.Item_Key
														AND npd.PackSize				= oo.PackSize
		LEFT JOIN	ProdHierarchyLevel4	(nolock) lv4	ON	lv4.ProdHierarchyLevel4_ID	= i.ProdHierarchyLevel4_ID
		LEFT JOIN	ProdHierarchyLevel3	(nolock) lv3	ON	lv3.ProdHierarchyLevel3_ID	= lv4.ProdHierarchyLevel3_ID														
	WHERE
		dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, NULL)		<>	1
		AND i.Remove_Item										<>	1
		AND i.Deleted_Item										<>	1
		AND i.SubTeam_No										=	ISNULL(@SubTeam_No, i.SubTeam_No)
		AND ii.Identifier										=	ISNULL(@Identifier,	ii.Identifier)
		AND i.EXEDistributed									=	1
	GROUP BY
		i.Item_Key,
		npd.VendorID,
		npd.VendorName,
		ii.Identifier,
		i.Item_Description, 
		i.Tie, 
		i.High,  
		npd.PackSize,
		npd.PackType,
		i.WFM_Item,
		i.HFM_Item,
		ic.Category_Name,
		lv3.Description,
		lv4.Description,
		wk.FourWeekAvg,
		wk.EightWeekAvg

	--**************************************************************************
	--Drop temp tables
	--**************************************************************************
	DROP TABLE #WHMFuture
	DROP TABLE #WHMOnHand
	DROP TABLE #WHMOnOrder
	DROP TABLE #WHMWeekly

	SET NOCOUNT OFF
END

SET QUOTED_IDENTIFIER ON


SET QUOTED_IDENTIFIER ON

SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WarehouseMovement] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WarehouseMovement] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WarehouseMovement] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WarehouseMovement] TO [IRMAReportsRole]
    AS [dbo];

