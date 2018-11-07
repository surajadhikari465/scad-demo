create PROCEDURE [dbo].[SLIM_ReProcessStoreSpecial]
 @requestId int
AS
	-- **************************************************************************
	-- Procedure: SLIM_ReProcessStoreSpecial()
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

	Update SLIM_InstoreSpecials
	set Status = (select statusid from slim_statustypes where status = 'Pending')
	WHERE requestId=@requestId

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_ReProcessStoreSpecial] TO [IRMASLIMRole]
    AS [dbo];

