CREATE PROCEDURE dbo.BERTReport
	@ItemIdentifierList varchar(8000),
	@StoreNumberList varchar(8000)
AS

DECLARE @LineDriveId int

SELECT @LineDriveId = PriceChgTypeId
FROM	PriceChgType
WHERE	PriceChgTypeDesc = 'LIN'

SELECT DISTINCT
	II.Identifier,
	I.Item_Description As ItemDescription,
	SIV.Store_No,
	S.Store_Name,
	CASE WHEN LEN(I.Item_Description) > 30 THEN 'Description cannot be longer than 30 characters.' ELSE '' END As DescCheck,
	CASE WHEN LEN(I.POS_Description) > 18 THEN 'POS Description cannot be longer than 18 characters.' ELSE '' END As POSDescCheck,
	CASE WHEN I.SubTeam_No IS NULL OR I.SubTeam_No = 0 THEN 'SbuTeam cannot be 0 or NULL.' ELSE '' END As SubTeamCheck,
	CASE WHEN I.Brand_ID IS NULL OR I.Brand_ID = 0 THEN 'Item Brand cannot be 0 or NULL.' ELSE '' END As ItemBrandCheck,
	CASE WHEN LEN(IB.Brand_Name) > 18 THEN 'Brand Name cannot be longer than 18 characters.' ELSE '' END As BrandNameCheck,
	CASE WHEN I.Category_ID IS NULL OR I.Category_ID = 0 THEN 'Category cannot be 0 or NULL.' ELSE '' END As CategoryCheck,
	CASE WHEN I.ClassID IS NULL OR I.ClassID = 0 THEN 'Class cannot be 0 or NULL.' ELSE '' END As ClassCheck,
	CASE WHEN I.Package_Desc1 IS NULL OR I.Package_Desc1 = 0 THEN 'Package cannot be 0 or NULL.' ELSE '' END As PackageCheck,
	CASE WHEN I.LabelType_ID IS NULL OR I.LabelType_ID = 0 THEN 'Label Type cannot be 0 or NULL.' ELSE '' END As LabelTypeCheck,
	CASE WHEN VCH.UnitCost IS NULL OR VCH.UnitCost < .01 THEN 'Cost cannot be less than 0 or NULL.' ELSE '' END As CostCheck,
	CASE WHEN (P.MSRPPrice IS NULL OR P.MSRPPrice < .01) AND (P.PriceChgTypeID = @LineDriveId) THEN 'MSRP cannot be less than 0 or NULL.' ELSE '' END As MSRPCheck,
	dbo.fn_ValidateDescriptionCharacters(I.Item_Description,'I') As ItemDescCharCheck,
	dbo.fn_ValidateDescriptionCharacters(I.POS_Description,'P') As POSDescCharCheck,
	dbo.fn_ValidateDescriptionCharacters(I.Sign_Description,'S') As SignDescCharCheck,
--	CASE WHEN I.SubTeam_No IN (180,181,220,221) AND (II.Identifier < 20000000000 OR II.Identifier > 29999900000) AND dbo.fn_GetTaxFlagAsInt(TF.TaxFlagKey) = 1
--		THEN 'Tax Flag 1 must be ''N''.' ELSE '' END As TaxFlagCheck1,
--	CASE WHEN I.SubTeam_No IN (190,191,210,211,225,227,228,230) AND (II.Identifier < 20000000000 OR II.Identifier > 29999900000) AND dbo.fn_GetTaxFlagAsInt(TF.TaxFlagKey) = 0
--		THEN 'Tax Flag 1 must be ''Y''.' ELSE '' END As TaxFlagCheck2,
	'' As TaxFlagCheck1,
	'' As TaxFlagCheck2,
	CASE WHEN dbo.fn_GetMargin(P.Price,P.Multiple,(VCH.UnitCost/VCH.Package_Desc1)) > 99.9 THEN 'Target Gross Margin cannot be greater than 99.9.' ELSE '' END As TargetMarginPosCheck,
	CASE WHEN dbo.fn_GetMargin(P.Price,P.Multiple,(VCH.UnitCost/VCH.Package_Desc1)) < 0 THEN 'Target Gross Margin cannot be less than 0.' ELSE '' END As TargetMarginNegCheck
FROM 
	ItemIdentifier II 
	JOIN 
		dbo.fn_ParseStringList(@ItemIdentifierList, '|') IIL
		ON IIL.Key_Value = II.Identifier			
	JOIN
		Item I (nolock)
		ON I.Item_Key = II.Item_Key
	LEFT JOIN
		ItemBrand IB (nolock)
		ON IB.Brand_ID = I.Brand_ID
	LEFT JOIN 
		StoreItemVendor SIV (nolock)
		ON SIV.Item_Key = I.Item_Key
	JOIN 
		dbo.fn_ParseStringList(@StoreNumberList, '|') SN
		ON SN.Key_Value = SIV.Store_No			
	JOIN
		Store S (nolock)
		ON S.Store_No = SIV.Store_No		
	LEFT JOIN
		VendorCostHistory VCH (nolock)
		ON VCH.StoreItemVendorID = SIV.StoreItemVendorID
	LEFT JOIN 
		TaxFlag TF (nolock)
		ON TF.TaxClassID = I.TaxClassID
	LEFT JOIN 
		Price P (nolock)
		ON P.Item_Key = I.Item_Key
WHERE 
	II.Default_Identifier = 1 AND
	P.Store_No = SIV.Store_No AND
	SIV.PrimaryVendor = 1
ORDER BY 
	II.Identifier
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BERTReport] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BERTReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BERTReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BERTReport] TO [IRMAReportsRole]
    AS [dbo];

