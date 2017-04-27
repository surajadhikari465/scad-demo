IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SlawPriceRefresh')
	EXEC('CREATE PROCEDURE [mammoth].[SlawPriceRefresh] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [mammoth].[SlawPriceRefresh] 
	@Identifiers varchar(max)
AS
BEGIN
	DECLARE @IdentifiersType dbo.IdentifiersType

	INSERT INTO  @IdentifiersType(Identifier)
	SELECT Key_Value FROM fn_ParseStringList(@Identifiers, '|')

	EXEC [mammoth].[GenerateEvents] @IdentifiersType, 'Price'
END

