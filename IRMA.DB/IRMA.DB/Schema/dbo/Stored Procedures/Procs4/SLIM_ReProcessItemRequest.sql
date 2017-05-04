CREATE PROCEDURE [dbo].[SLIM_ReProcessItemRequest]
 @requestId int
AS
	-- **************************************************************************
	-- Procedure: SLIM_ReProcessItemRequest()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	Update ItemRequest
	set ItemStatus_ID = (select statusid from slim_statustypes where status = 'Pending')
	WHERE ItemRequest_ID=@requestId

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_ReProcessItemRequest] TO [IRMASLIMRole]
    AS [dbo];

