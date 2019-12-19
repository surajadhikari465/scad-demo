CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesExt]
	@extAttributes dbo.ItemAttributesExtType READONLY
AS
BEGIN
	DECLARE @today DATETIME = GETDATE();

	SELECT DISTINCT *
	INTO #extAttributes
	FROM @extAttributes;

	CREATE INDEX ix_ItemId ON #extAttributes(ItemId);
	CREATE INDEX ix_AttrCode ON #extAttributes(AttributeCode);

	--Delete missing attributes and attributes with AttributeValue IS NULL
	DELETE ia
	FROM ItemAttributes_Ext ia
	JOIN Attributes a on ia.AttributeID = a.AttributeID
	WHERE ia.ItemID IN (SELECT DISTINCT ItemID FROM #extAttributes)
	  AND  NOT EXISTS
	    (SELECT 1
	     FROM #extAttributes ea
	    WHERE ea.ItemID = ia.ItemID AND (ea.AttributeCode = a.AttributeCode AND ea.AttributeValue Is NOT NULL));

	UPDATE iae
	SET AttributeValue = ea.AttributeValue,
		ModifiedDate = @today
	FROM dbo.ItemAttributes_Ext iae
	JOIN dbo.Attributes a ON iae.AttributeID = a.AttributeID
	JOIN #extAttributes ea ON iae.ItemID = ea.ItemID
		AND a.AttributeCode = ea.AttributeCode
	WHERE ea.AttributeValue IS NOT NULL

	INSERT INTO dbo.ItemAttributes_Ext(ItemID, AttributeID, AttributeValue, AddedDate)
	SELECT 
		ea.ItemID,
		a.AttributeID,
		ea.AttributeValue,
		@today
	FROM #extAttributes ea
	JOIN dbo.Attributes a ON ea.AttributeCode = a.AttributeCode
	WHERE ea.AttributeValue IS NOT NULL
		AND NOT EXISTS 
	(
		SELECT 1
		FROM dbo.ItemAttributes_Ext iae
		JOIN dbo.Attributes a ON iae.AttributeID = a.AttributeID
		WHERE iae.ItemID = ea.ItemID
			AND a.AttributeCode = ea.AttributeCode
	)

	--Verify missing attributes in Mammoth
	DECLARE @codes VARCHAR(8000) = NULL; 

	SELECT @codes = COALESCE(@codes + ', ', '') + 'ItemID: ' + CAST(ea.ItemID AS VARCHAR) + ' <Code: ' + ea.AttributeCode + ' (' + ea.AttributeValue + ')>'
	FROM #extAttributes ea
	LEFT JOIN Attributes a ON ea.AttributeCode = a.AttributeCode
	WHERE a.AttributeCode IS NULL;

	SELECT @codes as WarningMessage;
END
GO

GRANT EXECUTE ON [dbo].[AddOrUpdateItemAttributesExt] TO [MammothRole]
GO