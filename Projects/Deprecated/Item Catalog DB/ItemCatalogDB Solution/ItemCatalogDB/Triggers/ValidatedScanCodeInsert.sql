IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[ValidatedScanCodeInsert]'))
	EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[ValidatedScanCodeInsert] ON [dbo].[ValidatedScanCode] AFTER INSERT AS BEGIN SELECT 1 END'
GO

ALTER TRIGGER [dbo].[ValidatedScanCodeInsert]
	ON [dbo].[ValidatedScanCode]
	AFTER INSERT
AS
BEGIN
	-- Insert Scan Codes into an Identifier Type
	DECLARE @identifiers AS dbo.IdentifiersType;
	INSERT INTO @identifiers SELECT ScanCode FROM inserted vsc

	--generate item locale event for any newly validated scan code
	EXEC mammoth.GenerateEvents @identifiers, 'ItemLocaleAddOrUpdate';
	
	DECLARE @alternateIdentifiers AS dbo.IdentifiersType;
	INSERT INTO @alternateIdentifiers 
		SELECT ScanCode 
		FROM inserted vsc 
			INNER JOIN dbo.ItemIdentifier ii on ii.Identifier=vsc.ScanCode 
		WHERE ii.Default_Identifier = 0 AND ii.Deleted_Identifier = 0

	IF EXISTS (SELECT 1 FROM @alternateIdentifiers)
	BEGIN
		--generate price event only for validated alternate identifiers
		EXEC mammoth.GenerateEvents @alternateIdentifiers, 'Price';
	END
END
GO