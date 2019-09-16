
CREATE FUNCTION [dbo].[fn_RemoveSpecialChars] ( @Input VARCHAR(MAX), @includeAlphanumericOnly bit = 0 ) 
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
			IF     @CharCode BETWEEN 48 and 57  -- numerics 
				OR @CharCode BETWEEN 65 AND 90  -- upper case
				OR @CharCode BETWEEN 97 AND 122 -- lower case
				OR (@CharCode = 32 AND @includeAlphanumericOnly = 0)	--space character
				OR (@CharCode = 38 AND @includeAlphanumericOnly = 0)	-- '&' (ampersand)
				OR (@CharCode = 39 AND @includeAlphanumericOnly = 0)	--  ' (apostrophe)
				OR (@CharCode = 40 AND @includeAlphanumericOnly = 0)	--  ( (opening parenthesis)
				OR (@CharCode = 41 AND @includeAlphanumericOnly = 0)	--  ) (closing parenthesis)
				OR (@CharCode = 44 AND @includeAlphanumericOnly = 0)	-- ','
				OR (@CharCode = 45 AND @includeAlphanumericOnly = 0)	-- '-'
				OR (@CharCode = 46 AND @includeAlphanumericOnly = 0)	-- '.' (period)
				OR (@CharCode = 47 AND @includeAlphanumericOnly = 0)	-- '/' (slash)
				OR (@CharCode = 58 AND @includeAlphanumericOnly = 0)	-- ':' (colon)
				SET @Output = @Output + CHAR(@CharCode) SET @Counter = @Counter + 1 
		END 
	END 
	RETURN @Output 
END 
