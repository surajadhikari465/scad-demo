
CREATE FUNCTION [dbo].[fn_RemoveSpecialChars] ( @Input VARCHAR(MAX) ) 
RETURNS VARCHAR(MAX) 
BEGIN 
	DECLARE @Output VARCHAR(MAX ) 
	IF (ISNULL(@Input,'')='') 
		SET @Output = @Input 
	ELSE 
	BEGIN 
		DECLARE @Len INT 
		DECLARE @Counter INT 
		DECLARE @CharCode INT 
		SET @Output = '' 
		SET @Len = LEN(@Input) 
		SET @Counter = 1 
		WHILE @Counter <= @Len 
		BEGIN 
			SET @CharCode = ASCII(SUBSTRING(@Input, @Counter, 1)) 
			IF     @CharCode = 32 --space character
				OR @CharCode BETWEEN 48 and 57  -- numerics 
				OR @CharCode BETWEEN 65 AND 90  -- upper case
				OR @CharCode BETWEEN 97 AND 122 -- lower case
				OR @CharCode = 38 -- '&' (ampersand)
				OR @CharCode = 39 --  ' (apostrophe)
				OR @CharCode = 40 --  ( (opening parenthesis)
				OR @CharCode = 41 --  ) (closing parenthesis)
				OR @CharCode = 44 -- ','
				OR @CharCode = 45 -- '-'
				OR @CharCode = 46 -- '.' (period)
				OR @CharCode = 47 -- '/' (slash)
				OR @CharCode = 58 -- ':' (colon)
				SET @Output = @Output + CHAR(@CharCode) SET @Counter = @Counter + 1 
		END 
	END 
	RETURN @Output 
END 
