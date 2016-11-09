/*
All pop-data updates for each release go here.

Do not check in separate files to the '.../Scripts/PopulateData/Release/' folder,
just add your updates directly to IconMasterData.sql or IconPopulateData.sql.
*/

go

--Insert Fluid Ounces
IF NOT EXISTS(SELECT * FROM UOM WHERE uomName = 'FLUID OUNCES')
BEGIN
	INSERT INTO UOM(uomID, uomCode, uomName)
	VALUES(19, 'FZ', 'FLUID OUNCES')
END

GO

--Insert Store POS Types
IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'xNEW')
BEGIN
	insert into vim.StorePosType(Name)
	values ('xNEW')
END
IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'IBM')
BEGIN
	insert into vim.StorePosType(Name)
	values ('IBM')
END
IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'RTX')
BEGIN
	insert into vim.StorePosType(Name)
	values ('RTX')
END
IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'ACS')
BEGIN
	insert into vim.StorePosType(Name)
	values ('ACS')
END
IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'BEAN')
BEGIN
	insert into vim.StorePosType(Name)
	values ('BEAN')
END
IF NOT EXISTS(SELECT * FROM vim.StorePosType WHERE Name = 'CCP')
BEGIN
	insert into vim.StorePosType(Name)
	values ('CCP')
END

--Insert VIM Store fields
IF NOT EXISTS(SELECT * FROM Trait WHERE traitDesc = 'IRMA Store ID')
BEGIN
	insert into Trait(traitCode, traitPattern, traitDesc, traitGroupID)
	values('ISI', '^[a-zA-Z0-9_]*$', 'IRMA Store ID', 5)
END
IF NOT EXISTS(SELECT * FROM Trait WHERE traitDesc = 'Store POS Type')
BEGIN
	insert into Trait(traitCode, traitPattern, traitDesc, traitGroupID)
	values ('SPT', '^[a-zA-Z0-9_]*$', 'Store POS Type', 5)
END
IF NOT EXISTS(SELECT * FROM Trait WHERE traitDesc = 'FAX')
BEGIN
	insert into Trait(traitCode, traitPattern, traitDesc, traitGroupID)
	values ('FAX', '^[a-zA-Z0-9_]*$', 'Fax', 5)
END


go

-- TFS_11284_ItemMovement_Data_Surgery

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
	
go

