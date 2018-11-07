CREATE PROCEDURE dbo.GetPriceSearch
    @SubTeam_No int = NULL,
    @Category_ID int = NULL,
    @Vendor varchar(50) = NULL,
    @Vendor_ID varchar(20) = NULL,
    @Item_Description varchar(60) = NULL,
    @Identifier varchar(13) = NULL,
    @Discontinue_Item int = NULL,
    @WFM_Item int = NULL,
    @Not_Available int = NULL,
    @HFM_Item tinyint = NULL,
    @IncludeDeletedItems tinyint = NULL,
    @Brand_ID int = NULL,
	@DistSubTeam_No int = NULL,
	@LinkCodeID int = NULL,
    @ProdHierarchyLevel3_ID int = NULL,
    @ProdHierarchyLevel4_ID int = NULL,
    @Chain_ID int = NULL,
    @Vendor_PS varchar(40) = NULL,
	@Store_No int = NULL,
	@CompetitivePriceTypeID int = NULL,
	@ItemSearch tinyint = 0
AS 
--**************************************************************************
-- Procedure: GetPriceSearch
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
--**************************************************************************
BEGIN

DECLARE @SQL varchar(MAX)
DECLARE @Item_Category_Column varchar(50)

IF @Discontinue_Item IS NULL
	SET @Discontinue_Item = 0
IF @WFM_Item IS NULL
	SET @WFM_Item = 1
IF @Not_Available IS NULL
	SET @Not_Available = 0
IF @HFM_Item IS NULL
	SET @HFM_Item = 1
IF @IncludeDeletedItems IS NULL
	SET @IncludeDeletedItems = 0

SELECT
	@Item_Category_Column = CASE field_type WHEN 'Text1' THEN 'Text_1' ELSE NULL END
FROM
	AttributeIdentifier
WHERE
	Screen_Text = 'COMMODITY CODE'


SET @SQL = 'SELECT DISTINCT TOP 10000 ' +
	'I.Item_Key, '

IF @ItemSearch = 0
	SET @SQL = @SQL + 
	'P.Store_No, ' +
	'S.Store_Name, '
ELSE
	SET @SQL = @SQL + 
	'NULL AS Store_No,
	NULL AS Store_Name, '

SET @SQL = @SQL + 
	'ST.SubTeam_Name, ' +
	'IC.Category_Name, ' +
	'IB.Brand_Name, '

IF @Identifier IS NULL
BEGIN
	SET @SQL = @SQL + '(SELECT TOP 1 Identifier FROM ItemIdentifier WHERE ItemIdentifier.Item_Key = I.Item_Key ORDER BY Default_Identifier DESC) AS [Identifier], '
END
ELSE
BEGIN
	SET @SQL = @SQL + 'II.Identifier, '
END

SET @SQL = @SQL + 'I.Item_Description, '
	
-- Item Commodity Code (Category)
IF @Item_Category_Column IS NULL
	SET @SQL = @SQL + 'NULL AS FIACategory, '
ELSE
	SET @SQL = @SQL + 'IA.' + @Item_Category_Column + ' AS FIACategory, '

IF @ItemSearch = 0
	SET @SQL = @SQL + 
	'P.Multiple, ' +
	'P.Price, ' +
	'P.CompetitivePriceTypeID, ' +
	'P.BandwidthPercentageHigh, ' +
	'P.BandwidthPercentageLow '
ELSE
	SET @SQL = @SQL + 
	'NULL AS Multiple,
	NULL AS Price,
	NULL AS CompetitivePriceTypeID,
	NULL AS BandwidthPercentageHigh,
	NULL AS BandwidthPercentageLow '

SET @SQL = @SQL + 
'FROM ' +
	'Item I (nolock) '

IF @ItemSearch = 0
	SET @SQL = @SQL + 
	'INNER JOIN Price P (nolock) ON I.Item_Key = P.Item_Key ' +
	'INNER JOIN Store S (nolock) ON P.Store_No = S.Store_No '

SET @SQL = @SQL + 
	'INNER JOIN SubTeam ST (nolock) ON I.SubTeam_No = ST.SubTeam_No ' +
	'INNER JOIN ItemCategory IC (nolock) ON I.Category_ID = IC.Category_ID ' +
	'INNER JOIN ItemBrand IB (nolock) ON I.Brand_ID = IB.Brand_ID ' +
	'LEFT OUTER JOIN ProdHierarchyLevel4 (nolock) PHL4 ON I.ProdHierarchyLevel4_ID = PHL4.ProdHierarchyLevel4_ID ' +
	'LEFT OUTER JOIN ItemChainItem ICI (nolock) ON I.Item_Key = ICI.Item_Key ' 

IF (@Identifier IS NOT NULL) 
	SET @SQL = @SQL + 'INNER JOIN ItemIdentifier II (nolock) ON I.Item_Key = II.Item_Key '

IF @Item_Category_Column IS NOT NULL
	SET @SQL = @SQL + 'LEFT OUTER JOIN ItemAttribute IA ON I.Item_Key = IA.Item_Key '
	
IF (@Vendor_PS IS NOT NULL OR @Vendor IS NOT NULL OR @Vendor_ID IS NOT NULL)
	SET @SQL = @SQL + 'LEFT OUTER JOIN ItemVendor IV (nolock) ON I.Item_Key = IV.Item_Key ' 

IF @Vendor_PS IS NOT NULL OR @Vendor IS NOT NULL
	SET @SQL = @SQL + 'LEFT OUTER JOIN Vendor V (nolock) ON IV.Vendor_ID = V.Vendor_ID AND (IV.DeleteDate IS NULL OR IV.DeleteDate > GETDATE()) '

IF (@Vendor IS NOT NULL)
	SET @SQL = @SQL + 'WHERE V.CompanyName LIKE ''%' + @Vendor + '%'' '
ELSE
	SET @SQL = @SQL + 'WHERE 1=1 '

IF (@Vendor_PS IS NOT NULL)
	SET @SQL = @SQL + 'AND V.PS_Vendor_ID = ''' + @Vendor_PS + ''' '

IF (@Item_Description IS NOT NULL)
	SET @SQL = @SQL + 'AND I.Item_Description LIKE ''%' + @Item_Description + '%'' '

IF (@SubTeam_No IS NOT NULL)
	SET @SQL = @SQL + 'AND I.SubTeam_No = ' + CONVERT(VARCHAR(20), @SubTeam_No) + ' '

IF (@Category_ID IS NOT NULL)
	SET @SQL = @SQL + 'AND I.Category_ID = ' + CONVERT(VARCHAR(20), @Category_ID) + ' '

IF (@ProdHierarchyLevel3_ID IS NOT NULL)
	SET @SQL = @SQL + 'AND PHL4.ProdHierarchyLevel3_ID = ' + CONVERT(VARCHAR(20), @ProdHierarchyLevel3_ID) + ' '

IF (@ProdHierarchyLevel4_ID IS NOT NULL)
	SET @SQL = @SQL + 'AND I.ProdHierarchyLevel4_ID = ' + CONVERT(VARCHAR(20), @ProdHierarchyLevel4_ID) + ' '

IF (@Identifier IS NOT NULL)
	SET @SQL = @SQL + 'AND II.Identifier LIKE ''' + @Identifier + '%'' '

IF (@Discontinue_Item = 0)
	IF (@ItemSearch = 0)
		SET @SQL = @SQL + 'AND dbo.fn_GetDiscontinueStatus(I.Item_Key, S.Store_No, NULL) = 0 '
	ELSE
		SET @SQL = @SQL + 'AND dbo.fn_GetDiscontinueStatus(I.Item_Key, NULL, NULL) = 0 '

IF (@IncludeDeletedItems = 0)
	SET @SQL = @SQL + 'AND I.Deleted_Item = 0 AND I.Remove_Item = 0 '

IF (@Not_Available = 1)
	SET @SQL = @SQL + 'AND I.Not_Available = 0 '

IF (@WFM_Item  = 1)
	SET @SQL = @SQL + 'AND I.WFM_Item = 1 '

IF (@HFM_Item = 1)
	SET @SQL = @SQL + 'AND I.HFM_Item = 1 '

IF (@Vendor_ID IS NOT NULL)
BEGIN
	IF @Vendor IS NOT NULL
		SET @SQL = @SQL + 'AND I.Item_Key IN (SELECT Item_Key FROM ItemVendor (nolock) WHERE Item_ID = ''' + @Vendor_ID + ''') '
	ELSE
		SET @SQL = @SQL + 'AND IV.Item_ID = ''' + CONVERT(VARCHAR(20), @Vendor_ID) + ''' '
END

IF (@Brand_ID IS NOT NULL)
	SET @SQL = @SQL + 'AND I.Brand_ID = ' + CONVERT(VARCHAR(20), @Brand_ID) + ' '

IF (@DistSubTeam_No IS NOT NULL)
	SET @SQL = @SQL + 'AND I.DistSubTeam_No = ' + CONVERT(VARCHAR(20), @DistSubTeam_No) + ' '

IF (@LinkCodeID IS NOT NULL AND @LinkCodeID > 0)
	SET @SQL = @SQL + 'AND I.Item_Key <> ' + CONVERT(VARCHAR(20), @LinkCodeID) + ' '

IF (@Chain_ID IS NOT NULL)
	SET @SQL = @SQL + 'AND ICI.ItemChainId = ' + CONVERT(VARCHAR(20), @Chain_ID) + ' '

IF @ItemSearch = 0
BEGIN
	IF (@Store_No IS NOT NULL)
		SET @SQL = @SQL + 'AND S.Store_No = ' + CONVERT(VARCHAR(20), @Store_No) + ' '

	IF (@CompetitivePriceTypeID IS NOT NULL)
		SET @SQL = @SQL + 'AND P.CompetitivePriceTypeID = ' + CONVERT(VARCHAR(20), @CompetitivePriceTypeID) + ' '
END

SET @SQL = @SQL + ' ORDER BY I.Item_Description, '

IF @ItemSearch = 0
	SET @SQL = @SQL + 'S.Store_Name, '

SET @SQL = @SQL + 'ST.SubTeam_Name, IC.Category_Name, IB.Brand_Name'

EXEC(@SQL)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceSearch] TO [IRMAReportsRole]
    AS [dbo];

