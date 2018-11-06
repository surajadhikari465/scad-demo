IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateSuspendedPOAdminNotes]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UpdateSuspendedPOAdminNotes]
GO

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