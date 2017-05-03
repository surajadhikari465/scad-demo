
/****** Object:  UserDefinedFunction [dbo].[fn_Parse_List_Two]    Script Date: 07/18/2008 11:05:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_Parse_List_Two]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_Parse_List_Two]
GO
CREATE FUNCTION [dbo].[fn_Parse_List_Two] 
(@List varchar(max), 
@ListSeparator1 char(1), 
@ListSeparator2 char(1))
RETURNS @TblList TABLE (Key_Value1 int, Key_Value2 int, RowID int IDENTITY(1,1), PRIMARY KEY CLUSTERED(Key_Value1, Key_Value2, RowID))
AS
BEGIN
--20080718 - DaveStacey - Created this function to handle a multi-dimensional string array for Store Creation purposes.
--		this can be used for the need to have a string pass two values per row w/a second delimiter

    DECLARE @Left varchar(max), @i int, @j int

    DECLARE @Left2 varchar(max), @Right varchar(max), @i2 int, @j2 int

	IF @List is not null
	BEGIN
		if @ListSeparator1 is null
		 begin
			Select @ListSeparator1 = '|'
		 end
		if @ListSeparator2 is null
		 begin
			Select @ListSeparator2 = ','
		 end

		SELECT @i = 0
		SELECT @j = PATINDEX('%' + @ListSeparator1  + '%', SUBSTRING(@List, @i + 1, LEN(@List) - @i))

		WHILE (@j > 0)
		BEGIN
			SELECT @Left = SUBSTRING(@List, @i + 1, @j - 1)

			SELECT @i2 = 0
			SELECT @j2 = PATINDEX('%' + @ListSeparator2  + '%', SUBSTRING(@Left, @i2 + 1, LEN(@Left) - @i2))
			SELECT @Left2 = SUBSTRING(@Left, @i2 + 1, @j2 - 1)
			SELECT @Right = SUBSTRING(@Left, @j2 + 1, LEN(@Left))
			INSERT INTO @TblList (Key_Value1, Key_Value2)
			SELECT CONVERT(int, @Left2),  CONVERT(int, @Right)
	        
			SELECT @i = @i + @j

			SELECT @j = PATINDEX('%' + @ListSeparator1  + '%', SUBSTRING(@List, @i + 1, LEN(@List) - @i))
		END

	END
	
    RETURN
END

GO