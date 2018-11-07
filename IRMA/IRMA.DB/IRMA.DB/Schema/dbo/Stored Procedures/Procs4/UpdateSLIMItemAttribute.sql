CREATE PROCEDURE dbo.UpdateSLIMItemAttribute
        (
        @item_key int
        )
AS
	-- **************************************************************************
	-- Procedure: UpdateSLIMItemAttribute()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET NOCOUNT ON

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	Declare @UpdateField varchar(50)
	Declare @UploadFieldAttribute varchar(50)
	Declare @AttributeColumn varchar(50)
	Declare @SQL varchar(200)

	Select @UpdateField = field_type from AttributeIdentifier (NOLOCK) where screen_text = 'SLIM'

	--print @UpdateField

	if @UpdateField is not null
	begin

	select @UploadFieldAttribute = 'Check_Box_'

	set @AttributeColumn = @UploadFieldAttribute + right(@UpdateField,1)

	select @SQL = 'insert into ItemAttribute (Item_Key , ' + @AttributeColumn + ') values (' + CAST(@item_key AS VARCHAR(20)) + ', 1)'

	exec (@SQL)
	--print (@SQL)

	end

	COMMIT TRAN

	SET NOCOUNT OFF

	return
	--print N'Done'
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateSLIMItemAttribute] TO [IRMASLIMRole]
    AS [dbo];

