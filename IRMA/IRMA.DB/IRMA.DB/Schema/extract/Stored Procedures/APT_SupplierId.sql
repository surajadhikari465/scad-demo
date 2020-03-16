CREATE PROCEDURE [extract].[APT_SupplierId]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @region VARCHAR(2)

	SELECT @region = runmode
	FROM conversion_runmode

	SELECT @region AS Region
		,ii.Identifier
		,s.BusinessUnit_ID
		,v.PS_Vendor_ID
		,siv.PrimaryVendor
		,v.ActiveVendor
	FROM ItemIdentifier ii
	INNER JOIN StoreItemVendor siv ON siv.Item_Key = ii.Item_Key
	INNER JOIN Store s ON s.Store_No = siv.Store_No
	INNER JOIN Vendor v ON v.Vendor_ID = siv.Vendor_ID
	WHERE (
			(
				s.WFM_Store = 1
				OR s.Mega_Store = 1
				)
			AND s.Internal = 1
			)
		AND siv.deletedate IS NULL
		AND s.Manufacturer = 0
		AND s.Store_No not in (SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue]('LabAndClosedStoreNo', 'IRMA CLIENT'), '|') ) 

END
GO
GRANT EXECUTE
    ON OBJECT::[extract].[APT_SupplierId] TO [IConInterface]
    AS [dbo];

