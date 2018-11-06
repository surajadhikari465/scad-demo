/****** Object:  StoredProcedure [dbo].[SLIM_ReProcessStoreSpecial]    Script Date: 07/09/2007 22:58:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SLIM_ReProcessStoreSpecial]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SLIM_ReProcessStoreSpecial]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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