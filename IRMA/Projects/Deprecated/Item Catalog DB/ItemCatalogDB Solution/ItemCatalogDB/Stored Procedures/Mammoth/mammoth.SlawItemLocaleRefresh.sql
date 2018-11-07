IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SlawItemLocaleRefresh')
	EXEC('CREATE PROCEDURE [mammoth].[SlawItemLocaleRefresh] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [mammoth].[SlawItemLocaleRefresh] 
	@Identifiers varchar(max)
as
BEGIN
	DECLARE @IdentifiersType dbo.IdentifiersType

	INSERT INTO  @IdentifiersType(Identifier)
	SELECT Key_Value FROM fn_ParseStringList(@Identifiers, '|')

	EXEC [mammoth].[GenerateEvents] @IdentifiersType, 'ItemLocaleAddOrUpdate'
END
