CREATE FUNCTION [dbo].[fn_ConvertVarBinaryToHex] 
	(
		@InputValue varbinary(256),
		@ReverseByteOrder bit = 0
	)
	
RETURNS varchar(256)

AS

BEGIN
	DECLARE @HexValue varchar(256),
		@ReverseHexValue varchar(256),
		@Index int

	-- use built-in SQL function to convert a VarBinary to Hex; replace the leading '0x' hex indicator
	SELECT @HexValue = REPLACE(sys.fn_varbintohexstr(@InputValue), '0x', '')
		
	IF @ReverseByteOrder = 1
	  BEGIN
		-- set initial values before loop
		SELECT @Index = 1,
			@ReverseHexValue = ''

		-- check that hex string has an even number of digits; otherwise return empty string
		IF LEN(@HexValue) % 2 = 0
		  BEGIN
			WHILE @Index < LEN(@HexValue)
			  BEGIN
				-- build the reverse character couple string by prepending successive values
				SELECT @ReverseHexValue = SUBSTRING(@HexValue, @Index, 2) + @ReverseHexValue

				-- increment the position within the string by 2 characters
				SELECT @Index = @Index + 2
			  END
		  END
	  END

	RETURN ISNULL(@ReverseHexValue, @HexValue)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ConvertVarBinaryToHex] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ConvertVarBinaryToHex] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ConvertVarBinaryToHex] TO [IRMAClientRole]
    AS [dbo];

