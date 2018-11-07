SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAvgCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetAvgCost]
GO

CREATE PROCEDURE [dbo].[GetAvgCost]
    @Store_No		int,
    @SubTeam_No		int,
    @Category_ID	int,
    @Zone_ID		int,
    @PendOnly		bit
AS 
   -- **************************************************************************
   -- Procedure: GetAvgCost()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 12/10/2008  BBB	Updated SP to be more readable and updated table calls to
   --					reflect the location of data requested by LP. Ensured data
   --					elements being returned were in sync with initial request
   --					and that client was satisfied with data sample.
   -- 01/11/2013   MZ   TFS 8755 - Replace Item.Discontinue_Item with a function call to 
   --                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON   
	--**************************************************************************
	-- Select all values contained within inner_result and outer_result
	--**************************************************************************
	SELECT 
		*
	FROM 
		(
			--**************************************************************************
			-- Select the calculated values into based upon selections within inner_result
			--**************************************************************************
			SELECT	
				*, 
				CASE 
					WHEN dbo.fn_IsCaseItemUnit(CostUnit_ID) = 0 THEN
						CASE
							WHEN Price * Multiple > 0 THEN
								100.00 *((Price / Multiple) - Cost) / (Price / Multiple)
							ELSE
								0.0
						END
					ELSE
						CASE
							WHEN Price * Multiple > 0 THEN
								100.00 *((Price / Multiple) - (Cost / NULLIF([Retail Pack], 0))) / (Price / Multiple)
							ELSE
								0.0
						END
				END	AS Margin  
			FROM 
				(
					--**************************************************************************
					-- Select the static joined values based upon user-def params
					--**************************************************************************
					SELECT
						[SubTeam_Name]		=	CONVERT(varchar(10), st.SubTeam_No) + '-' + st.SubTeam_Name,
						[Category_Name]		=	ic.Category_Name,
						[Item]				=	i.Item_Key,
						[Identifier]		=	ii.Identifier,
						[Item_Description]	=	i.Item_Description,
						[Unit_Abbreviation]	=	vch.Unit_Abbreviation,
						[Origin_Name]		=	ig.Origin_Name,
						[Package_Desc1]		=	vch.Package_Desc1,
						[Package_Desc2]		=	SUBSTRING(CAST(i.Package_Desc2 AS varchar(MAX)),1,LEN(i.Package_Desc2)-1),
						[Zone_Name]			=	z.Zone_Name,
						[Price]				=	p.Price,
						[PendingPrice]		=	pp.PendingPrice,
						[Multiple]			=	p.Multiple,
						[Retail Pack]		=	i.Package_Desc1,
						[CostUnit_ID]		=	vch.CostUnit_ID,
						[Cost]				=	CASE
													WHEN ISNULL(dbo.fn_AvgCostHistory(i.Item_Key, s.Store_No, st.SubTeam_No, GETDATE()), 0) = 0 THEN
														dbo.fn_GetLastCost(i.Item_Key, s.Store_No)
													ELSE
														dbo.fn_AvgCostHistory(i.Item_Key, s.Store_No, st.SubTeam_No, GETDATE())
												END
					FROM
						Item						(nolock) i
						INNER JOIN	SubTeam			(nolock) st		ON	st.SubTeam_No			= i.SubTeam_No
						INNER JOIN	Team			(nolock) t		ON	st.Team_No				= t.Team_No
						LEFT JOIN	ItemBrand		(nolock) ib		ON	i.Brand_ID				= ib.Brand_ID
						INNER JOIN	ItemIdentifier	(nolock) ii		ON	i.Item_Key				= ii.Item_Key 
																	AND ii.Default_Identifier	= 1
						INNER JOIN	ItemUnit		(nolock) iu		ON	iu.Unit_Id				= i.Package_Unit_ID
						INNER JOIN	ItemCategory	(nolock) ic		ON	i.Category_ID			= ic.Category_ID
						LEFT  JOIN	ItemOrigin		(nolock) ig		ON	i.Origin_ID				= ig.Origin_ID
						INNER JOIN	StoreItemVendor	(nolock) siv	ON	i.Item_Key				= siv.Item_Key 
																	AND siv.PrimaryVendor		= 1
																	AND siv.DeleteDate			IS NULL
						INNER JOIN	Store			(nolock) s		ON	s.Store_No				= siv.Store_No 
						INNER JOIN	Vendor			(nolock) v		ON	v.Vendor_ID				= siv.Vendor_ID
						INNER JOIN	Zone			(nolock) z		ON	s.Zone_ID				= z.Zone_ID
						--**************************************************************************
						-- Get the latest Price/Sale Price for the item
						--**************************************************************************
						CROSS APPLY
									(
										SELECT TOP 1 
											[Price]	=	CASE
															WHEN Sale_Start_Date < GETDATE() AND Sale_End_Date > GETDATE () THEN
																Sale_Price
															ELSE
																Price
															END,
											Multiple
										FROM 
											Price	(nolock)
										WHERE
											Item_Key			= i.Item_Key
											AND Store_No		= s.Store_No
									) AS p
						--**************************************************************************
						-- Get the latest Case Pack information for the item
						--**************************************************************************
						CROSS APPLY
									(
										SELECT TOP 1 
											vh.Package_Desc1, 
											vh.CostUnit_ID,
											iu.Unit_Abbreviation
										FROM 
											VendorCostHistory			(nolock) vh
											INNER JOIN	StoreItemVendor (nolock) sv		ON	siv.StoreItemVendorID	= vh.StoreItemVendorID
																						AND siv.PrimaryVendor		= 1
																						AND siv.DeleteDate			IS NULL
											INNER JOIN	ItemUnit		(nolock) iu2	ON	CostUnit_ID				= iu2.Unit_ID
										WHERE
											sv.Vendor_ID									=	v.Vendor_ID 
											AND sv.Item_Key									=	i.Item_Key
											AND sv.Store_No									=	s.Store_No
											AND (
													(CONVERT(smalldatetime, GetDate(), 101)	>=	vh.StartDate) 
													AND 
													(CONVERT(smalldatetime, GetDate(), 101)	<=	vh.EndDate)
												)
											AND CONVERT(smalldatetime, GetDate(), 101)		<	ISNULL(sv.DeleteDate, DATEADD(day, 1, CONVERT(smalldatetime, GetDate(), 101)))
										ORDER BY 
											vh.Promotional			DESC, 
											vh.VendorCostHistoryID	DESC
									) AS vch	
						--**************************************************************************
						-- Get the latest Pending Price information for the item
						--**************************************************************************			
						OUTER APPLY
									(
										SELECT TOP 1 
											[PendingPrice]	=	pbd.Price / pbd.Multiple
										FROM 
											PriceBatchDetail				(nolock) pbd
											LEFT JOIN	PriceBatchHeader	(nolock) pbh	ON pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
										WHERE 
											ISNULL(PriceBatchStatusID, 0)	<	6
											AND pbd.Item_Key				=	i.Item_Key 
											AND pbd.Store_No				=	s.Store_No
											AND (pbd.Price / pbd.Multiple)	<>	p.Price
										ORDER BY 
											pbd.StartDate ASC
									) AS pp		
					WHERE
						i.Deleted_Item		     =	0 
						AND	siv.DiscontinueItem =	0 
						AND p.Price				 >	0
						AND s.Store_No			 =	@Store_No
						AND st.SubTeam_No		 =	ISNULL(@SubTeam_No,		st.SubTeam_No)
						AND z.Zone_ID			 =	ISNULL(@Zone_ID,		z.Zone_ID)
						AND ic.Category_ID		 =	ISNULL(@Category_ID,	ic.Category_ID)
						AND
						(
							(p.Price			<>	ISNULL(pp.PendingPrice, p.Price))
							OR 
							(@PendOnly			=	0)
						)
					) AS inner_result
			WHERE 
				Cost IS NOT NULL
		) AS outer_result

	SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
