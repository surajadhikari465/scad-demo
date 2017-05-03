CREATE PROCEDURE [dbo].[UpdateSuspendedPOAdminNotes]
	@OrderHeader_ID int,	
	@Notes varchar(128)	
AS 
BEGIN
	SET NOCOUNT ON

	Update OrderHeader
	Set AdminNotes = @Notes
	Where OrderHeader_ID = @OrderHeader_ID
				
	SET NOCOUNT OFF 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateSuspendedPOAdminNotes] TO [IRMAClientRole]
    AS [dbo];

