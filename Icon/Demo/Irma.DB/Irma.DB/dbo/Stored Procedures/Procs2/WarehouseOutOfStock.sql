CREATE PROCEDURE [dbo].[WarehouseOutOfStock]
    @Store_No int,
    @SubTeam_No int,
    @VendorID int
AS
   -- **************************************************************************
   -- Procedure: WarehouseOutOfStock()
   --    Author: Billy Blackerby
   --      Date: 06.10.09
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date			Init	Comment
   -- 06/15/2009	BBB		Corrected Group By in final query by adding SubTeamName
   --						and removing SubTeamNo
   -- 06/24/2009	BSR		Commented out Pack Size and Pack Desc from final output
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
   -- 01/14/2013	BAS		TFS 8755: Update i.Discontinue_Item to dbo.fn_GetDiscontinueStatus
   --						because of schema change
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON

	--**************************************************************************
	--Select OnHand Totals
	--**************************************************************************
    CREATE TABLE #WHOSOnHand
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			OnHandQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max)
			)

    INSERT INTO #WHOSOnHand
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
									[Pack]			=	i.Package_Desc1,
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
			
	--**************************************************************************
	--Select Future Arrival for the DC based upon Today's date
	--**************************************************************************
    CREATE TABLE #WHOSFuture
			(
			Item_Key	int,
			Identifier	varchar(max),
			SubTeam_No	int, 
			FutureQty	decimal(18,4), 
			PackSize	int,
			PackType	varchar(max)
			)

    INSERT INTO #WHOSFuture
		SELECT 
			oi.Item_Key,
			ii.Identifier,
			oh.Transfer_To_SubTeam,
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
			AND oh.Transfer_To_SubTeam	=	ISNULL(@SubTeam_No, oh.Transfer_To_SubTeam)
			AND siv.Vendor_ID			=	ISNULL(@VendorID, siv.Vendor_ID)
		GROUP BY 
			oi.Item_Key, 
			ii.Identifier,
			oh.Transfer_To_SubTeam, 
			oi.Package_Desc1,
			iu.Unit_Abbreviation

	--**************************************************************************
	--Select #Future, #OnHand values into report output
	--**************************************************************************
	SELECT
		[Item_Key]			=	i.Item_Key,
		[Identifier]		=	REPLICATE('0',12-LEN(RTRIM(ii.Identifier))) + RTRIM(ii.Identifier),
		[SubTeam]			=	st.SubTeam_Name,
		[Item_Description]	=	i.Item_Description, 
		[Tie]				=	i.Tie, 
		[High]				=	i.High,  
		--[PackSize]			=	npd.PackSize,
		--[PackType]			=	npd.PackType,
		[WFM_Item]			=	i.WFM_Item,
		[HFM_Item]			=	i.HFM_Item,
		[Category_Name]		=	ic.Category_Name,
		[Level3]			=	lv3.Description,
		[Level4]			=	lv4.Description,
		[FutureQty]			=	SUM(ISNULL(fu.FutureQty, 0)),
		[OnHandQty]			=	SUM(ISNULL(oh.OnHandQty, 0))
	FROM
		Item							(nolock) i
		INNER JOIN	ItemIdentifier		(nolock) ii		ON	i.Item_Key					= ii.Item_Key
														AND ii.Default_Identifier		= 1
		INNER JOIN	SubTeam				(nolock) st		ON	i.SubTeam_No				= st.SubTeam_No
		INNER JOIN 
					(
					SELECT DISTINCT
						[PackSize]	=	vch.Package_Desc1,
						[PackType]	=	iu.Unit_Abbreviation,
						[Item_Key]	=	siv.Item_Key,
						[Store_No]	=	siv.Store_No
					FROM 
						VendorCostHistory			(nolock) vch
						INNER JOIN	StoreItemVendor (nolock) siv	ON	siv.StoreItemVendorID	= vch.StoreItemVendorID
																	AND	siv.Store_No			= ISNULL(@Store_No,siv.Store_No)
																	AND siv.Vendor_ID			= ISNULL(@VendorID,siv.Vendor_ID)										
																	AND siv.DeleteDate			IS NULL
						INNER JOIN	ItemUnit		(nolock) iu		ON	CostUnit_ID				= iu.Unit_ID
					WHERE
						(
							(CONVERT(smalldatetime, GETDATE(), 101)		>= StartDate) 
							AND 
							(CONVERT(smalldatetime, GETDATE(), 101)		<= EndDate)
						)
						AND CONVERT(smalldatetime, GETDATE(), 101)		< ISNULL(DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GETDATE(), 101)))
					) npd								ON	i.Item_Key					= npd.Item_Key
		LEFT JOIN	ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
		LEFT JOIN	#WHOSFuture			(nolock) fu		ON	i.Item_Key					= fu.Item_Key
														AND npd.PackSize				= fu.PackSize
		LEFT JOIN	#WHOSOnHand			(nolock) oh		ON	i.Item_Key					= oh.Item_Key
														AND npd.PackSize				= oh.PackSize
		LEFT JOIN	ProdHierarchyLevel4	(nolock) lv4	ON	lv4.ProdHierarchyLevel4_ID	= i.ProdHierarchyLevel4_ID
		LEFT JOIN	ProdHierarchyLevel3	(nolock) lv3	ON	lv3.ProdHierarchyLevel3_ID	= lv4.ProdHierarchyLevel3_ID														
	WHERE
		dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, NULL)	<>	1
		AND i.Remove_Item									<>	1
		AND i.Deleted_Item									<>	1
		AND i.SubTeam_No									=	ISNULL(@SubTeam_No, i.SubTeam_No)
		AND i.EXEDistributed								=	1
		AND oh.OnHandQty									IS	NULL
	GROUP BY
		i.Item_Key,
		ii.Identifier,
		st.SubTeam_Name,
		i.Item_Description, 
		i.Tie, 
		i.High,  
		--npd.PackSize,
		--npd.PackType,
		i.WFM_Item,
		i.HFM_Item,
		ic.Category_Name,
		lv3.Description,
		lv4.Description

	--**************************************************************************
	--Drop temp tables
	--**************************************************************************
	DROP TABLE #WHOSFuture
	DROP TABLE #WHOSOnHand

	SET NOCOUNT OFF
END
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WarehouseOutOfStock] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[WarehouseOutOfStock] TO [IRMAReportsRole]
    AS [dbo];

