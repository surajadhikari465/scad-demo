create PROCEDURE [dbo].[SLIM_RejectItemRequest]
 @requestId int,
 @ProcessedBy varchar(25),
 @Comments varchar(255)
AS
	-- **************************************************************************
	-- Procedure: SLIM_RejectItemRequest()
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
	set 
		ItemStatus_ID = (select statusid from slim_statustypes where status = 'rejected'),
		ProcessedBy = @ProcessedBy,
		Comments = @Comments
	WHERE 
		ItemRequest_ID=@requestId

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_RejectItemRequest] TO [IRMASLIMRole]
    AS [dbo];

