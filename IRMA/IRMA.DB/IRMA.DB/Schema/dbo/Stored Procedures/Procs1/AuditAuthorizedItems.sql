CREATE PROCEDURE dbo.AuditAuthorizedItems @action VARCHAR(25)
	,@region VARCHAR(2)
	,@groupSize INT = 250000
	,@groupId INT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	IF IsNull(@groupSize, 0) <= 0
		SET @groupSize = 250000;

	IF @action = 'Initilize'
	BEGIN
		SELECT Count(*) [RowCount]
		FROM Item i
		INNER JOIN ItemIdentifier ii ON ii.Item_Key = i.Item_Key
		INNER JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
		INNER JOIN StoreItem si ON si.Item_Key = i.Item_Key
		INNER JOIN Store s ON s.Store_No = si.Store_No
		WHERE i.Deleted_Item = 0
			AND i.Remove_Item = 0
			AND ii.Deleted_Identifier = 0
			AND ii.Remove_Identifier = 0
			AND s.WFM_Store = 1;

		RETURN;
	END

	IF @action = 'Get'
	BEGIN
		DECLARE @minId INT = (@groupId * @groupSize) + (
				CASE 
					WHEN @groupID = 0
						THEN 0
					ELSE 1
					END
				);

		CREATE TABLE #group (Item_Key INT);

		CREATE INDEX ix_ItemId ON #group (Item_Key);

		WITH cte
		AS (
			SELECT Item_Key
				,Row_Number() OVER (
					ORDER BY Item_Key
					) rowID
			FROM dbo.Item
			WHERE Deleted_Item = 0
				AND Remove_Item = 0
			)
		INSERT INTO #group (Item_Key)
		SELECT TOP (@groupSize) Item_Key
		FROM cte
		WHERE rowID >= @minId
		ORDER BY Item_Key;

		SELECT vsc.inforItemId AS ITEM_ID
			,ii.Identifier AS SCAN_CODE
			,s.BusinessUnit_ID AS STORE_ID
			,si.Authorized AS AUTHORIZED
		FROM Item i
		JOIN #group g ON g.Item_Key = i.Item_Key
		JOIN ItemIdentifier ii ON ii.Item_Key = i.Item_Key
		JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
		JOIN StoreItem si ON si.Item_Key = i.Item_Key
		JOIN Store s ON s.Store_No = si.Store_No
		WHERE i.Deleted_Item = 0
			AND i.Remove_Item = 0
			AND ii.Deleted_Identifier = 0
			AND ii.Remove_Identifier = 0
			AND s.WFM_Store = 1
		ORDER BY vsc.inforItemId;

		IF (object_id('tempdb..#group') IS NOT NULL)
			DROP TABLE #group;
	END
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[AuditAuthorizedItems] TO [IRMAAdminRole] AS [dbo];
GO

GRANT EXECUTE ON OBJECT::[dbo].[AuditAuthorizedItems] TO [IRMAClientRole] AS [dbo];
GO