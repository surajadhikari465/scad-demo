﻿CREATE PROCEDURE [dbo].[AddOrUpdateItemAttributesExt]
	@extAttributes dbo.ItemAttributesExtType READONLY
AS
BEGIN
	DECLARE @today DATETIME = GETDATE();

	SELECT *
	INTO #extAttributes
	FROM @extAttributes

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

	DELETE ext
	FROM dbo.ItemAttributes_Ext ext
	WHERE EXISTS 
	(
		SELECT 1
		FROM #extAttributes ea
		JOIN Attributes a ON ea.AttributeCode = a.AttributeCode
		WHERE ea.AttributeValue IS NULL
			AND ea.ItemID = ext.ItemID
			AND a.AttributeID = ext.AttributeID
	)
END
GO

GRANT EXECUTE ON [dbo].[AddOrUpdateItemAttributesExt] TO [MammothRole]
GO