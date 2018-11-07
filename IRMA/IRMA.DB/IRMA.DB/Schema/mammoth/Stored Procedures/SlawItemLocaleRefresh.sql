
CREATE PROCEDURE [mammoth].[SlawItemLocaleRefresh] 
	@Identifiers varchar(max)
as
BEGIN
	DECLARE @IdentifiersType dbo.IdentifiersType

	INSERT INTO  @IdentifiersType(Identifier)
	SELECT Key_Value FROM fn_ParseStringList(@Identifiers, '|')

	EXEC [mammoth].[GenerateEvents] @IdentifiersType, 'ItemLocaleAddOrUpdate'
END

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[SlawItemLocaleRefresh] TO [IRMAClientRole]
    AS [dbo];

