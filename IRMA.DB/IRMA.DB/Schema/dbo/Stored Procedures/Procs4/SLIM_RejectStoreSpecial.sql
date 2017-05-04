﻿create PROCEDURE [dbo].[SLIM_RejectStoreSpecial]
 @requestId int,
 @ProcessedBy varchar(25),
 @Comments varchar(255)
AS
	-- **************************************************************************
	-- Procedure: SLIM_RejectStoreSpecial()
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
	set Status = (select statusid from slim_statustypes where status = 'rejected'),
	ProcessedBy = @ProcessedBy,
	Comments = @Comments
	WHERE requestId=@requestId

	COMMIT TRAN
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_RejectStoreSpecial] TO [IRMASLIMRole]
    AS [dbo];

