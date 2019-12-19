CREATE PROCEDURE [dbo].[AddOrUpdateAttributeCharaterSets] @CharacterSet dbo.AttributeCharacterSetsType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @AttributeId INT

	SET @AttributeId = (
			SELECT TOP 1 AttributeId
			FROM @CharacterSet
			)

	DELETE
	FROM dbo.AttributeCharacterSets
	WHERE AttributeId = @AttributeId

	INSERT INTO dbo.AttributeCharacterSets (
		[AttributeId]
		,[CharacterSetId]
		)
	SELECT [AttributeId]
		,[CharacterSetId]
	FROM @CharacterSet
END