CREATE FUNCTION [dbo].[fn_Parse_List_Four] 
(
	@List varchar(max), 
	@ListSeparator1 char(1), 
	@ListSeparator2 char(1)
)
RETURNS @TblList TABLE (Key_Value1 int, Key_Value2 int, Key_Value3 money, Key_Value4 decimal)
AS
-- **************************************************************************
-- Procedure:   fn_Parse_List_Four
--    Author:   Faisal Ahmed
--      Date:   03/18/2013
--
-- Description: This function parses a list of data valuses separated by delimeters. 
--  
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013/03/18	FA		8457	Initial creation
-- **************************************************************************
BEGIN
    DECLARE @Left varchar(max), @i int, @j int
	
    DECLARE @Left2 varchar(max), @Right varchar(max), @i2 int, @j2 int, @k2 int, @l2 int

    
	DECLARE @strRecord varchar(max)
	DECLARE @value1 varchar(max), @value2 varchar(max), @value3 varchar(max), @value4 varchar(max)
	
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
			SELECT @strRecord = SUBSTRING(@List, @i + 1, @j - 1)
			
			SELECT @i2 = 0
			SELECT @j2 = PATINDEX('%' + @ListSeparator2  + '%', SUBSTRING(@strRecord, 1, LEN(@strRecord)))
			SELECT @value1 = SUBSTRING(@strRecord, 1, @j2 - 1)
			
			SELECT @strRecord = SUBSTRING(@strRecord, @j2 + 1, LEN(@strRecord))			
			SELECT @i2 = 0
			SELECT @j2 = PATINDEX('%' + @ListSeparator2  + '%', SUBSTRING(@strRecord, 1, LEN(@strRecord)))
			SELECT @value2 = SUBSTRING(@strRecord, 1, @j2 - 1)
				
			SELECT @strRecord = SUBSTRING(@strRecord, @j2 + 1, LEN(@strRecord))			
			SELECT @i2 = 0
			SELECT @j2 = PATINDEX('%' + @ListSeparator2  + '%', SUBSTRING(@strRecord, 1, LEN(@strRecord)))
			SELECT @value3 = SUBSTRING(@strRecord, 1, @j2 - 1)

			SELECT @strRecord = SUBSTRING(@strRecord, @j2 + 1, LEN(@strRecord))			
			SELECT @value4 = @strRecord
			
			INSERT INTO @TblList (Key_Value1, Key_Value2, Key_Value3, Key_Value4) 
				SELECT CONVERT(int, @value1),  CONVERT(int, @value2), CONVERT(money, @value3), CONVERT(decimal, @value4)
	        
			SELECT @i = @i + @j

			SELECT @j = PATINDEX('%' + @ListSeparator1  + '%', SUBSTRING(@List, @i + 1, LEN(@List) - @i))
		END

	END
	
    RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_Four] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_Four] TO [IRMAClientRole]
    AS [dbo];

