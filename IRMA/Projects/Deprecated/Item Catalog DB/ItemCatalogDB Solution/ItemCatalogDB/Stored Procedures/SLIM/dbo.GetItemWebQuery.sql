if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemWebQuery]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemWebQuery]
GO

CREATE procedure dbo.GetItemWebQuery

    @Identifier varchar(13),
    @Item_Description varchar(60),
    @Team_No int,
    @SubTeam_No int,
    @Category_No int,
    @Level3_Hierarchy_ID int,
    @Level4_Hierarchy_ID int,
    @Brand_ID int,
    @Vendor_Id int,
    @ExtraText varchar(60),
        @Team_Name varchar(100) = NULL,
        @SubTeam_Name varchar(100) = NULL,
        @Category_Name varchar(35) = NULL,
        @Level3_Name varchar(50) = NULL,
        @Level4_Name varchar(50) = NULL,
        @Brand_Name varchar(25) = NULL,
        @Vendor_Name varchar(50) = NULL,
        @StoreJurisdictionID int
AS
   -- **************************************************************************
   -- Procedure: GetItemWebQuery()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from SLIM by WebQuery/SearchResults.aspx
   --
   -- Modification History:
   -- Date          Init	Comment
   -- 12/02/2009	BBB		Added in SIV.DeleteDate check
   -- 12/28/2012	DF		Removed WHERE clause which skips Discontinued Items
   -- 2013-09-10    FA		Add transaction isolation level
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON

    DECLARE @SQL varchar(2000)

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

    SELECT @SQL = 'SET NOCOUNT ON  SELECT  DISTINCT TOP 1001
                                                        i.Item_Key, '
    IF (@Identifier <> '*')
        SELECT @SQL = @SQL + '(SELECT TOP 1 Identifier FROM ItemIdentifier ii WHERE ii.Item_Key = i.Item_Key AND Identifier LIKE ''%' + @Identifier + '%'' ORDER BY Default_Identifier DESC) AS Identifier, '
    ELSE
        SELECT @SQL = @SQL + '(SELECT TOP 1 Identifier FROM ItemIdentifier ii WHERE ii.Item_Key = i.Item_Key ORDER BY Default_Identifier DESC) AS Identifier, '

        SELECT @SQL = @SQL + 'b.brand_id,
        b.brand_name,
        i.item_description,
        i.package_desc1,
        i.package_desc2,
        u.unit_name,
        s.subteam_name,
                c.category_name,
                sj.StoreJurisdictionDesc,
                sj.StoreJurisdictionID  '

-- ********* From clause and Joins ************

        SELECT @SQL = @SQL + 'FROM Item i (nolock)
                INNER JOIN Subteam s (nolock) ON i.Subteam_No = s.Subteam_No
                INNER JOIN ItemBrand b (nolock) ON i.Brand_ID = b.Brand_ID
                INNER JOIN ItemIdentifier ii (nolock) ON i.Item_Key = ii.Item_Key
                INNER JOIN itemunit u (nolock) ON u.unit_id = i.package_unit_id
                INNER JOIN StoreSubTeam sst (nolock) ON s.SubTeam_No = sst.SubTeam_No
                LEFT JOIN StoreJurisdiction sj (nolock) ON i.StoreJurisdictionID = sj.StoreJurisdictionID
                LEFT JOIN ItemOverride io (nolock) ON i.Item_Key = io.Item_Key
                INNER JOIN ItemCategory c (nolock) ON i.Category_ID = c.Category_ID '

        if (@ExtraText <> '*') SELECT @SQL = @SQL + ' INNER JOIN ItemScale scale on scale.Item_Key = ii.Item_Key '
        if (@ExtraText <> '*') SELECT @SQL = @SQL + ' INNER JOIN Scale_ExtraText ex on ex.Scale_ExtraText_Id = scale.Scale_ExtraText_Id '
        IF @Team_Name IS NOT NULL AND LEN(@Team_Name) > 0 SET @SQL = @SQL + 'INNER JOIN Team T (nolock) ON SST.Team_No = T.Team_No '
        IF (@Level3_Name IS NOT NULL AND LEN(@Level3_Name) > 0) OR (@Level4_Name IS NOT NULL AND LEN(@Level4_Name) > 0) SET @SQL = @SQL + 'INNER JOIN ProdHierarchyLevel3 L3 (nolock) ON C.Category_ID = L3.Category_ID '
        IF @Level4_Name IS NOT NULL AND LEN(@Level4_Name) > 0 SET @SQL = @SQL + 'INNER JOIN ProdHierarchyLevel4 L4 (nolock) ON L3.ProdHierarchyLevel3_ID = L4.ProdHierarchyLevel3_ID '
        IF (@Vendor_Name IS NOT NULL AND LEN(@Vendor_Name) > 0) OR @Vendor_Id <> 0
        BEGIN
                SET @SQL = @SQL + 'INNER JOIN StoreItemVendor SIV (nolock) ON SIV.Item_Key = I.Item_Key AND SIV.Store_No = SST.Store_No AND (SIV.Deletedate IS	NULL OR SIV.Deletedate > GETDATE())'

                IF @Vendor_Name IS NOT NULL AND LEN(@Vendor_Name) > 0
                        SET @SQL = @SQL + ' INNER JOIN Vendor V ON SIV.Vendor_ID = V.Vendor_ID '
        END

-- ****************** Where stuff *********************

        SELECT @SQL = @SQL + 'where i.Deleted_Item = 0 AND i.Not_Available = 0 and ii.Deleted_identifier = 0 '

        IF (@Item_Description <> '*') SELECT @SQL = @SQL + ' AND i.Item_Description LIKE ''%' + @Item_Description + '%'' '
        IF (@Brand_ID <> 0) SELECT @SQL = @SQL + ' AND b.Brand_ID = ' + CONVERT(VARCHAR(10), @Brand_ID) + ' '
        IF (@SubTeam_No <> 0) SELECT @SQL = @SQL + ' AND i.SubTeam_No = ' + CONVERT(VARCHAR(10), @SubTeam_No) + ' '
        IF (@Identifier <> '*') SELECT @SQL = @SQL + ' AND Identifier LIKE ''%' + @Identifier + '%'' '
        If (@Team_No <> 0) SELECT @SQL = @SQL + ' AND sst.Team_No = ' + CONVERT(VARCHAR(10), @Team_no) + ' '
        If (@Vendor_Id <> 0) SELECT @SQL = @SQL + ' AND SIV.Vendor_ID = ' + CONVERT(VARCHAR(10), @Vendor_Id) + ' '
        If (@Category_No <> 0) SELECT @SQL = @SQL + ' AND i.Category_Id = ' + CONVERT(VARCHAR(10), @Category_No) + ' '
        If (@StoreJurisdictionID <> 0) SELECT @SQL = @SQL + ' AND (i.StoreJurisdictionID = ' + CONVERT(VARCHAR(10), @StoreJurisdictionID) + ' OR io.StoreJurisdictionID = ' + CONVERT(VARCHAR(10), @StoreJurisdictionID) + ') '
        if (@ExtraText <> '*') SELECT @SQL = @SQL + ' AND ex.ExtraText like ''%' + @ExtraText +'%'' '

        IF @Team_Name IS NOT NULL AND LEN(@Team_Name) > 0 SET @SQL = @SQL + ' AND T.Team_Name LIKE ''%' + @Team_Name + '%'' '
        IF @SubTeam_Name IS NOT NULL AND LEN(@SubTeam_Name) > 0 SET @SQL = @SQL + ' AND S.SubTeam_Name LIKE ''%' + @SubTeam_Name + '%'' '
        IF @Category_Name IS NOT NULL AND LEN(@Category_Name) > 0 SET @SQL = @SQL + ' AND C.Category_Name LIKE ''%' + @Category_Name + '%'' '
        IF @Level3_Name IS NOT NULL AND LEN(@Level3_Name) > 0 SET @SQL = @SQL + ' AND L3.Description = ''' + @Level3_Name + ''' '
        IF @Level4_Name IS NOT NULL AND LEN(@Level4_Name) > 0 SET @SQL = @SQL + ' AND L4.Description = ''' + @Level4_Name + ''' '
        IF @Brand_Name IS NOT NULL AND LEN(@Brand_Name) > 0 SET @SQL = @SQL + ' AND B.Brand_Name LIKE ''%' + @Brand_Name + '%'' '
        IF @Vendor_Name IS NOT NULL AND LEN(@Vendor_Name) > 0 SET @SQL = @SQL + ' AND V.CompanyName LIKE ''%' + @Vendor_Name + '%'' '
        SELECT @SQL = @SQL + ' SET NOCOUNT OFF'

        EXECUTE(@SQL)

	COMMIT TRAN

    SET NOCOUNT OFF
END

GO
