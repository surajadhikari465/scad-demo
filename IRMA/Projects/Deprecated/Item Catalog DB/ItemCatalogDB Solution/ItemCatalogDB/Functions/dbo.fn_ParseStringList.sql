IF EXISTS (SELECT * 
	   FROM   sysobjects 
	   WHERE  name = N'fn_ParseStringList')
	DROP FUNCTION fn_ParseStringList
GO

CREATE FUNCTION [dbo].[fn_ParseStringList] 
	(@List varchar(max),
     @ListSeparator char(1))
RETURNS @TblList TABLE 
	(Key_Value varchar(max))
AS
BEGIN
    DECLARE @Left varchar(max), @i int, @j int

	if @ListSeparator is null
	 begin
		Select @ListSeparator = '|'
	 end

    SELECT @i = 0
    SELECT @j = PATINDEX('%' + @ListSeparator  + '%', SUBSTRING(@List, @i + 1, LEN(@List) - @i))

    WHILE (@j > 0)
    BEGIN
        SELECT @Left = SUBSTRING(@List, @i + 1, @j - 1)

        INSERT INTO @TblList
        SELECT @Left
        
        SELECT @i = @i + @j

        SELECT @j = PATINDEX('%' + @ListSeparator  + '%', SUBSTRING(@List, @i + 1, LEN(@List) - @i))
    END

    SELECT @Left = SUBSTRING(@List, @i + 1, LEN(@List) - @i)

    INSERT INTO @TblList
    SELECT @Left

    RETURN
END

GO