CREATE PROCEDURE [dbo].[SLIM_ProcessStoreSpecial]
  @requestId int,
  @ProcessedBy varchar(25)
AS
	-- **************************************************************************
	-- Procedure: SLIM_ProcessStoreSpecial()
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
	SET Status = (select statusid from slim_statustypes where status = 'InProcess'),
		ProcessedBy = @ProcessedBy
	WHERE requestId=@requestId

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_ProcessStoreSpecial] TO [IRMASLIMRole]
    AS [dbo];

