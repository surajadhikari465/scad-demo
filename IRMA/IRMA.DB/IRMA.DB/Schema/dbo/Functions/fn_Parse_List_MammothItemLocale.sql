CREATE FUNCTION dbo.fn_Parse_List_MammothItemLocale 
	(@List varchar(250),
     @ListSeparator char(1))
RETURNS @TblList TABLE 
	(Key_Value int, RowID int IDENTITY(1,1), PRIMARY KEY CLUSTERED(Key_Value, RowID))
AS
BEGIN
    DECLARE @Left varchar(250), @i int, @j int

	IF @List is not null
	BEGIN
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
			SELECT CONVERT(int, @Left)
	        
			SELECT @i = @i + @j

			SELECT @j = PATINDEX('%' + @ListSeparator  + '%', SUBSTRING(@List, @i + 1, LEN(@List) - @i))
		END

		SELECT @Left = SUBSTRING(@List, @i + 1, LEN(@List) - @i)

		INSERT INTO @TblList
		SELECT CONVERT(int, @Left)
	END
	
    RETURN
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [MammothRole]
    AS [dbo];

GO

GRANT SELECT
    ON OBJECT::[dbo].[fn_Parse_List_MammothItemLocale] TO [IRMAPDXExtractRole]
    AS [dbo];
