IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SLIM_DeleteItemRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SLIM_DeleteItemRequest]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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