CREATE Procedure [dbo].[GetWasteCorrections]
    @Store_No int, 
    @SubTeam_No int, 
    @StartDate smalldatetime, 
    @EndDate smalldatetime,
    @WasteType varchar(3)
AS
-- **************************************************************************
-- Procedure: GetWasteCorrections()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from WasteCorrectionsDAO
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 09/22/2010	BBB   	13534	Removed hard-coded value treatment and added
--								call to ItemAdjustment; applied coding standards
-- 03/21/2011   MD      1406    The Quantity and Weight do not need to be multiplied by Adjustment_Type
--								removed the multiplication
-- **************************************************************************

SELECT 
	Identifier, 
	Item_Description, 
	[ItemSubteam]	=	st.SubTeam_Name, 
	Category_Name, 
	Brand_Name, 
	DateStamp, 
	IH.SubTeam_No, 
	[Qty]			=	SUM(Quantity) + SUM(Weight),
	[wType]			=	CASE 
							WHEN iac.Abbreviation IS NULL THEN 
								'SP'
							ELSE 
								iac.Abbreviation 
						END, 
	UserName, 
	CostedByWeight,
	[UnitCost]		=	dbo.fn_GetUnitCostForSpoilage(ih.Item_Key, ih.Store_No, ih.SubTeam_No, ih.DateStamp)
FROM 
	ItemHistory							(nolock) ih
	INNER JOIN	Item					(nolock) i		ON	i.Item_Key						= ih.Item_Key
	INNER JOIN	ItemIdentifier			(nolock) ii		ON	ii.Item_Key						= i.Item_Key 
														AND ii.Default_Identifier			= 1
	INNER JOIN	SubTeam					(nolock) st		ON	st.SubTeam_No					= i.SubTeam_No
	INNER JOIN	Users					(nolock) u		ON	u.User_ID						= ih.CreatedBy
	INNER JOIN	ItemAdjustment			(nolock) ia		ON	ih.Adjustment_ID				= ia.Adjustment_ID
	LEFT JOIN	InventoryAdjustmentCode (nolock) iac	ON	iac.InventoryAdjustmentCode_ID	= ih.InventoryAdjustmentCode_ID
	LEFT JOIN	ItemCategory			(nolock) ic		ON	ic.Category_ID					= i.Category_ID
	LEFT JOIN	ItemBrand				(nolock) ib		ON	ib.Brand_ID						= i.Brand_ID
WHERE 
	ih.Store_No				= @Store_No 
	AND ih.SubTeam_No		= @SubTeam_No
	AND (
		ih.DateStamp		>= @StartDate 
		AND ih.DateStamp	< DATEADD(day, 1, @EndDate)
		)
	AND ih.Adjustment_ID	= 1
	AND (
		@WasteType			= 'ALL' 
		OR iac.Abbreviation = @WasteType
		)
GROUP BY 
	ih.Store_No, 
	Identifier, 
	ih.Item_Key, 
	Item_Description, 
	st.SubTeam_Name, 
	Category_Name, 
	Brand_Name, 
	DateStamp, 
	ih.SubTeam_No, 
	iac.Abbreviation, 
	UserName, 
	CostedByWeight 
HAVING
	SUM(Quantity + Weight) <> 0.0
ORDER BY 
	Identifier, 
	DateStamp DESC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteCorrections] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteCorrections] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWasteCorrections] TO [IRMAReportsRole]
    AS [dbo];

