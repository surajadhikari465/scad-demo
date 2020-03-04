CREATE PROCEDURE [extract].[APT_ItemStore]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @region VARCHAR(2)

	SELECT @region = runmode
	FROM conversion_runmode

	SELECT @region AS Region
		,ii.Identifier
		,s.BusinessUnit_ID
		,si.Authorized
		,siv.DiscontinueItem
		,i.Deleted_Item 
	FROM Item i
	INNER JOIN ItemIdentifier ii ON ii.Item_Key = i.Item_Key
	INNER JOIN StoreItem si ON si.Item_Key = i.Item_Key
	INNER JOIN Store s ON s.Store_No = si.Store_No
	INNER JOIN StoreItemVendor siv ON siv.Item_Key = i.Item_Key
		AND siv.Store_No = si.Store_No
	WHERE ii.Remove_Identifier = 0 and ii.Deleted_Identifier = 0
		AND (
			s.WFM_Store = 1
			OR s.Mega_Store = 1
			)
		AND s.Internal = 1
		AND siv.PrimaryVendor = 1
		AND s.Manufacturer = 0
		AND s.Store_No not in (SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('LabAndClosedStoreNo', 'IRMA CLIENT'), '|') ) 

END
GO
GRANT EXECUTE
    ON OBJECT::[extract].[APT_ItemStore] TO [IConInterface]
    AS [dbo];
