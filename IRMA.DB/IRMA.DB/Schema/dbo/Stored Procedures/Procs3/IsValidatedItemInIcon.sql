
CREATE PROCEDURE [dbo].[IsValidatedItemInIcon] 
	@ItemKey int,
	@IsValidatedInIcon bit OUTPUT
AS 
BEGIN

    SET @IsValidatedInIcon = 
		ISNULL((SELECT
			vsc.ScanCode
		FROM
			ValidatedScanCode vsc
		WHERE
			vsc.ScanCode = (SELECT ii.Identifier FROM ItemIdentifier ii WHERE ii.Item_Key = @ItemKey AND ii.Default_Identifier = 1)), 0)

	SELECT @IsValidatedInIcon

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsValidatedItemInIcon] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IsValidatedItemInIcon] TO [IConInterface]
    AS [dbo];

