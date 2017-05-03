Create PROCEDURE [dbo].[IsScaleIdentifier] 
	@Identifier varchar(13)
	,
	@IsScaleIdentifier bit OUTPUT
AS 
BEGIN
    
    SELECT @IsScaleIdentifier = dbo.fn_IsScaleIdentifier(@Identifier)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsScaleIdentifier] TO [IRMAClientRole]
    AS [dbo];

