CREATE PROCEDURE [dbo].[GetRandomWeightScalePLUs]
	@Store_No	INT,
	@SubTeam_No	INT,
	@Search		VARCHAR(25) = NULL

AS

-- ==========================================================================================
-- Author:		Hussain Hashim
-- Create date: 8/13/2007
-- Description:	Gets Random Weight Scale PLU's by Store and Sub Team
--		and/or free text search for PLU or Item Description.
--
-- MU	2008-01-22	????	Get unique records specific to Store Jurisdictions.
-- KM	2013-01-22	9394	Check ItemScaleOverride for new 4.8 override values (ForceTare);	
-- ==========================================================================================

BEGIN

DECLARE	@SQLString	VARCHAR(8000)

SET @SQLString = ''
SET @SQLString =  @SQLString + 'select * from '
SET @SQLString =  @SQLString + '(SELECT '     
SET @SQLString =  @SQLString + '	dbo.StoreJurisdiction.StoreJurisdictionDesc '
SET @SQLString =  @SQLString + '	, dbo.Item.Item_Key '
SET @SQLString =  @SQLString + '	, dbo.Item.Item_Description '
SET @SQLString =  @SQLString + '	, dbo.Item.SubTeam_No '
SET @SQLString =  @SQLString + '	, dbo.SubTeam.SubTeam_Name '
SET @SQLString =  @SQLString + '	, dbo.ItemIdentifier.Identifier '
SET @SQLString =  @SQLString + '	, dbo.ItemIdentifier.NumPluDigitsSentToScale '
SET @SQLString =  @SQLString + '	, (CASE WHEN dbo.ItemIdentifier.NumPluDigitsSentToScale = 4 THEN SUBSTRING(dbo.ItemIdentifier.Identifier, 4, 4)  '
SET @SQLString =  @SQLString + '		WHEN dbo.ItemIdentifier.NumPluDigitsSentToScale = 5 THEN SUBSTRING(dbo.ItemIdentifier.Identifier, 3, 5) END)  '
SET @SQLString =  @SQLString + '	 AS PLU '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.Scale_Description1 '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.Scale_Description2 '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.Scale_ScaleUOMUnit_ID '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.Scale_FixedWeight '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.Scale_Tare_ID '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.ShelfLife_Length '
SET @SQLString =  @SQLString + '	, dbo.Scale_ExtraText.ExtraText '
SET @SQLString =  @SQLString + '	, dbo.Scale_ExtraText.Scale_ExtraText_ID '
SET @SQLString =  @SQLString + '	, dbo.StoreItem.Store_No '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.ForceTare '
SET @SQLString =  @SQLString + '	, (CASE WHEN dbo.ItemScale.ForceTare = ''True'' THEN ''Y'' WHEN dbo.ItemScale.ForceTare = ''False'' THEN ''N'' END) AS Force, dbo.ItemUnit.Unit_Name '
SET @SQLString =  @SQLString + '	, dbo.ItemUnit.Unit_Abbreviation '
SET @SQLString =  @SQLString + '	, dbo.Scale_Tare.Description AS Scale_Tare_Description '
SET @SQLString =  @SQLString + '	, dbo.ItemScale.Scale_LabelStyle_ID '
SET @SQLString =  @SQLString + '	, dbo.Scale_LabelStyle.Description AS Scale_LabelStyle_Description '
SET @SQLString =  @SQLString + '	, dbo.Price.POSLinkCode  '
SET @SQLString =  @SQLString + 'FROM          '
SET @SQLString =  @SQLString + '	dbo.Item  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemScale ON dbo.Item.Item_Key = dbo.ItemScale.Item_Key  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemIdentifier ON dbo.Item.Item_Key = dbo.ItemIdentifier.Item_Key  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.SubTeam ON dbo.Item.SubTeam_No = dbo.SubTeam.SubTeam_No  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Scale_ExtraText ON dbo.ItemScale.Scale_ExtraText_ID = dbo.Scale_ExtraText.Scale_ExtraText_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.StoreItem ON dbo.Item.Item_Key = dbo.StoreItem.Item_Key  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemUnit ON dbo.ItemScale.Scale_ScaleUOMUnit_ID = dbo.ItemUnit.Unit_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Scale_Tare ON dbo.ItemScale.Scale_Tare_ID = dbo.Scale_Tare.Scale_Tare_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Scale_LabelStyle ON dbo.ItemScale.Scale_LabelStyle_ID = dbo.Scale_LabelStyle.Scale_LabelStyle_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Price ON dbo.Item.Item_Key = dbo.Price.Item_Key AND dbo.StoreItem.Store_No = dbo.Price.Store_No  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Store ON dbo.StoreItem.Store_No = dbo.Store.Store_No  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.StoreJurisdiction ON dbo.Item.StoreJurisdictionID = dbo.StoreJurisdiction.StoreJurisdictionID and dbo.Store.StoreJurisdictionID = dbo.StoreJurisdiction.StoreJurisdictionID '
SET @SQLString =  @SQLString + 'UNION '
SET @SQLString =  @SQLString + 'SELECT      '
SET @SQLString =  @SQLString + '	dbo.StoreJurisdiction.StoreJurisdictionDesc '
SET @SQLString =  @SQLString + '	, dbo.Item.Item_Key '
SET @SQLString =  @SQLString + '	, dbo.ItemOverride.Item_Description '
SET @SQLString =  @SQLString + '	, dbo.Item.SubTeam_No '
SET @SQLString =  @SQLString + '	, dbo.SubTeam.SubTeam_Name '
SET @SQLString =  @SQLString + '	, dbo.ItemIdentifier.Identifier '
SET @SQLString =  @SQLString + '	, dbo.ItemIdentifier.NumPluDigitsSentToScale '
SET @SQLString =  @SQLString + '	, (CASE WHEN dbo.ItemIdentifier.NumPluDigitsSentToScale = 4 THEN SUBSTRING(dbo.ItemIdentifier.Identifier, 4, 4)  '
SET @SQLString =  @SQLString + '		WHEN dbo.ItemIdentifier.NumPluDigitsSentToScale = 5 THEN SUBSTRING(dbo.ItemIdentifier.Identifier, 3, 5) END)  '
SET @SQLString =  @SQLString + '	 AS PLU '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.Scale_Description1 '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.Scale_Description2 '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.Scale_ScaleUOMUnit_ID '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.Scale_FixedWeight '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.Scale_Tare_ID '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.ShelfLife_Length '
SET @SQLString =  @SQLString + '	, dbo.Scale_ExtraText.ExtraText '
SET @SQLString =  @SQLString + '	, dbo.Scale_ExtraText.Scale_ExtraText_ID '
SET @SQLString =  @SQLString + '	, dbo.StoreItem.Store_No '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.ForceTare '
SET @SQLString =  @SQLString + '	, (CASE WHEN dbo.ItemScaleOverride.ForceTare = ''True'' THEN ''Y'' WHEN dbo.ItemScaleOverride.ForceTare = ''False'' THEN ''N'' END) AS Force, dbo.ItemUnit.Unit_Name '
SET @SQLString =  @SQLString + '	, dbo.ItemUnit.Unit_Abbreviation '
SET @SQLString =  @SQLString + '	, dbo.Scale_Tare.Description AS Scale_Tare_Description '
SET @SQLString =  @SQLString + '	, dbo.ItemScaleOverride.Scale_LabelStyle_ID '
SET @SQLString =  @SQLString + '	, dbo.Scale_LabelStyle.Description AS Scale_LabelStyle_Description '
SET @SQLString =  @SQLString + '	, dbo.Price.POSLinkCode  '
SET @SQLString =  @SQLString + 'FROM       '
SET @SQLString =  @SQLString + '	dbo.Item '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemOverride on dbo.Item.Item_Key = dbo.ItemOverride.Item_Key '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemScale ON dbo.Item.Item_Key = dbo.ItemScale.Item_Key  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemScaleOverride ON dbo.ItemOverride.Item_Key = dbo.ItemScaleOverride.Item_Key  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemIdentifier ON dbo.Item.Item_Key = dbo.ItemIdentifier.Item_Key  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.SubTeam ON dbo.Item.SubTeam_No = dbo.SubTeam.SubTeam_No  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Scale_ExtraText ON dbo.ItemScaleOverride.Scale_ExtraText_ID = dbo.Scale_ExtraText.Scale_ExtraText_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.StoreItem ON dbo.ItemOverride.Item_Key = dbo.StoreItem.Item_Key  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.ItemUnit ON dbo.ItemScaleOverride.Scale_ScaleUOMUnit_ID = dbo.ItemUnit.Unit_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Scale_Tare ON dbo.ItemScaleOverride.Scale_Tare_ID = dbo.Scale_Tare.Scale_Tare_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Scale_LabelStyle ON dbo.ItemScaleOverride.Scale_LabelStyle_ID = dbo.Scale_LabelStyle.Scale_LabelStyle_ID  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Price ON dbo.ItemOverride.Item_Key = dbo.Price.Item_Key AND dbo.StoreItem.Store_No = dbo.Price.Store_No  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.Store ON dbo.StoreItem.Store_No = dbo.Store.Store_No  '
SET @SQLString =  @SQLString + '	INNER JOIN dbo.StoreJurisdiction ON dbo.ItemOverride.StoreJurisdictionID = dbo.StoreJurisdiction.StoreJurisdictionID and dbo.Store.StoreJurisdictionID = dbo.StoreJurisdiction.StoreJurisdictionID '
SET @SQLString =  @SQLString + ') a '
SET @SQLString =  @SQLString + 'WHERE  (a.NumPluDigitsSentToScale = 4 OR a.NumPluDigitsSentToScale = 5) '

IF @Store_No IS NOT NULL
	BEGIN
		SET @SQLString =  @SQLString + ' AND (a.Store_No = ' + CONVERT(VARCHAR, @Store_No) + ') '
	END

IF @SubTeam_No IS NOT NULL
	BEGIN
		SET @SQLString =  @SQLString + 'AND (a.SubTeam_No = ' + CONVERT(VARCHAR, @SubTeam_No) + ') '
	END

IF @Search IS NOT NULL
	SET @SQLString = @SQLString + 'AND (((CASE WHEN a.NumPluDigitsSentToScale = 4 THEN SUBSTRING(a.Identifier, 4, 4) 
						  WHEN a.NumPluDigitsSentToScale = 5 THEN SUBSTRING(a.Identifier, 3, 5) END) LIKE ''%' + @Search + '%'') OR 
							(a.Item_Description LIKE ''%' + @Search + '%'')) '

SET @SQLString = @SQLString + '	ORDER BY a.Item_Description'

EXEC (@SQLString)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRandomWeightScalePLUs] TO [IRMAReportsRole]
    AS [dbo];

