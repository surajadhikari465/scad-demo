CREATE PROCEDURE dbo.RollbackElectronicOrder
    @OrderHeader_ID int
AS

BEGIN
    SET NOCOUNT ON

   UPDATE OrderHeader 
   SET Sent = 0, User_Id = NULL
   WHERE OrderHeader_ID = @OrderHeader_ID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RollbackElectronicOrder] TO [IRMAClientRole]
    AS [dbo];

