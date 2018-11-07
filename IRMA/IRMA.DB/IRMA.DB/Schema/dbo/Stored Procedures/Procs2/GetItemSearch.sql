CREATE PROCEDURE dbo.GetItemSearch
    @SubTeam_No					int,
    @Category_ID				int,
    @Vendor						varchar(50),
    @Vendor_ID					varchar(20),
    @Item_Description			varchar(60),
    @Identifier					varchar(13),
    @Discontinue_Item			int,
    @WFM_Item					int,
    @Not_Available				int,
    @HFM_Item					tinyint,
    @IncludeDeletedItems		tinyint,
    @Brand_ID					int,
    @DistSubTeam_No				int,
    @LinkCodeID					int,
	@Jurisdictions				varchar(20),
	@JurisdictionSeparator		char,
    
	-- new params placed at the end so they can be optional to not break existing calls that do not pass in a value for them
    @ProdHierarchyLevel3_ID		int				= 0,
    @ProdHierarchyLevel4_ID		int				= 0,
    @Chain_ID					int				= -1,
    @Vendor_PS					varchar(40)		= '',
    @DefaultIdentifiers			bit				= 0,
    @Vendor_Item_Description	varchar(255)	= '',
    @POS_Description			varchar(32)		= ''

AS

--**********************************************************************************************
-- Procedure: GetItemSearch()
--
-- Description:
-- This procedure builds a dynamic query to return information for the ItemSearch screen.
--
-- Change History:
-- Date			Init.	Description
-- 2007/04/19	RM		Add support for ItemChaining
-- 2010/03/23	DS		added > 0 for distsubteam_no so it works for non-DC regions
-- 2010/11/02	MU		allow user to select whether only default identifiers are displayed
-- 06/30/2011	RS		Added Item.Package_Desc1, Item.Package_Desc2, Item.Package_Unit_id UOM
--						to the return results to add to the grid on the form.
--						Cleaned up format for readability.
-- 08/17/2011	FA		Added three columns (Default, Deleted, and Discont) 
-- 08/23/2011	FA		Added two input parameters (Venodor_Item_Description and POS_Description)
-- 09/06/2011	KM		Moved the (iv.DeleteDate IS NULL...) condition from the WHERE clause to the
--						ItemVendor JOIN statement.  This allows the search to return valid items that
--						have no vendor associations.
-- 2012/12/25	KM		Added support for searching by jurisdiction and returning appropriate results from ItemOverride;
-- 2013/01/10	BS		Added temp table for Discontinue information and combined it with jurisdiction info
-- 2013/03/06	KM		Check ItemOverride for alt. UOM; Move the override information from the temp table back into the main query;  Create a new temp
--						table to hold the StoreJurisdictionIds that will be needed for the search;
-- 2013/05/08	KM		Check WFM_Store and Mega_Store values when determining discontinue status;
-- 2013/06/02	KM		Don't filter deleted items from the item search unless the user has chosen that option;
-- 2013/06/03	KM		Check WFM/HFM Item parameters with an OR condition if both parameters are true;
-- 2013/06/19   MZ      Fixed a syntx error in bug 12774.
--***********************************************************************************************

BEGIN

    SET NOCOUNT ON

    DECLARE @SQL varchar(MAX)

	-- Build a temp table for Discontinued information summarized to the item level.
	-- This gets all items from StoreItemVendor for the specific jurisdictions specified.
	SELECT
		siv.Item_Key,
		MIN(CAST (siv.DiscontinueItem AS INTEGER)) DiscontinueItem
	INTO 
		#ItemDisco
	FROM
		StoreItemVendor (nolock) siv
		JOIN Store		(nolock) s		ON siv.Store_No = s.Store_No
	WHERE
		siv.DeleteDate IS NULL
		AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	GROUP BY
		siv.Item_Key;

	CREATE INDEX #id_pk ON #ItemDisco (Item_Key);

	
	-- Build a temp table to represent which StoreJurisdictionIds we are searching.
	SELECT
		Key_Value 
	INTO
		#SearchJurisdictions
	FROM
		dbo.fn_Parse_List(@Jurisdictions, @JurisdictionSeparator)
		

	-- Build main select statement
	SET @SQL = '
		SELECT DISTINCT TOP 1001
			i.Item_Key,
			ISNULL(iov.Item_Description, i.Item_Description)							Item_Description,
			ii.Identifier,
			i.SubTeam_No,
			i.Pre_Order,
			i.EXEDistributed,
			ib.Brand_Name																Brand,
			(SELECT TOP 1 ItemChainID FROM ItemChainItem where Item_Key=i.Item_Key)		Chain_ID,
			ISNULL(iov.Package_Desc1, i.Package_Desc1)									Package_Desc1,
			ISNULL(ROUND(iov.Package_Desc2, 1), ROUND(i.Package_Desc2, 1))				Package_Desc2,
			pu.Unit_Abbreviation,
			ii.Default_Identifier														[Default],
			(CASE
				WHEN i.Deleted_Item = 0 AND i.Remove_Item = 0 THEN 0
				ELSE 1
			END)																		[Deleted],
			ISNULL(id.DiscontinueItem, 0)												[Discont]
		FROM
			Item i (nolock)

			JOIN ItemIdentifier ii (nolock)
				on ( ii.Item_Key = i.Item_Key )

			LEFT OUTER JOIN #ItemDisco id (nolock)
				on ( i.Item_Key = id.Item_Key )

			LEFT OUTER JOIN ItemOverride iov (nolock)
				on ( i.Item_Key = iov.Item_Key ) and ( iov.StoreJurisdictionID in ( SELECT Key_Value FROM dbo.fn_Parse_List(''' + @Jurisdictions + ''', ''' + @JurisdictionSeparator + ''') ) )

			LEFT JOIN ItemUnit pu (nolock)
				on ( pu.Unit_id = ISNULL( iov.Package_Unit_ID, i.Package_Unit_ID ) )

			LEFT JOIN ItemBrand ib (nolock)
				on ( ib.Brand_ID = ISNULL(iov.Brand_ID, i.Brand_ID ) )

			LEFT OUTER JOIN ItemVendor iv (nolock)
				on ( iv.Item_Key = i.Item_Key
					and ( iv.DeleteDate IS NULL OR iv.DeleteDate > GETDATE() ) )

			LEFT OUTER JOIN Vendor v (nolock)
				on ( v.Vendor_Id = iv.Vendor_Id )

			LEFT OUTER JOIN ProdHierarchyLevel4 l4 (nolock)
				on ( l4.ProdHierarchyLevel4_ID = i.ProdHierarchyLevel4_ID )
		WHERE
			((i.StoreJurisdictionID in (SELECT Key_Value FROM #SearchJurisdictions)) or (iov.StoreJurisdictionID in (SELECT Key_Value FROM #SearchJurisdictions)))
			';

	-- Add optional conditions if values are supplied
	IF (@SubTeam_No <> 0)					SET @SQL = @SQL + 'AND i.SubTeam_No = ' + CONVERT(VARCHAR(10), @SubTeam_No) + ' ' + CHAR(13);
	IF (@Category_ID <> 0)					SET @SQL = @SQL + 'AND i.Category_ID = ' + CONVERT(VARCHAR(10), @Category_ID) + ' ' + CHAR(13);
	IF (@Vendor <> '')						SET @SQL = @SQL + 'AND v.CompanyName LIKE ''%' + RTRIM(@Vendor) + '%'' ' + CHAR(13);
	IF (RTRIM(@Vendor_ID) <> '')			SET @SQL = @SQL + 'AND iv.Item_ID LIKE ''%' + RTRIM(@Vendor_ID) + '%'' ' + CHAR(13);
	IF (@Item_Description <> '')			SET @SQL = @SQL + 'AND (i.Item_Description LIKE ''%' + @Item_Description + '%'' OR iov.Item_Description LIKE ''%' + @Item_Description + '%'') ' + CHAR(13);
	IF (@POS_Description <> '')				SET @SQL = @SQL + 'AND (i.POS_Description LIKE ''%' + @POS_Description + '%'' OR iov.POS_Description LIKE ''%' + @POS_Description + '%'') ' + CHAR(13);
	IF (@Vendor_Item_Description <> '')		SET @SQL = @SQL + 'AND iv.VendorItemDescription LIKE ''%' + @Vendor_Item_Description + '%'' OR i.Item_Description LIKE ''%' + @Vendor_Item_Description + '%'' OR iov.Item_Description LIKE ''%' + @Vendor_Item_Description + '%'' ' + CHAR(13);
	IF (@Identifier <> '')					SET @SQL = @SQL + 'AND ii.Identifier LIKE ''%' + RTRIM(@Identifier) + '%'' ' + CHAR(13);
	IF (@Discontinue_Item = 0)				SET @SQL = @SQL + 'AND id.DiscontinueItem = 0 ' + CHAR(13);
	
	-- If both WFM and HFM checkboxes are checked (like in a quicksearch) return a result if either of the conditions is true.  Otherwise, check the parameters individually.
	IF (@WFM_Item = 1 AND @HFM_Item = 1)	SET @SQL = @SQL + 'AND (i.WFM_Item = 1 OR i.HFM_Item = 1) ' + CHAR(13);
		ELSE
			BEGIN
				IF (@HFM_Item = 1)				SET @SQL = @SQL + 'AND i.HFM_Item = 1 ' + CHAR(13);
				IF (@WFM_Item = 1)				SET @SQL = @SQL + 'AND i.WFM_Item = 1 ' + CHAR(13);
			END
	IF (@Not_Available = 1)					SET @SQL = @SQL + 'AND ISNULL(iov.Not_Available, i.Not_Available) = 0 ' + CHAR(13);
	IF (@IncludeDeletedItems = 0)			SET @SQL = @SQL + 'AND i.Deleted_Item = 0 AND i.Remove_Item = 0 ' + CHAR(13);
	IF (@Brand_ID IS NOT NULL)				SET @SQL = @SQL + 'AND ISNULL(iov.Brand_ID, i.Brand_ID) = ' + CONVERT(VARCHAR(20), @Brand_ID) + ' ' + CHAR(13);
	IF (@DistSubTeam_No > 0)				SET @SQL = @SQL + 'AND i.DistSubTeam_No = ' + CONVERT(VARCHAR(20), @DistSubTeam_No) + ' ' + CHAR(13);
	IF (@LinkCodeID > 0)					SET @SQL = @SQL + 'AND i.Item_Key <> ' + CONVERT(VARCHAR(20), @LinkCodeID) + ' ' + CHAR(13);
	IF (@ProdHierarchyLevel3_ID <> 0)		SET @SQL = @SQL + 'AND l4.ProdHierarchyLevel3_ID = ' + CONVERT(VARCHAR(10), @ProdHierarchyLevel3_ID) + ' ' + CHAR(13);
	IF (@ProdHierarchyLevel4_ID <> 0)		SET @SQL = @SQL + 'AND i.ProdHierarchyLevel4_ID = ' + CONVERT(VARCHAR(10), @ProdHierarchyLevel4_ID) + ' ' + CHAR(13);
	IF (@Chain_ID > 0)						SET @SQL = @SQL + 'AND Item.Item_Key in (SELECT Item_Key FROM ItemChainItem where ItemChainID=' + CONVERT(VARCHAR(20), @Chain_ID) + ') ' + CHAR(13);
	IF (@Vendor_PS <> '')					SET @SQL = @SQL + 'AND v.PS_Vendor_ID LIKE ''%' + RTRIM(@Vendor_PS) + '%'' ' + CHAR(13);
	IF (@DefaultIdentifiers = 1)			SET @SQL = @SQL + 'AND ii.Default_Identifier = 1' + CHAR(13);

	SET @SQL = @SQL + 'ORDER BY Item_Description'  + CHAR(13);

	--PRINT(@SQL)
	EXECUTE(@SQL);

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemSearch] TO [IRMAReportsRole]
    AS [dbo];

