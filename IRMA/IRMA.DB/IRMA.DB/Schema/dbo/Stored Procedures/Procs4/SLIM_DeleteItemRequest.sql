CREATE PROCEDURE [dbo].[SLIM_DeleteItemRequest]
 @requestId int
AS
	-- **************************************************************************
	-- Procedure: SLIM_DeleteItemRequest()
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

	delete from ItemRequest where ItemRequest_ID = @requestId

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_DeleteItemRequest] TO [IRMASLIMRole]
    AS [dbo];

