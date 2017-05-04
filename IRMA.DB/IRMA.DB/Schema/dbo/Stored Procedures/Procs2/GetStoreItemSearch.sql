CREATE PROCEDURE dbo.GetStoreItemSearch
    @Store_No int,
    @SubTeam_No int,
    @Category_ID int,
    @Vendor varchar(50),
    @Vendor_ID varchar(6),
    @Item_Description varchar(60),
    @Identifier varchar(13),
    @Discontinue_Item int,
    @WFM_Item int,
    @Not_Available int,
    @HFM_Item tinyint
AS 

-- **************************************************************************
-- Procedure: GetStoreItemSearch()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from ItemCatalogLib project within IRMA Client solution
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 01/14/2013  BAS		8755	Update i.Discontinue_Item reference to dbo.fn_GetDiscontinueStatus
--								to account for schema change
-- **************************************************************************

BEGIN
    SET NOCOUNT ON
    
    SELECT @Vendor_ID = RTRIM(@Vendor_ID)

    DECLARE @SQL varchar(2000)

    IF (@Identifier <> '') 
        SELECT @SQL = 'SELECT  DISTINCT TOP 1001 Item.Item_Key, Item_Description, Identifier, Item.SubTeam_No, Price, Multiple '
    ELSE
        SELECT @SQL = 'SELECT DISTINCT TOP 1001 Item.Item_Key, Item_Description, (SELECT TOP 1 Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0 AND Item_Key = Item.Item_Key ORDER BY Default_Identifier DESC) AS Identifier, Item.SubTeam_No, Price, Multiple '
							
    IF @Vendor <> '' 
    BEGIN
        SELECT @SQL = @SQL + 'FROM Item (nolock) INNER JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key AND Price.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' LEFT JOIN StoreItemVendor SIV (nolock) ON Item.Item_Key = SIV.Item_Key AND SIV.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' LEFT JOIN Vendor (nolock) ON SIV.Vendor_ID = Vendor.Vendor_ID '
	IF (@Identifier <> '') SELECT @SQL = @SQL + 'LEFT JOIN (SELECT Item_Key, Identifier, Default_Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0) ON Item_Key = Item.Item_Key '
        SELECT @SQL = @SQL + 'WHERE Vendor.CompanyName LIKE ''%' + @Vendor + '%'' '
    END 
    ELSE
    BEGIN
        SELECT @SQL = @SQL + 'FROM Item (nolock) INNER JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key AND Price.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' '
	IF (@Identifier <> '') SELECT @SQL = @SQL + 'INNER JOIN (SELECT Item_Key, Identifier, Default_Identifier FROM ItemIdentifier WHERE Deleted_Identifier = 0 AND Remove_Identifier = 0) I ON I.Item_Key = Item.Item_Key '
	SELECT @SQL = @SQL + 'WHERE 1=1 ' 
    END

    IF (@Item_Description <> '') SELECT @SQL = @SQL + 'AND Item.Item_Description LIKE ''%' + @Item_Description + '%'' '
    IF (@SubTeam_No <> 0) SELECT @SQL = @SQL + 'AND Item.SubTeam_No = ' + CONVERT(VARCHAR(10), @SubTeam_No) + ' '
    IF (@Category_ID <> 0) SELECT @SQL = @SQL + 'AND Item.Category_ID = ' + CONVERT(VARCHAR(10), @Category_ID) + ' ' 
    IF (@Identifier <> '') SELECT @SQL = @SQL + 'AND Identifier LIKE ''' + @Identifier + '%'' '
    IF (@Discontinue_Item = 0) SELECT @SQL = @SQL + 'AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, ' + CONVERT(VARCHAR(10), @Store_No) + ', NULL) = 0 '
    IF (@Not_Available = 1) SELECT @SQL = @SQL + 'AND Item.Not_Available = 0 '
    IF (@WFM_Item = 1) SELECT @SQL = @SQL + 'AND Item.WFM_Item = 1 '
    IF (@HFM_Item = 1) SELECT @SQL = @SQL + 'AND Item.HFM_Item = 1 '
    IF (@Vendor_ID <> '') SELECT @SQL = @SQL + 'AND Item.Item_Key IN (SELECT Item_Key FROM ItemVendor WHERE Item_ID = ''' + @Vendor_ID + ''') '

    SELECT @SQL = @SQL + 'ORDER BY Item_Description'
    EXECUTE(@SQL)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemSearch] TO [IRMAReportsRole]
    AS [dbo];

