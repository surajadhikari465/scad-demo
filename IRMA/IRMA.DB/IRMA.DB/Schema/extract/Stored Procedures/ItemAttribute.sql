CREATE PROCEDURE [extract].[ItemAttribute]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @region VARCHAR(2)

	SELECT @region = runmode
	FROM conversion_runmode

	SELECT @region AS Region
		,vsc.InforItemId AS ITEM_ID
		,vsc.ScanCode AS SCAN_CODE
		,vsc.ItemTypeCode AS ITEM_TYPE
		,i.Pos_Description AS POS_DESCRIPTION
		,i.Retail_Sale AS RETAIL_SALE
		,ii.deleted_identifier AS DELETED
		,i.Price_Required AS PRICE_REQUIRED
	FROM ItemIdentifier ii
	INNER JOIN Item i ON i.Item_Key = ii.Item_Key
	INNER JOIN ValidatedScanCode vsc ON ii.Identifier = vsc.Scancode
END
GO
GRANT EXECUTE
    ON OBJECT::[extract].[ItemAttribute] TO [IConInterface]
    AS [dbo];
GO