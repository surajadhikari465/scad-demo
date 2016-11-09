CREATE FUNCTION [dbo].[fn_GetScalePLU]
    (@Identifier				VARCHAR(13),
     @NumPluDigitsSentToScale	INT,
     @PluDigitsSentToScale		VARCHAR(20),
	 @IsCustomerFacingScalePlu	BIT)
RETURNS VARCHAR(6)
AS
BEGIN
/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110819	2672	Add check to make sure record is scale identifier
DN		20121012	6744	Add conditions to extract scale PLU for non-type-2 identifiers
DN		20130131	9995	Added conditions to extract scale PLU for type-2 and non-type-2 identifiers
BJL		20130513	11959	Modified function to take in the Identifier and NumPluDigitsSentToScale.
							Removed extraneous logic and join to ItemIdentifier. Calling sprocs 
							have this logic where needed.
***********************************************************************************************/
	DECLARE @ScalePLU VARCHAR(8)
	
	SELECT @ScalePLU = CASE 
							WHEN @IsCustomerFacingScalePlu = 1 THEN @Identifier
							WHEN @PluDigitsSentToScale = 'ALWAYS 4'
								AND SUBSTRING(@Identifier, 1, 1) = '2'
								AND RIGHT(@Identifier,5) = '00000' 
								AND LEN(RTRIM(@Identifier)) = 11
								THEN SUBSTRING(@Identifier, 3, 4) --SEND 4 (TYPE-2 ITEM)
							WHEN @PluDigitsSentToScale = 'ALWAYS 4'
								AND (SUBSTRING(@Identifier, 1, 1) != '2'
								OR (SUBSTRING(@Identifier, 1, 1) = '2'
								AND (RIGHT(@Identifier,5) != '00000' 
								OR LEN(RTRIM(@Identifier)) != 11)))
								THEN RIGHT(@Identifier, 4) --SEND 4 (NON TYPE-2 ITEM)	
							WHEN @PluDigitsSentToScale = 'ALWAYS 5' 
								AND SUBSTRING(@Identifier, 1, 1) = '2'
								AND RIGHT(@Identifier,5) = '00000' 
								AND LEN(RTRIM(@Identifier)) = 11
								THEN SUBSTRING(@Identifier, 2, 5) --SEND 5 (TYPE-2 ITEM)	
							WHEN @PluDigitsSentToScale = 'ALWAYS 5' 
								AND (SUBSTRING(@Identifier, 1, 1) != '2'
								OR (SUBSTRING(@Identifier, 1, 1) = '2'
								AND (RIGHT(@Identifier,5) != '00000' 
								OR LEN(RTRIM(@Identifier)) != 11)))
								THEN RIGHT(@Identifier, 5) --SEND 5 (NON TYPE-2 ITEM) 
							WHEN @PluDigitsSentToScale = 'VARIABLE BY ITEM'  -- SEND VALUE ON ItemIdentifier RECORD (TYPE-2 ITEM)
								AND @NumPluDigitsSentToScale = 4 
								AND SUBSTRING(@Identifier, 1, 1) = '2'
								AND RIGHT(@Identifier,5) = '00000' 
								AND LEN(RTRIM(@Identifier)) = 11
								THEN SUBSTRING(@Identifier, 3, 4) 
							WHEN @PluDigitsSentToScale = 'VARIABLE BY ITEM'  -- SEND VALUE ON ItemIdentifier RECORD (NON TYPE-2 ITEM)
								AND @NumPluDigitsSentToScale = 4 
								AND (SUBSTRING(@Identifier, 1, 1) != '2'
								OR (SUBSTRING(@Identifier, 1, 1) = '2'
								AND (RIGHT(@Identifier,5) != '00000' 
								OR LEN(RTRIM(@Identifier)) != 11)))
								THEN RIGHT(@Identifier, 4) 
							WHEN @PluDigitsSentToScale = 'VARIABLE BY ITEM'  -- SEND VALUE ON ItemIdentifier RECORD (TYPE-2 ITEM)
								AND @NumPluDigitsSentToScale = 5 
								AND SUBSTRING(@Identifier, 1, 1) = '2'
								AND RIGHT(@Identifier,5) = '00000' 
								AND LEN(RTRIM(@Identifier)) = 11
								THEN SUBSTRING(@Identifier, 2, 5) 
							WHEN @PluDigitsSentToScale = 'VARIABLE BY ITEM'  -- SEND VALUE ON ItemIdentifier RECORD (NON TYPE-2 ITEM)
								AND @NumPluDigitsSentToScale = 5 
								AND (SUBSTRING(@Identifier, 1, 1) != '2'
								OR (SUBSTRING(@Identifier, 1, 1) = '2'
								AND (RIGHT(@Identifier,5) != '00000' 
								OR LEN(RTRIM(@Identifier)) != 11)))
								THEN RIGHT(@Identifier, 5) 
							ELSE SUBSTRING(@Identifier, 3, 4) --DEFAULT TO LENGTH OF 4 IN EVENT DATA IS MISSING
		               END

    RETURN @ScalePLU
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetScalePLU] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetScalePLU] TO [IRMAClientRole]
    AS [dbo];

