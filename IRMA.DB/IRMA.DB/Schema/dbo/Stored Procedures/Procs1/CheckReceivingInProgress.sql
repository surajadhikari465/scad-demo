CREATE PROCEDURE dbo.CheckReceivingInProgress
   @Store_No int,
   @User_ID int
AS
BEGIN	
    SET NOCOUNT ON

    SELECT IsNull((SELECT 1 FROM store WHERE Store_No = @Store_No AND ISNULL(RecvLogUser_ID, @User_ID) != @User_ID), 0)

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckReceivingInProgress] TO [IRMAAdminRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckReceivingInProgress] TO [IRMAClientRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckReceivingInProgress] TO [IRMASchedJobsRole]
    AS [dbo];