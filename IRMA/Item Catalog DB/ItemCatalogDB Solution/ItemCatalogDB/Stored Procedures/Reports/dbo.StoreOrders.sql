/****** Object:  StoredProcedure [dbo].[StoreOrders]    Script Date: 10/04/2012 16:16:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreOrders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[StoreOrders]
GO


/****** Object:  StoredProcedure [dbo].[StoreOrders]    Script Date: 10/04/2012 16:16:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[StoreOrders]
    @Store_No	int,
    @SubTeam_No int,
    @StartDate	smalldatetime,
	@Identifier varchar(13)
AS
   -- **************************************************************************
   -- Procedure: StoreOrders()
   --    Author: Billy Blackerby
   --      Date: 03.09.2009
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 03/09/2009  BBB	Created SP based upon InventoryWeeklyHistoryPreAllocation
   -- 03/10/2009  BBB	Added in end date parameter to OnOrder query
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
   -- 01/11/2013  BAS	Removed reference to i.Discontinue_Item, and added
   --					siv.DiscontinueItem to siv3 subquery
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Select Store On Order DayOfWeekTotals
	--**************************************************************************
    CREATE TABLE #SOOnOrder
			(
			Item_Key	varchar(max),
			Store		int,
			SubTeam		int,
			Mon			int,
			Tue			int,
			Wed			int,
			Thu			int,
			Fri			int,
			Sat			int,
			Sun			int
			)

    INSERT INTO #SOOnOrder
		SELECT
			[Item_Key]	=	Item_Key,
			[Store]		=	Store,
			[SubTeam]	=	SubTeam,	
			[Mon]		=	SUM(CASE WHEN ExpecDay = 'Monday'		THEN Qty ELSE 0 END),
			[Tue]		=	SUM(CASE WHEN ExpecDay = 'Tuesday'		THEN Qty ELSE 0 END),
			[Wed]		=	SUM(CASE WHEN ExpecDay = 'Wednesday'	THEN Qty ELSE 0 END),
			[Thu]		=	SUM(CASE WHEN ExpecDay = 'Thursday'		THEN Qty ELSE 0 END),
			[Fri]		=	SUM(CASE WHEN ExpecDay = 'Friday'		THEN Qty ELSE 0 END),
			[Sat]		=	SUM(CASE WHEN ExpecDay = 'Saturday'		THEN Qty ELSE 0 END),
			[Sun]		=	SUM(CASE WHEN ExpecDay = 'Sunday'		THEN Qty ELSE 0 END)
		FROM		
			(
				SELECT
					[Item_Key]		=	oi.Item_Key,
					[Store]			=	ss.Store_No,
					[SubTeam]		=	oh.Transfer_To_SubTeam,
					[Qty]			=	SUM(oi.QUantityOrdered),
					[ExpecDate]		=	oh.Expected_Date,
					[ExpecDay]		=	DATENAME(dw, oh.Expected_Date)
				FROM
					OrderHeader						(nolock) oh
					INNER JOIN OrderItem			(nolock) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
					INNER JOIN Vendor				(nolock) vw		ON	oh.Vendor_ID			= vw.Vendor_ID
					INNER JOIN Vendor				(nolock) vs		ON	oh.ReceiveLocation_ID	= vs.Vendor_ID
					INNER JOIN Store				(nolock) sw		ON	vw.Store_No				= sw.Store_No
					INNER JOIN Store				(nolock) ss		ON	vs.Store_No				= ss.Store_No
				WHERE 
					oh.CloseDate				IS	NULL
					AND 
						(
						sw.Distribution_Center	=	1
						OR sw.Manufacturer		=	1
						)
					AND Expected_Date			>=	@StartDate
					AND Expected_Date			<=	DATEADD(d, 6, @StartDate)
					AND oh.Transfer_To_SubTeam	=	ISNULL(@SubTeam_No, oh.Transfer_To_SubTeam)
					AND ss.Store_No				=	ISNULL(@Store_No, ss.Store_No)
				GROUP BY 
					oi.Item_Key, 
					ss.Store_No,
					oh.Transfer_To_SubTeam,
					oh.Expected_Date
			) inner_result
		GROUP BY
			Item_Key,
			Store,
			SubTeam

	--**************************************************************************
	--Select OnHand Totals
	--**************************************************************************
    CREATE TABLE #SOOnHand
			(
			Item_Key	int,
			Store		int,
			SubTeam		int, 
			OnHandQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max)
			)

    INSERT INTO #SOOnHand
		SELECT
			i.Item_Key,
			s.Store_No,	
			st.SubTeam_No,
			coh.OnHand,
			coh.Pack,
			coh.PackUOM																				
		FROM
			Item								(nolock) i		
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

	--**************************************************************************
	--Select #OnHand, #OnOrder values into report output
	--**************************************************************************
	SELECT
		[Store]				=	s.Store_Name,
		[SubTeam]			=	st.SubTeam_Name,
		[Item_Key]			=	i.Item_Key,
		[Identifier]		=	idt.Identifier,
		[Description]		=	i.Item_Description,
		[Brand]				=	ib.Brand_Name,
		[Cost]				=	vch.NetCost,
		[Price]				=	p.POSPrice,
		[OnHandQty]			=	SUM(ISNULL(oh.OnHandQty, 0)),
		[Mon]				=	SUM(oo.Mon),
		[Tue]				=	SUM(oo.Tue),
		[Wed]				=	SUM(oo.Wed),
		[Thu]				=	SUM(oo.Thu),
		[Fri]				=	SUM(oo.Fri),
		[Sat]				=	SUM(oo.Sat),
		[Sun]				=	SUM(oo.Sun)
	FROM
		Item							(nolock) i
		INNER JOIN	ItemBrand			(nolock) ib		ON	i.Brand_ID		= ib.Brand_ID
		INNER JOIN	#SOOnOrder			(nolock) oo		ON	i.Item_Key		= oo.Item_Key
		INNER JOIN	Store				(nolock) s		ON	oo.Store		= s.Store_No
		INNER JOIN  SubTeam				(nolock) st		ON	oo.SubTeam		= st.SubTeam_No
		INNER JOIN	Price				(nolock) p		ON	p.Item_Key		= i.Item_Key
														AND	p.Store_No		= s.Store_No
		LEFT JOIN	#SOOnHand			(nolock) oh		ON	i.Item_Key		= oh.Item_Key
		CROSS APPLY
					(
					--**************************************************************************
					-- Select the latest NetCost for the item
					--**************************************************************************
					SELECT TOP 1 
						[NetCost]	=	(ISNULL(UnitCost, 0) + ISNULL(UnitFreight, 0)) / Package_Desc1
					FROM
						VendorCostHistory			(nolock) vch2
						INNER JOIN StoreItemVendor	(nolock) siv2	ON siv2.StoreItemVendorID	= vch2.StoreItemVendorID
					WHERE 
						vch2.StoreItemVendorId	=
												(		
												SELECT TOP 1 
													siv3.storeitemvendorid
												FROM
													StoreItemVendor (nolock) siv3
												WHERE
													siv3.item_key				= i.Item_Key
													AND siv3.store_no			= p.Store_No
													AND siv3.primaryvendor		= 1
													AND siv3.DiscontinueItem	= 0
												)
						AND StartDate			<= GETDATE()
						AND EndDate				>= GETDATE()
					ORDER BY 
						VendorCostHistoryID DESC
					) vch
		CROSS APPLY 
					(
					--**************************************************************************
					-- Select SKU or Default_ID based upon ItemIdentifier entries
					--**************************************************************************
					SELECT TOP 1 
						[Identifier]
					FROM 
						ItemIdentifier	(nolock) ii
					WHERE
						ii.Item_Key			= i.Item_Key
					ORDER BY 
						Default_Identifier	DESC,
						IdentifierType		DESC						
					) idt
	WHERE
		i.Remove_Item			<>	1
		AND i.Deleted_Item		<>	1
		AND i.SubTeam_No		=	ISNULL(@SubTeam_No, i.SubTeam_No)
		AND idt.Identifier		=	ISNULL(@Identifier,	idt.Identifier)
	GROUP BY
		s.Store_Name,
		st.SubTeam_Name,
		i.Item_Key,
		idt.Identifier,
		i.Item_Description,
		ib.Brand_Name,
		vch.NetCost,
		p.POSPrice

	--**************************************************************************
	--Drop temp tables
	--**************************************************************************
	DROP TABLE #SOOnHand
	DROP TABLE #SOOnOrder
	
	
	SET NOCOUNT OFF
END
GO


