 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_ConvertFromPascalCaseToUnderscore]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_ConvertFromPascalCaseToUnderscore]
GO

CREATE FUNCTION [dbo].[fn_ConvertFromPascalCaseToUnderscore]
(@PascalCasedText varchar(8000))

RETURNS varchar(8000)
BEGIN

	DECLARE @UnderscoredText varchar(8000),
		@StartPos int,
		@EndPos int,
		@PreviousChar char,
		@CurrentChar char,
		@InsertUnderscore bit
	
	SET @UnderscoredText = ''
	SET @InsertUnderscore = 1
	SET @StartPos = 1
	SET @EndPos = 1
	
	WHILE @EndPos <= LEN(@PascalCasedText) + 1
	BEGIN
	
		SET @CurrentChar = SUBSTRING(@PascalCasedText, @EndPos, 1)

		-- is the char upper case, numeric, or the end?
		If @EndPos > 1 AND
				(ASCII(@CurrentChar) < 97 OR (ASCII(@CurrentChar) >= 48 AND ASCII(@CurrentChar) <= 57))
		BEGIN
		
			IF (ASCII(@CurrentChar) >= 48 AND ASCII(@CurrentChar) <= 57) AND
					(ASCII(@PreviousChar) >= 48 AND ASCII(@PreviousChar) <= 57)
			BEGIN
				-- the current and previous chars are numeric
				-- we need to keep all groups of numeral together
				SET @InsertUnderscore = 0
			END
			ELSE
			BEGIN
				SET @InsertUnderscore = 1
			END
			
			IF @InsertUnderscore = 1
			BEGIN
				IF LEN(@UnderscoredText) > 0
				BEGIN
					SET @UnderscoredText = @UnderscoredText + '_'
				END
				
				SET @UnderscoredText = @UnderscoredText + SUBSTRING(@PascalCasedText, @StartPos, @EndPos - @StartPos)
			
				SET @StartPos = @EndPos
			END
		END
		
		SET @PreviousChar = @CurrentChar
		SET @EndPos = @EndPos + 1
	
	END
	
    RETURN @UnderscoredText

END
GO