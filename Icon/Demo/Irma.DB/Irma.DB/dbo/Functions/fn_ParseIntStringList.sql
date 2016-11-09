CREATE FUNCTION [dbo].[fn_ParseIntStringList] 
(@KeyList varchar(max), 
@ListSeparator1 char(1), 
@ListSeparator2 char(1))
RETURNS @TblList TABLE (Key_Value1 int, Key_Value2 varchar(50), RowID int IDENTITY(1,1), PRIMARY KEY CLUSTERED(Key_Value1, Key_Value2, RowID))
AS
BEGIN
--20100805 - DaveStacey - Created this function to handle a multi-dimensional string array for Competitive Price batch purposes.
--		this can be used for the need to have a string pass two values per row w/a second delimiter

    DECLARE @Left varchar(max), @i int, @j int

    DECLARE @Left2 varchar(max), @Right varchar(50), @i2 int, @j2 int

	IF @KeyList is not null
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
		SELECT @j = PATINDEX('%' + @ListSeparator1  + '%', SUBSTRING(@KeyList, @i + 1, LEN(@KeyList) - @i))

		WHILE (@j > 0)
		BEGIN
			SELECT @Left = SUBSTRING(@KeyList, @i + 1, @j - 1)

			SELECT @i2 = 0
			SELECT @j2 = PATINDEX('%' + @ListSeparator2  + '%', SUBSTRING(@Left, @i2 + 1, LEN(@Left) - @i2))
			SELECT @Left2 = SUBSTRING(@Left, @i2 + 1, @j2 - 1)
			SELECT @Right = SUBSTRING(@Left, @j2 + 1, LEN(@Left))
			INSERT INTO @TblList (Key_Value1, Key_Value2)
			SELECT CONVERT(int, @Left2),  CONVERT(varchar(50), @Right)
	        
			SELECT @i = @i + @j

			SELECT @j = PATINDEX('%' + @ListSeparator1  + '%', SUBSTRING(@KeyList, @i + 1, LEN(@KeyList) - @i))
		END

	END
	
    RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ParseIntStringList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ParseIntStringList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ParseIntStringList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ParseIntStringList] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ParseIntStringList] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_ParseIntStringList] TO [IRMASLIMRole]
    AS [dbo];

