CREATE PROCEDURE [dbo].[InventoryValueReport]
    @BusUnit	int,
    @TeamNo		int,
    @SubTeamNo	int,
    @categoryID int,
	@Level3		int,
	@Level4		int,
	@Identifier varchar(13)
AS 
      -- **************************************************************************
   -- Procedure: InventoryValueReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 09/19/2008  BBB	Updated SP to be more readable and updated table calls to
   --					reflect the location of data requested by LP and Jon. Ensured
   --					data elements being returned were in sync with initial request
   --					and that client was satisfied with data sample.
   -- 10/03/2008  BBB	Modified call to PackSize inner query to utilize a left join
   --					and returning back CostUnit and CaseQty per PackSize.
   -- 10/07/2008  BBB   Added in calls to ItemAdjustment, CostUpcharge tables, and
   --					optimized query to utilize joins instead of subqueries
   -- 10/08/2008  BBB   Removed last of subqueries and now utilizing all inner joins
   --                   this is based on the fact that in production no OnHand item should
   --                   be missing ItemHistory data
   -- 10/13/2008  BBB	Removed internal calls to ItemHistory and ItemCaseHistory and
   --					instead called code utilized by RS, apparently the lack of a group by
   --					in my internal queries were breaking the numbers
   -- 11/11/2008  BBB	Removed aggregates on ItemCaseHistory and instead pulling totals from
   --					ItemHistory to take into account CatchWeight items
   -- 11/19/2008  BBB	Updated query to leverage two joins to pull OnHand Summary and then Detail
   --					as opposed to doing an aggregate on the Detail
   -- 01/25/2010  RDE   Removed WHERE clause that limited to positive values. (TFS 11691)
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
  -- **************************************************************************
BEGIN    
	--**************************************************************************
	--Select calculated values based upon calls from inner query
	--**************************************************************************
	SELECT 
		[BusUnit]			=	ohq.Store_Name, --ohq.Store_No,
		[Team]				=	(SELECT TOP 1 Team_Name FROM TEAM WHERE TEAM.TEAM_No = ohq.Team_No),--ohq.Team_No,
		[SubTeam]			=	ohq.SubTeam_Name,--SubTeam_No,
		[Item_Key]			=	ohq.Item_Key,
		[Identifier]		=	ohq.Identifier,
		[Item_Description]	=	ohq.Item_Description,
		[Unit_Name]			=	ohq.QuantityUOM,
		[Unit_Abbreviation]	=	ohq.PackUOM,
		[WeightOnHand]		=	ohq.Weight,
		[UnitsOnHand]		=	ohq.Quantity,
		[CaseUpchargeAmt]	=	ohq.CaseUpchargeAmt,
		[CaseUpchargePct]	=	ohq.CaseUpchargePct,
		[PackSize]			=	ohq.PackSize,
		[CaseQty]			=	ohq.CaseQty,
		[AvgCostHistory]	=	ohq.AvgCostHistory,
		[LandedCaseCost]	=	(ohq.AvgCostHistory * ohq.PackSize),
		[ClassName]			=	ohq.ClassName,
		[ExtLandedCost]		=	CASE ohq.Weight
									WHEN 0 THEN
										(ohq.AvgCostHistory * ohq.Quantity)
									ELSE
										CASE ohq.Quantity
											WHEN 0 THEN
												(ohq.AvgCostHistory * ohq.Weight)
											ELSE
												(ohq.AvgCostHistory * ohq.Weight) + (ohq.AvgCostHistory * ohq.Quantity)
										END
								END,
		--**************************************************************************
		--Math is handled differently for Percent or Amount --- CaseUpchargeAmt
		--**************************************************************************	
		[CaseUpchargeValue]	=	CASE  
									WHEN ohq.CaseUpchargePct > 0 THEN
										((ohq.AvgCostHistory * ((100 + ohq.CaseUpchargePct) / 100) * ohq.PackSize) - (ohq.AvgCostHistory * ohq.PackSize))
									ELSE
										ohq.CaseUpchargeAmt
								END,
		--**************************************************************************
		--Math is handled differently for Percent or Amount --- CaseUpchargeAmt
		--**************************************************************************	
		[LoadedCaseCost]	=	CASE  
									WHEN ohq.CaseUpchargePct > 0 THEN
										((ohq.AvgCostHistory * ((100 + ohq.CaseUpchargePct) / 100) * ohq.PackSize))
									ELSE
										((ohq.AvgCostHistory * ohq.PackSize) + ohq.CaseUpchargeAmt)
								END,
		--**************************************************************************
		--Math is handled differently for Percent or Amount --- CaseUpchargeAmt
		--**************************************************************************	
		[ExtLoadedCost]		=	CASE  
									WHEN ohq.CaseUpchargePct > 0 THEN
										CASE ohq.Weight
											WHEN 0 THEN
												((ohq.AvgCostHistory * ((100 + ohq.CaseUpchargePct) / 100) * ohq.PackSize) * ohq.Quantity)
											ELSE
												((ohq.AvgCostHistory * ((100 + ohq.CaseUpchargePct) / 100) * ohq.PackSize) * ohq.Weight)
										END			
									ELSE
										CASE ohq.Weight
											WHEN 0 THEN
												(((ohq.AvgCostHistory * ohq.PackSize) + ohq.CaseUpchargeAmt) * ohq.Weight)
											ELSE
												(((ohq.AvgCostHistory * ohq.PackSize) + ohq.CaseUpchargeAmt) * ohq.Quantity)
										END		
								END
	FROM 
		(
		--**************************************************************************
		--Select OnHand quantities
		--**************************************************************************
		SELECT
			[Store_No]			=	s.Store_No,	
			[Store_Name]		=	s.Store_Name,
			[SubTeam_Name]		=	st.SubTeam_Name,
			[Team_No]			=	st.Team_No,
			[SubTeam_No]		=	st.SubTeam_No,
			[Item_Key]			=	i.Item_Key,	
			[Item_Description]	=	i.Item_Description,
			[Identifier]		=	ii.Identifier,
			[Category_Name]		=	ic.Category_Name,
			[Level3]			=	lv3.Description,
			[Level4]			=	lv4.Description,
			[AvgCostHistory]	=	IsNull(dbo.fn_AvgCostHistory(i.Item_Key, s.Store_No, st.SubTeam_No, GETDATE()), 0),
			[PackSize]			=	coh.PackSize,
			[QuantityUOM]		=	coh.QuantityUOM,
			[PackUOM]			=	coh.PackUOM,
			[ClassName]			=	nic.ClassName,
			[Weight]			=	coh.WeightOnHand,
			[Quantity]			=	coh.UnitsOnHand,
			[CaseQty]			=	coh.CaseQty,
			--**************************************************************************
			--Determine CaseHandlingCharge Percent or Money
			--**************************************************************************	
			[CaseUpchargePct]	=	IsNull(dbo.fn_CaseUpchargePct(st.SubTeam_No), 0),
			[CaseUpchargeAmt]	=	CASE IsNull(iv.CaseDistHandlingChargeOverride, 0)
										WHEN 0 THEN
											IsNull(v.CaseDistHandlingCharge, 0)
									ELSE
										iv.CaseDistHandlingChargeOverride
									END
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
			--**************************************************************************
			--Select Current OnHand Total for the Store and then all CasePacks making up Total
			--**************************************************************************
			INNER JOIN		(
							SELECT
								[Store_No]		=	oh.Store_No,
								[Item_Key]		=	i.Item_Key,
								[SubTeam_No]	=	IsNull(oh.SubTeam_No, i.SubTeam_No),
								[UnitsOnHand]	=	IsNull(Quantity,0),
								[WeightOnHand]	=	IsNull(Weight,0),
								[PackSize]		=	dtl.PackSize, 
								[CaseQty]		=	dtl.OnHand,
								[PackUOM]		=	dtl.PackUOM,
								[QuantityUOM]	=	dtl.QuantityUOM
							FROM 
								Item				(nolock) i
								LEFT JOIN	OnHand	(nolock) oh	ON	oh.Item_Key		= i.Item_Key 
																AND oh.Store_No		= @BusUnit
								INNER JOIN
											(
												SELECT 
													[Store_No]		=	oh.Store_No,
													[SubTeam_No]	=	oh.SubTeam_No,
													[Item_Key]		=	ih.Item_Key,
													[PackUOM]		=	dbo.fn_GetRetailUnitAbbreviation(ih.Item_Key),
													[QuantityUOM]	=	dbo.fn_GetDistributionUnitAbbreviation(ih.Item_Key),
													[PackSize]		=	 i.Package_Desc1,
													[OnHand]		=	SUM(
																			CASE 
																									WHEN IsNull(ih.Quantity, 0) > 0 THEN 
																										ih.Quantity /			CASE 
																																	WHEN i.Package_Desc1 <> 0 THEN 
																																		i.Package_Desc1 
																																	ELSE 
																																		1 
																																END
																									ELSE 
																										IsNull(ih.Weight, 0) /	CASE 
																																	WHEN i.Package_Desc1 * i.Package_Desc2 <> 0 THEN 
																																		(i.Package_Desc1 * i.Package_Desc2) 
																																	ELSE 
																																		1 
																																END 
																								END 
																			* ia.Adjustment_Type)
												FROM 
													OnHand						(nolock) oh
													INNER JOIN	ItemHistory		(nolock) ih		ON	ih.Item_Key				= oh.Item_Key 
																								AND ih.Store_No				= oh.Store_No 
																								AND ih.SubTeam_No			= oh.SubTeam_No
													INNER JOIN	ItemAdjustment	(nolock) ia		ON	ih.Adjustment_ID		= ia.Adjustment_ID
													INNER JOIN	Item			(nolock) i		ON	i.Item_Key				= oh.Item_Key
													INNER JOIN	ItemIdentifier	(nolock) ii		ON	ii.Item_Key				= i.Item_Key 
																								AND ii.Default_Identifier	= 1													
												WHERE 
													oh.Store_No			=	IsNull(@BusUnit, oh.Store_No)
													AND oh.SubTeam_No	=	IsNull(@SubTeamNo, oh.SubTeam_No)
													AND ih.DateStamp	>=	IsNull(oh.LastReset, ih.DateStamp)
													AND i.Deleted_Item	= 0
												GROUP BY 
													ih.Item_Key, 
													i.Package_Desc1,
													oh.Store_No,
													oh.SubTeam_No
													/*
													**************************************************************************
													Remove limitation to show positive and negative values. (TFS 11691)
													**************************************************************************

												HAVING SUM(
															IsNull(ich.Quantity,CASE 
																					WHEN IsNull(ih.Quantity, 0) > 0 THEN 
																						ih.Quantity /			CASE 
																													WHEN i.Package_Desc1 <> 0 THEN 
																														i.Package_Desc1 
																													ELSE 
																														1 
																												END
																					ELSE 
																						IsNull(ih.Weight, 0) /	CASE 
																													WHEN i.Package_Desc1 * i.Package_Desc2 <> 0 THEN 
																														(i.Package_Desc1 * i.Package_Desc2) 
																													ELSE 
																														1 
																												END 
																				END) 
															* ia.Adjustment_Type) > 0 */
											) dtl	ON	dtl.Item_Key	= i.Item_Key
													AND	dtl.Store_No	= oh.Store_No
													AND dtl.SubTeam_No	= oh.SubTeam_No
							WHERE 
								oh.Store_No			= IsNull(@BusUnit, oh.Store_No)
								AND oh.SubTeam_No	= IsNull(@SubTeamNo, oh.SubTeam_No)

							)	AS						 coh	ON	coh.Item_Key				= i.Item_Key
																AND coh.Store_No				= s.Store_No
																AND coh.SubTeam_No				= st.SubTeam_No
			LEFT JOIN	ProdHierarchyLevel4		(nolock) lv4	ON	lv4.ProdHierarchyLevel4_ID	= i.ProdHierarchyLevel4_ID
			LEFT JOIN	ProdHierarchyLevel3		(nolock) lv3	ON	lv3.ProdHierarchyLevel3_ID	= lv4.ProdHierarchyLevel3_ID
			LEFT JOIN	NatItemClass			(nolock) nic	ON	nic.ClassID					= i.ClassID
		WHERE
			s.Store_No						= IsNull(@BusUnit, s.Store_No)
			AND st.SubTeam_No				= IsNull(@SubTeamNo, st.SubTeam_No)
			AND st.Team_No					= IsNull(@TeamNo, st.Team_No)
--			AND i.Category_ID				= IsNull(@CategoryID, i.Category_ID)
--			AND lv3.ProdHierarchyLevel3_ID	= IsNull(@Level3, lv3.ProdHierarchyLevel3_ID)
--			AND i.ProdHierarchyLevel4_ID	= IsNull(@Level4, i.ProdHierarchyLevel4_ID)
			AND ii.Identifier				= IsNull(@Identifier, ii.Identifier)
		) AS ohq
		/*
		**************************************************************************
		Remove limitation to show positive and negative values. (TFS 11691)
		**************************************************************************
	WHERE 
		Quantity	> 0 
		OR Weight	> 0
		*/
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InventoryValueReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InventoryValueReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InventoryValueReport] TO [IRMAReportsRole]
    AS [dbo];

