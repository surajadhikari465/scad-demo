CREATE PROCEDURE [dbo].[Reporting_BOHCompare]
    @SKU varchar(13)
AS
   -- **************************************************************************
   -- Procedure: Reporting_BOHCompare()
   --    Author: Sekhara Kothuri
   --      Date: 10/12/07
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures. It returns the BOH Compare discrepancies between IRMA and EXE.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 12/23/2008  BBB	Culled existing code base and replaced with OnHand functionality
   --					from InventoryValue and WarehouseMovement report. Taking into
   --					account multiple pack sizes
   -- 01/08/2009  BBB	Corrected issue with join on SubTeam, to eliminate duplicate rows
   --					updated SysBOHDifference to return Absolute Value and not actual 
   --					diff, added group by to eliminate multiple rows per sku, corrected
   --					OnHand calculation to match as case against EXE calculation
   -- 01/21/2009  BBB	Added Having clause to eliminate items without variance from the results.
   --					Added in return of cases for both systems to be consumed by RDL column.
   -- 08/13/2009  BBB	Modified call to COH to be a cross apply utilizing subteam/StoreNo/Item
   --					as the joins to alleviate doubling BOH values; query was returning
   --					based upon PackUOM as the delineator as opposed to SubTeamNo like existing
   --					BOH query within IRMA;
   -- 08/27/2009  BBB	Join need to item within cross apply, as it was only being applied when 
   --					an identifier was supplied;
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON

	--**************************************************************************
	--Select OnHand for all Distribution Centers
	--**************************************************************************
	SELECT
		[DC Business Unit]		= s.BusinessUnit_ID,
		[SubTeam]				= coh.SubTeam_No,
		[SKU]					= ii.Identifier,
		[Description]			= i.Item_Description,
		[Pack Size]				= coh.Pack,
		[IRMA BOH Cases]		= SUM(coh.OnHand),
		[WMS BOH Cases]			= SUM(ISNULL(wi.Tot_BOH, 0) / coh.Pack),
		[IRMA BOH Available]	= SUM(coh.OnHand * coh.Pack),
		[WMS BOH Available]		= SUM(ISNULL(wi.Tot_BOH, 0)),
		[Sys BOH Difference]	= CASE
									WHEN SUM(ISNULL(((coh.OnHand * coh.Pack) - wi.Tot_BOH), 0)) < 0 THEN
										SUM(ISNULL(((coh.OnHand * coh.Pack) - wi.Tot_BOH), 0)) * -1
									ELSE
										SUM(ISNULL(((coh.OnHand * coh.Pack) - wi.Tot_BOH), (coh.OnHand * coh.Pack)))
								   END
	FROM
		Item								(nolock) i		
		INNER JOIN		ItemIdentifier		(nolock) ii		ON	ii.Item_Key					= i.Item_Key 
															AND ii.Default_Identifier		= 1
		INNER JOIN		ItemCategory		(nolock) ic		ON	i.Category_ID				= ic.Category_ID
		INNER JOIN		SubTeam				(nolock) st		ON	st.SubTeam_No				= i.SubTeam_No
		INNER JOIN		Store				(nolock) s		ON	s.Store_No					= s.Store_No
															AND s.EXEWarehouse				= 1
		INNER JOIN		StoreItemVendor		(nolock) siv	ON	siv.Store_No				= s.Store_No
															AND siv.PrimaryVendor			= 1
															AND siv.Item_Key				= i.Item_Key
		INNER JOIN		Vendor				(nolock) v		ON  v.Vendor_ID					= siv.Vendor_ID
		INNER JOIN		ItemVendor			(nolock) iv		ON	siv.Vendor_ID				= iv.Vendor_ID
															AND	siv.Item_Key				= iv.Item_Key
		LEFT JOIN		Warehouse_Inventory (nolock) wi		ON	ii.Identifier				= wi.Product_ID
		CROSS APPLY		(
						SELECT 
								ih.Item_Key, 
								 i2.Package_Desc1				AS Pack,
								dbo.fn_GetRetailUnitAbbreviation(ih.Item_Key)		AS PackUOM,
								dbo.fn_GetDistributionUnitAbbreviation(ih.Item_Key)	AS QuantityUOM,
								ISNULL(ih.SubTeam_No, i.SubTeam_No)					AS SubTeam_No,
								SUM(									
											CASE 
												WHEN ISNULL(ih.Quantity, 0) > 0 THEN 
													ih.Quantity / 
																			CASE 
																				WHEN i2.Package_Desc1 <> 0 THEN 
																					i2.Package_Desc1 
																				ELSE 
																					1 
																			END
												ELSE 
													ISNULL(ih.Weight, 0) / 
																			CASE 
																				WHEN i2.Package_Desc1 * i2.Package_Desc2 <> 0 THEN 
																					(i2.Package_Desc1 * i2.Package_Desc2) 
																				ELSE 
																					1 
																			END 
											END
											 * ia.Adjustment_Type)					AS OnHand,
								ia.Adjustment_Type
							FROM 
								OnHand						(nolock) oh
								INNER JOIN ItemHistory		(nolock) ih		ON	ih.Item_Key				= oh.Item_Key 
																			AND ih.Store_No				= oh.Store_No 
																			AND ih.SubTeam_No			= oh.SubTeam_No
								INNER JOIN ItemAdjustment	(nolock) ia		ON	ih.Adjustment_ID		= ia.Adjustment_ID
								INNER JOIN Item				(nolock) i2		ON	i2.Item_Key				= oh.Item_Key
								INNER JOIN ItemIdentifier	(nolock) ii2	ON	ii2.Item_Key			= i2.Item_Key 
																			AND ii2.Default_Identifier	= 1
							WHERE 
								oh.Store_No				=	s.Store_No							
								AND ih.SubTeam_No		=	st.SubTeam_No
								AND ih.DateStamp		>=	ISNULL(oh.LastReset, ih.DateStamp)
								AND ii2.Identifier		=	ISNULL(@Sku, ii2.Identifier)
								AND ii2.Item_Key		=	i.Item_Key
								AND ia.Adjustment_Type	> 0
							GROUP BY 
								ih.Item_Key, 
								 i2.Package_Desc1,
								ih.SubTeam_No,
								ISNULL(ih.SubTeam_No, i2.SubTeam_No),
								ia.Adjustment_Type
							HAVING 
								SUM( 
														CASE 
															WHEN ISNULL(ih.Quantity, 0) > 0 THEN 
																ih.Quantity /	
																						CASE 
																							WHEN i2.Package_Desc1 <> 0 THEN 
																								i2.Package_Desc1 
																							ELSE 
																								1 
																						END
															ELSE 
																ISNULL(ih.Weight, 0) / 
																						CASE 
																							WHEN i2.Package_Desc1 * i2.Package_Desc2 <> 0 THEN 
																								(i2.Package_Desc1 * i2.Package_Desc2) 
																							ELSE 
																								1 
																						END 
														END * ia.Adjustment_Type) <> 0
						) coh
	WHERE
		ii.Identifier = ISNULL(@Sku, ii.Identifier)
	GROUP BY
		s.BusinessUnit_ID,
		coh.SubTeam_No,
		ii.Identifier,
		i.Item_Description,
		coh.Pack
	HAVING
		SUM(coh.OnHand * coh.Pack) <> SUM(wi.Tot_BOH)
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_BOHCompare] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_BOHCompare] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_BOHCompare] TO [IRMAReportsRole]
    AS [dbo];

