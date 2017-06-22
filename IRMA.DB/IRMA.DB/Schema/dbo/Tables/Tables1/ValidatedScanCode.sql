CREATE TABLE [dbo].[ValidatedScanCode] (
    [Id]         INT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ScanCode]   VARCHAR (13) NOT NULL,
    [InsertDate] DATETIME     NOT NULL,
	[InforItemId] INT		  NOT NULL,
    CONSTRAINT [PK_ValidatedScanCode] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [ScanCodeExistsConstraint] CHECK ([dbo].[fn_DoesScanCodeExist]([ScanCode])<>(0)),
    CONSTRAINT [UniqueScanCodeConstraint] UNIQUE NONCLUSTERED ([ScanCode] ASC) WITH (FILLFACTOR = 80)
);


GO

CREATE TRIGGER [dbo].[ValidatedScanCodeInsert]
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
GRANT ALTER
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ValidatedScanCode] TO [IRMAPDXExtractRole]
    AS [dbo];

