
CREATE FUNCTION dbo.fn_ValidatedScanCodeExists
	(
	@Item_Key INT
	)
RETURNS BIT
AS
	BEGIN
		DECLARE @ReceiveUpdateFromIcon INT
		DECLARE @ValidatedScanCodeExists SMALLINT = 0

		SET @ReceiveUpdateFromIcon = (SELECT dbo.fn_ReceiveUPCPLUUpdateFromIcon())
		IF @ReceiveUpdateFromIcon > 0 
			BEGIN
				IF EXISTS (SELECT Item_Key FROM ItemIdentifier II (NOLOCK) INNER JOIN ValidatedScanCode VSC (NOLOCK) ON II.Identifier = VSC.Scancode WHERE II.Item_Key = @Item_Key)
					SET @ValidatedScanCodeExists = 1
				ELSE
					SET @ValidatedScanCodeExists = 0
			END
		ELSE
			SET @ValidatedScanCodeExists = 0

		RETURN @ValidatedScanCodeExists

	END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidatedScanCodeExists] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ValidatedScanCodeExists] TO [IRSUser]
    AS [dbo];

