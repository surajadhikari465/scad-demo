
CREATE PROCEDURE [mammoth].[SlawPriceRefresh] 
	@Identifiers varchar(max)
AS
BEGIN
	DECLARE @IdentifiersType dbo.IdentifiersType

	INSERT INTO  @IdentifiersType(Identifier)
	SELECT Key_Value FROM fn_ParseStringList(@Identifiers, '|')

	EXEC [mammoth].[GenerateEvents] @IdentifiersType, 'Price'
END

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[SlawPriceRefresh] TO [IRMAClientRole]
    AS [dbo];

