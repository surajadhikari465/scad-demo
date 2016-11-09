BEGIN TRY
	BEGIN TRANSACTION

	; -- Need this for SQL Compatibility Level 90 because MERGE is NOT recognized as a reserved word at this level, resulting in a parsing error at compat level lower than 100.
	MERGE app.ItemMovement AS im
		USING 
		ScanCode sc
		ON CONVERT(VARCHAR(13),im.Identifier) = CONVERT(VARCHAR(13),sc.ItemId)
		
		WHEN MATCHED THEN 
			UPDATE SET im.Identifier = sc.ScanCode;

	INSERT INTO app.ItemMovementErrorQueue (
		ESBMessageID,
		BusinessUnitID,
		RegisterNumber,
		TransactionSequenceNumber,
		LineItemNumber,
		Identifier,
		TransDate,
		Quantity,
		ItemVoid,
		ItemType,
		BasePrice,
		[Weight],
		MarkDownAmount,
		InsertDate,
		InProcessBy,
		ProcessFailedDate)
	SELECT 
		ESBMessageID,
		BusinessUnitID,
		RegisterNumber,
		TransactionSequenceNumber,
		LineItemNumber,
		Identifier,
		TransDate,
		Quantity,
		ItemVoid,
		ItemType,
		BasePrice,
		[Weight],
		MarkDownAmount,
		InsertDate,
		InProcessBy,
		ProcessFailedDate
	FROM app.ItemMovement im
	WHERE im.Identifier NOT IN (SELECT CONVERT(VARCHAR(13),ItemId) FROM ScanCode);

	DELETE FROM app.ItemMovement WHERE Identifier NOT IN (SELECT CONVERT(VARCHAR(13),ItemId) FROM ScanCode)

	COMMIT TRANSACTION

	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
	
