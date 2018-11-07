CREATE PROCEDURE dbo.GetItemWebQueryStore
	@Identifier				varchar(13),
    @Item_Description		varchar(60),
    @Team_No				int,
    @SubTeam_No				int,
    @Category_No			int, 
    @Level3_Hierarchy_ID	int,
    @Level4_Hierarchy_ID	int,
    @Vendor_Id				int,
    @ExtraText				varchar(60),
    @Brand_ID				int,
    @Store_No				int

AS

-- ****************************************************************************************************************
-- Procedure: GetItemWebQueryStore()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Called by SLIM to find items associated with a particular store.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-04-26	KM		12001	Add update history template; Remove the DiscontinueItem restriction from the WHERE clause;
-- 2013-09-10   FA		13661	Add transaction isolation level
-- ****************************************************************************************************************

BEGIN
    SET NOCOUNT ON

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

    DECLARE @SQL varchar(5000)

    IF (@Identifier <> '*')
        SELECT @SQL = 'SELECT  DISTINCT TOP 1001
							i.Item_Key,
							p.store_no,
							ii.Identifier,
							ii.Identifier_ID,
							b.brand_name,
							i.item_description,
							i.package_desc1,
							i.package_desc2,
							u.unit_abbreviation,
							s.subteam_abbreviation,
							p.price,
							p.multiple,
							siv.primaryvendor,
							sj.StoreJurisdictionDesc '

    ELSE
        SELECT @SQL = 'SELECT DISTINCT TOP 1001
                            i.Item_Key,
                            (SELECT TOP 1 Identifier FROM ItemIdentifier ii WHERE ii.Item_Key = i.Item_Key ORDER BY Default_Identifier DESC) AS Identifier,
                            (SELECT TOP 1 Identifier_ID FROM ItemIdentifier ii WHERE ii.Item_Key = i.Item_Key ORDER BY Default_Identifier DESC) AS Identifier_ID,
                            p.store_no,
                            b.brand_name,
                            i.item_description,
                            i.package_desc1,
                            i.package_desc2,
                            u.unit_abbreviation,
                            s.subteam_abbreviation,
                            p.price,
                            p.multiple,
                            siv.primaryvendor,
                            sj.StoreJurisdictionDesc '


	SELECT @SQL = @SQL +	' FROM Item i (nolock) '
    SELECT @SQL = @SQL +	' INNER JOIN
									Subteam s (nolock)
									ON i.Subteam_No = s.Subteam_No
								INNER JOIN
									ItemBrand b (nolock)
									ON i.Brand_ID = b.Brand_ID
								INNER JOIN
									ItemIdentifier ii (nolock)
									ON i.Item_Key = ii.Item_Key
								INNER JOIN
									Price p (nolock)
									on i.Item_Key = p.Item_Key
								INNER JOIN
									itemunit u (nolock)
									ON u.unit_id = i.package_unit_id
								INNER JOIN StoreSubTeam sst (nolock)
									ON s.SubTeam_No = sst.SubTeam_No
									AND sst.Store_No = p.Store_No
								INNER JOIN
									StoreItemVendor siv (nolock)
									on i.item_key = siv.item_key and siv.PrimaryVendor = 1 
									AND siv.Store_No = p.Store_No 
								INNER JOIN
									StoreJurisdiction sj
									ON sj.StoreJurisdictionID = i.StoreJurisdictionID '
                                            
    if (@Store_No > 0)     SELECT @SQL = @SQL + ' and siv.Store_no = ' + convert(varchar(10), @Store_No) + ' '              
	if (@ExtraText <> '*') SELECT @SQL = @SQL + ' INNER JOIN ItemScale scale on scale.Item_Key = ii.Item_Key '
	if (@ExtraText <> '*') SELECT @SQL = @SQL + ' INNER JOIN Scale_ExtraText ex on ex.Scale_ExtraText_Id = scale.Scale_ExtraText_Id '

    SELECT @SQL = @SQL + ' where 1=1 '
    SELECT @SQL = @SQL + ' and i.Deleted_Item = 0 and i.Not_Available = 0 and ii.Deleted_identifier = 0 and siv.PrimaryVendor = 1 '

	IF (@Item_Description <> '*') SELECT @SQL = @SQL + ' AND i.Item_Description LIKE ''%' + @Item_Description + '%'' '
    IF (@Brand_ID <> 0) SELECT @SQL = @SQL + ' AND b.Brand_ID = ' + CONVERT(VARCHAR(10), @Brand_ID) + ' '
    IF (@SubTeam_No <> 0) SELECT @SQL = @SQL + ' AND i.SubTeam_No = ' + CONVERT(VARCHAR(10), @SubTeam_No) + ' '
    IF (@Identifier <> '*') SELECT @SQL = @SQL + ' AND Identifier LIKE ''%' + @Identifier + '%'' '
    If (@Team_No <> 0) SELECT @SQL = @SQL + ' AND sst.Team_No = ' + CONVERT(VARCHAR(10), @Team_no) + ' ' 
	If (@Vendor_Id <> 0) SELECT @SQL = @SQL + ' AND ' + CONVERT(VARCHAR(10), @Vendor_Id)  + '  in (select distinct vendor_id from StoreItemVendor  siv (nolock) where siv.Item_Key = ii.item_key) ' 
	If (@Category_No <> 0) SELECT @SQL = @SQL + ' AND i.Category_Id = ' + CONVERT(VARCHAR(10), @Category_No) + ' '  
	if (@ExtraText <> '*') SELECT @SQL = @SQL + ' AND ex.ExtraText like ''%' + @ExtraText +'%'' '

	Execute(@SQL)

	COMMIT TRAN

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemWebQueryStore] TO [IRMASLIMRole]
    AS [dbo];

